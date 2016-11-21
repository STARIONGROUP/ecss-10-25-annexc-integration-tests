// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryTestFixture.cs" company="RHEA System">
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
    public class CategoryTestFixture : WebClientTestFixtureBase
    {
        /// <summary>
        /// Verification that the Category objects are returned from the data-source and that the 
        /// values of the Category properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedCategoryIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var categoryUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/definedCategory"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(categoryUri);

            //check if there are two Category objects 
            Assert.AreEqual(3, jArray.Count);

            CategoryTestFixture.VerifyProperties(jArray);
        }

        [Test]
        public void VerifyThatExpectedCategoryWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var categoryUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/definedCategory?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(categoryUri);

            //check if there are 4 objects
            Assert.AreEqual(5, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary =
                jArray.Single(x => (string) x["iid"] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            CategoryTestFixture.VerifyProperties(jArray);
        }

        /// <summary>
        /// Verifies all properties of the Category <see cref="JToken"/>
        /// </summary>
        /// <param name="category">
        /// The <see cref="JToken"/> that contains the properties of
        /// the Category object
        /// </param>
        public static void VerifyProperties(JToken category)
        {
            // assert that all objects are what is expected
            var categoryObject =
                category.Single(x => (string) x["iid"] == "cf059b19-235c-48be-87a3-9a8942c8e3e0");
            Assert.AreEqual("cf059b19-235c-48be-87a3-9a8942c8e3e0", (string) categoryObject["iid"]);
            Assert.AreEqual(1, (int) categoryObject["revisionNumber"]);
            Assert.AreEqual("Category", (string) categoryObject["classKind"]);

            Assert.IsFalse((bool) categoryObject["isAbstract"]);
            Assert.IsFalse((bool) categoryObject["isDeprecated"]);
            Assert.AreEqual("Test Category", (string) categoryObject["name"]);
            Assert.AreEqual("TestCategory", (string) categoryObject["shortName"]);

            var expectedSuperCategories = new string[] {};
            var superCategoriesArray = (JArray) categoryObject["superCategory"];
            IList<string> superCategories = superCategoriesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedSuperCategories, superCategories);

            var expectedPermissibleClasses = new string[]
            {
                "ElementDefinition",
                "ElementUsage"
            };
            var permissibleClassesArray = (JArray) categoryObject["permissibleClass"];
            IList<string> permissibleClasses = permissibleClassesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedPermissibleClasses, permissibleClasses);

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) categoryObject["alias"];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) categoryObject["definition"];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) categoryObject["hyperLink"];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);

            //next object
            categoryObject =
                category.Single(x => (string) x["iid"] == "107fc408-7e6d-4f1a-895a-1b6a6025ac20");
            Assert.AreEqual("107fc408-7e6d-4f1a-895a-1b6a6025ac20", (string) categoryObject["iid"]);
            Assert.AreEqual(1, (int) categoryObject["revisionNumber"]);
            Assert.AreEqual("Category", (string) categoryObject["classKind"]);

            Assert.IsFalse((bool) categoryObject["isAbstract"]);
            Assert.IsFalse((bool) categoryObject["isDeprecated"]);
            Assert.AreEqual("Test Category - BinaryRelationship", (string) categoryObject["name"]);
            Assert.AreEqual("TestCategoryBinaryRelationship", (string) categoryObject["shortName"]);

            expectedSuperCategories = new string[] {};
            superCategoriesArray = (JArray) categoryObject["superCategory"];
            superCategories = superCategoriesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedSuperCategories, superCategories);

            expectedPermissibleClasses = new string[]
            {
                "BinaryRelationship"
            };
            permissibleClassesArray = (JArray) categoryObject["permissibleClass"];
            permissibleClasses = permissibleClassesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedPermissibleClasses, permissibleClasses);

            expectedAliases = new string[] {};
            aliasesArray = (JArray) categoryObject["alias"];
            aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            expectedDefinitions = new string[] {};
            definitionsArray = (JArray) categoryObject["definition"];
            definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            expectedHyperlinks = new string[] {};
            hyperlinksArray = (JArray) categoryObject["hyperLink"];
            h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);
        }
    }
}