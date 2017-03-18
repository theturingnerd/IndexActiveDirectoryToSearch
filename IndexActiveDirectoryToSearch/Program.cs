using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace IndexActiveDirectoryToSearch
{
    class Program
    {
   


        //test
        static void Main(string[] args)
        {
            HostFactory.Run(x =>                                 
            {
                x.Service<Services.ActiveDirectoryIndexer>(s =>                       
                {
                    s.ConstructUsing(name => new Services.ActiveDirectoryIndexer());     
                    s.WhenStarted(ad => ad.Start());              
                    s.WhenStopped(ad => ad.Stop());               
                });
                x.RunAsNetworkService();                         

                x.SetDescription("Active Directory Seed & Index Service for Search");        
                x.SetDisplayName("IH Index Active Directory to Search");                       
                x.SetServiceName("IH.IndexActiveDirectoryToSearch");                       
            });
        }
    }
}
