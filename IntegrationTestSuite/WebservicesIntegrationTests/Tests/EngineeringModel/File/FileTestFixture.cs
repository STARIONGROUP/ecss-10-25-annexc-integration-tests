// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileTestFixture.cs" company="RHEA System">
//
//   Copyright 2016 RHEA System 
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
    using NUnit.Framework;
    using Newtonsoft.Json.Linq;

    [TestFixture]
    public class FileTestFixture : WebClientTestFixtureBase
    {
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