// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterOverrideValueSetTestFixture.cs" company="Starion Group S.A.">
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
    using System.Net;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class ParameterOverrideValueSetTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedParameterOverrideValueSetIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var parameterOverrideValueSetUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/fe9295c5-af99-494e-86ff-e715837806ae/containedElement/75399754-ee45-4bca-b033-63e2019870d1/parameterOverride/93f767ed-4d22-45f6-ae97-d1dab0d36e1c/valueSet");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(parameterOverrideValueSetUri);

            //check if there is the only one ParameterOverrideValueSet object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific ParameterOverrideValueSet from the result by it's unique id
            var parameterOverrideValueSet = jArray.Single(x => (string)x[PropertyNames.Iid] == "985db346-a297-4ce6-956b-e675d53d415e");

            ParameterOverrideValueSetTestFixture.VerifyProperties(parameterOverrideValueSet);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedParameterOverrideValueSetWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var parameterOverrideValueSetUri =
                new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/fe9295c5-af99-494e-86ff-e715837806ae/containedElement/75399754-ee45-4bca-b033-63e2019870d1/parameterOverride/93f767ed-4d22-45f6-ae97-d1dab0d36e1c/valueSet?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(parameterOverrideValueSetUri);

            //check if there are 6 objects
            Assert.AreEqual(6, jArray.Count);

            // get a specific Iteration from the result by it's unique id
            var iteration = jArray.Single(x => (string)x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific ElementDefinition from the result by it's unique id
            ElementDefinitionTestFixture.VerifyProperties(jArray);

            // get a specific ElementUsage from the result by it's unique id
            ElementUsageTestFixture.VerifyProperties(jArray);

            // get a specific ParameterOverride from the result by it's unique id
            var parameterOverride = jArray.Single(x => (string)x[PropertyNames.Iid] == "93f767ed-4d22-45f6-ae97-d1dab0d36e1c");
            ParameterOverrideTestFixture.VerifyProperties(parameterOverride);

            // get a specific ParameterOverrideValueSet from the result by it's unique id
            var parameterOverrideValueSet = jArray.Single(x => (string)x[PropertyNames.Iid] == "985db346-a297-4ce6-956b-e675d53d415e");
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

        [Test]
        [Category("POST")]
        [TestCase("ccd430e7-2200-4289-8eee-6b78838876a8", new[]{"1.5","2"}, new[]{"1.5","2"}, new[]{"2,5", "2"}, new []{"-"}, new []{"-","-","-"})] //ArrayParameterType
        [TestCase("4b4d36db-21b3-4781-830e-4f56fafea401", new[]{"1"}, new[]{"true"}, new[]{"2"}, new []{"-","-"})] //BooleanParameterType
        [TestCase("07877cdf-060a-4256-bb37-b791fbbe240d", new[]{"2024-01-05T00:00:00"}, new[]{"2024-01-05"}, new[]{"5th January 2024"}, new []{"-","-"})] //DateParameterType
        [TestCase("87d42051-eaa8-4c03-8dd6-4a8caa6d6544", new[]{"2024-01-05T00:00:01"}, new[]{"2024-01-05T00:00:01.0000000Z"}, new[]{"5th January 2024"}, new []{"-","-"})] //DateTimeParameterType
        [TestCase("3bd5bbdd-6cb5-434d-a7a5-c2c25d128114", new[]{"1.5"}, new[]{"1.5"}, new[]{"1,5"}, new []{"-","-"})] //DerivedParameterType with REAL_NUMBER
        [TestCase("2bd23d9d-009a-4f03-9262-f983bc0d0a87", new[]{"TestEnumerationValueDefinitionA"}, new[]{"TestEnumerationValueDefinitionA"}, new[]{"TestEnumerationValueDefinitionA|TestEnumerationValueDefinitionB"}, new[]{"Test Enumeration Value Definition A"}, new []{"-","-"})] //EnumerationParameterType
        [TestCase("d2ad2581-2157-4e0b-924e-150753f598b8", new[] { "00:00:01" }, new[] { "00:00:01" }, new[] { "0002-01-01T00:00:01.0000000Z" }, new[] { "-", "-" })] //TimeOfDayParameterType
        [TestCase("d2ad2581-2157-4e0b-924e-150753f598b8", new[] { "00:00:01Z" }, new[] { "00:00:01Z" }, new[] { "0002-01-01T00:00:01.0000000Z" }, new[] { "-", "-" })] //TimeOfDayParameterType
        [TestCase("d2ad2581-2157-4e0b-924e-150753f598b8", new[] { "00:00:01.001Z" }, new[] { "00:00:01.001Z" }, new[] { "0002-01-01T00:00:01.0000000Z" }, new[] { "-", "-" })] //TimeOfDayParameterType
        [TestCase("d2ad2581-2157-4e0b-924e-150753f598b8", new[] { "0001-01-01T00:00:01" }, new[] { "0001-01-01T00:00:01" }, new[] { "0002-01-01T00:00:01.0000000Z" }, new[] { "-", "-" })] //TimeOfDayParameterType
        [TestCase("d2ad2581-2157-4e0b-924e-150753f598b8", new[] { "0001-01-01T00:00:01.001Z" }, new[] { "0001-01-01T00:00:01.001Z" }, new[] { "0002-01-01T00:00:01.0000000Z" }, new[] { "-", "-" })] //TimeOfDayParameterType
        [TestCase("ebd334f7-34f2-47e2-8738-a77f4f36ae51", new[]{"1.5","2"}, new[]{"1.5","2"}, new[]{"2,5", "2"}, new []{"-"}, new []{"-","-","-"})] //CompoundParameterType
        [TestCase("985db346-a297-4ce6-956b-e675d53d415e", new[]{"Any text"}, new[]{"Any text"}, new[]{" "}, new []{""}, new []{"-","-"})] //TextParameterType
        public void VerifyParameterTypeValidationWorks(string parameterId, string[] validValue, string[] expectedCleanedValue, params string[][] invalidValues)
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/ParameterOverrideValueSet/PostNewParameterAndOverrideForEachParameterType.json");

            var postBody = this.GetJsonFromFile(postBodyPath);

            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var parameterValueSetId = "985db346-a297-4ce6-956b-e675d53d415e";

            if (parameterId != parameterValueSetId)
            {
                var parameterOverride = jArray.Single(x =>(string)x[PropertyNames.ClassKind] == "ParameterOverride" &&(string)x[PropertyNames.Parameter] == parameterId);
                parameterValueSetId = (string)(parameterOverride[PropertyNames.ValueSet]![0]);
            }

            var valueSetUpdatePath = this.GetPath("Tests/EngineeringModel/ParameterOverrideValueSet/PostUpdateParameterOverrideValueSetTemplate.json");

            var initialValueSetContent = this.GetJsonFromFile(valueSetUpdatePath).Replace("985db346-a297-4ce6-956b-e675d53d415e", parameterValueSetId);

            foreach (var invalidValue in invalidValues)
            {
                postBody = initialValueSetContent.Replace("<INNERJSON>", JsonConvert.SerializeObject(invalidValue));
                Assert.That(() => this.WebClient.PostDto(iterationUri, postBody), Throws.Exception.TypeOf<WebException>());
            }

            postBody = initialValueSetContent.Replace("<INNERJSON>", JsonConvert.SerializeObject(validValue));
            jArray = this.WebClient.PostDto(iterationUri, postBody);
            var newValueSet = jArray.Single(x => (string)x[PropertyNames.Iid] == parameterValueSetId);

            Assert.Multiple(() =>
            {
                Assert.That(JsonConvert.DeserializeObject<List<string>>((string)newValueSet[PropertyNames.Reference]), Is.EquivalentTo(expectedCleanedValue));
                Assert.That(JsonConvert.DeserializeObject<List<string>>((string)newValueSet[PropertyNames.Manual]), Is.EquivalentTo(expectedCleanedValue));
                Assert.That(JsonConvert.DeserializeObject<List<string>>((string)newValueSet[PropertyNames.Computed]), Is.EquivalentTo(expectedCleanedValue));
                Assert.That(JsonConvert.DeserializeObject<List<string>>((string)newValueSet[PropertyNames.Published]), Is.EquivalentTo(expectedCleanedValue));
                Assert.That(JsonConvert.DeserializeObject<List<string>>((string)newValueSet[PropertyNames.Formula]), Is.EquivalentTo(validValue));
            });
        }
    }
}
