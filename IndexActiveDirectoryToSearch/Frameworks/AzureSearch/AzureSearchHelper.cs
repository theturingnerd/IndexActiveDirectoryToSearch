using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndexActiveDirectoryToSearch.Frameworks.AzureSearch
{
    public static class AzureSearchHelper
    {
        /// <summary>
        /// Deleyes and recreates the index
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceClient"></param>
        /// <param name="index"></param>
        /// <param name="keyField"></param>
        /// <param name="ignoreSearchableFields"></param>
        public static void CreateIndex<T>(SearchServiceClient serviceClient, string index, string keyField, List<String> ignoreSearchableFields)
        {
            var definition = new Index()
            {
                Name = index,
                Fields = FieldBuilder.BuildForType<T>()
            };

            definition.Fields.FirstOrDefault(p => p.Name == keyField).IsKey = true;
            foreach(var field in definition.Fields)
            {
                   if(ignoreSearchableFields.Count(p=>p.Contains(field.Name)) > 0)
                {
                    field.IsSearchable = false;
                }              
                   else
                {
                    field.IsSearchable = true;
                }
            }
            serviceClient.Indexes.Delete(index);
            serviceClient.Indexes.Create(definition);
        }
    }
}
