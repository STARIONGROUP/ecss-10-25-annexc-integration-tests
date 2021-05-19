// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuiltInRuleVerificationTestFixture.cs" company="RHEA System S.A.">
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
    using System.Linq;
    
    using Newtonsoft.Json.Linq;
    
    using NUnit.Framework;
    
    [TestFixture]
    public class BuiltInRuleVerificationTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedBuiltInRuleVerificationIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var builtInRuleVerificationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/ruleVerificationList/dc482120-2a11-439b-913d-6a924de9ee5f/ruleVerification");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(builtInRuleVerificationUri);

            //check if there are 2 RuleVerification objects 
            Assert.AreEqual(2, jArray.Count);

            // get a specific BuiltInRuleVerification from the result by it's unique id
            var builtInRuleVerification =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "ccb44a8e-2460-4892-ab75-02d8828db233");
            BuiltInRuleVerificationTestFixture.VerifyProperties(builtInRuleVerification);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedBuiltInRuleVerificationWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var builtInRuleVerificationUri =
                new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/ruleVerificationList/dc482120-2a11-439b-913d-6a924de9ee5f/ruleVerification?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(builtInRuleVerificationUri);

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

            // get a specific BuiltInRuleVerification from the result by it's unique id
            var builtInRuleVerification =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "ccb44a8e-2460-4892-ab75-02d8828db233");
            BuiltInRuleVerificationTestFixture.VerifyProperties(builtInRuleVerification);
        }

        /// <summary>
        /// Verifies all properties of the BuiltInRuleVerification <see cref="JToken"/>
        /// </summary>
        /// <param name="builtInRuleVerification">
        /// The <see cref="JToken"/> that contains the properties of
        /// the BuiltInRuleVerification object
        /// </param>
        public static void VerifyProperties(JToken builtInRuleVerification)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(7, builtInRuleVerification.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("ccb44a8e-2460-4892-ab75-02d8828db233", (string) builtInRuleVerification[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) builtInRuleVerification[PropertyNames.RevisionNumber]);
            Assert.AreEqual("BuiltInRuleVerification", (string) builtInRuleVerification[PropertyNames.ClassKind]);

            Assert.AreEqual("Test BuiltInRuleVerification", (string) builtInRuleVerification[PropertyNames.Name]);
            Assert.AreEqual("NONE", (string) builtInRuleVerification[PropertyNames.Status]);
            Assert.IsNull((string) builtInRuleVerification[PropertyNames.ExecutedOn]);
            Assert.IsFalse((bool) builtInRuleVerification[PropertyNames.IsActive]);
        }
    }
}
