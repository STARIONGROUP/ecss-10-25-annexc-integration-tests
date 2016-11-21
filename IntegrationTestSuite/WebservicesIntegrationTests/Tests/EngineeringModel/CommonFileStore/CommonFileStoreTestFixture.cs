// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommonFileStoreTestFixture.cs" company="RHEA System">
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
    public class CommonFileStoreTestFixture : WebClientTestFixtureBase
    {
        /// <summary>
        /// Verification that the CommonFileStore objects are returned from the data-source and that the 
        /// values of the CommonFileStore properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedCommonFileStoreIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var commonFileStoreUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/commonFileStore"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(commonFileStoreUri);

            //check if there is the only one CommonFileStore object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific CommonFileStore from the result by it's unique id
            var commonFileStore =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "8e5ca9cc-3da8-4e66-9172-7c3b2464a59c");

            CommonFileStoreTestFixture.VerifyProperties(commonFileStore);
        }

        [Test]
        public void VerifyThatExpectedCommonFileStoreWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var commonFileStoreUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/commonFileStore?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(commonFileStoreUri);

            //check if there are 2 objects
            Assert.AreEqual(2, jArray.Count);

            // get a specific CommonFileStore from the result by it's unique id
            var commonFileStore =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "8e5ca9cc-3da8-4e66-9172-7c3b2464a59c");
            CommonFileStoreTestFixture.VerifyProperties(commonFileStore);
        }

        /// <summary>
        /// Verifies all properties of the CommonFileStore <see cref="JToken"/>
        /// </summary>
        /// <param name="commonFileStore">
        /// The <see cref="JToken"/> that contains the properties of
        /// the CommonFileStore object
        /// </param>
        public static void VerifyProperties(JToken commonFileStore)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(8, commonFileStore.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("8e5ca9cc-3da8-4e66-9172-7c3b2464a59c", (string) commonFileStore[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) commonFileStore[PropertyNames.RevisionNumber]);
            Assert.AreEqual("CommonFileStore", (string) commonFileStore[PropertyNames.ClassKind]);

            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string) commonFileStore[PropertyNames.Owner]);
            Assert.AreEqual("2016-10-19T09:30:36.186Z", (string) commonFileStore[PropertyNames.CreatedOn]);
            Assert.AreEqual("TestFileStore", (string) commonFileStore[PropertyNames.Name]);

            var expectedFiles = new string[] {};
            var filesArray = (JArray) commonFileStore[PropertyNames.File];
            IList<string> files = filesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedFiles, files);

            var expectedFolders = new string[] {};
            var foldersArray = (JArray) commonFileStore[PropertyNames.Folder];
            IList<string> folders = foldersArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedFolders, folders);
        }
    }
}