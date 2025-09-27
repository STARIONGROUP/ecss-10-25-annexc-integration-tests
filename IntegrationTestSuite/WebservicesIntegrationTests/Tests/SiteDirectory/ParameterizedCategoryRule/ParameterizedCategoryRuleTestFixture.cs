// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterizedCategoryRuleTestFixture.cs" company="Starion Group S.A.">
//
//   Copyright 2016-2021 Starion Group S.A.
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
    public class ParameterizedCategoryRuleTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedRuleIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var parameterizedCategoryRuleUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/rule/7a6186ca-10c1-4074-bec1-4a92ce6ae59d");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(parameterizedCategoryRuleUri);

            //check if there is the only one ParameterizedCategoryRule object 
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific ParameterizedCategoryRule from the result by it's unique id
            var parameterizedCategoryRule = jArray.Single(x => (string) x["iid"] == "7a6186ca-10c1-4074-bec1-4a92ce6ae59d");

            ParameterizedCategoryRuleTestFixture.VerifyProperties(parameterizedCategoryRule);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedRuleWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var parameterizedCategoryRuleUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/rule/7a6186ca-10c1-4074-bec1-4a92ce6ae59d?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(parameterizedCategoryRuleUri);

            //check if there are 3 objects
            Assert.That(jArray.Count, Is.EqualTo(3));

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary = jArray.Single(x => (string) x["iid"] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific ParameterizedCategoryRule from the result by it's unique id
            var parameterizedCategoryRule = jArray.Single(x => (string) x["iid"] == "7a6186ca-10c1-4074-bec1-4a92ce6ae59d");
            ParameterizedCategoryRuleTestFixture.VerifyProperties(parameterizedCategoryRule);
        }

        /// <summary>
        /// Verifies all properties of the ParameterizedCategoryRule <see cref="JToken"/>
        /// </summary>
        /// <param name="parameterizedCategoryRule">
        /// The <see cref="JToken"/> that contains the properties of
        /// the ParameterizedCategoryRule object
        /// </param>
        public static void VerifyProperties(JToken parameterizedCategoryRule)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(11, parameterizedCategoryRule.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("7a6186ca-10c1-4074-bec1-4a92ce6ae59d", (string) parameterizedCategoryRule["iid"]);
            Assert.AreEqual(1, (int) parameterizedCategoryRule["revisionNumber"]);
            Assert.AreEqual("ParameterizedCategoryRule", (string) parameterizedCategoryRule["classKind"]);

            Assert.IsFalse((bool) parameterizedCategoryRule["isDeprecated"]);
            Assert.AreEqual("TestParameterizedCategoryRule", (string) parameterizedCategoryRule["name"]);
            Assert.AreEqual("Test Parameterized Category Rule", (string) parameterizedCategoryRule["shortName"]);

            Assert.AreEqual("107fc408-7e6d-4f1a-895a-1b6a6025ac20",
                (string) parameterizedCategoryRule["category"]);

            var expectedParameterTypes = new string[]
            {
                "35a9cf05-4eba-4cda-b60c-7cfeaac8f892",
                "33cf1171-3cd2-4494-8d54-639bfc583155"
            };
            var parameterTypesArray = (JArray) parameterizedCategoryRule["parameterType"];
            IList<string> containedCategories = parameterTypesArray.Select(x => (string) x).ToList();
            Assert.That(containedCategories, Is.EquivalentTo(expectedParameterTypes));

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) parameterizedCategoryRule["alias"];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            Assert.That(aliases, Is.EquivalentTo(expectedAliases));

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) parameterizedCategoryRule["definition"];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            Assert.That(definitions, Is.EquivalentTo(expectedDefinitions));

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) parameterizedCategoryRule["hyperLink"];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            Assert.That(h, Is.EquivalentTo(expectedHyperlinks));
        }
    }
}
