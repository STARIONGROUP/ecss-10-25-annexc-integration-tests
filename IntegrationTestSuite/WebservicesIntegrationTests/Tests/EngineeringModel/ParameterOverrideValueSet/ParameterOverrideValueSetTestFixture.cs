// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterOverrideValueSetTestFixture.cs" company="RHEA System">
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
    public class ParameterOverrideValueSetTestFixture : WebClientTestFixtureBase
    {
        /// <summary>
        /// Verification that the ParameterOverrideValueSet objects are returned from the data-source and that the 
        /// values of the ParameterOverrideValueSet properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedParameterOverrideValueSetIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var parameterOverrideValueSetUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/fe9295c5-af99-494e-86ff-e715837806ae/containedElement/75399754-ee45-4bca-b033-63e2019870d1/parameterOverride/93f767ed-4d22-45f6-ae97-d1dab0d36e1c/valueSet"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(parameterOverrideValueSetUri);

            //check if there is the only one ParameterOverrideValueSet object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific ParameterOverrideValueSet from the result by it's unique id
            var parameterOverrideValueSet =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "985db346-a297-4ce6-956b-e675d53d415e");

            ParameterOverrideValueSetTestFixture.VerifyProperties(parameterOverrideValueSet);
        }

        [Test]
        public void VerifyThatExpectedParameterOverrideValueSetWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var parameterOverrideValueSetUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/fe9295c5-af99-494e-86ff-e715837806ae/containedElement/75399754-ee45-4bca-b033-63e2019870d1/parameterOverride/93f767ed-4d22-45f6-ae97-d1dab0d36e1c/valueSet?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(parameterOverrideValueSetUri);

            //check if there are 6 objects
            Assert.AreEqual(6, jArray.Count);

            // get a specific Iteration from the result by it's unique id
            var iteration =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific ElementDefinition from the result by it's unique id
            ElementDefinitionTestFixture.VerifyProperties(jArray);

            // get a specific ElementUsage from the result by it's unique id
            ElementUsageTestFixture.VerifyProperties(jArray);

            // get a specific ParameterOverride from the result by it's unique id
            var parameterOverride =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "93f767ed-4d22-45f6-ae97-d1dab0d36e1c");
            ParameterOverrideTestFixture.VerifyProperties(parameterOverride);

            // get a specific ParameterOverrideValueSet from the result by it's unique id
            var parameterOverrideValueSet =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "985db346-a297-4ce6-956b-e675d53d415e");
            ParameterOverrideValueSetTestFixture.VerifyProperties(parameterOverrideValueSet);
        }

        /// <summary>
        /// Verifies all properties of the ParameterOverrideValueSet <see cref="JToken"/>
        /// </summary>
        /// <param name="parameterOverrideValueSet">
        /// The <see cref="JToken"/> that contains the properties of
        /// the ParameterOverrideValueSet object
        /// </param>
        public static void VerifyProperties(JToken parameterOverrideValueSet)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(10, parameterOverrideValueSet.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("985db346-a297-4ce6-956b-e675d53d415e",
                (string)parameterOverrideValueSet[PropertyNames.Iid]);
            Assert.AreEqual(1, (int)parameterOverrideValueSet[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ParameterOverrideValueSet", (string)parameterOverrideValueSet[PropertyNames.ClassKind]);

            Assert.AreEqual("MANUAL", (string)parameterOverrideValueSet[PropertyNames.ValueSwitch]);

            const string emptyProperty = "[\"-\"]";
            Assert.AreEqual(emptyProperty, (string)parameterOverrideValueSet[PropertyNames.Published]);
            Assert.AreEqual(emptyProperty, (string)parameterOverrideValueSet[PropertyNames.Formula]);
            Assert.AreEqual(emptyProperty, (string)parameterOverrideValueSet[PropertyNames.Computed]);
            Assert.AreEqual(emptyProperty, (string)parameterOverrideValueSet[PropertyNames.Manual]);
            Assert.AreEqual(emptyProperty, (string)parameterOverrideValueSet[PropertyNames.Reference]);

            Assert.AreEqual("af5c88c6-301f-497b-81f7-53748c3900ed", (string)parameterOverrideValueSet[PropertyNames.ParameterValueSet]);
        }
    }
}
