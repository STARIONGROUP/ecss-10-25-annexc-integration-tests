// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RuleVerificationTestFixture.cs" company="RHEA System S.A.">
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
    public class RuleVerificationTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("POST")]
        public void VerifyThatARuleVerificationCanBeCreatedWithWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/RuleVerification/PostNewRuleVerification.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeringModel = jArray.Single(x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");

            // Verify the amount of returned properties of the EngineeringModel
            Assert.AreEqual(8, engineeringModel.Children().Count());

            // Assert the properties of EngineeringModel have expected values
            var expectedIterations = new[] { "e163c5ad-f32b-4387-b805-f4b34600bc2c" };
            var iterationsArray = (JArray) engineeringModel[PropertyNames.Iteration];
            IList<string> iterations = iterationsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedIterations, iterations);

            var expectedLogEntries = new[] { "4e2375eb-8e37-4df2-9c7b-dd896683a891" };
            var logEntriesArray = (JArray) engineeringModel[PropertyNames.LogEntry];
            IList<string> logEntries = logEntriesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedLogEntries, logEntries);

            var expectedCommonFileStores = new[] { "8e5ca9cc-3da8-4e66-9172-7c3b2464a59c" };
            var commonFileStoresArray = (JArray) engineeringModel[PropertyNames.CommonFileStore];
            IList<string> commonFileStores = commonFileStoresArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedCommonFileStores, commonFileStores);

            Assert.AreEqual("EngineeringModel", (string) engineeringModel[PropertyNames.ClassKind]);
            Assert.AreEqual("116f6253-89bb-47d4-aa24-d11d197e43c9", (string) engineeringModel[PropertyNames.EngineeringModelSetup]);
            Assert.AreEqual("9ec982e4-ef72-4953-aa85-b158a95d8d56", (string) engineeringModel[PropertyNames.Iid]);
            Assert.AreEqual(2, (int) engineeringModel[PropertyNames.RevisionNumber]);

            // get a specific RuleVerification from the result by it's unique id
            var ruleVerificationList = jArray.Single(x => (string) x[PropertyNames.Iid] == "dc482120-2a11-439b-913d-6a924de9ee5f");

            // verify the amount of returned properties 
            Assert.AreEqual(10, ruleVerificationList.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("dc482120-2a11-439b-913d-6a924de9ee5f", (string) ruleVerificationList[PropertyNames.Iid]);
            Assert.AreEqual(2, (int) ruleVerificationList[PropertyNames.RevisionNumber]);
            Assert.AreEqual("RuleVerificationList", (string) ruleVerificationList[PropertyNames.ClassKind]);

            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string) ruleVerificationList[PropertyNames.Owner]);
            Assert.AreEqual("Test RuleVerificationList", (string) ruleVerificationList[PropertyNames.Name]);
            Assert.AreEqual("TestRuleVerificationList", (string) ruleVerificationList[PropertyNames.ShortName]);

            var expectedAliases = new string[] { };
            var aliasesArray = (JArray) ruleVerificationList[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] { };
            var definitionsArray = (JArray) ruleVerificationList[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] { };
            var hyperlinksArray = (JArray) ruleVerificationList[PropertyNames.HyperLink];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);

            var expectedRuleVerifications = new List<OrderedItem>
            {
                new OrderedItem(1, "c953263c-fc25-4048-9bb6-343a10200a0c"),
                new OrderedItem(2, "ccb44a8e-2460-4892-ab75-02d8828db233"),
                new OrderedItem(3, "4efbc475-37f3-4219-8571-e896a78545d5"),
                new OrderedItem(4, "486f4b97-fcb1-409e-b5c0-057c240f41b6")
            };

            var ruleVerificationsArray = JsonConvert.DeserializeObject<List<OrderedItem>>(
                ruleVerificationList[PropertyNames.RuleVerification].ToString());

            CollectionAssert.AreEquivalent(expectedRuleVerifications, ruleVerificationsArray);

            // get a specific RuleVerification from the result by it's unique id
            var builtInRuleVerification = jArray.Single(x => (string) x[PropertyNames.Iid] == "4efbc475-37f3-4219-8571-e896a78545d5");

            // verify the amount of returned properties 
            Assert.AreEqual(7, builtInRuleVerification.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("4efbc475-37f3-4219-8571-e896a78545d5", (string) builtInRuleVerification[PropertyNames.Iid]);
            Assert.AreEqual(2, (int) builtInRuleVerification[PropertyNames.RevisionNumber]);
            Assert.AreEqual("BuiltInRuleVerification", (string) builtInRuleVerification[PropertyNames.ClassKind]);
            Assert.AreEqual("Test Built In Rule Verification", (string) builtInRuleVerification[PropertyNames.Name]);
            Assert.AreEqual("NONE", (string) builtInRuleVerification[PropertyNames.Status]);
            Assert.IsNull((string) builtInRuleVerification[PropertyNames.ExecutedOn]);
            Assert.IsFalse((bool) builtInRuleVerification[PropertyNames.IsActive]);

            // get a specific RuleVerification from the result by it's unique id
            var userRuleVerification = jArray.Single(x => (string) x[PropertyNames.Iid] == "486f4b97-fcb1-409e-b5c0-057c240f41b6");

            // verify the amount of returned properties 
            Assert.AreEqual(7, userRuleVerification.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("486f4b97-fcb1-409e-b5c0-057c240f41b6", (string) userRuleVerification[PropertyNames.Iid]);
            Assert.AreEqual(2, (int) userRuleVerification[PropertyNames.RevisionNumber]);
            Assert.AreEqual("UserRuleVerification", (string) userRuleVerification[PropertyNames.ClassKind]);
            Assert.AreEqual("NONE", (string) userRuleVerification[PropertyNames.Status]);
            Assert.IsNull((string) userRuleVerification[PropertyNames.ExecutedOn]);
            Assert.IsFalse((bool) userRuleVerification[PropertyNames.IsActive]);
            Assert.AreEqual("8a5cd66e-7313-4843-813f-37081ca81bb8", (string) userRuleVerification[PropertyNames.Rule]);
        }

        [Test]
        [Ignore("Doen't work because RuleVerification ordered implementation is not OK. See https://github.com/RHEAGROUP/CDP4-WebServices-Community-Edition/issues/126")]
        [Category("POST")]
        public void VerifyThatARuleVerificationCanBeReorderedWithWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/RuleVerification/PostNewRuleVerification.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            postBodyPath = this.GetPath("Tests/EngineeringModel/RuleVerification/PostReorderRuleVerification.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(iterationUri, postBody);

            Assert.AreEqual(3, jArray.Count);

            var ruleVerificationList = jArray.Single(x => (string) x[PropertyNames.Iid] == "dc482120-2a11-439b-913d-6a924de9ee5f");
            Assert.AreEqual(3, (int) ruleVerificationList[PropertyNames.RevisionNumber]);

            var expectedRuleVerifications = new List<OrderedItem>
            {
                new OrderedItem(1, "486f4b97-fcb1-409e-b5c0-057c240f41b6"),
                new OrderedItem(2, "4efbc475-37f3-4219-8571-e896a78545d5"),
                new OrderedItem(3, "ccb44a8e-2460-4892-ab75-02d8828db233"),
                new OrderedItem(4, "c953263c-fc25-4048-9bb6-343a10200a0c")
            };

            var ruleVerificationsArray = JsonConvert.DeserializeObject<List<OrderedItem>>(
                ruleVerificationList[PropertyNames.RuleVerification].ToString());

            CollectionAssert.AreEquivalent(expectedRuleVerifications, ruleVerificationsArray);
        }
    }
}
