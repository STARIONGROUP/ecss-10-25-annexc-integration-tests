// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExclusiveOrExpressionTestFixture.cs" company="RHEA System">
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
    using System.Collections.Generic;

    [TestFixture]
    public class ExclusiveOrExpressionTestFixture : WebClientTestFixtureBase
    {
        /// <summary>
        /// Verification that the ExclusiveOrExpression objects are returned from the data-source and that the 
        /// values of the ExclusiveOrExpression properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedExclusiveOrExpressionIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var exclusiveOrExpressionUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/requirementsSpecification/bf0cde90-9086-43d5-bcff-32a2f8331800/requirement/614e2a69-d602-46be-9311-2fb4d3273e87/parametricConstraint/88200dbc-711a-47e0-a54a-dac4baca6e83/expression/8c6df21f-07ae-4d0b-ab9b-866dd1f90158"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(exclusiveOrExpressionUri);

            //check if there is the only one ExclusiveOrExpression object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific ExclusiveOrExpression from the result by it's unique id
            var exclusiveOrExpression =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "8c6df21f-07ae-4d0b-ab9b-866dd1f90158");
            ExclusiveOrExpressionTestFixture.VerifyProperties(exclusiveOrExpression);
        }

        [Test]
        public void VerifyThatExpectedExclusiveOrExpressionWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var exclusiveOrExpressionUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/requirementsSpecification/bf0cde90-9086-43d5-bcff-32a2f8331800/requirement/614e2a69-d602-46be-9311-2fb4d3273e87/parametricConstraint/88200dbc-711a-47e0-a54a-dac4baca6e83/expression/8c6df21f-07ae-4d0b-ab9b-866dd1f90158?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(exclusiveOrExpressionUri);

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
            RequirementTestFixture.VerifyProperties(jArray);

            // get a specific ParametricConstraint from the result by it's unique id
            var parametricConstraint =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "88200dbc-711a-47e0-a54a-dac4baca6e83");
            ParametricConstraintTestFixture.VerifyProperties(parametricConstraint);

            // get a specific ExclusiveOrExpression from the result by it's unique id
            var exclusiveOrExpression =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "8c6df21f-07ae-4d0b-ab9b-866dd1f90158");
            ExclusiveOrExpressionTestFixture.VerifyProperties(exclusiveOrExpression);
        }

        /// <summary>
        /// Verifies all properties of the ExclusiveOrExpression <see cref="JToken"/>
        /// </summary>
        /// <param name="exclusiveOrExpression">
        /// The <see cref="JToken"/> that contains the properties of
        /// the ExclusiveOrExpression object
        /// </param>
        public static void VerifyProperties(JToken exclusiveOrExpression)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(4, exclusiveOrExpression.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("8c6df21f-07ae-4d0b-ab9b-866dd1f90158", (string) exclusiveOrExpression[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) exclusiveOrExpression[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ExclusiveOrExpression", (string) exclusiveOrExpression[PropertyNames.ClassKind]);

            var expectedTerms = new string[]
            {
                "000484d0-cefd-47be-9317-a9eae72c94ce",
                "deaa2560-b704-4b2c-950b-aad02ff84052"
            };
            var termsArray = (JArray) exclusiveOrExpression[PropertyNames.Term];
            IList<string> terms = termsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedTerms, terms);
        }
    }
}