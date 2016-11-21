// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileTypeTestFixture.cs" company="RHEA System">
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
    public class FileTypeTestFixture : WebClientTestFixtureBase
    {
        /// <summary>
        /// Verification that the FileType objects are returned from the data-source and that the 
        /// values of the FileType properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedFileTypeIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var fileTypeUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/fileType"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(fileTypeUri);

            //check if there is the only one FileType object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific FileType from the result by it's unique id
            var fileType =
                jArray.Single(x => (string)x["iid"] == "db04ac55-dd60-4607-a4e1-a9f91c9704e6");

            FileTypeTestFixture.VerifyProperties(fileType);
        }

        [Test]
        public void VerifyThatExpectedFileTypeWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var fileTypeUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/fileType?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(fileTypeUri);

            //check if there are 3 objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string)x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary =
                jArray.Single(x => (string)x["iid"] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific FileType from the result by it's unique id
            var fileType =
                jArray.Single(x => (string)x["iid"] == "db04ac55-dd60-4607-a4e1-a9f91c9704e6");
            FileTypeTestFixture.VerifyProperties(fileType);
        }

        /// <summary>
        /// Verifies all properties of the FileType <see cref="JToken"/>
        /// </summary>
        /// <param name="fileType">
        /// The <see cref="JToken"/> that contains the properties of
        /// the FileType object
        /// </param>
        public static void VerifyProperties(JToken fileType)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(11, fileType.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("db04ac55-dd60-4607-a4e1-a9f91c9704e6", (string)fileType["iid"]);
            Assert.AreEqual(1, (int)fileType["revisionNumber"]);
            Assert.AreEqual("FileType", (string)fileType["classKind"]);

            Assert.IsFalse((bool)fileType["isDeprecated"]);
            Assert.AreEqual("Test File Type", (string)fileType["name"]);
            Assert.AreEqual("TestFileType", (string)fileType["shortName"]);

            Assert.AreEqual("tst", (string)fileType["extension"]);
           
            var expectedCategories = new string[] {};
            var categoriesArray = (JArray)fileType["category"];
            IList<string> containedCategories = categoriesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedCategories, containedCategories);

            var expectedAliases = new string[] { };
            var aliasesArray = (JArray)fileType["alias"];
            IList<string> aliases = aliasesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] { };
            var definitionsArray = (JArray)fileType["definition"];
            IList<string> definitions = definitionsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] { };
            var hyperlinksArray = (JArray)fileType["hyperLink"];
            IList<string> h = hyperlinksArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);
        }
    }
}
