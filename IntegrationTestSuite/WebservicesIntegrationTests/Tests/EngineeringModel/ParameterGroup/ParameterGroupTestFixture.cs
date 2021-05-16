// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterGroupTestFixture.cs" company="RHEA System S.A.">
//
//   Copyright 2016-2021 RHEA System S.A.
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
    public class ParameterGroupTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedParameterGroupIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var parameterGroupUri = new Uri(
                string.Format(
                    UriFormat,
                    this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/f73860b2-12f0-43e4-b8b2-c81862c0a159/parameterGroup"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(parameterGroupUri);

            // check if there is the only one ParameterGroup object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific ParameterGroup from the result by it's unique id
            var parameterGroup = jArray.Single(
                x => (string)x[PropertyNames.Iid] == "b739b3c6-9cc0-4e64-9cc4-ef7463edf559");

            ParameterGroupTestFixture.VerifyProperties(parameterGroup);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedParameterGroupWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var parameterGroupUri = new Uri(
                string.Format(
                    UriFormat,
                    this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/f73860b2-12f0-43e4-b8b2-c81862c0a159/parameterGroup?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(parameterGroupUri);

            // check if there are 4 objects
            Assert.AreEqual(4, jArray.Count);

            // get a specific Iteration from the result by it's unique id
            var iteration = jArray.Single(x => (string)x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific ElementDefinition from the result by it's unique id
            ElementDefinitionTestFixture.VerifyProperties(jArray);

            // get a specific ParameterGroup from the result by it's unique id
            var parameterGroup = jArray.Single(
                x => (string)x[PropertyNames.Iid] == "b739b3c6-9cc0-4e64-9cc4-ef7463edf559");
            ParameterGroupTestFixture.VerifyProperties(parameterGroup);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatParameterGroupCanBeDeletedAndContainedParametersReturnedFromWebApi()
        {
            var iterationUri = new Uri(
                string.Format(
                    UriFormat,
                    this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c"));
            var postBodyPath = this.GetPath("Tests/EngineeringModel/ParameterGroup/PostDeleteParameterGroup.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            // check if there are appropriate amount of objects
            Assert.AreEqual(3, jArray.Count);

            var engineeeringModel = jArray.Single(
                x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.AreEqual(2, (int)engineeeringModel[PropertyNames.RevisionNumber]);

            // get a specific ElementDefinition from the result by it's unique id
            var elementDefinition = jArray.Single(
                x => (string)x[PropertyNames.Iid] == "f73860b2-12f0-43e4-b8b2-c81862c0a159");
            Assert.AreEqual(2, (int)elementDefinition[PropertyNames.RevisionNumber]);
            var expectedParameterGroups = new string[] { };
            var parameterGroupsArray = (JArray)elementDefinition[PropertyNames.ParameterGroup];
            IList<string> parameterGroups = parameterGroupsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedParameterGroups, parameterGroups);

            // get a specific Parameter from the result by it's unique id
            var parameter = jArray.Single(x => (string)x[PropertyNames.Iid] == "6c5aff74-f983-4aa8-a9d6-293b3429307c");
            Assert.AreEqual(2, (int)parameter[PropertyNames.RevisionNumber]);
            Assert.IsNull((string)parameter[PropertyNames.Group]);
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
            Assert.AreEqual("b739b3c6-9cc0-4e64-9cc4-ef7463edf559", (string)parameterGroup[PropertyNames.Iid]);
            Assert.AreEqual(1, (int)parameterGroup[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ParameterGroup", (string)parameterGroup[PropertyNames.ClassKind]);

            Assert.AreEqual("Test ParameterGroup", (string)parameterGroup[PropertyNames.Name]);
            Assert.IsNull((string)parameterGroup[PropertyNames.ContainingGroup]);
        }
    }
}
