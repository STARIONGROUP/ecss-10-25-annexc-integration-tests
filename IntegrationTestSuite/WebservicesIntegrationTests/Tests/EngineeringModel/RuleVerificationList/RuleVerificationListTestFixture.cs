// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RuleVerificationListTestFixture.cs" company="RHEA System S.A.">
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
            var ruleVerificationListUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/ruleVerificationList"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(ruleVerificationListUri);

            //check if there is the only one RuleVerificationList object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific RuleVerificationList from the result by it's unique id
            var ruleVerificationList =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "dc482120-2a11-439b-913d-6a924de9ee5f");
            RuleVerificationListTestFixture.VerifyProperties(ruleVerificationList);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedRuleVerificationListWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var ruleVerificationListUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/ruleVerificationList?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(ruleVerificationListUri);

            //check if there are 3 objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific Iteration from the result by it's unique id
            var iteration =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific RuleVerificationList from the result by it's unique id
            var ruleVerificationList =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "dc482120-2a11-439b-913d-6a924de9ee5f");
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
            Assert.AreEqual(10, ruleVerificationList.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("dc482120-2a11-439b-913d-6a924de9ee5f", (string) ruleVerificationList[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) ruleVerificationList[PropertyNames.RevisionNumber]);
            Assert.AreEqual("RuleVerificationList", (string) ruleVerificationList[PropertyNames.ClassKind]);

            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string) ruleVerificationList[PropertyNames.Owner]);
            Assert.AreEqual("Test RuleVerificationList", (string) ruleVerificationList[PropertyNames.Name]);
            Assert.AreEqual("TestRuleVerificationList", (string) ruleVerificationList[PropertyNames.ShortName]);

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) ruleVerificationList[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) ruleVerificationList[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) ruleVerificationList[PropertyNames.HyperLink];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);

            var expectedRuleVerifications = new List<OrderedItem>
            {
                new OrderedItem(1, "c953263c-fc25-4048-9bb6-343a10200a0c"),
                new OrderedItem(2, "ccb44a8e-2460-4892-ab75-02d8828db233")
            };
            var ruleVerificationsArray = JsonConvert.DeserializeObject<List<OrderedItem>>(
                ruleVerificationList[PropertyNames.RuleVerification].ToString());
            CollectionAssert.AreEquivalent(expectedRuleVerifications, ruleVerificationsArray);
        }
    }
}
