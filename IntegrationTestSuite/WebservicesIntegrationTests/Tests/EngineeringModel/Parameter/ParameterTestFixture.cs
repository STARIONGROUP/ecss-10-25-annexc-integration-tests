// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterTestFixture.cs" company="RHEA System">
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
    public class ParameterTestFixture : WebClientTestFixtureBase
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
        /// Verification that the Parameter objects are returned from the data-source and that the 
        /// values of the Parameter properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedParameterIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var parameterUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/f73860b2-12f0-43e4-b8b2-c81862c0a159/parameter"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(parameterUri);

            //check if there is the only one Parameter object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific Parameter from the result by it's unique id
            var parameter =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "6c5aff74-f983-4aa8-a9d6-293b3429307c");

            ParameterTestFixture.VerifyProperties(parameter);
        }

        [Test]
        public void VerifyThatExpectedParameterWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var parameterUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/f73860b2-12f0-43e4-b8b2-c81862c0a159/parameter?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(parameterUri);

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

            // get a specific Parameter from the result by it's unique id
            var parameter =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "6c5aff74-f983-4aa8-a9d6-293b3429307c");
            ParameterTestFixture.VerifyProperties(parameter);
        }

        [Test]
        public void VerifyThatAParameterCanBeCreatedWithWebApi()
        {
            var iterationUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c"));
            var postBodyPath = this.GetPath("Tests/EngineeringModel/Parameter/PostNewParameter.json");

            var postBody = base.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeeringModel =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.AreEqual(2, (int) engineeeringModel[PropertyNames.RevisionNumber]);

            // get a specific ElementDefinition from the result by it's unique id
            var elementDefinition =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "f73860b2-12f0-43e4-b8b2-c81862c0a159");
            Assert.AreEqual(2, (int) elementDefinition[PropertyNames.RevisionNumber]);

            var expectedParameters = new string[]
            {
                "6c5aff74-f983-4aa8-a9d6-293b3429307c",
                "2cd4eb9c-e92c-41b2-968c-f03ff7010bad"
            };
            var parametersArray = (JArray) elementDefinition[PropertyNames.Parameter];
            IList<string> parameters = parametersArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedParameters, parameters);

            // get the added Parameter from the result by it's unique id
            var parameter = jArray.Single(x => (string) x[PropertyNames.Iid] == "2cd4eb9c-e92c-41b2-968c-f03ff7010bad");

            // verify the amount of returned properties 
            Assert.AreEqual(14, parameter.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("2cd4eb9c-e92c-41b2-968c-f03ff7010bad",
                (string) parameter[PropertyNames.Iid]);
            Assert.AreEqual(2, (int) parameter[PropertyNames.RevisionNumber]);
            Assert.AreEqual("Parameter", (string) parameter[PropertyNames.ClassKind]);

            Assert.IsNull((string) parameter[PropertyNames.RequestedBy]);
            Assert.IsFalse((bool) parameter[PropertyNames.AllowDifferentOwnerOfOverride]);
            Assert.IsFalse((bool) parameter[PropertyNames.ExpectsOverride]);
            Assert.AreEqual("a21c15c4-3e1e-46b5-b109-5063dec1e254", (string) parameter[PropertyNames.ParameterType]);
            Assert.IsNull((string) parameter[PropertyNames.Scale]);
            Assert.IsNull((string) parameter[PropertyNames.StateDependence]);
            Assert.IsNull((string) parameter[PropertyNames.Group]);
            Assert.IsFalse((bool) parameter[PropertyNames.IsOptionDependent]);
            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string) parameter[PropertyNames.Owner]);

            var expectedParameterSubscriptions = new string[] {};
            var parameterSubscriptionsArray = (JArray) parameter[PropertyNames.ParameterSubscription];
            IList<string> parameterSubscriptions = parameterSubscriptionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedParameterSubscriptions, parameterSubscriptions);

            // get the created ParameterValueSet as a side effect of creating Parameter from the result by it's unique id
            var valueSetsArray = (JArray) parameter[PropertyNames.ValueSet];
            IList<string> valueSets = valueSetsArray.Select(x => (string) x).ToList();
            Assert.AreEqual(1, valueSets.Count);

            var parameterValueSet = jArray.Single(x => (string) x[PropertyNames.Iid] == valueSets[0]);

            // verify the amount of returned properties 
            Assert.AreEqual(11, parameterValueSet.Children().Count());
            // assert that the properties are what is expected
            Assert.AreEqual(valueSets[0],
                (string) parameterValueSet[PropertyNames.Iid]);
            Assert.AreEqual(2, (int) parameterValueSet[PropertyNames.RevisionNumber]);
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

        [Test]
        public void VerifyThatAParameterOfCompoundParameterTypeCanBeCreatedWithWebApi()
        {
            var iterationUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c"));
            var postBodyPath =
                this.GetPath("Tests/EngineeringModel/Parameter/PostNewParameterOfCompoundParameterType.json");

            var postBody = base.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeeringModel =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.AreEqual(2, (int) engineeeringModel[PropertyNames.RevisionNumber]);

            // get a specific ElementDefinition from the result by it's unique id
            var elementDefinition =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "f73860b2-12f0-43e4-b8b2-c81862c0a159");
            Assert.AreEqual(2, (int) elementDefinition[PropertyNames.RevisionNumber]);

            var expectedParameters = new string[]
            {
                "6c5aff74-f983-4aa8-a9d6-293b3429307c",
                "2460b6a5-08ff-4cc3-a2cc-8fd5c5cf2736"
            };
            var parametersArray = (JArray) elementDefinition[PropertyNames.Parameter];
            IList<string> parameters = parametersArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedParameters, parameters);

            // get the added Parameter from the result by it's unique id
            var parameter = jArray.Single(x => (string) x[PropertyNames.Iid] == "2460b6a5-08ff-4cc3-a2cc-8fd5c5cf2736");

            // verify the amount of returned properties 
            Assert.AreEqual(14, parameter.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("2460b6a5-08ff-4cc3-a2cc-8fd5c5cf2736",
                (string) parameter[PropertyNames.Iid]);
            Assert.AreEqual(2, (int) parameter[PropertyNames.RevisionNumber]);
            Assert.AreEqual("Parameter", (string) parameter[PropertyNames.ClassKind]);

            Assert.IsNull((string) parameter[PropertyNames.RequestedBy]);
            Assert.IsFalse((bool) parameter[PropertyNames.AllowDifferentOwnerOfOverride]);
            Assert.IsFalse((bool) parameter[PropertyNames.ExpectsOverride]);
            Assert.AreEqual("4a783624-b2bc-4e6d-95b3-11d036f6e917", (string) parameter[PropertyNames.ParameterType]);
            Assert.IsNull((string) parameter[PropertyNames.Scale]);
            Assert.IsNull((string) parameter[PropertyNames.StateDependence]);
            Assert.IsNull((string) parameter[PropertyNames.Group]);
            Assert.IsFalse((bool) parameter[PropertyNames.IsOptionDependent]);
            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string) parameter[PropertyNames.Owner]);

            var expectedParameterSubscriptions = new string[] {};
            var parameterSubscriptionsArray = (JArray) parameter[PropertyNames.ParameterSubscription];
            IList<string> parameterSubscriptions = parameterSubscriptionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedParameterSubscriptions, parameterSubscriptions);

            // get the created ParameterValueSet as a side effect of creating Parameter from the result by it's unique id
            var valueSetsArray = (JArray) parameter[PropertyNames.ValueSet];
            IList<string> valueSets = valueSetsArray.Select(x => (string) x).ToList();
            Assert.AreEqual(1, valueSets.Count);

            var parameterValueSet = jArray.Single(x => (string) x[PropertyNames.Iid] == valueSets[0]);

            // verify the amount of returned properties 
            Assert.AreEqual(11, parameterValueSet.Children().Count());
            // assert that the properties are what is expected
            Assert.AreEqual(valueSets[0],
                (string) parameterValueSet[PropertyNames.Iid]);
            Assert.AreEqual(2, (int) parameterValueSet[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ParameterValueSet", (string) parameterValueSet[PropertyNames.ClassKind]);

            Assert.AreEqual("MANUAL", (string) parameterValueSet[PropertyNames.ValueSwitch]);

            const string emptyProperty = "[\"-\",\"-\"]";
            Assert.AreEqual(emptyProperty, (string) parameterValueSet[PropertyNames.Published]);
            Assert.AreEqual(emptyProperty, (string) parameterValueSet[PropertyNames.Formula]);
            Assert.AreEqual(emptyProperty, (string) parameterValueSet[PropertyNames.Computed]);
            Assert.AreEqual(emptyProperty, (string) parameterValueSet[PropertyNames.Manual]);
            Assert.AreEqual(emptyProperty, (string) parameterValueSet[PropertyNames.Reference]);

            Assert.IsNull((string) parameterValueSet[PropertyNames.ActualState]);
            Assert.IsNull((string) parameterValueSet[PropertyNames.ActualOption]);
        }

        [Test]
        public void VerifyThatAnOptionDependentParameterCanBeCreatedWithWebApi()
        {
            var iterationUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c"));
            var postBodyPath = this.GetPath("Tests/EngineeringModel/Parameter/PostNewOptionDependentParameter.json");

            var postBody = base.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeeringModel =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.AreEqual(2, (int) engineeeringModel[PropertyNames.RevisionNumber]);

            // get a specific ElementDefinition from the result by it's unique id
            var elementDefinition =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "f73860b2-12f0-43e4-b8b2-c81862c0a159");
            Assert.AreEqual(2, (int) elementDefinition[PropertyNames.RevisionNumber]);

            var expectedParameters = new string[]
            {
                "6c5aff74-f983-4aa8-a9d6-293b3429307c",
                "9600b225-a4be-47b1-92b1-4dc2d8894ea3"
            };
            var parametersArray = (JArray) elementDefinition[PropertyNames.Parameter];
            IList<string> parameters = parametersArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedParameters, parameters);

            // get the added Parameter from the result by it's unique id
            var parameter = jArray.Single(x => (string) x[PropertyNames.Iid] == "9600b225-a4be-47b1-92b1-4dc2d8894ea3");

            // verify the amount of returned properties 
            Assert.AreEqual(14, parameter.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("9600b225-a4be-47b1-92b1-4dc2d8894ea3",
                (string) parameter[PropertyNames.Iid]);
            Assert.AreEqual(2, (int) parameter[PropertyNames.RevisionNumber]);
            Assert.AreEqual("Parameter", (string) parameter[PropertyNames.ClassKind]);

            Assert.IsNull((string) parameter[PropertyNames.RequestedBy]);
            Assert.IsFalse((bool) parameter[PropertyNames.AllowDifferentOwnerOfOverride]);
            Assert.IsFalse((bool) parameter[PropertyNames.ExpectsOverride]);
            Assert.AreEqual("a21c15c4-3e1e-46b5-b109-5063dec1e254", (string) parameter[PropertyNames.ParameterType]);
            Assert.IsNull((string) parameter[PropertyNames.Scale]);
            Assert.IsNull((string) parameter[PropertyNames.StateDependence]);
            Assert.IsNull((string) parameter[PropertyNames.Group]);
            Assert.IsTrue((bool) parameter[PropertyNames.IsOptionDependent]);
            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string) parameter[PropertyNames.Owner]);

            var expectedParameterSubscriptions = new string[] {};
            var parameterSubscriptionsArray = (JArray) parameter[PropertyNames.ParameterSubscription];
            IList<string> parameterSubscriptions = parameterSubscriptionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedParameterSubscriptions, parameterSubscriptions);

            // get the created ParameterValueSet as a side effect of creating Parameter from the result by it's unique id
            var valueSetsArray = (JArray) parameter[PropertyNames.ValueSet];
            IList<string> valueSets = valueSetsArray.Select(x => (string) x).ToList();
            Assert.AreEqual(1, valueSets.Count);

            var parameterValueSet = jArray.Single(x => (string) x[PropertyNames.Iid] == valueSets[0]);

            // verify the amount of returned properties 
            Assert.AreEqual(11, parameterValueSet.Children().Count());
            // assert that the properties are what is expected
            Assert.AreEqual(valueSets[0],
                (string) parameterValueSet[PropertyNames.Iid]);
            Assert.AreEqual(2, (int) parameterValueSet[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ParameterValueSet", (string) parameterValueSet[PropertyNames.ClassKind]);

            Assert.AreEqual("MANUAL", (string) parameterValueSet[PropertyNames.ValueSwitch]);

            const string emptyProperty = "[\"-\"]";
            Assert.AreEqual(emptyProperty, (string) parameterValueSet[PropertyNames.Published]);
            Assert.AreEqual(emptyProperty, (string) parameterValueSet[PropertyNames.Formula]);
            Assert.AreEqual(emptyProperty, (string) parameterValueSet[PropertyNames.Computed]);
            Assert.AreEqual(emptyProperty, (string) parameterValueSet[PropertyNames.Manual]);
            Assert.AreEqual(emptyProperty, (string) parameterValueSet[PropertyNames.Reference]);

            Assert.IsNull((string) parameterValueSet[PropertyNames.ActualState]);
            Assert.AreEqual("bebcc9f4-ff20-4569-bbf6-d1acf27a8107",
                (string) parameterValueSet[PropertyNames.ActualOption]);
        }

        /// <summary>
        /// Verifies all properties of the Parameter <see cref="JToken"/>
        /// </summary>
        /// <param name="parameter">
        /// The <see cref="JToken"/> that contains the properties of
        /// the Parameter object
        /// </param>
        public static void VerifyProperties(JToken parameter)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(14, parameter.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("6c5aff74-f983-4aa8-a9d6-293b3429307c",
                (string) parameter[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) parameter[PropertyNames.RevisionNumber]);
            Assert.AreEqual("Parameter", (string) parameter[PropertyNames.ClassKind]);

            Assert.IsNull((string) parameter[PropertyNames.RequestedBy]);
            Assert.IsFalse((bool) parameter[PropertyNames.AllowDifferentOwnerOfOverride]);
            Assert.IsFalse((bool) parameter[PropertyNames.ExpectsOverride]);
            Assert.AreEqual("a21c15c4-3e1e-46b5-b109-5063dec1e254", (string) parameter[PropertyNames.ParameterType]);
            Assert.IsNull((string) parameter[PropertyNames.Scale]);
            Assert.IsNull((string) parameter[PropertyNames.StateDependence]);
            Assert.IsNull((string) parameter[PropertyNames.Group]);
            Assert.IsFalse((bool) parameter[PropertyNames.IsOptionDependent]);
            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string) parameter[PropertyNames.Owner]);

            var expectedValueSets = new string[]
            {
                "af5c88c6-301f-497b-81f7-53748c3900ed"
            };
            var valueSetsArray = (JArray) parameter[PropertyNames.ValueSet];
            IList<string> valueSets = valueSetsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedValueSets, valueSets);

            var expectedParameterSubscriptions = new string[]
            {
                "f1f076c4-5307-42b8-a171-3263a9e7bb21"
            };
            var parameterSubscriptionsArray = (JArray) parameter[PropertyNames.ParameterSubscription];
            IList<string> parameterSubscriptions = parameterSubscriptionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedParameterSubscriptions, parameterSubscriptions);
        }
    }
}