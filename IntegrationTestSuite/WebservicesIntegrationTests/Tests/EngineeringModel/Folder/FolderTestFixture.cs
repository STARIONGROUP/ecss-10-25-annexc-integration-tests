// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FolderTestFixture.cs" company="Starion Group S.A.">
//
//   Copyright 2016-2025 Starion Group S.A.
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
    using System.Linq;
    using System.Net;

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    using WebservicesIntegrationTests.Net;

    [TestFixture]
    public class FolderTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedFolderIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var folderUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/domainFileStore/da7dddaa-02aa-4897-9935-e8d66c811a96/folder");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(folderUri);

            //check if there is the only one Folder object 
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific Folder from the result by it's unique id
            var folder =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "67cdb7de-7721-40a0-9ca2-10a5cf7742fc");

            VerifyProperties(folder);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedFolderWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var folderUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/domainFileStore/da7dddaa-02aa-4897-9935-e8d66c811a96/folder?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(folderUri);

            //check if there are 4 objects
            Assert.That(jArray.Count, Is.EqualTo(4));

            // get a specific Iteration from the result by it's unique id
            var iteration =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");

            IterationTestFixture.VerifyProperties(iteration);

            // get a specific DomainFileStore from the result by it's unique id
            var domainFileStore =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "da7dddaa-02aa-4897-9935-e8d66c811a96");

            DomainFileStoreTestFixture.VerifyProperties(domainFileStore);

            // get a specific Folder from the result by it's unique id
            var folder =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "67cdb7de-7721-40a0-9ca2-10a5cf7742fc");

            VerifyProperties(folder);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatFolderCanBeAddedWithWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postJsonPath = this.GetPath("Tests/EngineeringModel/Folder/PostNewFolder.json");
            var postBody = this.GetJsonFromFile(postJsonPath);

            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            Assert.That(jArray.Count, Is.EqualTo(3));

            // get a specific EngineeeringModel from the result by it's unique id
            var engineeeringModel = jArray.Single(x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.That((int)engineeeringModel[PropertyNames.RevisionNumber], Is.EqualTo(2));

            // get a specific CommonFileStore from the result by it's unique id
            var domainFileStore = jArray.Single(x => (string)x[PropertyNames.Iid] == "da7dddaa-02aa-4897-9935-e8d66c811a96");

            Assert.That((int)domainFileStore[PropertyNames.RevisionNumber], Is.EqualTo(2));

            // get a specific Folder from the result by it's unique id
            var folder = jArray.Single(x => (string)x[PropertyNames.Iid] == "e80daca0-5c6e-4236-ae34-d23c36244059");

            Assert.Multiple(() =>
            {
                Assert.That((int)folder[PropertyNames.RevisionNumber], Is.EqualTo(2));
                Assert.That((string)folder[PropertyNames.ClassKind], Is.EqualTo("Folder"));
            });

            //Check CreatedOn for raw 10-25 (non CDP extension) data
            var newCreatedOn = folder[PropertyNames.CreatedOn];
            var newModifiedOn = folder[PropertyNames.ModifiedOn];

            Assert.Multiple(() =>
            {
                Assert.That(newCreatedOn, Is.Not.Null);
                Assert.That(newModifiedOn, Is.Null);
            });
        }

        [Test]
        [Category("POST")]
        [CdpVersion_1_1_0]
        public void VerifyModifiedOnAndCreatedOnForNewAndUpdatedFolder()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postJsonPath = this.GetPath("Tests/EngineeringModel/Folder/PostNewFolder.json");
            var postBody = this.GetJsonFromFile(postJsonPath);

            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            Assert.That(jArray.Count, Is.EqualTo(3));

            // get a specific Folder from the result by it's unique id
            var folder = jArray.Single(x => (string)x[PropertyNames.Iid] == "e80daca0-5c6e-4236-ae34-d23c36244059");

            Assert.Multiple(() =>
            {
                Assert.That((int)folder[PropertyNames.RevisionNumber], Is.EqualTo(2));
                Assert.That((string)folder[PropertyNames.ClassKind], Is.EqualTo("Folder"));
            });

            var newCreatedOn = folder[PropertyNames.CreatedOn];
            var newModifiedOn = folder[PropertyNames.ModifiedOn];

            Assert.Multiple(() =>
            {
                Assert.That(newCreatedOn, Is.Not.Null);
                Assert.That(newModifiedOn, Is.Not.Null);
                Assert.That(newCreatedOn, Is.EqualTo(newModifiedOn));
            });

            postJsonPath = this.GetPath("Tests/EngineeringModel/Folder/UpdateNewFolder.json");
            postBody = this.GetJsonFromFile(postJsonPath);

            jArray = this.WebClient.PostDto(iterationUri, postBody);

            folder = jArray.Single(x => (string)x[PropertyNames.Iid] == "e80daca0-5c6e-4236-ae34-d23c36244059");

            Assert.Multiple(() =>
            {
                Assert.That((int)folder[PropertyNames.RevisionNumber], Is.EqualTo(3));
                Assert.That((string)folder[PropertyNames.ClassKind], Is.EqualTo("Folder"));
            });

            var updateCreatedOn = folder[PropertyNames.CreatedOn];
            var updateModifiedOn = folder[PropertyNames.ModifiedOn];

            Assert.Multiple(() =>
            {
                Assert.That(updateCreatedOn, Is.Not.Null);
                Assert.That(updateModifiedOn, Is.Not.Null);
                Assert.That(updateCreatedOn, Is.Not.EqualTo(updateModifiedOn));

                Assert.That(newCreatedOn, Is.Not.EqualTo(updateModifiedOn));
                Assert.That(updateModifiedOn, Is.Not.EqualTo(newModifiedOn));
                Assert.That(newCreatedOn, Is.EqualTo(updateCreatedOn));
            });
        }

        [Test]
        [Category("POST")]
        public void VerifyThatFolderCannotBeAddedWithWebApiWhenParticipantIsNotAnOwner()
        {
            SiteDirectoryTestFixture.AddDomainExpertUserJane(this, out var userName, out var passWord);
            this.CreateNewWebClientForUser(userName, passWord);

            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postJsonPath = this.GetPath("Tests/EngineeringModel/Folder/PostNewFolder.json");
            var postBody = this.GetJsonFromFile(postJsonPath);

            // Jane is not allowed to upload
            Assert.That(() => this.WebClient.PostDto(iterationUri, postBody), Throws.TypeOf<WebException>());
        }

        /// <summary>
        /// Verifies all properties of the Folder <see cref="JToken"/>
        /// </summary>
        /// <param name="folder">
        /// The <see cref="JToken"/> that contains the properties of
        /// the Folder object
        /// </param>
        public static void VerifyProperties(JToken folder)
        {
            Assert.Multiple(() =>
            {
                // verify the amount of returned properties 
                Assert.That(folder.Children().Count(), Is.EqualTo(8));

                // assert that the properties are what is expected
                Assert.That((string)folder[PropertyNames.Iid], Is.EqualTo("67cdb7de-7721-40a0-9ca2-10a5cf7742fc"));
                Assert.That((int)folder[PropertyNames.RevisionNumber], Is.EqualTo(1));
                Assert.That((string)folder[PropertyNames.ClassKind], Is.EqualTo("Folder"));

                Assert.That((string)folder[PropertyNames.Name], Is.EqualTo("Test Folder"));
                Assert.That((string)folder[PropertyNames.Owner], Is.EqualTo("0e92edde-fdff-41db-9b1d-f2e484f12535"));

                Assert.That((string)folder[PropertyNames.CreatedOn], Is.EqualTo("2016-11-02T13:58:35.936Z"));
                Assert.That((string)folder[PropertyNames.Creator], Is.EqualTo("284334dd-e8e5-42d6-bc8a-715c507a7f02"));
                Assert.That((string)folder[PropertyNames.ContainingFolder], Is.Null);
            });
        }
    }
}
