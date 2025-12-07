// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RelationalExpressionTestFixture.cs" company="Starion Group S.A.">
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
    public class RelationalExpressionTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedRelationalExpressionsIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var relationalExpressionUri = new Uri(string.Format($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/requirementsSpecification/bf0cde90-9086-43d5-bcff-32a2f8331800/requirement/614e2a69-d602-46be-9311-2fb4d3273e87/parametricConstraint/88200dbc-711a-47e0-a54a-dac4baca6e83/expression"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(relationalExpressionUri);

            //check if there are the correct amount of objects 
            Assert.That(jArray.Count, Is.EqualTo(6));

            RelationalExpressionTestFixture.VerifyProperties(jArray);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedRelationalExpressionsWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var relationalExpressionUri = new Uri(string.Format($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/requirementsSpecification/bf0cde90-9086-43d5-bcff-32a2f8331800/requirement/614e2a69-d602-46be-9311-2fb4d3273e87/parametricConstraint/88200dbc-711a-47e0-a54a-dac4baca6e83/expression?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(relationalExpressionUri);

            // verify that the correct amount of objects is returned
            Assert.That(jArray.Count, Is.EqualTo(11));

            // get a specific Iteration from the result by it's unique id
            var iteration = jArray.Single(x => (string) x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific RequirementsSpecification from the result by it's unique id
            var requirementsSpecification = jArray.Single(x => (string) x[PropertyNames.Iid] == "bf0cde90-9086-43d5-bcff-32a2f8331800");
            RequirementsSpecificationTestFixture.VerifyProperties(requirementsSpecification);

            // get a specific Requirement from the result by it's unique id
            RequirementTestFixture.VerifyProperties(jArray);

            // get a specific ParametricConstraint from the result by it's unique id
            var parametricConstraint = jArray.Single(x => (string) x[PropertyNames.Iid] == "88200dbc-711a-47e0-a54a-dac4baca6e83");
            ParametricConstraintTestFixture.VerifyProperties(parametricConstraint);

            RelationalExpressionTestFixture.VerifyProperties(jArray);
        }

        /// <summary>
        /// Verifies all properties of the RelationalExpression <see cref="JToken"/>
        /// </summary>
        /// <param name="relationalExpression">
        /// The <see cref="JToken"/> that contains the properties of
        /// the RelationalExpression object
        /// </param>
        public static void VerifyProperties(JToken relationalExpression)
        {
            // assert that all objects are what is expected
            var relationalExpressionObject = relationalExpression.Single(x => (string) x[PropertyNames.Iid] == "deaa2560-b704-4b2c-950b-aad02ff84052");

            // assert that the properties are what is expected
            Assert.That((string) relationalExpressionObject[PropertyNames.Iid], Is.EqualTo("deaa2560-b704-4b2c-950b-aad02ff84052"));
            Assert.That((int) relationalExpressionObject[PropertyNames.RevisionNumber], Is.EqualTo(1));
            Assert.That((string) relationalExpressionObject[PropertyNames.ClassKind], Is.EqualTo("RelationalExpression"));

            Assert.That((string) relationalExpressionObject[PropertyNames.Scale], Is.Null);
            Assert.That((string) relationalExpressionObject[PropertyNames.Value], Is.EqualTo("[\"true\"]"));
            Assert.That((string) relationalExpressionObject[PropertyNames.RelationalOperator], Is.EqualTo("EQ"));
            Assert.That((string) relationalExpressionObject[PropertyNames.ParameterType], Is.EqualTo("35a9cf05-4eba-4cda-b60c-7cfeaac8f892"));

            relationalExpressionObject = relationalExpression.Single(x => (string) x[PropertyNames.Iid] == "a6e44651-7c4a-4a57-bdf9-c0290497f392");

            // assert that the properties are what is expected
            Assert.That((string) relationalExpressionObject[PropertyNames.Iid], Is.EqualTo("a6e44651-7c4a-4a57-bdf9-c0290497f392"));
            Assert.That((int) relationalExpressionObject[PropertyNames.RevisionNumber], Is.EqualTo(1));
            Assert.That((string) relationalExpressionObject[PropertyNames.ClassKind], Is.EqualTo("RelationalExpression"));

            Assert.That((string) relationalExpressionObject[PropertyNames.Scale], Is.Null);
            Assert.That((string) relationalExpressionObject[PropertyNames.Value], Is.EqualTo("[\"test\"]"));
            Assert.That((string) relationalExpressionObject[PropertyNames.RelationalOperator], Is.EqualTo("EQ"));
            Assert.That((string) relationalExpressionObject[PropertyNames.ParameterType], Is.EqualTo("a21c15c4-3e1e-46b5-b109-5063dec1e254"));
        }
    }
}
