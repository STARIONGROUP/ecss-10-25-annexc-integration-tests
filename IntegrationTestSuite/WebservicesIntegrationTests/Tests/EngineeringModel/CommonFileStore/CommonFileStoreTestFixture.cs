// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommonFileStoreTestFixture.cs" company="Starion Group S.A.">
//
//   Copyright 2016-2023 Starion Group S.A.
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

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class CommonFileStoreTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedCommonFileStoreIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var commonFileStoreUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/commonFileStore");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(commonFileStoreUri);

            //check if there is the only one CommonFileStore object 
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific CommonFileStore from the result by it's unique id
            var commonFileStore =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "8e5ca9cc-3da8-4e66-9172-7c3b2464a59c");

            CommonFileStoreTestFixture.VerifyProperties(commonFileStore);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedCommonFileStoreWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var commonFileStoreUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/commonFileStore?includeAllContainers=true");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(commonFileStoreUri);

            //check if there are 2 objects
            Assert.That(jArray.Count, Is.EqualTo(2));

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
            Assert.That(commonFileStore.Children().Count(), Is.EqualTo(8));

            // assert that the properties are what is expected
            Assert.That((string)commonFileStore[PropertyNames.Iid], Is.EqualTo("8e5ca9cc-3da8-4e66-9172-7c3b2464a59c"));
            Assert.That((int)commonFileStore[PropertyNames.RevisionNumber], Is.EqualTo(1));
            Assert.That((string)commonFileStore[PropertyNames.ClassKind], Is.EqualTo("CommonFileStore"));

            Assert.That((string)commonFileStore[PropertyNames.Owner], Is.EqualTo("0e92edde-fdff-41db-9b1d-f2e484f12535"));
            Assert.That((string)commonFileStore[PropertyNames.CreatedOn], Is.EqualTo("2016-10-19T09:30:36.186Z"));
            Assert.That((string)commonFileStore[PropertyNames.Name], Is.EqualTo("TestFileStore"));

            var expectedFiles = new string[]
            {
                "95bf0f17-1273-4338-98ae-839016242776"
            };

            var filesArray = (JArray) commonFileStore[PropertyNames.File];
            IList<string> files = filesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedFiles, files);

            var expectedFolders = new string[]
            {
                "67cdb7de-7721-40a0-9ca2-10a5cf7742fd"
            };

            var foldersArray = (JArray) commonFileStore[PropertyNames.Folder];
            IList<string> folders = foldersArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedFolders, folders);
        }
    }
}
