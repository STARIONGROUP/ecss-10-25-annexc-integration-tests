// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrExpressionTestFixture.cs" company="Starion Group S.A.">
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
    public class OrExpressionTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedOrExpressionIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var orExpressionUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/requirementsSpecification/bf0cde90-9086-43d5-bcff-32a2f8331800/requirement/614e2a69-d602-46be-9311-2fb4d3273e87/parametricConstraint/88200dbc-711a-47e0-a54a-dac4baca6e83/expression/5f90327f-95a2-4c5a-9efe-581f8daf08ed");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(orExpressionUri);

            //check if there is the only one OrExpression object 
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific OrExpression from the result by it's unique id
            var orExpression = jArray.Single(x => (string)x[PropertyNames.Iid] == "5f90327f-95a2-4c5a-9efe-581f8daf08ed");
            OrExpressionTestFixture.VerifyProperties(orExpression);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedOrExpressionWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var orExpressionUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/requirementsSpecification/bf0cde90-9086-43d5-bcff-32a2f8331800/requirement/614e2a69-d602-46be-9311-2fb4d3273e87/parametricConstraint/88200dbc-711a-47e0-a54a-dac4baca6e83/expression/5f90327f-95a2-4c5a-9efe-581f8daf08ed?includeAllContainers=true");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(orExpressionUri);

            // verify that the correct amount of objects is returned
            Assert.That(jArray.Count, Is.EqualTo(6));

            // get a specific Iteration from the result by it's unique id
            var iteration = jArray.Single(x => (string)x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific RequirementsSpecification from the result by it's unique id
            var requirementsSpecification = jArray.Single(x => (string)x[PropertyNames.Iid] == "bf0cde90-9086-43d5-bcff-32a2f8331800");
            RequirementsSpecificationTestFixture.VerifyProperties(requirementsSpecification);

            // get a specific Requirement from the result by it's unique id
            RequirementTestFixture.VerifyProperties(jArray);

            // get a specific ParametricConstraint from the result by it's unique id
            var parametricConstraint = jArray.Single(x => (string)x[PropertyNames.Iid] == "88200dbc-711a-47e0-a54a-dac4baca6e83");
            ParametricConstraintTestFixture.VerifyProperties(parametricConstraint);

            // get a specific OrExpression from the result by it's unique id
            var orExpression = jArray.Single(x => (string)x[PropertyNames.Iid] == "5f90327f-95a2-4c5a-9efe-581f8daf08ed");
            OrExpressionTestFixture.VerifyProperties(orExpression);
        }

        /// <summary>
        /// Verifies all properties of the OrExpression <see cref="JToken"/>
        /// </summary>
        /// <param name="orExpression">
        /// The <see cref="JToken"/> that contains the properties of
        /// the OrExpression object
        /// </param>
        public static void VerifyProperties(JToken orExpression)
        {
            // verify the amount of returned properties 
            Assert.That(orExpression.Children().Count(), Is.EqualTo(4));

            // assert that the properties are what is expected
            Assert.That((string)orExpression[PropertyNames.Iid], Is.EqualTo("5f90327f-95a2-4c5a-9efe-581f8daf08ed"));
            Assert.That((int)orExpression[PropertyNames.RevisionNumber], Is.EqualTo(1));
            Assert.That((string)orExpression[PropertyNames.ClassKind], Is.EqualTo("OrExpression"));

            var expectedTerms = new string[]
            {
                "8c6df21f-07ae-4d0b-ab9b-866dd1f90158",
                "a6e44651-7c4a-4a57-bdf9-c0290497f392"
            };

            var termsArray = (JArray)orExpression[PropertyNames.Term];
            IList<string> terms = termsArray.Select(x => (string)x).ToList();
            Assert.That(terms, Is.EquivalentTo(expectedTerms));
        }
    }
}
