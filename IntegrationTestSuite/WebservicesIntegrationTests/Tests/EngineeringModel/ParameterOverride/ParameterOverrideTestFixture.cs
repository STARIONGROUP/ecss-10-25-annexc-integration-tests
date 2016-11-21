// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterOverrideTestFixture.cs" company="RHEA System">
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
    public class ParameterOverrideTestFixture : WebClientTestFixtureBase
    {
        /// <summary>
        /// Verification that the ParameterOverride objects are returned from the data-source and that the 
        /// values of the ParameterOverride properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedParameterOverrideIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var parameterOverrideUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/f73860b2-12f0-43e4-b8b2-c81862c0a159/containedElement/75399754-ee45-4bca-b033-63e2019870d1/parameterOverride"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(parameterOverrideUri);

            //check if there is the only one ParameterOverride object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific ParameterOverride from the result by it's unique id
            var parameterOverride =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "93f767ed-4d22-45f6-ae97-d1dab0d36e1c");

            ParameterOverrideTestFixture.VerifyProperties(parameterOverride);
        }

        [Test]
        public void VerifyThatExpectedParameterOverrideWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var parameterOverrideUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/f73860b2-12f0-43e4-b8b2-c81862c0a159/containedElement/75399754-ee45-4bca-b033-63e2019870d1/parameterOverride?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(parameterOverrideUri);

            //check if there are 5 objects
            Assert.AreEqual(5, jArray.Count);

            // get a specific Iteration from the result by it's unique id
            var iteration =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific ElementDefinition from the result by it's unique id
            var elementDefinition =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "f73860b2-12f0-43e4-b8b2-c81862c0a159");
            ElementDefinitionTestFixture.VerifyProperties(elementDefinition);

            // get a specific ElementUsage from the result by it's unique id
            var elementUsage =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "75399754-ee45-4bca-b033-63e2019870d1");
            ElementUsageTestFixture.VerifyProperties(elementUsage);

            // get a specific ParameterOverride from the result by it's unique id
            var parameterOverride =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "93f767ed-4d22-45f6-ae97-d1dab0d36e1c");
            ParameterOverrideTestFixture.VerifyProperties(parameterOverride);
        }

        /// <summary>
        /// Verifies all properties of the ParameterOverride <see cref="JToken"/>
        /// </summary>
        /// <param name="parameterOverride">
        /// The <see cref="JToken"/> that contains the properties of
        /// the ParameterOverride object
        /// </param>
        public static void VerifyProperties(JToken parameterOverride)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(7, parameterOverride.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("93f767ed-4d22-45f6-ae97-d1dab0d36e1c",
                (string) parameterOverride[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) parameterOverride[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ParameterOverride", (string) parameterOverride[PropertyNames.ClassKind]);

            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string) parameterOverride[PropertyNames.Owner]);
            Assert.AreEqual("6c5aff74-f983-4aa8-a9d6-293b3429307c",
                (string) parameterOverride[PropertyNames.Parameter]);
            
            var expectedParameterSubscriptions = new string[] {};
            var parameterSubscriptionsArray = (JArray) parameterOverride[PropertyNames.ParameterSubscription];
            IList<string> parameterSubscriptions = parameterSubscriptionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedParameterSubscriptions, parameterSubscriptions);

            var expectedValueSets = new string[]
            {
                "985db346-a297-4ce6-956b-e675d53d415e"
            };
            var valueSetsArray = (JArray) parameterOverride[PropertyNames.ValueSet];
            IList<string> valueSets = valueSetsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedValueSets, valueSets);
        }
    }
}