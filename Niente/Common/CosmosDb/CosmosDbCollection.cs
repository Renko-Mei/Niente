using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Niente.Common.CosmosDb
{
    public struct CosmosDbCollection
    {
        public string DatabaseName { get; }
        public string CollectionName { get; }

        public CosmosDbCollection(string dbName, string colName)
        {
            DatabaseName = dbName;
            CollectionName = colName;
        }
    }
}
