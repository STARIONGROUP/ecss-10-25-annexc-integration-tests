// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterGroupTestFixture.cs" company="RHEA System">
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
    using Newtonsoft.Json;

    [TestFixture]
    public class ParameterGroupTestFixture : WebClientTestFixtureBase
    {
        /// <summary>
        /// Verification that the ParameterGroup objects are returned from the data-source and that the 
        /// values of the ParameterGroup properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedParameterGroupIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var parameterGroupUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/f73860b2-12f0-43e4-b8b2-c81862c0a159/parameterGroup"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(parameterGroupUri);

            //check if there is the only one ParameterGroup object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific ParameterGroup from the result by it's unique id
            var parameterGroup =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "b739b3c6-9cc0-4e64-9cc4-ef7463edf559");

            ParameterGroupTestFixture.VerifyProperties(parameterGroup);
        }

        [Test]
        public void VerifyThatExpectedParameterGroupWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var parameterGroupUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/f73860b2-12f0-43e4-b8b2-c81862c0a159/parameterGroup?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(parameterGroupUri);

            //check if there are 4 objects
            Assert.AreEqual(4, jArray.Count);

            // get a specific Iteration from the result by it's unique id
            var iteration =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific ElementDefinition from the result by it's unique id
            var elementDefinition =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "f73860b2-12f0-43e4-b8b2-c81862c0a159");
            ElementDefinitionTestFixture.VerifyProperties(elementDefinition);

            // get a specific ParameterGroup from the result by it's unique id
            var parameterGroup =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "b739b3c6-9cc0-4e64-9cc4-ef7463edf559");
            ParameterGroupTestFixture.VerifyProperties(parameterGroup);
        }

        /// <summary>
        /// Verifies all properties of the ParameterGroup <see cref="JToken"/>
        /// </summary>
        /// <param name="parameterGroup">
        /// The <see cref="JToken"/> that contains the properties of
        /// the ParameterGroup object
        /// </param>
        public static void VerifyProperties(JToken parameterGroup)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(5, parameterGroup.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("b739b3c6-9cc0-4e64-9cc4-ef7463edf559",
                (string) parameterGroup[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) parameterGroup[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ParameterGroup", (string) parameterGroup[PropertyNames.ClassKind]);

            Assert.AreEqual("Test ParameterGroup", (string) parameterGroup[PropertyNames.Name]);
            Assert.IsNull((string) parameterGroup[PropertyNames.ContainingGroup]);
        }
    }
}