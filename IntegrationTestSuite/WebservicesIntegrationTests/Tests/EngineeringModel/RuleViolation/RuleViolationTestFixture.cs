// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RuleViolationTestFixture.cs" company="RHEA System">
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
    public class RuleViolationTestFixture : WebClientTestFixtureBase
    {
        /// <summary>
        /// Verification that the RuleViolation objects are returned from the data-source and that the 
        /// values of the RuleViolation properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedRuleViolationIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var ruleViolationUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/ruleVerificationList/dc482120-2a11-439b-913d-6a924de9ee5f/ruleVerification/c953263c-fc25-4048-9bb6-343a10200a0c/violation"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(ruleViolationUri);

            //check if there is the only one RuleViolation object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific RuleViolation from the result by it's unique id
            var ruleViolation =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "4333ba99-4a64-460b-971c-3ee00350470f");
            RuleViolationTestFixture.VerifyProperties(ruleViolation);
        }

        [Test]
        public void VerifyThatExpectedRuleViolationWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var ruleViolationUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/ruleVerificationList/dc482120-2a11-439b-913d-6a924de9ee5f/ruleVerification/c953263c-fc25-4048-9bb6-343a10200a0c/violation?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(ruleViolationUri);

            //check if there are 5 objects
            Assert.AreEqual(5, jArray.Count);

            // get a specific Iteration from the result by it's unique id
            var iteration =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific RuleVerificationList from the result by it's unique id
            var ruleVerificationList =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "dc482120-2a11-439b-913d-6a924de9ee5f");
            RuleVerificationListTestFixture.VerifyProperties(ruleVerificationList);

            // get a specific UserRuleVerification from the result by it's unique id
            var userRuleVerification =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "c953263c-fc25-4048-9bb6-343a10200a0c");
            UserRuleVerificationTestFixture.VerifyProperties(userRuleVerification);

            // get a specific RuleViolation from the result by it's unique id
            var ruleViolation =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "4333ba99-4a64-460b-971c-3ee00350470f");
            RuleViolationTestFixture.VerifyProperties(ruleViolation);
        }

        /// <summary>
        /// Verifies all properties of the RuleViolation <see cref="JToken"/>
        /// </summary>
        /// <param name="ruleViolation">
        /// The <see cref="JToken"/> that contains the properties of
        /// the RuleViolation object
        /// </param>
        public static void VerifyProperties(JToken ruleViolation)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(5, ruleViolation.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("4333ba99-4a64-460b-971c-3ee00350470f", (string) ruleViolation[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) ruleViolation[PropertyNames.RevisionNumber]);
            Assert.AreEqual("RuleViolation", (string) ruleViolation[PropertyNames.ClassKind]);

            Assert.AreEqual("RuleViolationTest", (string) ruleViolation[PropertyNames.Description]);

            var expectedViolatingThings = new string[] {};
            var violatingThingsArray = (JArray) ruleViolation[PropertyNames.ViolatingThing];
            IList<string> violatingThings = violatingThingsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedViolatingThings, violatingThings);
        }
    }
}