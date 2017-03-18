using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndexActiveDirectoryToSearch.Services
{
    public static class AzureSearchIndexService
    {

        public static void Test()
        {
          
        }
        public static SearchServiceClient CreateSearchServiceClient()
        {
            string adminApiKey = System.Configuration.ConfigurationManager.AppSettings["Azure.Search.APIKey.Admin"];
            string searchServiceName = System.Configuration.ConfigurationManager.AppSettings["Azure.Search.ServiceName"];

            SearchServiceClient serviceClient = new SearchServiceClient(searchServiceName, new SearchCredentials(adminApiKey));
            return serviceClient;
        }

       
    }
}
