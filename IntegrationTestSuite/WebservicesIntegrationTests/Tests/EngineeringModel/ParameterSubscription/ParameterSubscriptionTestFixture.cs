// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterSubscriptionTestFixture.cs" company="Starion Group S.A.">
//
//   Copyright 2016-2024 Starion Group S.A.
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
            var parameterSubscriptionUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/f73860b2-12f0-43e4-b8b2-c81862c0a159/parameter/6c5aff74-f983-4aa8-a9d6-293b3429307c/parameterSubscription");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(parameterSubscriptionUri);

            //check if there is the only one ParameterSubscription object 
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific ParameterSubscription from the result by it's unique id
            var parameterSubscription = jArray.Single(x => (string) x[PropertyNames.Iid] == "f1f076c4-5307-42b8-a171-3263a9e7bb21");

            ParameterSubscriptionTestFixture.VerifyProperties(parameterSubscription);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedParameterSubscriptionWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var parameterSubscriptionUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/f73860b2-12f0-43e4-b8b2-c81862c0a159/parameter/6c5aff74-f983-4aa8-a9d6-293b3429307c/parameterSubscription?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(parameterSubscriptionUri);

            //check if there are 5 objects
            Assert.That(jArray.Count, Is.EqualTo(5));

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
        }

        [Test]
        [Category("POST")]
        public void VerifyThatParameterSubscriptionOnNormalParameterCanBeCreatedFromWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/ParameterSubscription/PostNewParameterSubscription.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeeringModel = jArray.Single(x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.That((int)engineeeringModel[PropertyNames.RevisionNumber], Is.EqualTo(2));

            // get the updated Parameter from the result by it's unique id
            var parameter = jArray.Single(x => (string)x[PropertyNames.Iid] == "3f05483f-66ff-4f21-bc76-45956779f66e");

            // assert that the properties are what is expected
            Assert.That((int)parameter[PropertyNames.RevisionNumber], Is.EqualTo(2));

            var expectedParameterSubscriptions = new string[]
                                                     {
                                                         "41aaa45f-090f-461c-8b0c-a4018e8edc9d"
                                                     };
            var parameterSubscriptionsArray = (JArray)parameter[PropertyNames.ParameterSubscription];
            IList<string> parameterSubscriptions = parameterSubscriptionsArray.Select(x => (string)x).ToList();
            Assert.That(parameterSubscriptions, Is.EquivalentTo(expectedParameterSubscriptions));

            // get a specific ParameterSubscription from the result by it's unique id
            var parameterSubscription = jArray.Single(x => (string)x[PropertyNames.Iid] == "41aaa45f-090f-461c-8b0c-a4018e8edc9d");

            // verify the amount of returned properties 
            Assert.That(parameterSubscription.Children().Count(), Is.EqualTo(5));

            // assert that the properties are what is expected
            Assert.That((string)parameterSubscription[PropertyNames.Iid], Is.EqualTo("41aaa45f-090f-461c-8b0c-a4018e8edc9d"));
            Assert.That((int)parameterSubscription[PropertyNames.RevisionNumber], Is.EqualTo(2));
            Assert.That((string)parameterSubscription[PropertyNames.ClassKind], Is.EqualTo("ParameterSubscription"));

            Assert.That((string)parameterSubscription[PropertyNames.Owner], Is.EqualTo("0e92edde-fdff-41db-9b1d-f2e484f12535"));

            var valueSetsArray = (JArray)parameterSubscription[PropertyNames.ValueSet];
            Assert.That(valueSetsArray.Count, Is.EqualTo(1));

            // get a specific ParameterSubscriptionValueSet from the result by it's unique id
            var parameterSubscriptionValueSet = jArray.Single(x => (string)x[PropertyNames.Iid] == (string)valueSetsArray[0]);

            // verify the amount of returned properties 
            Assert.That(parameterSubscriptionValueSet.Children().Count(), Is.EqualTo(6));

            // assert that the properties are what is expected
            Assert.That((int)parameterSubscriptionValueSet[PropertyNames.RevisionNumber], Is.EqualTo(2));
            Assert.That((string)parameterSubscriptionValueSet[PropertyNames.ClassKind], Is.EqualTo("ParameterSubscriptionValueSet"));

            Assert.That((string)parameterSubscriptionValueSet[PropertyNames.SubscribedValueSet], Is.EqualTo("72ec3701-bcb5-4bf6-bd78-30fd1b65e3be"));

            const string emptyProperty = "[\"-\"]";
            Assert.That((string)parameterSubscriptionValueSet[PropertyNames.Manual], Is.EqualTo(emptyProperty));

            Assert.That((string)parameterSubscriptionValueSet[PropertyNames.ValueSwitch], Is.EqualTo("COMPUTED"));
        }

        [Test]
        [Category("POST")]
        public void VerifyThatParameterSubscriptionOnParameterOverrideCanBeCreatedFromWebApi()
        {
            //Create ParameterOverride
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/ParameterOverride/PostNewParameterOverride.json");

            var postBody = base.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeeringModel = jArray.Single(x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.That((int)engineeeringModel[PropertyNames.RevisionNumber], Is.EqualTo(2));

            //CreateOverride
            postBodyPath = this.GetPath("Tests/EngineeringModel/ParameterSubscription/PostNewParameterSubscriptionOnParameterOverride.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(iterationUri, postBody);

            engineeeringModel = jArray.Single(x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.That((int)engineeeringModel[PropertyNames.RevisionNumber], Is.EqualTo(3));
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
            Assert.That(parameterSubscription.Children().Count(), Is.EqualTo(5));

            // assert that the properties are what is expected
            Assert.That((string)parameterSubscription[PropertyNames.Iid], Is.EqualTo("f1f076c4-5307-42b8-a171-3263a9e7bb21"));
            Assert.That((int)parameterSubscription[PropertyNames.RevisionNumber], Is.EqualTo(1));
            Assert.That((string)parameterSubscription[PropertyNames.ClassKind], Is.EqualTo("ParameterSubscription"));

            Assert.That((string)parameterSubscription[PropertyNames.Owner], Is.EqualTo("0e92edde-fdff-41db-9b1d-f2e484f12535"));

            var expectedValueSets = new string[]
            {
                "a179af79-fd09-44c8-a8a6-3c4c602c7dbf"
            };
            var valueSetsArray = (JArray) parameterSubscription[PropertyNames.ValueSet];
            IList<string> valueSets = valueSetsArray.Select(x => (string) x).ToList();
            Assert.That(valueSets, Is.EquivalentTo(expectedValueSets));
        }
    }
}
