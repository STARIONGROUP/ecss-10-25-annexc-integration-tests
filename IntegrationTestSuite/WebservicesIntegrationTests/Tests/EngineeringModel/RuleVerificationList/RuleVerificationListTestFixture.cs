// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RuleVerificationListTestFixture.cs" company="Starion Group S.A.">
//
//   Copyright 2016-2025 Starion Group S.A.
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

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class RuleVerificationListTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedRuleVerificationListIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var ruleVerificationListUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/ruleVerificationList");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(ruleVerificationListUri);

            //check if there is the only one RuleVerificationList object 
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific RuleVerificationList from the result by it's unique id
            var ruleVerificationList = jArray.Single(x => (string) x[PropertyNames.Iid] == "dc482120-2a11-439b-913d-6a924de9ee5f");
            RuleVerificationListTestFixture.VerifyProperties(ruleVerificationList);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedRuleVerificationListWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var ruleVerificationListUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/ruleVerificationList?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(ruleVerificationListUri);

            //check if there are 3 objects
            Assert.That(jArray.Count, Is.EqualTo(3));

            // get a specific Iteration from the result by it's unique id
            var iteration = jArray.Single(x => (string) x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific RuleVerificationList from the result by it's unique id
            var ruleVerificationList = jArray.Single(x => (string) x[PropertyNames.Iid] == "dc482120-2a11-439b-913d-6a924de9ee5f");
            RuleVerificationListTestFixture.VerifyProperties(ruleVerificationList);
        }

        /// <summary>
        /// Verifies all properties of the RuleVerificationList <see cref="JToken"/>
        /// </summary>
        /// <param name="ruleVerificationList">
        /// The <see cref="JToken"/> that contains the properties of
        /// the RuleVerificationList object
        /// </param>
        public static void VerifyProperties(JToken ruleVerificationList)
        {
            // verify the amount of returned properties 
            Assert.That(ruleVerificationList.Children().Count(), Is.EqualTo(10));

            // assert that the properties are what is expected
            Assert.That((string) ruleVerificationList[PropertyNames.Iid], Is.EqualTo("dc482120-2a11-439b-913d-6a924de9ee5f"));
            Assert.That((int) ruleVerificationList[PropertyNames.RevisionNumber], Is.EqualTo(1));
            Assert.That((string) ruleVerificationList[PropertyNames.ClassKind], Is.EqualTo("RuleVerificationList"));

            Assert.That((string) ruleVerificationList[PropertyNames.Owner], Is.EqualTo("0e92edde-fdff-41db-9b1d-f2e484f12535"));
            Assert.That((string) ruleVerificationList[PropertyNames.Name], Is.EqualTo("Test RuleVerificationList"));
            Assert.That((string) ruleVerificationList[PropertyNames.ShortName], Is.EqualTo("TestRuleVerificationList"));

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) ruleVerificationList[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            Assert.That(aliases, Is.EquivalentTo(expectedAliases));

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) ruleVerificationList[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            Assert.That(definitions, Is.EquivalentTo(expectedDefinitions));

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) ruleVerificationList[PropertyNames.HyperLink];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            Assert.That(h, Is.EquivalentTo(expectedHyperlinks));

            var expectedRuleVerifications = new List<OrderedItem>
            {
                new OrderedItem(1, "c953263c-fc25-4048-9bb6-343a10200a0c"),
                new OrderedItem(2, "ccb44a8e-2460-4892-ab75-02d8828db233")
            };
            var ruleVerificationsArray = JsonConvert.DeserializeObject<List<OrderedItem>>(
                ruleVerificationList[PropertyNames.RuleVerification].ToString());
            Assert.That(ruleVerificationsArray, Is.EquivalentTo(expectedRuleVerifications));
        }
    }
}
