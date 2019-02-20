using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Niente.Common.CosmosDb
{
    public static partial class DocumentClientExtensions
    {
        public static async Task<TResponse> ExecuteStoredProcedure<TResponse>(this DocumentClient client, string databaseName, string collectionName, string storedProcedureName)
        {
            string relUri = $"/dbs/{databaseName}/colls/{collectionName}/sprocs/{storedProcedureName}";
            StoredProcedureResponse<TResponse> response = await client.ExecuteStoredProcedureAsync<TResponse>(relUri);
            return response.Response;
        }

        public static async Task<TResponse> ExecuteStoredProcedure<TResponse>(this DocumentClient client, CosmosDbCollection collection, string storedProcedureName)
        {
            return await client.ExecuteStoredProcedure<TResponse>(collection.DatabaseName, collection.CollectionName, storedProcedureName);
        }
    }
}
