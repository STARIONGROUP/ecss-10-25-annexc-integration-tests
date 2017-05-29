// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileTestFixture.cs" company="RHEA System">
//
//   Copyright 2017 RHEA System 
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

    using Newtonsoft.Json;

    using NUnit.Framework;
    using Newtonsoft.Json.Linq;

    [TestFixture]
    public class FileTestFixture : WebClientTestFixtureBase
    {
        public override void SetUp()
        {
            base.SetUp();

            this.WebClient.Restore(this.Settings.Hostname);
        }

        public override void TearDown()
        {
            this.WebClient.Restore(this.Settings.Hostname);

            base.TearDown();
        }

        /// <summary>
        /// Verification that the File objects are returned from the data-source and that the 
        /// values of the File properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedFileIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var fileUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/domainFileStore/da7dddaa-02aa-4897-9935-e8d66c811a96/file"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(fileUri);

            //check if there is the only one File object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific File from the result by it's unique id
            var file =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "95bf0f17-1273-4338-98ae-839016242775");
            FileTestFixture.VerifyProperties(file);
        }

        [Test]
        public void VerifyThatExpectedFileWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var fileUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/domainFileStore/da7dddaa-02aa-4897-9935-e8d66c811a96/file?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(fileUri);

            //check if there are 4 objects
            Assert.AreEqual(4, jArray.Count);

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
        }

        [Test]
        public void VerifyThatFileCanBeUploadedWithWebApi()
        {
            var iterationUri = new Uri(string.Format(UriFormat, this.Settings.Hostname, "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c"));
            var postJsonPath = this.GetPath("Tests/EngineeringModel/File/PostNewFile.json");
            var postFilePath = this.GetPath("Tests/EngineeringModel/File/B95EC201AE3EE89D407449D692E69BB97C228A7E");

            var jArray = this.WebClient.PostFile(iterationUri, postJsonPath, postFilePath);

            // check if there is a correct amount of objects
            Assert.AreEqual(4, jArray.Count);

            // get a specific EngineeeringModel from the result by it's unique id
            var engineeeringModel = jArray.Single(x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.AreEqual(2, (int)engineeeringModel[PropertyNames.RevisionNumber]);

            // get a specific CommonFileStore from the result by it's unique id
            var commonFileStore =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "8e5ca9cc-3da8-4e66-9172-7c3b2464a59c");
            Assert.AreEqual(2, (int)commonFileStore[PropertyNames.RevisionNumber]);

            // get a specific File from the result by it's unique id
            var file =
               jArray.Single(x => (string)x[PropertyNames.Iid] == "8ac6db3e-9525-4f3e-93ea-707076c07fc1");
            Assert.AreEqual(2, (int)commonFileStore[PropertyNames.RevisionNumber]);
            Assert.AreEqual("File", (string)file[PropertyNames.ClassKind]);

            Assert.IsNull((string)file[PropertyNames.LockedBy]);
            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string)file[PropertyNames.Owner]);

            var expectedCategories = new string[] { };
            var categoriesArray = (JArray)file[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedCategories, categories);

            var expectedFileRevisions = new string[]
            {
                "76e9b7fc-edc4-4ca3-89ba-eac014e7d9f8"
            };
            var fileRevisionsArray = (JArray)file[PropertyNames.FileRevision];
            IList<string> fileRevisions = fileRevisionsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedFileRevisions, fileRevisions);

            // get a specific FileRevision from the result by it's unique id
            var fileRevision =
               jArray.Single(x => (string)x[PropertyNames.Iid] == "76e9b7fc-edc4-4ca3-89ba-eac014e7d9f8");
            Assert.AreEqual(2, (int)fileRevision[PropertyNames.RevisionNumber]);
            Assert.AreEqual("FileRevision", (string)fileRevision[PropertyNames.ClassKind]);
            Assert.AreEqual("FileTest", (string)fileRevision[PropertyNames.Name]);

            Assert.IsNull((string)fileRevision[PropertyNames.ContainingFolder]);
            Assert.AreEqual("284334dd-e8e5-42d6-bc8a-715c507a7f02", (string)fileRevision[PropertyNames.Creator]);
            Assert.AreEqual("B95EC201AE3EE89D407449D692E69BB97C228A7E", (string)fileRevision[PropertyNames.ContentHash]);

            var expectedFileTypes = new List<OrderedItem>
            {
                new OrderedItem(3177, "b16894e4-acb5-4e81-a118-16c00eb86d8f")
            };
            var fileTypesArray = JsonConvert.DeserializeObject<List<OrderedItem>>(
                fileRevision[PropertyNames.FileType].ToString());
            CollectionAssert.AreEquivalent(expectedFileTypes, fileTypesArray);
        }

        /// <summary>
        /// Verifies all properties of the File <see cref="JToken"/>
        /// </summary>
        /// <param name="file">
        /// The <see cref="JToken"/> that contains the properties of
        /// the File object
        /// </param>
        public static void VerifyProperties(JToken file)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(7, file.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("95bf0f17-1273-4338-98ae-839016242775", (string) file[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) file[PropertyNames.RevisionNumber]);
            Assert.AreEqual("File", (string) file[PropertyNames.ClassKind]);

            Assert.IsNull((string) file[PropertyNames.LockedBy]);
            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string) file[PropertyNames.Owner]);

            var expectedCategories = new string[] {};
            var categoriesArray = (JArray) file[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedCategories, categories);

            var expectedFileRevisions = new string[]
            {
                "5544bb87-dc38-45d5-9d92-c580d3fe0442"
            };
            var fileRevisionsArray = (JArray) file[PropertyNames.FileRevision];
            IList<string> fileRevisions = fileRevisionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedFileRevisions, fileRevisions);
        }
    }
}