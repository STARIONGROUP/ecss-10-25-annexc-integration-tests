// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterTestFixture.cs" company="Starion Group S.A.">
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
    using System.Net;

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class ParameterTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedParameterIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var parameterUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/f73860b2-12f0-43e4-b8b2-c81862c0a159/parameter");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(parameterUri);

            // check if there appropriate amount of Parameter objects 
            Assert.AreEqual(2, jArray.Count);

            // get a specific Parameter from the result by it's unique id
            var parameter = jArray.Single(x => (string) x[PropertyNames.Iid] == "6c5aff74-f983-4aa8-a9d6-293b3429307c");

            VerifyProperties(parameter);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedParameterWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var parameterUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/f73860b2-12f0-43e4-b8b2-c81862c0a159/parameter?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(parameterUri);

            // check if there are appropriate amount of objects
            Assert.AreEqual(5, jArray.Count);

            // get a specific Iteration from the result by it's unique id
            var iteration = jArray.Single(x => (string) x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific ElementDefinition from the result by it's unique id
            ElementDefinitionTestFixture.VerifyProperties(jArray);

            // get a specific Parameter from the result by it's unique id
            var parameter = jArray.Single(x => (string) x[PropertyNames.Iid] == "6c5aff74-f983-4aa8-a9d6-293b3429307c");
            VerifyProperties(parameter);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatAParameterCanBeCreatedWithWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");

            var postBodyPath = this.GetPath("Tests/EngineeringModel/Parameter/PostNewParameter.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeeringModel = jArray.Single(x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");

            Assert.AreEqual(2, (int) engineeeringModel[PropertyNames.RevisionNumber]);

            // get a specific ElementDefinition from the result by it's unique id
            var elementDefinition = jArray.Single(x => (string) x[PropertyNames.Iid] == "f73860b2-12f0-43e4-b8b2-c81862c0a159");

            Assert.AreEqual(2, (int) elementDefinition[PropertyNames.RevisionNumber]);

            var expectedParameters = new[]
            {
                "6c5aff74-f983-4aa8-a9d6-293b3429307c",
                "3f05483f-66ff-4f21-bc76-45956779f66e",
                "2cd4eb9c-e92c-41b2-968c-f03ff7010bad"
            };

            var parametersArray = (JArray) elementDefinition[PropertyNames.Parameter];
            IList<string> parameters = parametersArray.Select(x => (string) x).ToList();
            Assert.That(parameters, Is.EquivalentTo(expectedParameters));

            // get the added Parameter from the result by it's unique id
            var parameter = jArray.Single(x => (string) x[PropertyNames.Iid] == "2cd4eb9c-e92c-41b2-968c-f03ff7010bad");

            // verify the amount of returned properties 
            Assert.AreEqual(14, parameter.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("2cd4eb9c-e92c-41b2-968c-f03ff7010bad", (string) parameter[PropertyNames.Iid]);
            Assert.AreEqual(2, (int) parameter[PropertyNames.RevisionNumber]);
            Assert.AreEqual("Parameter", (string) parameter[PropertyNames.ClassKind]);

            Assert.IsNull((string) parameter[PropertyNames.RequestedBy]);
            Assert.IsFalse((bool) parameter[PropertyNames.AllowDifferentOwnerOfOverride]);
            Assert.IsFalse((bool) parameter[PropertyNames.ExpectsOverride]);
            Assert.AreEqual("35a9cf05-4eba-4cda-b60c-7cfeaac8f892", (string) parameter[PropertyNames.ParameterType]);
            Assert.IsNull((string) parameter[PropertyNames.Scale]);
            Assert.IsNull((string) parameter[PropertyNames.StateDependence]);
            Assert.IsNull((string) parameter[PropertyNames.Group]);
            Assert.IsFalse((bool) parameter[PropertyNames.IsOptionDependent]);
            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string) parameter[PropertyNames.Owner]);

            var expectedParameterSubscriptions = new string[] { };
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
            Assert.AreEqual(valueSets[0], (string) parameterValueSet[PropertyNames.Iid]);
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
        [Category("POST")]
        public void VerifyThatParameterCannotBeUpdatesWithExistingParameterTypeWithWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");

            //Step 1 : create new parameters
            var postBodyPath = this.GetPath("Tests/EngineeringModel/Parameter/PostNewParameter.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            //Step 2: try update newly created parameter
            postBodyPath = this.GetPath("Tests/EngineeringModel/Parameter/PostUpdateParameterTypeToExisting.json");

            postBody = this.GetJsonFromFile(postBodyPath);

            Assert.That(() =>
                this.WebClient.PostDto(iterationUri, postBody), Throws.TypeOf<WebException>()
                .With
                .Property(nameof(WebException.Response))
                .TypeOf<HttpWebResponse>()
                .And.Property(nameof(WebException.Response))
                .Property(nameof(HttpWebResponse.StatusCode))
                .EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        [Category("POST")]
        public void VerifyThatExistingParameterTypeCannotBeCreatedWithWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");

            var postBodyPath = this.GetPath("Tests/EngineeringModel/Parameter/PostNewParameterHavingExistingParameterType.json");

            var postBody = this.GetJsonFromFile(postBodyPath);

            Assert.That(() => 
                    this.WebClient.PostDto(iterationUri, postBody), Throws.TypeOf<WebException>()
                    .With
                    .Property(nameof(WebException.Response))
                    .TypeOf<HttpWebResponse>()
                    .And.Property(nameof(WebException.Response))
                        .Property(nameof(HttpWebResponse.StatusCode))
                        .EqualTo(HttpStatusCode.BadRequest)
            );
        }

        [Test]
        [Category("POST")]
        public void VerifyThatSameParameterTypeCannotBeCreatedWithWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");

            var postBodyPath = this.GetPath("Tests/EngineeringModel/Parameter/PostNewParametersHavingSameParameterType.json");

            var postBody = this.GetJsonFromFile(postBodyPath);

            Assert.That(() =>
                    this.WebClient.PostDto(iterationUri, postBody), Throws.TypeOf<WebException>()
                    .With
                    .Property(nameof(WebException.Response))
                    .TypeOf<HttpWebResponse>()
                    .And.Property(nameof(WebException.Response))
                    .Property(nameof(HttpWebResponse.StatusCode))
                    .EqualTo(HttpStatusCode.BadRequest)
            );
        }

        [Test]
        [Category("POST")]
        public void VerifyThatAParameterCanBeDeletedAndCreatedInOnRequestWithWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");

            var postBodyPath = this.GetPath("Tests/EngineeringModel/Parameter/PostDeleteCreateParameter.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeeringModel = jArray.Single(x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");

            Assert.AreEqual(2, (int) engineeeringModel[PropertyNames.RevisionNumber]);

            // get a specific ElementDefinition from the result by it's unique id
            var elementDefinition = jArray.Single(x => (string) x[PropertyNames.Iid] == "f73860b2-12f0-43e4-b8b2-c81862c0a159");

            Assert.AreEqual(2, (int) elementDefinition[PropertyNames.RevisionNumber]);

            var expectedParameters = new[]
            {
                "3f05483f-66ff-4f21-bc76-45956779f66e",
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
            Assert.AreEqual("2cd4eb9c-e92c-41b2-968c-f03ff7010bad", (string) parameter[PropertyNames.Iid]);
            Assert.AreEqual(2, (int) parameter[PropertyNames.RevisionNumber]);
            Assert.AreEqual("Parameter", (string) parameter[PropertyNames.ClassKind]);

            Assert.IsNull((string) parameter[PropertyNames.RequestedBy]);
            Assert.IsFalse((bool) parameter[PropertyNames.AllowDifferentOwnerOfOverride]);
            Assert.IsFalse((bool) parameter[PropertyNames.ExpectsOverride]);
            Assert.AreEqual("35a9cf05-4eba-4cda-b60c-7cfeaac8f892", (string) parameter[PropertyNames.ParameterType]);
            Assert.IsNull((string) parameter[PropertyNames.Scale]);
            Assert.IsNull((string) parameter[PropertyNames.StateDependence]);
            Assert.IsNull((string) parameter[PropertyNames.Group]);
            Assert.IsFalse((bool) parameter[PropertyNames.IsOptionDependent]);
            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string) parameter[PropertyNames.Owner]);

            var expectedParameterSubscriptions = new string[] { };
            var parameterSubscriptionsArray = (JArray) parameter[PropertyNames.ParameterSubscription];
            IList<string> parameterSubscriptions = parameterSubscriptionsArray.Select(x => (string) x).ToList();
            Assert.That(parameterSubscriptions, Is.EquivalentTo(expectedParameterSubscriptions));

            // get the created ParameterValueSet as a side effect of creating Parameter from the result by it's unique id
            var valueSetsArray = (JArray) parameter[PropertyNames.ValueSet];
            IList<string> valueSets = valueSetsArray.Select(x => (string) x).ToList();
            Assert.AreEqual(1, valueSets.Count);

            var parameterValueSet = jArray.Single(x => (string) x[PropertyNames.Iid] == valueSets[0]);

            // verify the amount of returned properties 
            Assert.AreEqual(11, parameterValueSet.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual(valueSets[0], (string) parameterValueSet[PropertyNames.Iid]);
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
        [Category("POST")]
        public void VerifyThatAParameterOfCompoundParameterTypeCanBeCreatedWithWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");

            var postBodyPath = this.GetPath("Tests/EngineeringModel/Parameter/PostNewParameterOfCompoundParameterType.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeeringModel = jArray.Single(x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");

            Assert.AreEqual(2, (int) engineeeringModel[PropertyNames.RevisionNumber]);

            // get a specific ElementDefinition from the result by it's unique id
            var elementDefinition = jArray.Single(x => (string) x[PropertyNames.Iid] == "f73860b2-12f0-43e4-b8b2-c81862c0a159");

            Assert.AreEqual(2, (int) elementDefinition[PropertyNames.RevisionNumber]);

            var expectedParameters = new[]
            {
                "6c5aff74-f983-4aa8-a9d6-293b3429307c",
                "3f05483f-66ff-4f21-bc76-45956779f66e",
                "2460b6a5-08ff-4cc3-a2cc-8fd5c5cf2736"
            };

            var parametersArray = (JArray) elementDefinition[PropertyNames.Parameter];
            IList<string> parameters = parametersArray.Select(x => (string) x).ToList();
            Assert.That(parameters, Is.EquivalentTo(expectedParameters));

            // get the added Parameter from the result by it's unique id
            var parameter = jArray.Single(x => (string) x[PropertyNames.Iid] == "2460b6a5-08ff-4cc3-a2cc-8fd5c5cf2736");

            // verify the amount of returned properties 
            Assert.AreEqual(14, parameter.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("2460b6a5-08ff-4cc3-a2cc-8fd5c5cf2736", (string) parameter[PropertyNames.Iid]);
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

            var expectedParameterSubscriptions = new string[] { };
            var parameterSubscriptionsArray = (JArray) parameter[PropertyNames.ParameterSubscription];
            IList<string> parameterSubscriptions = parameterSubscriptionsArray.Select(x => (string) x).ToList();
            Assert.That(parameterSubscriptions, Is.EquivalentTo(expectedParameterSubscriptions));

            // get the created ParameterValueSet as a side effect of creating Parameter from the result by it's unique id
            var valueSetsArray = (JArray) parameter[PropertyNames.ValueSet];
            IList<string> valueSets = valueSetsArray.Select(x => (string) x).ToList();
            Assert.AreEqual(1, valueSets.Count);

            var parameterValueSet = jArray.Single(x => (string) x[PropertyNames.Iid] == valueSets[0]);

            // verify the amount of returned properties 
            Assert.AreEqual(11, parameterValueSet.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual(valueSets[0], (string) parameterValueSet[PropertyNames.Iid]);
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
        [Category("POST")]
        public void VerifyThatAnOptionDependentParameterCanBeCreatedWithWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/Parameter/PostNewOptionDependentParameter.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeeringModel = jArray.Single(x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");

            Assert.AreEqual(2, (int) engineeeringModel[PropertyNames.RevisionNumber]);

            // get a specific ElementDefinition from the result by it's unique id
            var elementDefinition = jArray.Single(x => (string) x[PropertyNames.Iid] == "f73860b2-12f0-43e4-b8b2-c81862c0a159");

            Assert.AreEqual(2, (int) elementDefinition[PropertyNames.RevisionNumber]);

            var expectedParameters = new[]
            {
                "6c5aff74-f983-4aa8-a9d6-293b3429307c",
                "3f05483f-66ff-4f21-bc76-45956779f66e",
                "9600b225-a4be-47b1-92b1-4dc2d8894ea3"
            };

            var parametersArray = (JArray) elementDefinition[PropertyNames.Parameter];
            IList<string> parameters = parametersArray.Select(x => (string) x).ToList();
            Assert.That(parameters, Is.EquivalentTo(expectedParameters));

            // get the added Parameter from the result by it's unique id
            var parameter = jArray.Single(x => (string) x[PropertyNames.Iid] == "9600b225-a4be-47b1-92b1-4dc2d8894ea3");

            // verify the amount of returned properties 
            Assert.AreEqual(14, parameter.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("9600b225-a4be-47b1-92b1-4dc2d8894ea3", (string) parameter[PropertyNames.Iid]);
            Assert.AreEqual(2, (int) parameter[PropertyNames.RevisionNumber]);
            Assert.AreEqual("Parameter", (string) parameter[PropertyNames.ClassKind]);

            Assert.IsNull((string) parameter[PropertyNames.RequestedBy]);
            Assert.IsFalse((bool) parameter[PropertyNames.AllowDifferentOwnerOfOverride]);
            Assert.IsFalse((bool) parameter[PropertyNames.ExpectsOverride]);
            Assert.AreEqual("35a9cf05-4eba-4cda-b60c-7cfeaac8f892", (string) parameter[PropertyNames.ParameterType]);
            Assert.IsNull((string) parameter[PropertyNames.Scale]);
            Assert.IsNull((string) parameter[PropertyNames.StateDependence]);
            Assert.IsNull((string) parameter[PropertyNames.Group]);
            Assert.IsTrue((bool) parameter[PropertyNames.IsOptionDependent]);
            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string) parameter[PropertyNames.Owner]);

            var expectedParameterSubscriptions = new string[] { };
            var parameterSubscriptionsArray = (JArray) parameter[PropertyNames.ParameterSubscription];
            IList<string> parameterSubscriptions = parameterSubscriptionsArray.Select(x => (string) x).ToList();
            Assert.That(parameterSubscriptions, Is.EquivalentTo(expectedParameterSubscriptions));

            // get the created ParameterValueSet as a side effect of creating Parameter from the result by it's unique id
            var valueSetsArray = (JArray) parameter[PropertyNames.ValueSet];
            IList<string> valueSets = valueSetsArray.Select(x => (string) x).ToList();
            Assert.AreEqual(1, valueSets.Count);

            var parameterValueSet = jArray.Single(x => (string) x[PropertyNames.Iid] == valueSets[0]);

            // verify the amount of returned properties 
            Assert.AreEqual(11, parameterValueSet.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual(valueSets[0], (string) parameterValueSet[PropertyNames.Iid]);
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

            Assert.AreEqual(
                "bebcc9f4-ff20-4569-bbf6-d1acf27a8107",
                (string) parameterValueSet[PropertyNames.ActualOption]);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatAParameterCanBeUpdatedToOptionDependentWithWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/Parameter/PostUpdateParameterToOptionDependent.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeeringModel = jArray.Single(x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");

            Assert.AreEqual(2, (int) engineeeringModel[PropertyNames.RevisionNumber]);

            var parameterValueSet = jArray.Single(x => (string) x[PropertyNames.ClassKind] == "ParameterValueSet");
            Assert.AreEqual(2, (int) parameterValueSet[PropertyNames.RevisionNumber]);

            var parameterOverrideValueSet = jArray.Single(x => (string) x[PropertyNames.ClassKind] == "ParameterOverrideValueSet");

            Assert.AreEqual(2, (int) parameterOverrideValueSet[PropertyNames.RevisionNumber]);

            Assert.AreEqual(
                (string) parameterValueSet[PropertyNames.Iid],
                (string) parameterOverrideValueSet[PropertyNames.ParameterValueSet]);

            var parameterSubscriptionValueSet =
                jArray.Single(x => (string) x[PropertyNames.ClassKind] == "ParameterSubscriptionValueSet");

            Assert.AreEqual(2, (int) parameterSubscriptionValueSet[PropertyNames.RevisionNumber]);

            Assert.AreEqual(
                (string) parameterValueSet[PropertyNames.Iid],
                (string) parameterSubscriptionValueSet[PropertyNames.SubscribedValueSet]);

            var parameterSubscription = jArray.Single(
                x => (string) x[PropertyNames.Iid] == "f1f076c4-5307-42b8-a171-3263a9e7bb21");

            Assert.AreEqual(2, (int) parameterSubscription[PropertyNames.RevisionNumber]);

            Assert.AreEqual(
                (string) parameterSubscriptionValueSet[PropertyNames.Iid],
                (string) parameterSubscription[PropertyNames.ValueSet][0]);

            var parameter = jArray.Single(x => (string) x[PropertyNames.Iid] == "6c5aff74-f983-4aa8-a9d6-293b3429307c");
            Assert.AreEqual(2, (int) parameter[PropertyNames.RevisionNumber]);
            var expectedValueSets = new[] { (string) parameterValueSet[PropertyNames.Iid] };
            var valueSetsArray = (JArray) parameter[PropertyNames.ValueSet];
            IList<string> valueSets = valueSetsArray.Select(x => (string) x).ToList();
            Assert.That(valueSets, Is.EquivalentTo(expectedValueSets));

            var parameterOverride = jArray.Single(
                x => (string) x[PropertyNames.Iid] == "93f767ed-4d22-45f6-ae97-d1dab0d36e1c");

            Assert.AreEqual(2, (int) parameterOverride[PropertyNames.RevisionNumber]);
            expectedValueSets = new[] { (string) parameterOverrideValueSet[PropertyNames.Iid] };
            valueSetsArray = (JArray) parameterOverride[PropertyNames.ValueSet];
            valueSets = valueSetsArray.Select(x => (string) x).ToList();
            Assert.That(valueSets, Is.EquivalentTo(expectedValueSets));
        }

        [Test]
        [Category("POST")]
        public void VerifyThatAParameterCanBeUpdatedToStateDependentWithWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/Parameter/PostUpdateParameterToStateDependent.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeeringModel = jArray.Single(
                x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");

            Assert.AreEqual(2, (int) engineeeringModel[PropertyNames.RevisionNumber]);

            var parameterValueSet = jArray.Single(x => (string) x[PropertyNames.ClassKind] == "ParameterValueSet");
            Assert.AreEqual(2, (int) parameterValueSet[PropertyNames.RevisionNumber]);

            const string EmptyProperty = "[\"-\"]";
            Assert.AreEqual("MANUAL", (string) parameterValueSet[PropertyNames.ValueSwitch]);
            Assert.AreEqual(EmptyProperty, (string) parameterValueSet[PropertyNames.Published]);
            Assert.AreEqual(EmptyProperty, (string) parameterValueSet[PropertyNames.Formula]);
            Assert.AreEqual(EmptyProperty, (string) parameterValueSet[PropertyNames.Computed]);
            Assert.AreEqual(EmptyProperty, (string) parameterValueSet[PropertyNames.Manual]);
            Assert.AreEqual(EmptyProperty, (string) parameterValueSet[PropertyNames.Reference]);

            Assert.AreEqual(
                "b91bfdbb-4277-4a03-b519-e4db839ef5d4",
                (string) parameterValueSet[PropertyNames.ActualState]);

            Assert.IsNull((string) parameterValueSet[PropertyNames.ActualOption]);

            var parameterOverride = jArray.Single(x => (string) x[PropertyNames.ClassKind] == "ParameterOverride");
            Assert.AreEqual(2, (int) parameterOverride[PropertyNames.RevisionNumber]);
            var valueSetsArray = (JArray) parameterOverride[PropertyNames.ValueSet];
            var valueSets = valueSetsArray.Select(x => (string) x).ToList();
            Assert.AreEqual(1, valueSets.Count);
            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string) parameterOverride[PropertyNames.Owner]);
            Assert.AreEqual("6c5aff74-f983-4aa8-a9d6-293b3429307c", (string) parameterOverride[PropertyNames.Parameter]);

            var expectedParameterSubscriptions = new string[] { };
            var parameterSubscriptionsArray = (JArray) parameterOverride[PropertyNames.ParameterSubscription];
            IList<string> parameterSubscriptions = parameterSubscriptionsArray.Select(x => (string) x).ToList();
            Assert.That(parameterSubscriptions, Is.EquivalentTo(expectedParameterSubscriptions));

            var parameterOverrideValueSet = jArray.Single(
                x => (string) x[PropertyNames.Iid] == (string) parameterOverride[PropertyNames.ValueSet][0]);

            Assert.AreEqual(2, (int) parameterOverrideValueSet[PropertyNames.RevisionNumber]);

            Assert.AreEqual(
                (string) parameterValueSet[PropertyNames.Iid],
                (string) parameterOverrideValueSet[PropertyNames.ParameterValueSet]);

            Assert.AreEqual("MANUAL", (string) parameterOverrideValueSet[PropertyNames.ValueSwitch]);

            Assert.AreEqual(EmptyProperty, (string) parameterOverrideValueSet[PropertyNames.Published]);
            Assert.AreEqual(EmptyProperty, (string) parameterOverrideValueSet[PropertyNames.Formula]);
            Assert.AreEqual(EmptyProperty, (string) parameterOverrideValueSet[PropertyNames.Computed]);
            Assert.AreEqual(EmptyProperty, (string) parameterOverrideValueSet[PropertyNames.Manual]);
            Assert.AreEqual(EmptyProperty, (string) parameterOverrideValueSet[PropertyNames.Reference]);

            var parameterSubscription = jArray.Single(
                x => (string) x[PropertyNames.Iid] == "f1f076c4-5307-42b8-a171-3263a9e7bb21");

            Assert.AreEqual(2, (int) parameterSubscription[PropertyNames.RevisionNumber]);
            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string) parameterOverride[PropertyNames.Owner]);

            valueSetsArray = (JArray) parameterSubscription[PropertyNames.ValueSet];
            valueSets = valueSetsArray.Select(x => (string) x).ToList();
            Assert.AreEqual(1, valueSets.Count);

            var parameterSubscriptionValueSet = jArray.Single(
                x => (string) x[PropertyNames.Iid] == (string) parameterSubscription[PropertyNames.ValueSet][0]);

            Assert.AreEqual(2, (int) parameterSubscriptionValueSet[PropertyNames.RevisionNumber]);

            Assert.AreEqual(
                (string) parameterValueSet[PropertyNames.Iid],
                (string) parameterSubscriptionValueSet[PropertyNames.SubscribedValueSet]);

            Assert.AreEqual(
                (string) parameterValueSet[PropertyNames.Iid],
                (string) parameterSubscriptionValueSet[PropertyNames.SubscribedValueSet]);

            Assert.AreEqual(EmptyProperty, (string) parameterSubscriptionValueSet[PropertyNames.Manual]);
            Assert.AreEqual("MANUAL", (string) parameterSubscriptionValueSet[PropertyNames.ValueSwitch]);

            var parameter = jArray.Single(x => (string) x[PropertyNames.Iid] == "6c5aff74-f983-4aa8-a9d6-293b3429307c");
            Assert.AreEqual(2, (int) parameter[PropertyNames.RevisionNumber]);
            var expectedValueSets = new[] { (string) parameterValueSet[PropertyNames.Iid] };
            valueSetsArray = (JArray) parameter[PropertyNames.ValueSet];
            valueSets = valueSetsArray.Select(x => (string) x).ToList();
            Assert.That(valueSets, Is.EquivalentTo(expectedValueSets));
            Assert.AreEqual("db690d7d-761c-47fd-96d3-840d698a89dc", (string) parameter[PropertyNames.StateDependence]);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatAStateDependentParameterCanBeUpdatedToAnotherStateDependentWithWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/Parameter/PostUpdateParameterToStateDependent.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeeringModel = jArray.Single(x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");

            Assert.AreEqual(2, (int) engineeeringModel[PropertyNames.RevisionNumber]);

            var parameterValueSet = jArray.Single(x => (string) x[PropertyNames.ClassKind] == "ParameterValueSet");
            Assert.AreEqual(2, (int) parameterValueSet[PropertyNames.RevisionNumber]);

            postBodyPath = this.GetPath("Tests/EngineeringModel/ActualFiniteStateList/PostNewActualFiniteStateList.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(iterationUri, postBody);

            engineeeringModel = jArray.Single(
                x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");

            Assert.AreEqual(3, (int) engineeeringModel[PropertyNames.RevisionNumber]);

            var iteration = jArray.Single(x => (string) x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            Assert.AreEqual(3, (int) iteration[PropertyNames.RevisionNumber]);

            postBodyPath = this.GetPath("Tests/EngineeringModel/Parameter/PostUpdateStateDependentParameterToAnotherState.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(iterationUri, postBody);

            engineeeringModel = jArray.Single(
                x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");

            Assert.AreEqual(4, (int) engineeeringModel[PropertyNames.RevisionNumber]);

            parameterValueSet = jArray.Single(x => (string) x[PropertyNames.ClassKind] == "ParameterValueSet");
            Assert.AreEqual(4, (int) parameterValueSet[PropertyNames.RevisionNumber]);

            const string EmptyProperty = "[\"-\"]";
            Assert.AreEqual("MANUAL", (string) parameterValueSet[PropertyNames.ValueSwitch]);
            Assert.AreEqual(EmptyProperty, (string) parameterValueSet[PropertyNames.Published]);
            Assert.AreEqual(EmptyProperty, (string) parameterValueSet[PropertyNames.Formula]);
            Assert.AreEqual(EmptyProperty, (string) parameterValueSet[PropertyNames.Computed]);
            Assert.AreEqual(EmptyProperty, (string) parameterValueSet[PropertyNames.Manual]);
            Assert.AreEqual(EmptyProperty, (string) parameterValueSet[PropertyNames.Reference]);

            var parameter = jArray.Single(x => (string) x[PropertyNames.ClassKind] == "Parameter");
            Assert.AreEqual(4, (int) parameter[PropertyNames.RevisionNumber]);

            var parameterSubscription = jArray.Single(x => (string) x[PropertyNames.ClassKind] == "ParameterSubscription");
            Assert.AreEqual(4, (int) parameterSubscription[PropertyNames.RevisionNumber]);

            var parameterOverride = jArray.Single(x => (string) x[PropertyNames.ClassKind] == "ParameterOverride");
            Assert.AreEqual(4, (int) parameterOverride[PropertyNames.RevisionNumber]);
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
            if ((string) parameter[PropertyNames.Iid] == "6c5aff74-f983-4aa8-a9d6-293b3429307c")
            {
                // verify the amount of returned properties 
                Assert.AreEqual(14, parameter.Children().Count());

                // assert that the properties are what is expected
                Assert.AreEqual("6c5aff74-f983-4aa8-a9d6-293b3429307c", (string) parameter[PropertyNames.Iid]);
                Assert.AreEqual(1, (int) parameter[PropertyNames.RevisionNumber]);
                Assert.AreEqual("Parameter", (string) parameter[PropertyNames.ClassKind]);

                Assert.IsNull((string) parameter[PropertyNames.RequestedBy]);
                Assert.IsFalse((bool) parameter[PropertyNames.AllowDifferentOwnerOfOverride]);
                Assert.IsFalse((bool) parameter[PropertyNames.ExpectsOverride]);
                Assert.AreEqual("a21c15c4-3e1e-46b5-b109-5063dec1e254", (string) parameter[PropertyNames.ParameterType]);
                Assert.IsNull((string) parameter[PropertyNames.Scale]);
                Assert.IsNull((string) parameter[PropertyNames.StateDependence]);
                Assert.AreEqual((string) parameter[PropertyNames.Group], "b739b3c6-9cc0-4e64-9cc4-ef7463edf559");
                Assert.IsFalse((bool) parameter[PropertyNames.IsOptionDependent]);
                Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string) parameter[PropertyNames.Owner]);

                var expectedValueSets = new[] { "af5c88c6-301f-497b-81f7-53748c3900ed" };
                var valueSetsArray = (JArray) parameter[PropertyNames.ValueSet];
                IList<string> valueSets = valueSetsArray.Select(x => (string) x).ToList();
                Assert.That(valueSets, Is.EquivalentTo(expectedValueSets));

                var expectedParameterSubscriptions = new[] { "f1f076c4-5307-42b8-a171-3263a9e7bb21" };
                var parameterSubscriptionsArray = (JArray) parameter[PropertyNames.ParameterSubscription];
                IList<string> parameterSubscriptions = parameterSubscriptionsArray.Select(x => (string) x).ToList();
                Assert.That(parameterSubscriptions, Is.EquivalentTo(expectedParameterSubscriptions));
            }

            if ((string) parameter[PropertyNames.Iid] == "3f05483f-66ff-4f21-bc76-45956779f66e")
            {
                // verify the amount of returned properties 
                Assert.AreEqual(14, parameter.Children().Count());

                // assert that the properties are what is expected
                Assert.AreEqual("3f05483f-66ff-4f21-bc76-45956779f66e", (string) parameter[PropertyNames.Iid]);
                Assert.AreEqual(1, (int) parameter[PropertyNames.RevisionNumber]);
                Assert.AreEqual("Parameter", (string) parameter[PropertyNames.ClassKind]);

                Assert.IsNull((string) parameter[PropertyNames.RequestedBy]);
                Assert.IsFalse((bool) parameter[PropertyNames.AllowDifferentOwnerOfOverride]);
                Assert.IsFalse((bool) parameter[PropertyNames.ExpectsOverride]);
                Assert.AreEqual("a21c15c4-3e1e-46b5-b109-5063dec1e254", (string) parameter[PropertyNames.ParameterType]);
                Assert.IsNull((string) parameter[PropertyNames.Scale]);
                Assert.IsNull((string) parameter[PropertyNames.StateDependence]);
                Assert.AreEqual((string) parameter[PropertyNames.Group], "b739b3c6-9cc0-4e64-9cc4-ef7463edf559");
                Assert.IsFalse((bool) parameter[PropertyNames.IsOptionDependent]);
                Assert.AreEqual("eb759723-14b9-49f4-8611-544d037bb764", (string) parameter[PropertyNames.Owner]);

                var expectedValueSets = new[] { "72ec3701-bcb5-4bf6-bd78-30fd1b65e3be" };
                var valueSetsArray = (JArray) parameter[PropertyNames.ValueSet];
                IList<string> valueSets = valueSetsArray.Select(x => (string) x).ToList();
                Assert.That(valueSets, Is.EquivalentTo(expectedValueSets));

                var expectedParameterSubscriptions = new[] { "f1f076c4-5307-42b8-a171-3263a9e7bb21" };
                var parameterSubscriptionsArray = (JArray) parameter[PropertyNames.ParameterSubscription];
                IList<string> parameterSubscriptions = parameterSubscriptionsArray.Select(x => (string) x).ToList();
                Assert.That(parameterSubscriptions, Is.EquivalentTo(expectedParameterSubscriptions));
            }
        }
    }
}
