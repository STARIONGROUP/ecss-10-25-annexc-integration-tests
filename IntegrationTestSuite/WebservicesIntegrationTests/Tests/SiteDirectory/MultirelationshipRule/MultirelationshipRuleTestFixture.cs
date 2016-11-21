// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultirelationshipRuleTestFixture.cs" company="RHEA System">
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
    public class MultirelationshipRuleTestFixture : WebClientTestFixtureBase
    {
        /// <summary>
        /// Verification that the MultirelationshipRule objects are returned from the data-source and that the 
        /// values of the MultirelationshipRule properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedRuleIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var multirelationshipRuleRuleUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/rule/2615f9ec-30a4-4c0e-a9d3-1d067959c248"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(multirelationshipRuleRuleUri);

            //check if there is the only one MultirelationshipRule object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific MultirelationshipRule from the result by it's unique id
            var multirelationshipRule =
                jArray.Single(x => (string) x["iid"] == "2615f9ec-30a4-4c0e-a9d3-1d067959c248");

            MultirelationshipRuleTestFixture.VerifyProperties(multirelationshipRule);
        }

        [Test]
        public void VerifyThatExpectedRuleWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var multirelationshipRuleUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/rule/2615f9ec-30a4-4c0e-a9d3-1d067959c248?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(multirelationshipRuleUri);

            //check if there are 3 objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary =
                jArray.Single(x => (string) x["iid"] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific MultirelationshipRule from the result by it's unique id
            var multirelationshipRule =
                jArray.Single(x => (string) x["iid"] == "2615f9ec-30a4-4c0e-a9d3-1d067959c248");
            MultirelationshipRuleTestFixture.VerifyProperties(multirelationshipRule);
        }

        /// <summary>
        /// Verifies all properties of the MultirelationshipRule <see cref="JToken"/>
        /// </summary>
        /// <param name="multirelationshipRule">
        /// The <see cref="JToken"/> that contains the properties of
        /// the MultirelationshipRule object
        /// </param>
        public static void VerifyProperties(JToken multirelationshipRule)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(13, multirelationshipRule.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("2615f9ec-30a4-4c0e-a9d3-1d067959c248", (string) multirelationshipRule["iid"]);
            Assert.AreEqual(1, (int) multirelationshipRule["revisionNumber"]);
            Assert.AreEqual("MultiRelationshipRule", (string) multirelationshipRule["classKind"]);

            Assert.IsFalse((bool) multirelationshipRule["isDeprecated"]);
            Assert.AreEqual("Test Multi Relationship Rule", (string) multirelationshipRule["name"]);
            Assert.AreEqual("TestMultiRelationshipRule", (string) multirelationshipRule["shortName"]);

            Assert.AreEqual("107fc408-7e6d-4f1a-895a-1b6a6025ac20",
                (string) multirelationshipRule["relationshipCategory"]);
            Assert.AreEqual(0, (int) multirelationshipRule["minRelated"]);
            Assert.AreEqual(-1, (int) multirelationshipRule["maxRelated"]);

            var expectedRelatedCategories = new string[]
            {
                "cf059b19-235c-48be-87a3-9a8942c8e3e0"
            };
            var relatedCategoriesArray = (JArray) multirelationshipRule["relatedCategory"];
            IList<string> relatedCategories = relatedCategoriesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedRelatedCategories, relatedCategories);

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) multirelationshipRule["alias"];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) multirelationshipRule["definition"];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) multirelationshipRule["hyperLink"];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);
        }
    }
}