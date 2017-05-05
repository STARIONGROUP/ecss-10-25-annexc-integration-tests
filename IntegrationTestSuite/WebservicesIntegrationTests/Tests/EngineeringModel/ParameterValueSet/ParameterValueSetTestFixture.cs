// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterValueSetTestFixture.cs" company="RHEA System">
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
    public class ParameterValueSetTestFixture : WebClientTestFixtureBase
    {
        public override void SetUp()
        {
            base.SetUp();

            this.WebClient.Restore(this.Settings.Hostname);
        }

        public override void TearDown()
        {
            this.WebClient.Restore(this.Settings.Hostname);

            base.TearDown();
        }

        /// <summary>
        /// Verification that the ParameterValueSet objects are returned from the data-source and that the 
        /// values of the ParameterValueSet properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedParameterValueSetIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var parameterValueSetUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/f73860b2-12f0-43e4-b8b2-c81862c0a159/parameter/6c5aff74-f983-4aa8-a9d6-293b3429307c/valueSet"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(parameterValueSetUri);

            //check if there is the only one ParameterValueSet object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific ParameterValueSet from the result by it's unique id
            var parameterValueSet =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "af5c88c6-301f-497b-81f7-53748c3900ed");

            ParameterValueSetTestFixture.VerifyProperties(parameterValueSet);
        }

        [Test]
        public void VerifyThatExpectedParameterValueSetWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var parameterValueSetUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/f73860b2-12f0-43e4-b8b2-c81862c0a159/parameter/6C5AFF74-F983-4AA8-A9D6-293B3429307C/valueSet?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(parameterValueSetUri);

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

            // get a specific Parameter from the result by it's unique id
            var parameter =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "6c5aff74-f983-4aa8-a9d6-293b3429307c");
            ParameterTestFixture.VerifyProperties(parameter);

            // get a specific ParameterValueSet from the result by it's unique id
            var parameterValueSet =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "af5c88c6-301f-497b-81f7-53748c3900ed");
            ParameterValueSetTestFixture.VerifyProperties(parameterValueSet);
        }

        [Test]
        public void VerifyThatAParameteValueSetCanBeDeletedAndCreatedtWithWebApi()
        {
            var iterationUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c"));
            var postBodyPath = this.GetPath("Tests/EngineeringModel/ParameterValueSet/PostNewParameterValueSetAndDeleteOne.json");

            var postBody = base.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeeringModel =
               jArray.Single(x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.AreEqual(2, (int)engineeeringModel[PropertyNames.RevisionNumber]);

            var parameterSubscription =
               jArray.Single(x => (string)x[PropertyNames.Iid] == "f1f076c4-5307-42b8-a171-3263a9e7bb21");
            Assert.AreEqual(2, (int)parameterSubscription[PropertyNames.RevisionNumber]);

            var parameterOverride =
              jArray.Single(x => (string)x[PropertyNames.Iid] == "93f767ed-4d22-45f6-ae97-d1dab0d36e1c");
            Assert.AreEqual(2, (int)parameterOverride[PropertyNames.RevisionNumber]);

            var parameter =
              jArray.Single(x => (string)x[PropertyNames.Iid] == "6c5aff74-f983-4aa8-a9d6-293b3429307c");
            Assert.AreEqual(2, (int)parameter[PropertyNames.RevisionNumber]);

            var expectedValueSets = new string[]
            {
                "d2936657-95b3-4b27-bf98-a19752dc2c7f"
            };
            var valueSetsArray = (JArray)parameter[PropertyNames.ValueSet];
            IList<string> valueSets = valueSetsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedValueSets, valueSets);

            var parameterValueSet =
              jArray.Single(x => (string)x[PropertyNames.Iid] == "d2936657-95b3-4b27-bf98-a19752dc2c7f");
            Assert.AreEqual(2, (int)parameter[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ParameterValueSet", (string)parameterValueSet[PropertyNames.ClassKind]);

            Assert.AreEqual("MANUAL", (string)parameterValueSet[PropertyNames.ValueSwitch]);

            const string emptyProperty = "[\"-\"]";
            Assert.AreEqual(emptyProperty, (string)parameterValueSet[PropertyNames.Published]);
            Assert.AreEqual(emptyProperty, (string)parameterValueSet[PropertyNames.Formula]);
            Assert.AreEqual(emptyProperty, (string)parameterValueSet[PropertyNames.Computed]);
            Assert.AreEqual(emptyProperty, (string)parameterValueSet[PropertyNames.Manual]);
            Assert.AreEqual(emptyProperty, (string)parameterValueSet[PropertyNames.Reference]);

            Assert.IsNull((string)parameterValueSet[PropertyNames.ActualState]);
            Assert.IsNull((string)parameterValueSet[PropertyNames.ActualOption]);
        }

        /// <summary>
        /// Verifies all properties of the ParameterValueSet <see cref="JToken"/>
        /// </summary>
        /// <param name="parameterValueSet">
        /// The <see cref="JToken"/> that contains the properties of
        /// the ParameterValueSet object
        /// </param>
        public static void VerifyProperties(JToken parameterValueSet)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(11, parameterValueSet.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("af5c88c6-301f-497b-81f7-53748c3900ed",
                (string) parameterValueSet[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) parameterValueSet[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ParameterValueSet", (string) parameterValueSet[PropertyNames.ClassKind]);

            Assert.AreEqual("MANUAL", (string) parameterValueSet[PropertyNames.ValueSwitch]);

            const string emptyProperty = "[\"-\"]";
            Assert.AreEqual(emptyProperty, (string) parameterValueSet[PropertyNames.Published]);
            Assert.AreEqual(emptyProperty, (string) parameterValueSet[PropertyNames.Formula]);
            Assert.AreEqual(emptyProperty, (string) parameterValueSet[PropertyNames.Computed]);
            Assert.AreEqual(emptyProperty, (string) parameterValueSet[PropertyNames.Manual]);
            Assert.AreEqual(emptyProperty, (string) parameterValueSet[PropertyNames.Reference]);

            Assert.IsNull((string) parameterValueSet[PropertyNames.ActualState]);
            Assert.IsNull((string) parameterValueSet[PropertyNames.ActualOption]);
        }
    }
}