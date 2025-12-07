// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterSubscriptionValueSetTestFixture.cs" company="Starion Group S.A.">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class ParameterSubscriptionValueSetTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedParameterSubscriptionValueSetIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var parameterSubscriptionValueSetUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/f73860b2-12f0-43e4-b8b2-c81862c0a159/parameter/6c5aff74-f983-4aa8-a9d6-293b3429307c/parameterSubscription/f1f076c4-5307-42b8-a171-3263a9e7bb21/valueSet");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(parameterSubscriptionValueSetUri);

            //check if there is the only one ParameterSubscriptionValueSet object 
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific ParameterSubscriptionValueSet from the result by it's unique id
            var parameterSubscriptionValueSet = jArray.Single(x => (string) x[PropertyNames.Iid] == "a179af79-fd09-44c8-a8a6-3c4c602c7dbf");

            ParameterSubscriptionValueSetTestFixture.VerifyProperties(parameterSubscriptionValueSet);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedParameterSubscriptionValueSetWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var parameterSubscriptionValueSetUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/f73860b2-12f0-43e4-b8b2-c81862c0a159/parameter/6c5aff74-f983-4aa8-a9d6-293b3429307c/parameterSubscription/f1f076c4-5307-42b8-a171-3263a9e7bb21/valueSet?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(parameterSubscriptionValueSetUri);

            //check if there are 6 objects
            Assert.That(jArray.Count, Is.EqualTo(6));

            // get a specific Iteration from the result by it's unique id
            var iteration = jArray.Single(x => (string) x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific ElementDefinition from the result by it's unique id
            ElementDefinitionTestFixture.VerifyProperties(jArray);

            // get a specific Parameter from the result by it's unique id
            var parameter = jArray.Single(x => (string) x[PropertyNames.Iid] == "6c5aff74-f983-4aa8-a9d6-293b3429307c");
            ParameterTestFixture.VerifyProperties(parameter);

            // get a specific ParameterSubscription from the result by it's unique id
            var parameterSubscription = jArray.Single(x => (string) x[PropertyNames.Iid] == "f1f076c4-5307-42b8-a171-3263a9e7bb21");
            ParameterSubscriptionTestFixture.VerifyProperties(parameterSubscription);

            // get a specific ParameterSubscriptionValueSet from the result by it's unique id
            var parameterSubscriptionValueSet = jArray.Single(x => (string) x[PropertyNames.Iid] == "a179af79-fd09-44c8-a8a6-3c4c602c7dbf");
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
            Assert.That(parameterSubscriptionValueSet.Children().Count(), Is.EqualTo(6));

            // assert that the properties are what is expected
            Assert.That((string) parameterSubscriptionValueSet[PropertyNames.Iid], Is.EqualTo("a179af79-fd09-44c8-a8a6-3c4c602c7dbf"));
            Assert.That((int) parameterSubscriptionValueSet[PropertyNames.RevisionNumber], Is.EqualTo(1));
            Assert.That((string) parameterSubscriptionValueSet[PropertyNames.ClassKind], Is.EqualTo("ParameterSubscriptionValueSet"));

            Assert.That((string) parameterSubscriptionValueSet[PropertyNames.SubscribedValueSet], Is.EqualTo("af5c88c6-301f-497b-81f7-53748c3900ed"));

            const string emptyProperty = "[\"-\"]";
            Assert.That((string) parameterSubscriptionValueSet[PropertyNames.Manual], Is.EqualTo(emptyProperty));

            Assert.That((string) parameterSubscriptionValueSet[PropertyNames.ValueSwitch], Is.EqualTo("MANUAL"));
        }

        [Test]
        [Category("POST")]
        [TestCase("ccd430e7-2200-4289-8eee-6b78838876a8", new[]{"1.5","2"}, new[]{"1.5","2"}, new[]{"2,5", "2"}, new []{"-"}, new []{"-","-","-"})] //ArrayParameterType
        [TestCase("4b4d36db-21b3-4781-830e-4f56fafea401", new[]{"1"}, new[]{"true"}, new[]{"2"}, new []{"-","-"})] //BooleanParameterType
        [TestCase("07877cdf-060a-4256-bb37-b791fbbe240d", new[]{"2024-01-05T00:00:00"}, new[]{"2024-01-05"}, new[]{"5th January 2024"}, new []{"-","-"})] //DateParameterType
        [TestCase("87d42051-eaa8-4c03-8dd6-4a8caa6d6544", new[]{"2024-01-05T00:00:01"}, new[]{"2024-01-05T00:00:01.0000000Z"}, new[]{"5th January 2024"}, new []{"-","-"})] //DateTimeParameterType
        [TestCase("3bd5bbdd-6cb5-434d-a7a5-c2c25d128114", new[]{"1.5"}, new[]{"1.5"}, new[]{"1,5"}, new []{"-","-"})] //DerivedParameterType with REAL_NUMBER
        [TestCase("2bd23d9d-009a-4f03-9262-f983bc0d0a87", new[]{"TestEnumerationValueDefinitionA"}, new[]{"TestEnumerationValueDefinitionA"}, new[]{"TestEnumerationValueDefinitionA|TestEnumerationValueDefinitionB"}, new[]{"Test Enumeration Value Definition A"}, new []{"-","-"})] //EnumerationParameterType
        [TestCase("d2ad2581-2157-4e0b-924e-150753f598b8", new[]{"00:00:01"}, new[]{ "00:00:01" }, new[]{"0002-01-01T00:00:01.0000000Z"}, new []{"-","-"})] //TimeOfDayParameterType
        [TestCase("d2ad2581-2157-4e0b-924e-150753f598b8", new[] { "00:00:01Z" }, new[] { "00:00:01Z" }, new[] { "0002-01-01T00:00:01.0000000Z" }, new[] { "-", "-" })] //TimeOfDayParameterType
        [TestCase("d2ad2581-2157-4e0b-924e-150753f598b8", new[] { "00:00:01.001Z" }, new[] { "00:00:01.001Z" }, new[] { "0002-01-01T00:00:01.0000000Z" }, new[] { "-", "-" })] //TimeOfDayParameterType
        [TestCase("d2ad2581-2157-4e0b-924e-150753f598b8", new[] { "0001-01-01T00:00:01" }, new[] { "0001-01-01T00:00:01" }, new[] { "0002-01-01T00:00:01.0000000Z" }, new[] { "-", "-" })] //TimeOfDayParameterType
        [TestCase("d2ad2581-2157-4e0b-924e-150753f598b8", new[] { "0001-01-01T00:00:01.001Z" }, new[] { "0001-01-01T00:00:01.001Z" }, new[] { "0002-01-01T00:00:01.0000000Z" }, new[] { "-", "-" })] //TimeOfDayParameterType
        [TestCase("ebd334f7-34f2-47e2-8738-a77f4f36ae51", new[]{"1.5","2"}, new[]{"1.5","2"}, new[]{"2,5", "2"}, new []{"-"}, new []{"-","-","-"})] //CompoundParameterType
        [TestCase("a179af79-fd09-44c8-a8a6-3c4c602c7dbf", new[]{"Any text"}, new[]{"Any text"}, new[]{" "}, new []{""}, new []{"-","-"})] //TextParameterType
        public void VerifyParameterTypeValidationWorks(string parameterId, string[] validValue, string[] expectedCleanedValue, params string[][] invalidValues)
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/ParameterSubscriptionValueSet/PostNewParameterAndSubscriptionForEachParameterType.json");

            var postBody = this.GetJsonFromFile(postBodyPath);

            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var parameterValueSetId = "a179af79-fd09-44c8-a8a6-3c4c602c7dbf";

            if (parameterId != parameterValueSetId)
            {
                var parameter = jArray.Single(x => (string)x[PropertyNames.Iid] == parameterId);
                var parameterSubscriptionId = (string)(parameter[PropertyNames.ParameterSubscription]![0]);
                var parameterSubscription = jArray.Single(x =>(string)x[PropertyNames.ClassKind] == "ParameterSubscription" &&(string)x[PropertyNames.Iid] == parameterSubscriptionId);
                parameterValueSetId = (string)(parameterSubscription[PropertyNames.ValueSet]![0]);
            }

            var valueSetUpdatePath = this.GetPath("Tests/EngineeringModel/ParameterSubscriptionValueSet/PostUpdateParameterSubscriptionValueSetTemplate.json");

            var initialValueSetContent = this.GetJsonFromFile(valueSetUpdatePath).Replace("a179af79-fd09-44c8-a8a6-3c4c602c7dbf", parameterValueSetId);

            foreach (var invalidValue in invalidValues)
            {
                postBody = initialValueSetContent.Replace("<INNERJSON>", JsonConvert.SerializeObject(invalidValue));
                Assert.That(() => this.WebClient.PostDto(iterationUri, postBody), Throws.Exception.TypeOf<WebException>());
            }

            postBody = initialValueSetContent.Replace("<INNERJSON>", JsonConvert.SerializeObject(validValue));
            jArray = this.WebClient.PostDto(iterationUri, postBody);
            var newValueSet = jArray.Single(x => (string)x[PropertyNames.Iid] == parameterValueSetId);
            Assert.That(JsonConvert.DeserializeObject<List<string>>((string)newValueSet[PropertyNames.Manual]), Is.EquivalentTo(expectedCleanedValue));
        }
    }
}
