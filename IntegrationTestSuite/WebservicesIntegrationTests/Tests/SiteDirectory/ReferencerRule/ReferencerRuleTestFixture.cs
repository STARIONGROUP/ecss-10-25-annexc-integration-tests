// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReferencerRuleTestFixture.cs" company="RHEA System S.A.">
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
    public class ReferencerRuleTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedRuleIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var referencerRuleUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/rule/e7e4eec5-ad39-40a0-9548-9c40d8e6df1b"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(referencerRuleUri);

            //check if there is the only one ReferencerRule object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific ReferencerRule from the result by it's unique id
            var referencerRule =
                jArray.Single(x => (string) x["iid"] == "e7e4eec5-ad39-40a0-9548-9c40d8e6df1b");

            ReferencerRuleTestFixture.VerifyProperties(referencerRule);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedRuleWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var referencerRuleUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/rule/e7e4eec5-ad39-40a0-9548-9c40d8e6df1b?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(referencerRuleUri);

            //check if there are 3 objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary =
                jArray.Single(x => (string) x["iid"] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific ReferencerRule from the result by it's unique id
            var referencerRule =
                jArray.Single(x => (string) x["iid"] == "e7e4eec5-ad39-40a0-9548-9c40d8e6df1b");
            ReferencerRuleTestFixture.VerifyProperties(referencerRule);
        }

        /// <summary>
        /// Verifies all properties of the ReferencerRule <see cref="JToken"/>
        /// </summary>
        /// <param name="referencerRule">
        /// The <see cref="JToken"/> that contains the properties of
        /// the ReferencerRule object
        /// </param>
        public static void VerifyProperties(JToken referencerRule)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(13, referencerRule.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("e7e4eec5-ad39-40a0-9548-9c40d8e6df1b", (string) referencerRule["iid"]);
            Assert.AreEqual(1, (int) referencerRule["revisionNumber"]);
            Assert.AreEqual("ReferencerRule", (string) referencerRule["classKind"]);

            Assert.IsFalse((bool) referencerRule["isDeprecated"]);
            Assert.AreEqual("TestReferencerRule", (string) referencerRule["name"]);
            Assert.AreEqual("Test Referencer Rule", (string) referencerRule["shortName"]);

            Assert.AreEqual("cf059b19-235c-48be-87a3-9a8942c8e3e0", (string) referencerRule["referencingCategory"]);
            Assert.AreEqual(0, (int) referencerRule["minReferenced"]);
            Assert.AreEqual(-1, (int) referencerRule["maxReferenced"]);

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) referencerRule["alias"];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) referencerRule["definition"];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) referencerRule["hyperLink"];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);

            var expectedReferencedCategories = new string[]
            {
                "cf059b19-235c-48be-87a3-9a8942c8e3e0"
            };
            var referencedCategoriesArray = (JArray) referencerRule["referencedCategory"];
            IList<string> referencedCategoriesList = referencedCategoriesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedReferencedCategories, referencedCategoriesList);
        }
    }
}
