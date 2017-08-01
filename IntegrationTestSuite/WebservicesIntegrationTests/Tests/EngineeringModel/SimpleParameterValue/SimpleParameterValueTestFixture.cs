// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleParameterValueTestFixture.cs" company="RHEA System">
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
    public class SimpleParameterValueTestFixture : WebClientTestFixtureBase
    {
        /// <summary>
        /// Verification that the SimpleParameterValue objects are returned from the data-source and that the 
        /// values of the SimpleParameterValue properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedSimpleParameterValuesIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var simpleParameterValueUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/requirementsSpecification/bf0cde90-9086-43d5-bcff-32a2f8331800/requirement/614e2a69-d602-46be-9311-2fb4d3273e87/parameterValue"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(simpleParameterValueUri);

            //check if there are the correct amount of SimpleParameterValue objects 
            Assert.AreEqual(2, jArray.Count);

            SimpleParameterValueTestFixture.VerifyProperties(jArray);
        }

        [Test]
        public void VerifyThatExpectedSimpleParameterValuesWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var simpleParameterValueUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/requirementsSpecification/bf0cde90-9086-43d5-bcff-32a2f8331800/requirement/614e2a69-d602-46be-9311-2fb4d3273e87/parameterValue?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(simpleParameterValueUri);

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

            SimpleParameterValueTestFixture.VerifyProperties(jArray);
        }

        /// <summary>
        /// Verifies all properties of the SimpleParameterValue <see cref="JToken"/>
        /// </summary>
        /// <param name="simpleParameterValue">
        /// The <see cref="JToken"/> that contains the properties of
        /// the SimpleParameterValue object
        /// </param>
        public static void VerifyProperties(JToken simpleParameterValue)
        {
            // assert that all objects are what is expected
            var simpleParameterValueObject =
                simpleParameterValue.Single(x => (string) x[PropertyNames.Iid] == "ef3b5740-6e0e-463c-99df-f255e38a32b6");

            // assert that the properties are what is expected
            Assert.AreEqual("ef3b5740-6e0e-463c-99df-f255e38a32b6",
                (string) simpleParameterValueObject[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) simpleParameterValueObject[PropertyNames.RevisionNumber]);
            Assert.AreEqual("SimpleParameterValue", (string) simpleParameterValueObject[PropertyNames.ClassKind]);

            Assert.IsNull((string) simpleParameterValueObject[PropertyNames.Scale]);
            Assert.AreEqual("35a9cf05-4eba-4cda-b60c-7cfeaac8f892",
                (string) simpleParameterValueObject[PropertyNames.ParameterType]);
            Assert.AreEqual("[\"true\"]", (string) simpleParameterValueObject[PropertyNames.Value]);

            simpleParameterValueObject =
                simpleParameterValue.Single(x => (string) x[PropertyNames.Iid] == "bcedefb0-b3ee-4a0b-8137-6561fa23b37f");

            // assert that the properties are what is expected
            Assert.AreEqual("bcedefb0-b3ee-4a0b-8137-6561fa23b37f",
                (string) simpleParameterValueObject[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) simpleParameterValueObject[PropertyNames.RevisionNumber]);
            Assert.AreEqual("SimpleParameterValue", (string) simpleParameterValueObject[PropertyNames.ClassKind]);

            Assert.IsNull((string) simpleParameterValueObject[PropertyNames.Scale]);
            Assert.AreEqual("a21c15c4-3e1e-46b5-b109-5063dec1e254",
                (string) simpleParameterValueObject[PropertyNames.ParameterType]);
            Assert.AreEqual("[\"test\"]", (string) simpleParameterValueObject[PropertyNames.Value]);
        }
    }
}