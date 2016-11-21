// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DecompositionRuleTestFixture.cs" company="RHEA System">
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
    public class DecompositionRuleTestFixture : WebClientTestFixtureBase
    {
        /// <summary>
        /// Verification that the DecompositionRule objects are returned from the data-source and that the 
        /// values of the DecompositionRule properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedRuleIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var decompositionRuleUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/rule/8a5cd66e-7313-4843-813f-37081ca81bb8"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(decompositionRuleUri);

            //check if there is the only one DecompositionRule object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific DecompositionRule from the result by it's unique id
            var decompositionRule =
                jArray.Single(x => (string) x["iid"] == "8a5cd66e-7313-4843-813f-37081ca81bb8");

            DecompositionRuleTestFixture.VerifyProperties(decompositionRule);
        }

        [Test]
        public void VerifyThatExpectedRuleWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var decompositionRuleUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/rule/8a5cd66e-7313-4843-813f-37081ca81bb8?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(decompositionRuleUri);

            //check if there are 3 objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary =
                jArray.Single(x => (string) x["iid"] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific DecompositionRule from the result by it's unique id
            var decompositionRule =
                jArray.Single(x => (string) x["iid"] == "8a5cd66e-7313-4843-813f-37081ca81bb8");
            DecompositionRuleTestFixture.VerifyProperties(decompositionRule);
        }

        /// <summary>
        /// Verifies all properties of the DecompositionRule <see cref="JToken"/>
        /// </summary>
        /// <param name="decompositionRule">
        /// The <see cref="JToken"/> that contains the properties of
        /// the DecompositionRule object
        /// </param>
        public static void VerifyProperties(JToken decompositionRule)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(13, decompositionRule.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("8a5cd66e-7313-4843-813f-37081ca81bb8", (string) decompositionRule["iid"]);
            Assert.AreEqual(1, (int) decompositionRule["revisionNumber"]);
            Assert.AreEqual("DecompositionRule", (string) decompositionRule["classKind"]);

            Assert.IsFalse((bool) decompositionRule["isDeprecated"]);
            Assert.AreEqual("Test Decomposition Rule", (string) decompositionRule["name"]);
            Assert.AreEqual("TestDecompositionRule", (string) decompositionRule["shortName"]);

            Assert.AreEqual("cf059b19-235c-48be-87a3-9a8942c8e3e0",
                (string) decompositionRule["containingCategory"]);
            Assert.AreEqual(0, (int) decompositionRule["minContained"]);
            Assert.IsEmpty(decompositionRule["maxContained"]);

            var expectedContainedCategories = new string[]
            {
                "cf059b19-235c-48be-87a3-9a8942c8e3e0"
            };
            var containedCategoriesArray = (JArray) decompositionRule["containedCategory"];
            IList<string> containedCategories = containedCategoriesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedContainedCategories, containedCategories);

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) decompositionRule["alias"];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) decompositionRule["definition"];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) decompositionRule["hyperLink"];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);
        }
    }
}