// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotExpressionTestFixture.cs" company="RHEA System">
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
    using System.Linq;
    using NUnit.Framework;
    using Newtonsoft.Json.Linq;

    [TestFixture]
    public class NotExpressionTestFixture : WebClientTestFixtureBase
    {
        /// <summary>
        /// Verification that the NotExpression objects are returned from the data-source and that the 
        /// values of the NotExpression properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedNotExpressionIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var notExpressionUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/requirementsSpecification/bf0cde90-9086-43d5-bcff-32a2f8331800/requirement/614e2a69-d602-46be-9311-2fb4d3273e87/parametricConstraint/88200dbc-711a-47e0-a54a-dac4baca6e83/expression/30cb785a-9e72-477f-ad1a-8df6ab623e3d"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(notExpressionUri);

            //check if there is the only one NotExpression object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific NotExpression from the result by it's unique id
            var notExpression =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "30cb785a-9e72-477f-ad1a-8df6ab623e3d");
            NotExpressionTestFixture.VerifyProperties(notExpression);
        }

        [Test]
        public void VerifyThatExpectedNotExpressionWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var notExpressionUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/requirementsSpecification/bf0cde90-9086-43d5-bcff-32a2f8331800/requirement/614e2a69-d602-46be-9311-2fb4d3273e87/parametricConstraint/88200dbc-711a-47e0-a54a-dac4baca6e83/expression/30cb785a-9e72-477f-ad1a-8df6ab623e3d?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(notExpressionUri);

            // verify that the correct amount of objects is returned
            Assert.AreEqual(6, jArray.Count);

            // get a specific Iteration from the result by it's unique id
            var iteration = jArray.Single(x => (string) x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific RequirementsSpecification from the result by it's unique id
            var requirementsSpecification =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "bf0cde90-9086-43d5-bcff-32a2f8331800");
            RequirementsSpecificationTestFixture.VerifyProperties(requirementsSpecification);

            // get a specific Requirement from the result by it's unique id
            var requirement = jArray.Single(x => (string) x[PropertyNames.Iid] == "614e2a69-d602-46be-9311-2fb4d3273e87");
            RequirementTestFixture.VerifyProperties(requirement);

            // get a specific ParametricConstraint from the result by it's unique id
            var parametricConstraint =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "88200dbc-711a-47e0-a54a-dac4baca6e83");
            ParametricConstraintTestFixture.VerifyProperties(parametricConstraint);

            // get a specific NotExpression from the result by it's unique id
            var notExpression =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "30cb785a-9e72-477f-ad1a-8df6ab623e3d");
            NotExpressionTestFixture.VerifyProperties(notExpression);
        }

        /// <summary>
        /// Verifies all properties of the NotExpression <see cref="JToken"/>
        /// </summary>
        /// <param name="notExpression">
        /// The <see cref="JToken"/> that contains the properties of
        /// the NotExpression object
        /// </param>
        public static void VerifyProperties(JToken notExpression)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(4, notExpression.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("30cb785a-9e72-477f-ad1a-8df6ab623e3d", (string) notExpression[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) notExpression[PropertyNames.RevisionNumber]);
            Assert.AreEqual("NotExpression", (string) notExpression[PropertyNames.ClassKind]);
            Assert.AreEqual("5f90327f-95a2-4c5a-9efe-581f8daf08ed", (string) notExpression[PropertyNames.Term]);
        }
    }
}