// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Settings.cs" company="Starion Group S.A.">
//
//   Copyright 2016-2021 Starion Group S.A.
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
//
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MessagePackIntegrationTests
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;

    using CDP4Common.MetaInfo;

    using CDP4MessagePackSerializer;

    using Microsoft.Extensions.Configuration;
    
    using NUnit.Framework;

    /// <summary>
    /// Suite of tests that verifies that MessagePack responses are returned
    /// </summary>
    [TestFixture]
    public class MessagePactTestsFixture
    {
        private IConfiguration configuration;

        private HttpClient httpClient;

        [SetUp]
        public void Setup()
        {
            var appsettings = "";
#if LOCAL
            appsettings = "appsettings.Local.json";
#endif

#if DEBUG
            appsettings = "appsettings.Debug.json";
#endif
            this.configuration = new ConfigurationBuilder()
                .SetBasePath(TestContext.CurrentContext.TestDirectory)
                .AddJsonFile(appsettings)
                .Build();
            
            this.httpClient = this.CreateHttpClient(
                this.configuration.GetSection("Username").Value,
                this.configuration.GetSection("Password").Value,
                this.configuration.GetSection("Hostname").Value);

        }

        private HttpClient CreateHttpClient(string username, string password, string url, bool withMessagePack = true)
        {
            var result = new HttpClient();

            result.BaseAddress = new Uri(url);
            result.DefaultRequestHeaders.Accept.Clear();
            result.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (withMessagePack)
            {
                result.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/msgpack"));
            }
            
            result.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{username}:{password}")));
            result.DefaultRequestHeaders.Add("Accept-CDP", "1.2.0");
            result.DefaultRequestHeaders.Add("User-Agent", "CDP4 (ECSS-E-TM-10-25 Annex C.2) CDPServicesDal");

            return result;
        }

        [Test]
        [Category("GET")]
        public async Task Verify_that_MessagePack_and_Json_content_are_the_same_when_returned_from_the_server()
        {
            var cts = new CancellationTokenSource();

            var httpResponseMessage =  await this.httpClient.GetAsync("/SiteDirectory?extent=deep&includeReferenceData=true", cts.Token);

            Assert.That(httpResponseMessage.Content.Headers.ContentType.MediaType , Is.EqualTo("application/msgpack"));

            var messagePackSerializer = new MessagePackSerializer();

            var stream = await httpResponseMessage.Content.ReadAsStreamAsync(cts.Token);

            var messagePackThings = await messagePackSerializer.DeserializeAsync(stream, cts.Token);

            messagePackThings = messagePackThings.OrderBy(x => x.Iid);

            this.httpClient = this.CreateHttpClient(
                this.configuration.GetSection("Username").Value,
                this.configuration.GetSection("Password").Value,
                this.configuration.GetSection("Hostname").Value, false);

            httpResponseMessage = await this.httpClient.GetAsync("/SiteDirectory?extent=deep&includeReferenceData=true", cts.Token);

            var metaDataProvider = new MetaDataProvider();
            var version = new Version(1,0,0);
            var jsonserialer = new CDP4JsonSerializer.Cdp4JsonSerializer(metaDataProvider, version);

            stream = await httpResponseMessage.Content.ReadAsStreamAsync(cts.Token);
            var jsonThings = jsonserialer.Deserialize(stream).OrderBy(x => x.Iid);

            Assert.That(jsonThings.Count(), Is.EqualTo(messagePackThings.Count()));

            foreach (var jsonThing in jsonThings)
            {
                Assert.That(messagePackThings.SingleOrDefault(x => x.Iid == jsonThing.Iid), Is.Not.Null);
            }

            var jsonSerialized = JsonSerializer.Serialize(jsonThings);
            var messagePackSerialized = JsonSerializer.Serialize(messagePackThings);

            Assert.That(messagePackSerialized, Is.EqualTo(jsonSerialized));
        }
    }
}
