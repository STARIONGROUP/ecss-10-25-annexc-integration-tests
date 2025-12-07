// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserRuleVerificationTestFixture.cs" company="Starion Group S.A.">
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
    using System.Linq;
    
    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class UserRuleVerificationTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedUserRuleVerificationIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var userRuleVerificationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/ruleVerificationList/dc482120-2a11-439b-913d-6a924de9ee5f/ruleVerification");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(userRuleVerificationUri);

            //check if there are 2 RuleVerification objects 
            Assert.That(jArray.Count, Is.EqualTo(2));

            // get a specific UserRuleVerification from the result by it's unique id
            var userRuleVerification = jArray.Single(x => (string) x[PropertyNames.Iid] == "c953263c-fc25-4048-9bb6-343a10200a0c");
            UserRuleVerificationTestFixture.VerifyProperties(userRuleVerification);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedUserRuleVerificationWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var userRuleVerificationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/ruleVerificationList/dc482120-2a11-439b-913d-6a924de9ee5f/ruleVerification?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(userRuleVerificationUri);

            //check if there are 5 objects
            Assert.That(jArray.Count, Is.EqualTo(5));

            // get a specific Iteration from the result by it's unique id
            var iteration = jArray.Single(x => (string) x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific RuleVerificationList from the result by it's unique id
            var ruleVerificationList = jArray.Single(x => (string) x[PropertyNames.Iid] == "dc482120-2a11-439b-913d-6a924de9ee5f");
            RuleVerificationListTestFixture.VerifyProperties(ruleVerificationList);

            // get a specific UserRuleVerification from the result by it's unique id
            var userRuleVerification = jArray.Single(x => (string) x[PropertyNames.Iid] == "c953263c-fc25-4048-9bb6-343a10200a0c");
            UserRuleVerificationTestFixture.VerifyProperties(userRuleVerification);
        }

        /// <summary>
        /// Verifies all properties of the UserRuleVerification <see cref="JToken"/>
        /// </summary>
        /// <param name="userRuleVerification">
        /// The <see cref="JToken"/> that contains the properties of
        /// the UserRuleVerification object
        /// </param>
        public static void VerifyProperties(JToken userRuleVerification)
        {
            Assert.That(userRuleVerification.Children().Count(), Is.EqualTo(7));

            Assert.That((string) userRuleVerification[PropertyNames.Iid], Is.EqualTo("c953263c-fc25-4048-9bb6-343a10200a0c"));
            Assert.That((int) userRuleVerification[PropertyNames.RevisionNumber], Is.EqualTo(1));
            Assert.That((string) userRuleVerification[PropertyNames.ClassKind], Is.EqualTo("UserRuleVerification"));
            Assert.That((string) userRuleVerification[PropertyNames.Status], Is.EqualTo("PASSED"));
            Assert.That((string) userRuleVerification[PropertyNames.ExecutedOn], Is.Null);
            Assert.That((bool) userRuleVerification[PropertyNames.IsActive], Is.True);
            Assert.That((string) userRuleVerification[PropertyNames.Rule], Is.EqualTo("8a5cd66e-7313-4843-813f-37081ca81bb8"));
        }
    }
}
