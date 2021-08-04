// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileRevisionTestFixture.cs" company="RHEA System S.A.">
//
//   Copyright 2016-2021 RHEA System S.A.
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

namespace WebservicesIntegrationTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class FileRevisionTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedFileRevisionIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var fileRevisionUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/domainFileStore/da7dddaa-02aa-4897-9935-e8d66c811a96/file/95bf0f17-1273-4338-98ae-839016242775/fileRevision");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(fileRevisionUri);

            //check if there is the only one FileRevision object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific FileRevision from the result by it's unique id
            var fileRevision =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "5544bb87-dc38-45d5-9d92-c580d3fe0442");
            FileRevisionTestFixture.VerifyProperties(fileRevision);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedFileRevisionWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var fileRevisionUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/domainFileStore/da7dddaa-02aa-4897-9935-e8d66c811a96/file/95bf0f17-1273-4338-98ae-839016242775/fileRevision?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(fileRevisionUri);

            //check if there are appropriate amount of objects
            Assert.AreEqual(5, jArray.Count);

            // get a specific Iteration from the result by it's unique id
            var iteration =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific DomainFileStore from the result by it's unique id
            var domainFileStore =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "da7dddaa-02aa-4897-9935-e8d66c811a96");
            DomainFileStoreTestFixture.VerifyProperties(domainFileStore);

            // get a specific File from the result by it's unique id
            var file =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "95bf0f17-1273-4338-98ae-839016242775");
            FileTestFixture.VerifyProperties(file);

            // get a specific FileRevision from the result by it's unique id
            var fileRevision =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "5544bb87-dc38-45d5-9d92-c580d3fe0442");
            FileRevisionTestFixture.VerifyProperties(fileRevision);
        }

        [Test]
        [Category("POST")]
        public async Task VerifyThatFileRevisionCannotBeUploadedWhenParticipantIsNotOwner()
        {
            SiteDirectoryTestFixture.AddDomainExpertUserJane(this, out var userName, out var passWord);
            this.CreateNewWebClientForUser(userName, passWord);

            // Subsequent revision
            var fileUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postJsonPath = this.GetPath("Tests/EngineeringModel/File/PostNewFileRevision.json");
            var postFilePath = this.GetPath("Tests/EngineeringModel/File/1525ED651E5B609DAE099DEEDA8DBDB49CFF956F");

            var exception = Assert.CatchAsync<HttpRequestException>(async () => await this.WebClient.PostFile(fileUri, postJsonPath, postFilePath));

            if (exception?.StatusCode == HttpStatusCode.InternalServerError)
            {
                Assert.Fail("This fails on a Nancy COMET Webservice");
            }
            else
            {
                Assert.That(exception?.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            }
        }

        /// <summary>
        /// Verifies all properties of the FileRevision <see cref="JToken"/>
        /// </summary>
        /// <param name="fileRevision">
        /// The <see cref="JToken"/> that contains the properties of
        /// the FileRevision object
        /// </param>
        public static void VerifyProperties(JToken fileRevision)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(9, fileRevision.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("5544bb87-dc38-45d5-9d92-c580d3fe0442", (string) fileRevision[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) fileRevision[PropertyNames.RevisionNumber]);
            Assert.AreEqual("FileRevision", (string) fileRevision[PropertyNames.ClassKind]);

            Assert.AreEqual("FileRevision", (string) fileRevision[PropertyNames.Name]);
            Assert.AreEqual("2016-11-02T13:58:35.936Z", (string) fileRevision[PropertyNames.CreatedOn]);

            Assert.IsNull((string) fileRevision[PropertyNames.ContainingFolder]);
            Assert.AreEqual("284334dd-e8e5-42d6-bc8a-715c507a7f02", (string) fileRevision[PropertyNames.Creator]);
            Assert.AreEqual("B95EC201AE3EE89D407449D692E69BB97C228A7E", (string) fileRevision[PropertyNames.ContentHash]);

            var expectedFileTypes = new List<OrderedItem>
            {
                new OrderedItem(2, "db04ac55-dd60-4607-a4e1-a9f91c9704e6")
            };
            var fileTypesArray = JsonConvert.DeserializeObject<List<OrderedItem>>(
                fileRevision[PropertyNames.FileType].ToString());
            CollectionAssert.AreEquivalent(expectedFileTypes, fileTypesArray);
        }
    }
}
