﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Octokit.GraphQL.Core;
using Octokit.GraphQL.Core.Builders;
using Octokit.GraphQL.Core.Deserializers;
using Octokit.GraphQL.Core.Serializers;
using Octokit.GraphQL.Internal;

namespace Octokit.GraphQL
{
    public class Connection
    {
        /// <summary>
        /// The address of the GitHub GraphQL API.
        /// </summary>
        public static readonly Uri GithubApiUri = new Uri("https://api.github.com/graphql");

        private readonly ICredentialStore credentialStore;
        private readonly HttpClient httpClient;

        public Connection(ProductHeaderValue productInformation, string token)
            : this(productInformation, GithubApiUri, token)
        {
        }

        public Connection(ProductHeaderValue productInformation, Uri uri, string token)
            : this(productInformation, uri, new InMemoryCredentialStore(token))
        {
        }

        public Connection(ProductHeaderValue productInformation, ICredentialStore credentialStore)
            : this(productInformation, GithubApiUri, credentialStore)
        {
        }

        public Connection(ProductHeaderValue productInformation, Uri uri, ICredentialStore credentialStore)
        {
            Uri = uri;
            this.credentialStore = credentialStore;
            httpClient = CreateHttpClient(productInformation);
        }

        public Uri Uri { get; }

        public async Task<T> Run<T>(IQueryableValue<T> queryable)
        {
            var builder = new QueryBuilder();
            var query = builder.Build(queryable);
            var payload = query.GetPayload();
            var token = await credentialStore.GetCredentials();
            var request = new HttpRequestMessage(HttpMethod.Post, Uri);
            request.Content = new StringContent(payload);
            request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await httpClient.SendAsync(request);
            var data = await response.Content.ReadAsStringAsync();
            var deserializer = new ResponseDeserializer();
            return deserializer.Deserialize(query, data);
        }

        public async Task<IEnumerable<T>> Run<T>(IQueryableList<T> queryable)
        {
            var builder = new QueryBuilder();
            var query = builder.Build(queryable);
            var payload = query.GetPayload();
            var token = await credentialStore.GetCredentials();
            var request = new HttpRequestMessage(HttpMethod.Post, Uri);
            request.Content = new StringContent(payload);
            request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await httpClient.SendAsync(request);
            var data = await response.Content.ReadAsStringAsync();
            var deserializer = new ResponseDeserializer();
            return deserializer.Deserialize(query, data);
        }

        private HttpClient CreateHttpClient(ProductHeaderValue header)
        {
            var result = new HttpClient();
            var userAgent = new ProductInfoHeaderValue(header.Name, header.Version);
            result.DefaultRequestHeaders.UserAgent.Add(userAgent);
            return result;
        }
    }
}
