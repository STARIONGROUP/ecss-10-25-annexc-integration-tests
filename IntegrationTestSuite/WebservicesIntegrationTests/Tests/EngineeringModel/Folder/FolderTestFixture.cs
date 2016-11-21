// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FolderTestFixture.cs" company="RHEA System">
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
    using Newtonsoft.Json;

    [TestFixture]
    public class FolderTestFixture : WebClientTestFixtureBase
    {
        /// <summary>
        /// Verification that the Folder objects are returned from the data-source and that the 
        /// values of the Folder properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedFolderIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var folderUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/domainFileStore/da7dddaa-02aa-4897-9935-e8d66c811a96/folder"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(folderUri);

            //check if there is the only one Folder object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific Folder from the result by it's unique id
            var folder =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "67cdb7de-7721-40a0-9ca2-10a5cf7742fc");
            FolderTestFixture.VerifyProperties(folder);
        }

        [Test]
        public void VerifyThatExpectedFolderWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var folderUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/domainFileStore/da7dddaa-02aa-4897-9935-e8d66c811a96/folder?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(folderUri);

            //check if there are 4 objects
            Assert.AreEqual(4, jArray.Count);

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
            FolderTestFixture.VerifyProperties(folder);
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
            // verify the amount of returned properties 
            Assert.AreEqual(8, folder.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("67cdb7de-7721-40a0-9ca2-10a5cf7742fc", (string)folder[PropertyNames.Iid]);
            Assert.AreEqual(1, (int)folder[PropertyNames.RevisionNumber]);
            Assert.AreEqual("Folder", (string)folder[PropertyNames.ClassKind]);

            Assert.AreEqual("Test Folder", (string)folder[PropertyNames.Name]);
            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string)folder[PropertyNames.Owner]);

            Assert.AreEqual("2016-11-02T13:58:35.936Z", (string)folder[PropertyNames.CreatedOn]);
            Assert.AreEqual("284334dd-e8e5-42d6-bc8a-715c507a7f02", (string)folder[PropertyNames.Creator]);
            Assert.IsNull((string)folder[PropertyNames.ContainingFolder]);
        }
    }
}
