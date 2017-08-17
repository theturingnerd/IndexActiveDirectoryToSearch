using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace IndexActiveDirectoryToSearch.Services
{
    public class ActiveDirectoryIndexer
    {
        readonly Timer _timer;
        List<Task> _jobs;
        string _indexName;
        Frameworks.Loggers.Logger _logger;

        public ActiveDirectoryIndexer()
        {
            _jobs = new List<Task>();
            _timer = new Timer(1000) {AutoReset = true};
            _timer.Elapsed += _timer_Elapsed;
            _indexName = System.Configuration.ConfigurationManager.AppSettings["Azure.Search.IndexName"];
            _logger = new Frameworks.Loggers.Logger(System.Reflection.Assembly.GetEntryAssembly().GetName().Name);
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            //search active directory
            
            _logger.LogEvent("Fetching Mail enabled users from AD...");
            var client = Services.AzureSearchIndexService.CreateSearchServiceClient();
            Frameworks.AzureSearch.AzureSearchHelper.CreateIndex<Frameworks.ActiveDirectory.ADUserDetail>(client, _indexName, "ObjectGUID", 
                new List<string>() { "ManagerName", "ThumbnailPhoto" });
            ISearchIndexClient indexClient = client.Indexes.GetClient(_indexName);

            var adhelper = new Frameworks.ActiveDirectory.ActiveDirectoryHelper();
            var users = adhelper.GetAllMailUsers();
            _logger.LogEvent("Beginning index upload...");

            for(int i =0;i<users.Count; i+= 999)
            {
                var batch = IndexBatch.Upload(users.Skip(i).Take(999));
                try
                {
                    indexClient.Documents.Index(batch);
                }
                catch (IndexBatchException exc)
                {
                    // Sometimes when your Search service is under load, indexing will fail for some of the documents in
                    // the batch. Depending on your application, you can take compensating actions like delaying and
                    // retrying. For this simple demo, we just log the failed document keys and continue.
                    _logger.LogEvent(string.Format(
                        "Failed to index some of the documents: {0}",
                        String.Join(", ", exc.IndexingResults.Where(r => !r.Succeeded).Select(r => r.Key))));
                }
            }

            _logger.LogEvent("Job Complete...!");
            _logger.LogEvent("Will resume in 24 hours!");
            
                  
                _timer.Interval = Double.Parse(System.Configuration.ConfigurationManager.AppSettings["RefreshInterval"]);
            ;
            _timer.Start();


           

        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
            foreach(var job in _jobs)
            {
                job.Wait(30);
            }
        }
    }
}
