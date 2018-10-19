// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RuleVerificationTestFixture.cs" company="RHEA System">
//
//   Copyright 2018 RHEA System S.A.
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
    public class RuleVerificationTestFixture : WebClientTestFixtureBase
    {
        public override void SetUp()
        {
            base.SetUp();

            this.WebClient.Restore(this.Settings.Hostname);
        }

        public override void TearDown()
        {
            this.WebClient.Restore(this.Settings.Hostname);

            base.TearDown();
        }

        [Test]
        public void VerifyThatARuleVerificationCanBeCreatedWithWebApi()
        {
            var iterationUri = new Uri(string.Format(UriFormat, this.Settings.Hostname, "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c"));
            var postBodyPath = this.GetPath("Tests/EngineeringModel/ElementDefinition/PostNewElementDefinition.json");

            var postBody = base.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeringModel = jArray.Single(x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");

            // Verify the amount of returned properties of the EngineeringModel
            Assert.AreEqual(8, engineeringModel.Children().Count());

            // Assert the properties of EngineeringModel have expected values
            var expectedIterations = new[] { "e163c5ad-f32b-4387-b805-f4b34600bc2c" };
            var iterationsArray = (JArray)engineeringModel[PropertyNames.Iteration];
            IList<string> iterations = iterationsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedIterations, iterations);

            var expectedLogEntries = new[] { "4e2375eb-8e37-4df2-9c7b-dd896683a891" };
            var logEntriesArray = (JArray)engineeringModel[PropertyNames.LogEntry];
            IList<string> logEntries = logEntriesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedLogEntries, logEntries);

            var expectedCommonFileStores = new[] { "8e5ca9cc-3da8-4e66-9172-7c3b2464a59c" };
            var commonFileStoresArray = (JArray)engineeringModel[PropertyNames.CommonFileStore];
            IList<string> commonFileStores = commonFileStoresArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedCommonFileStores, commonFileStores);

            Assert.AreEqual("EngineeringModel", (string)engineeringModel[PropertyNames.ClassKind]);
            Assert.AreEqual("116f6253-89bb-47d4-aa24-d11d197e43c9", (string)engineeringModel[PropertyNames.EngineeringModelSetup]);
            Assert.AreEqual("9ec982e4-ef72-4953-aa85-b158a95d8d56", (string)engineeringModel[PropertyNames.Iid]);
            Assert.AreEqual(2, (int)engineeringModel[PropertyNames.RevisionNumber]);

            // get a specific RuleVerification from the result by it's unique id
            var ruleVerificationList = jArray.Single(x => (string)x[PropertyNames.Iid] == "f959dc33-58ff-4b6f-a3b0-d265690b4084");

            // verify the amount of returned properties 
            Assert.AreEqual(10, ruleVerificationList.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("dc482120-2a11-439b-913d-6a924de9ee5f", (string)ruleVerificationList[PropertyNames.Iid]);
            Assert.AreEqual(1, (int)ruleVerificationList[PropertyNames.RevisionNumber]);
            Assert.AreEqual("RuleVerificationList", (string)ruleVerificationList[PropertyNames.ClassKind]);

            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string)ruleVerificationList[PropertyNames.Owner]);
            Assert.AreEqual("Test RuleVerificationList", (string)ruleVerificationList[PropertyNames.Name]);
            Assert.AreEqual("TestRuleVerificationList", (string)ruleVerificationList[PropertyNames.ShortName]);

            var expectedAliases = new string[] { };
            var aliasesArray = (JArray)ruleVerificationList[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] { };
            var definitionsArray = (JArray)ruleVerificationList[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] { };
            var hyperlinksArray = (JArray)ruleVerificationList[PropertyNames.HyperLink];
            IList<string> h = hyperlinksArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);

            var expectedRuleVerifications = new List<OrderedItem>
            {
                new OrderedItem(1, "c953263c-fc25-4048-9bb6-343a10200a0c"),
                new OrderedItem(2, "ccb44a8e-2460-4892-ab75-02d8828db233")
            };
            var ruleVerificationsArray = JsonConvert.DeserializeObject<List<OrderedItem>>(
                ruleVerificationList[PropertyNames.RuleVerification].ToString());
            CollectionAssert.AreEquivalent(expectedRuleVerifications, ruleVerificationsArray);

            // get a specific RuleVerification from the result by it's unique id
            var ruleVerification = jArray.Single(x => (string)x[PropertyNames.Iid] == "f959dc33-58ff-4b6f-a3b0-d265690b4084");

            // verify the amount of returned properties 
            Assert.AreEqual(14, ruleVerification.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("f959dc33-58ff-4b6f-a3b0-d265690b4084", (string)ruleVerification[PropertyNames.Iid]);
            Assert.AreEqual(2, (int)ruleVerification[PropertyNames.RevisionNumber]);
            Assert.AreEqual("RuleVerification", (string)ruleVerification[PropertyNames.ClassKind]);
            Assert.AreEqual("Test Rule Verification", (string)ruleVerification[PropertyNames.Name]);
            Assert.AreEqual("TestRuleVerification", (string)ruleVerification[PropertyNames.Status]);
            Assert.AreEqual("TestRuleVerification", (string)ruleVerification[PropertyNames.ExecutedOn]);
            Assert.IsFalse((bool)ruleVerification[PropertyNames.IsActive]);
        }
    }
}