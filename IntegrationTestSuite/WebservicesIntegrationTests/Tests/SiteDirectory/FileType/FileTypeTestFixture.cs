// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileTypeTestFixture.cs" company="RHEA System S.A.">
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

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class FileTypeTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedFileTypeIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var fileTypeUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/fileType");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(fileTypeUri);

            //check if there are correct amount of FileType objects 
            Assert.AreEqual(3, jArray.Count);

            FileTypeTestFixture.VerifyProperties(jArray);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedFileTypeWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var fileTypeUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/fileType?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(fileTypeUri);

            //check if there are correct amount of objects
            Assert.AreEqual(5, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string)x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary = jArray.Single(x => (string)x[PropertyNames.Iid] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            FileTypeTestFixture.VerifyProperties(jArray);
        }

        /// <summary>
        /// Verifies all properties of the FileType <see cref="JToken"/>
        /// </summary>
        /// <param name="jArray">
        /// The <see cref="JToken"/> that contains the properties of
        /// the FileType object
        /// </param>
        public static void VerifyProperties(JToken jArray)
        {
            // get a specific FileType from the result by it's unique id
            var fileType = jArray.Single(x => (string)x[PropertyNames.Iid] == "db04ac55-dd60-4607-a4e1-a9f91c9704e6");
            // verify the amount of returned properties 
            Assert.AreEqual(12, fileType.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("db04ac55-dd60-4607-a4e1-a9f91c9704e6", (string)fileType[PropertyNames.Iid]);
            Assert.AreEqual(1, (int)fileType[PropertyNames.RevisionNumber]);
            Assert.AreEqual("FileType", (string)fileType[PropertyNames.ClassKind]);

            Assert.IsFalse((bool)fileType[PropertyNames.IsDeprecated]);
            Assert.AreEqual("Test File Type", (string)fileType[PropertyNames.Name]);
            Assert.AreEqual("TestFileType", (string)fileType[PropertyNames.ShortName]);

            Assert.AreEqual("tst", (string)fileType[PropertyNames.Extension]);
           
            var expectedCategories = new string[] {};
            var categoriesArray = (JArray)fileType[PropertyNames.Category];
            IList<string> containedCategories = categoriesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedCategories, containedCategories);

            var expectedAliases = new string[] { };
            var aliasesArray = (JArray)fileType[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] { };
            var definitionsArray = (JArray)fileType[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] { };
            var hyperlinksArray = (JArray)fileType[PropertyNames.HyperLink];
            IList<string> h = hyperlinksArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);

            // get a specific FileType from the result by it's unique id
            fileType = jArray.Single(x => (string)x[PropertyNames.Iid] == "b16894e4-acb5-4e81-a118-16c00eb86d8f");
            // verify the amount of returned properties 
            Assert.AreEqual(12, fileType.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("b16894e4-acb5-4e81-a118-16c00eb86d8f", (string)fileType[PropertyNames.Iid]);
            Assert.AreEqual(1, (int)fileType[PropertyNames.RevisionNumber]);
            Assert.AreEqual("FileType", (string)fileType[PropertyNames.ClassKind]);

            Assert.IsFalse((bool)fileType[PropertyNames.IsDeprecated]);
            Assert.AreEqual("Test Text Type", (string)fileType[PropertyNames.Name]);
            Assert.AreEqual("Text", (string)fileType[PropertyNames.ShortName]);

            Assert.AreEqual("txt", (string)fileType[PropertyNames.Extension]);

            expectedCategories = new string[] { };
            categoriesArray = (JArray)fileType[PropertyNames.Category];
            containedCategories = categoriesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedCategories, containedCategories);

            expectedAliases = new string[] { };
            aliasesArray = (JArray)fileType[PropertyNames.Alias];
            aliases = aliasesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            expectedDefinitions = new string[] { };
            definitionsArray = (JArray)fileType[PropertyNames.Definition];
            definitions = definitionsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            expectedHyperlinks = new string[] { };
            hyperlinksArray = (JArray)fileType[PropertyNames.HyperLink];
            h = hyperlinksArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);

            // get a specific FileType from the result by it's unique id
            fileType =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "f340df66-d65b-4814-a063-01d4dea1941c");
            // verify the amount of returned properties 
            Assert.AreEqual(12, fileType.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("f340df66-d65b-4814-a063-01d4dea1941c", (string)fileType[PropertyNames.Iid]);
            Assert.AreEqual(1, (int)fileType[PropertyNames.RevisionNumber]);
            Assert.AreEqual("FileType", (string)fileType[PropertyNames.ClassKind]);

            Assert.IsFalse((bool)fileType[PropertyNames.IsDeprecated]);
            Assert.AreEqual("Test Unknown Type", (string)fileType[PropertyNames.Name]);
            Assert.AreEqual("Unknown", (string)fileType[PropertyNames.ShortName]);

            Assert.AreEqual("?", (string)fileType[PropertyNames.Extension]);

            expectedCategories = new string[] { };
            categoriesArray = (JArray)fileType[PropertyNames.Category];
            containedCategories = categoriesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedCategories, containedCategories);

            expectedAliases = new string[] { };
            aliasesArray = (JArray)fileType[PropertyNames.Alias];
            aliases = aliasesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            expectedDefinitions = new string[] { };
            definitionsArray = (JArray)fileType[PropertyNames.Definition];
            definitions = definitionsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            expectedHyperlinks = new string[] { };
            hyperlinksArray = (JArray)fileType[PropertyNames.HyperLink];
            h = hyperlinksArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);
        }
    }
}
