// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DomainFileStoreTestFixture.cs" company="Starion Group S.A.">
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

namespace WebservicesIntegrationTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    
    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class DomainFileStoreTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedDomainFileStoreIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var domainFileStoreUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/domainFileStore");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(domainFileStoreUri);

            //check if there is the only one DomainFileStore object 
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific DomainFileStore from the result by it's unique id
            var domainFileStore = jArray.Single(x => (string) x[PropertyNames.Iid] == "da7dddaa-02aa-4897-9935-e8d66c811a96");
            DomainFileStoreTestFixture.VerifyProperties(domainFileStore);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedDomainFileStoreWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var domainFileStoreUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/domainFileStore?includeAllContainers=true");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(domainFileStoreUri);

            //check if there are 3 objects
            Assert.That(jArray.Count, Is.EqualTo(3));

            // get a specific Iteration from the result by it's unique id
            var iteration = jArray.Single(x => (string) x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific DomainFileStore from the result by it's unique id
            var domainFileStore = jArray.Single(x => (string) x[PropertyNames.Iid] == "da7dddaa-02aa-4897-9935-e8d66c811a96");
            DomainFileStoreTestFixture.VerifyProperties(domainFileStore);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatIsHiddenWorks()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/DomainFileStore/PostUpdateDomainFileStoreIsHidden.json");

            var postBody = base.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            // define the URI on which to perform a domainFileStore GET request
            var domainFileStoreUri =
                new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/domainFileStore?extent=deep");

            // define the URI on which to perform a file GET request
            var fileUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/domainFileStore/da7dddaa-02aa-4897-9935-e8d66c811a96/file?extent=deep");

            // define the URI on which to perform a folder GET request
            var folderUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/domainFileStore/da7dddaa-02aa-4897-9935-e8d66c811a96/folder?extent=deep");

            // define the URI on which to perform a fileRevision GET request
            var fileRevisionUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/domainFileStore/da7dddaa-02aa-4897-9935-e8d66c811a96/file/95bf0f17-1273-4338-98ae-839016242775/fileRevision");

            // get a response from the data-source as a JArray (JSON Array)
            jArray = this.WebClient.GetDto(domainFileStoreUri);
            Assert.That(jArray.Count, Is.EqualTo(4));

            // get a specific DomainFileStore from the result by it's unique id
            var domainFileStore = jArray.Single(x => (string)x[PropertyNames.Iid] == "da7dddaa-02aa-4897-9935-e8d66c811a96");

            var file = jArray.Single(x => (string)x[PropertyNames.Iid] == "95bf0f17-1273-4338-98ae-839016242775");

            var folder = jArray.Single(x => (string)x[PropertyNames.Iid] == "67cdb7de-7721-40a0-9ca2-10a5cf7742fc");

            var fileRevision = jArray.Single(x => (string)x[PropertyNames.Iid] == "5544bb87-dc38-45d5-9d92-c580d3fe0442");

            Assert.That((string)domainFileStore[PropertyNames.IsHidden], Is.EqualTo("True"));

            jArray = this.WebClient.GetDto(fileUri);
            Assert.That(jArray.Count, Is.EqualTo(2));

            jArray = this.WebClient.GetDto(folderUri);
            Assert.That(jArray.Count, Is.EqualTo(1));

            jArray = this.WebClient.GetDto(fileRevisionUri);
            Assert.That(jArray.Count, Is.EqualTo(1));

            SiteDirectoryTestFixture.AddDomainExpertUserJane(this, out var userName, out var passWord);
            this.CreateNewWebClientForUser(userName, passWord);

            // Jane is not allowed to read it
            Assert.Throws<WebException>(() => this.WebClient.GetDto(domainFileStoreUri));

            // Jane is not allowed to read it
            Assert.Throws<WebException>(() => this.WebClient.GetDto(fileUri));

            // Jane is not allowed to read it
            Assert.Throws<WebException>(() => this.WebClient.GetDto(folderUri));

            // Jane is not allowed to read it
            Assert.Throws<WebException>(() => this.WebClient.GetDto(fileRevisionUri));

            jArray = this.WebClient.GetDto(iterationUri);

            domainFileStore = jArray.SingleOrDefault(x => (string)x[PropertyNames.Iid] == "da7dddaa-02aa-4897-9935-e8d66c811a96");

            file = jArray.SingleOrDefault(x => (string)x[PropertyNames.Iid] == "95bf0f17-1273-4338-98ae-839016242775");

            folder = jArray.SingleOrDefault(x => (string)x[PropertyNames.Iid] == "67cdb7de-7721-40a0-9ca2-10a5cf7742fc");

            fileRevision = jArray.SingleOrDefault(x => (string)x[PropertyNames.Iid] == "5544bb87-dc38-45d5-9d92-c580d3fe0442");

            Assert.That(domainFileStore, Is.Null);
            Assert.That(file, Is.Null);
            Assert.That(folder, Is.Null);
            Assert.That(fileRevision, Is.Null);
        }

        /// <summary>
        /// Verifies all properties of the DomainFileStore <see cref="JToken"/>
        /// </summary>
        /// <param name="domainFileStore">
        /// The <see cref="JToken"/> that contains the properties of
        /// the DomainFileStore object
        /// </param>
        public static void VerifyProperties(JToken domainFileStore)
        {
            // verify the amount of returned properties 
            Assert.That(domainFileStore.Children().Count(), Is.EqualTo(9));

            // assert that the properties are what is expected
            Assert.That((string)domainFileStore[PropertyNames.Iid], Is.EqualTo("da7dddaa-02aa-4897-9935-e8d66c811a96"));
            Assert.That((int)domainFileStore[PropertyNames.RevisionNumber], Is.EqualTo(1));
            Assert.That((string)domainFileStore[PropertyNames.ClassKind], Is.EqualTo("DomainFileStore"));

            Assert.That((string)domainFileStore[PropertyNames.Name], Is.EqualTo("Test DomainFileStore"));
            Assert.That((string)domainFileStore[PropertyNames.Owner], Is.EqualTo("0e92edde-fdff-41db-9b1d-f2e484f12535"));

            Assert.That((string)domainFileStore[PropertyNames.CreatedOn], Is.EqualTo("2016-11-02T13:58:35.936Z"));
            Assert.That((bool) domainFileStore[PropertyNames.IsHidden], Is.False);

            var expectedFiles = new string[]
            {
                "95bf0f17-1273-4338-98ae-839016242775"
            };

            var filesArray = (JArray) domainFileStore[PropertyNames.File];
            IList<string> files = filesArray.Select(x => (string) x).ToList();
            Assert.That(files, Is.EquivalentTo(expectedFiles));

            var expectedFolders = new string[]
            {
                "67cdb7de-7721-40a0-9ca2-10a5cf7742fc"
            };

            var foldersArray = (JArray) domainFileStore[PropertyNames.Folder];
            IList<string> folders = foldersArray.Select(x => (string) x).ToList();
            
            Assert.That(folders, Is.EquivalentTo(expectedFolders));
        }
    }
}
