using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndexActiveDirectoryToSearch.Frameworks.Loggers
{
    internal class Logger
    {
        private string _sourceName;

        public Logger(string source)
        {
            _sourceName = source;

            if (!System.Diagnostics.EventLog.SourceExists(_sourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(_sourceName, "Application");
            }
        }

        public void LogEvent(string text)
        {
            System.Diagnostics.EventLog.WriteEntry(_sourceName, text);
            Console.WriteLine(text);
        }
    }
}
