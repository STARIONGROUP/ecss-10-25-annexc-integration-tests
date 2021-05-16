// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterSubscriptionTestFixture.cs" company="RHEA System S.A.">
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
    public class ParameterSubscriptionTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedParameterSubscriptionIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var parameterSubscriptionUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/f73860b2-12f0-43e4-b8b2-c81862c0a159/parameter/6c5aff74-f983-4aa8-a9d6-293b3429307c/parameterSubscription"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(parameterSubscriptionUri);

            //check if there is the only one ParameterSubscription object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific ParameterSubscription from the result by it's unique id
            var parameterSubscription =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "f1f076c4-5307-42b8-a171-3263a9e7bb21");

            ParameterSubscriptionTestFixture.VerifyProperties(parameterSubscription);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedParameterSubscriptionWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var parameterSubscriptionUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/f73860b2-12f0-43e4-b8b2-c81862c0a159/parameter/6c5aff74-f983-4aa8-a9d6-293b3429307c/parameterSubscription?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(parameterSubscriptionUri);

            //check if there are 5 objects
            Assert.AreEqual(5, jArray.Count);

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
        }

        [Test]
        [Category("POST")]
        public void VerifyThatParameterSubscriptionCanBeCreatedFromWebApi()
        {
            var iterationUri = new Uri(
                string.Format(
                    UriFormat,
                    this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c"));
            var postBodyPath = this.GetPath("Tests/EngineeringModel/ParameterSubscription/PostNewParameterSubscription.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeeringModel = jArray.Single(
                x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.AreEqual(2, (int)engineeeringModel[PropertyNames.RevisionNumber]);

            // get the updated Parameter from the result by it's unique id
            var parameter = jArray.Single(x => (string)x[PropertyNames.Iid] == "3f05483f-66ff-4f21-bc76-45956779f66e");

            // assert that the properties are what is expected
            Assert.AreEqual(2, (int)parameter[PropertyNames.RevisionNumber]);

            var expectedParameterSubscriptions = new string[]
                                                     {
                                                         "41aaa45f-090f-461c-8b0c-a4018e8edc9d"
                                                     };
            var parameterSubscriptionsArray = (JArray)parameter[PropertyNames.ParameterSubscription];
            IList<string> parameterSubscriptions = parameterSubscriptionsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedParameterSubscriptions, parameterSubscriptions);

            // get a specific ParameterSubscription from the result by it's unique id
            var parameterSubscription =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "41aaa45f-090f-461c-8b0c-a4018e8edc9d");

            // verify the amount of returned properties 
            Assert.AreEqual(5, parameterSubscription.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("41aaa45f-090f-461c-8b0c-a4018e8edc9d", (string)parameterSubscription[PropertyNames.Iid]);
            Assert.AreEqual(2, (int)parameterSubscription[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ParameterSubscription", (string)parameterSubscription[PropertyNames.ClassKind]);

            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string)parameterSubscription[PropertyNames.Owner]);

            var valueSetsArray = (JArray)parameterSubscription[PropertyNames.ValueSet];
            Assert.AreEqual(1, valueSetsArray.Count);

            // get a specific ParameterSubscriptionValueSet from the result by it's unique id
            var parameterSubscriptionValueSet =
                jArray.Single(x => (string)x[PropertyNames.Iid] == (string)valueSetsArray[0]);

            // verify the amount of returned properties 
            Assert.AreEqual(6, parameterSubscriptionValueSet.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual(2, (int)parameterSubscriptionValueSet[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ParameterSubscriptionValueSet",
                (string)parameterSubscriptionValueSet[PropertyNames.ClassKind]);

            Assert.AreEqual("72ec3701-bcb5-4bf6-bd78-30fd1b65e3be",
                (string)parameterSubscriptionValueSet[PropertyNames.SubscribedValueSet]);

            const string emptyProperty = "[\"-\"]";
            Assert.AreEqual(emptyProperty, (string)parameterSubscriptionValueSet[PropertyNames.Manual]);

            Assert.AreEqual("COMPUTED", (string)parameterSubscriptionValueSet[PropertyNames.ValueSwitch]);
        }

        /// <summary>
        /// Verifies all properties of the ParameterSubscription <see cref="JToken"/>
        /// </summary>
        /// <param name="parameterSubscription">
        /// The <see cref="JToken"/> that contains the properties of
        /// the ParameterSubscription object
        /// </param>
        public static void VerifyProperties(JToken parameterSubscription)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(5, parameterSubscription.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("f1f076c4-5307-42b8-a171-3263a9e7bb21", (string) parameterSubscription[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) parameterSubscription[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ParameterSubscription", (string) parameterSubscription[PropertyNames.ClassKind]);

            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string) parameterSubscription[PropertyNames.Owner]);

            var expectedValueSets = new string[]
            {
                "a179af79-fd09-44c8-a8a6-3c4c602c7dbf"
            };
            var valueSetsArray = (JArray) parameterSubscription[PropertyNames.ValueSet];
            IList<string> valueSets = valueSetsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedValueSets, valueSets);
        }
    }
}
