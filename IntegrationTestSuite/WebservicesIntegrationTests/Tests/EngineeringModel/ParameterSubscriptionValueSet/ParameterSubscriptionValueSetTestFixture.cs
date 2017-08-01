// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterSubscriptionValueSetTestFixture.cs" company="RHEA System">
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
    public class ParameterSubscriptionValueSetTestFixture : WebClientTestFixtureBase
    {
        /// <summary>
        /// Verification that the ParameterSubscriptionValueSet objects are returned from the data-source and that the 
        /// values of the ParameterSubscriptionValueSet properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedParameterSubscriptionValueSetIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var parameterSubscriptionValueSetUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/f73860b2-12f0-43e4-b8b2-c81862c0a159/parameter/6c5aff74-f983-4aa8-a9d6-293b3429307c/parameterSubscription/f1f076c4-5307-42b8-a171-3263a9e7bb21/valueSet"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(parameterSubscriptionValueSetUri);

            //check if there is the only one ParameterSubscriptionValueSet object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific ParameterSubscriptionValueSet from the result by it's unique id
            var parameterSubscriptionValueSet =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "a179af79-fd09-44c8-a8a6-3c4c602c7dbf");

            ParameterSubscriptionValueSetTestFixture.VerifyProperties(parameterSubscriptionValueSet);
        }

        [Test]
        public void VerifyThatExpectedParameterSubscriptionValueSetWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var parameterSubscriptionValueSetUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/f73860b2-12f0-43e4-b8b2-c81862c0a159/parameter/6c5aff74-f983-4aa8-a9d6-293b3429307c/parameterSubscription/f1f076c4-5307-42b8-a171-3263a9e7bb21/valueSet?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(parameterSubscriptionValueSetUri);

            //check if there are 6 objects
            Assert.AreEqual(6, jArray.Count);

            // get a specific Iteration from the result by it's unique id
            var iteration =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific ElementDefinition from the result by it's unique id
            ElementDefinitionTestFixture.VerifyProperties(jArray);

            // get a specific Parameter from the result by it's unique id
            var parameter =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "6c5aff74-f983-4aa8-a9d6-293b3429307c");
            ParameterTestFixture.VerifyProperties(parameter);

            // get a specific ParameterSubscription from the result by it's unique id
            var parameterSubscription =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "f1f076c4-5307-42b8-a171-3263a9e7bb21");
            ParameterSubscriptionTestFixture.VerifyProperties(parameterSubscription);

            // get a specific ParameterSubscriptionValueSet from the result by it's unique id
            var parameterSubscriptionValueSet =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "a179af79-fd09-44c8-a8a6-3c4c602c7dbf");
            ParameterSubscriptionValueSetTestFixture.VerifyProperties(parameterSubscriptionValueSet);
        }

        /// <summary>
        /// Verifies all properties of the ParameterSubscriptionValueSet <see cref="JToken"/>
        /// </summary>
        /// <param name="parameterSubscriptionValueSet">
        /// The <see cref="JToken"/> that contains the properties of
        /// the ParameterSubscriptionValueSet object
        /// </param>
        public static void VerifyProperties(JToken parameterSubscriptionValueSet)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(6, parameterSubscriptionValueSet.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("a179af79-fd09-44c8-a8a6-3c4c602c7dbf",
                (string) parameterSubscriptionValueSet[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) parameterSubscriptionValueSet[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ParameterSubscriptionValueSet",
                (string) parameterSubscriptionValueSet[PropertyNames.ClassKind]);

            Assert.AreEqual("af5c88c6-301f-497b-81f7-53748c3900ed",
                (string) parameterSubscriptionValueSet[PropertyNames.SubscribedValueSet]);

            const string emptyProperty = "[\"-\"]";
            Assert.AreEqual(emptyProperty, (string) parameterSubscriptionValueSet[PropertyNames.Manual]);

            Assert.AreEqual("MANUAL", (string) parameterSubscriptionValueSet[PropertyNames.ValueSwitch]);
        }
    }
}