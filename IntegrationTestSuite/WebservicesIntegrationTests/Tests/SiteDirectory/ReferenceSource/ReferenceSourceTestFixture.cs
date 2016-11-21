// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReferenceSourceTestFixture.cs" company="RHEA System">
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
    public class ReferenceSourceTestFixture : WebClientTestFixtureBase
    {
        /// <summary>
        /// Verification that the ReferenceSource objects are returned from the data-source and that the 
        /// values of the ReferenceSource properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedReferenceSourceIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var referenceSourceUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/referenceSource"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(referenceSourceUri);

            //check if there is the only one ReferenceSource object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific ReferenceSource from the result by it's unique id
            var referenceSource =
                jArray.Single(x => (string) x["iid"] == "ffd6c100-6c72-4d2a-8565-ff24bd576a89");

            ReferenceSourceTestFixture.VerifyProperties(referenceSource);
        }

        [Test]
        public void VerifyThatExpectedReferenceSourceWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var referenceSourceUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/referenceSource/ffd6c100-6c72-4d2a-8565-ff24bd576a89?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(referenceSourceUri);

            //check if there are 3 objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary =
                jArray.Single(x => (string) x["iid"] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific ReferenceSource from the result by it's unique id
            var referenceSource =
                jArray.Single(x => (string) x["iid"] == "ffd6c100-6c72-4d2a-8565-ff24bd576a89");
            ReferenceSourceTestFixture.VerifyProperties(referenceSource);
        }

        /// <summary>
        /// Verifies all properties of the ReferenceSource <see cref="JToken"/>
        /// </summary>
        /// <param name="referenceSource">
        /// The <see cref="JToken"/> that contains the properties of
        /// the ReferenceSource object
        /// </param>
        public static void VerifyProperties(JToken referenceSource)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(17, referenceSource.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("ffd6c100-6c72-4d2a-8565-ff24bd576a89", (string) referenceSource["iid"]);
            Assert.AreEqual(1, (int) referenceSource["revisionNumber"]);
            Assert.AreEqual("ReferenceSource", (string) referenceSource["classKind"]);

            Assert.IsFalse((bool) referenceSource["isDeprecated"]);
            Assert.AreEqual("Test Reference Source", (string) referenceSource["name"]);
            Assert.AreEqual("TestReferenceSource", (string) referenceSource["shortName"]);

            Assert.IsNull((string) referenceSource["versionIdentifier"]);
            Assert.IsNull((string) referenceSource["versionDate"]);
            Assert.AreEqual("", (string)referenceSource["author"]);            
            Assert.AreEqual(1999, (int) referenceSource["publicationYear"]);
            Assert.IsNull((string) referenceSource["publisher"]);
            Assert.IsNull((string) referenceSource["publishedIn"]);
            Assert.AreEqual("", (string)referenceSource["language"]);
            
            var expectedCategories = new string[] {};
            var categoriesArray = (JArray) referenceSource["category"];
            IList<string> categories = categoriesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedCategories, categories);

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) referenceSource["alias"];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) referenceSource["definition"];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) referenceSource["hyperLink"];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);
        }
    }
}