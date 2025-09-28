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
            Assert.That(jArray.Count, Is.EqualTo(2));

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
            Assert.That(jArray.Count, Is.EqualTo(5));

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

            Assert.That((int) engineeeringModel[PropertyNames.RevisionNumber], Is.EqualTo(2));

            // get a specific ElementDefinition from the result by it's unique id
            var elementDefinition = jArray.Single(x => (string) x[PropertyNames.Iid] == "f73860b2-12f0-43e4-b8b2-c81862c0a159");

            Assert.That((int) elementDefinition[PropertyNames.RevisionNumber], Is.EqualTo(2));

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
            Assert.That(parameter.Children().Count(), Is.EqualTo(14));

            // assert that the properties are what is expected
            Assert.That((string) parameter[PropertyNames.Iid], Is.EqualTo("2cd4eb9c-e92c-41b2-968c-f03ff7010bad"));
            Assert.That((int) parameter[PropertyNames.RevisionNumber], Is.EqualTo(2));
            Assert.That((string) parameter[PropertyNames.ClassKind], Is.EqualTo("Parameter"));

            Assert.That((string) parameter[PropertyNames.RequestedBy], Is.Null);
            Assert.That((bool) parameter[PropertyNames.AllowDifferentOwnerOfOverride], Is.False);
            Assert.That((bool) parameter[PropertyNames.ExpectsOverride], Is.False);
            Assert.That((string) parameter[PropertyNames.ParameterType], Is.EqualTo("35a9cf05-4eba-4cda-b60c-7cfeaac8f892"));
            Assert.That((string) parameter[PropertyNames.Scale], Is.Null);
            Assert.That((string) parameter[PropertyNames.StateDependence], Is.Null);
            Assert.That((string) parameter[PropertyNames.Group], Is.Null);
            Assert.That((bool) parameter[PropertyNames.IsOptionDependent], Is.False);
            Assert.That((string) parameter[PropertyNames.Owner], Is.EqualTo("0e92edde-fdff-41db-9b1d-f2e484f12535"));

            var expectedParameterSubscriptions = new string[] { };
            var parameterSubscriptionsArray = (JArray) parameter[PropertyNames.ParameterSubscription];
            IList<string> parameterSubscriptions = parameterSubscriptionsArray.Select(x => (string) x).ToList();
            Assert.That(parameterSubscriptions, Is.EquivalentTo(expectedParameterSubscriptions));

            // get the created ParameterValueSet as a side effect of creating Parameter from the result by it's unique id
            var valueSetsArray = (JArray) parameter[PropertyNames.ValueSet];
            IList<string> valueSets = valueSetsArray.Select(x => (string) x).ToList();
            Assert.That(valueSets.Count, Is.EqualTo(1));

            var parameterValueSet = jArray.Single(x => (string) x[PropertyNames.Iid] == valueSets[0]);

            // verify the amount of returned properties 
            Assert.That(parameterValueSet.Children().Count(), Is.EqualTo(11));

            // assert that the properties are what is expected
            Assert.That((string) parameterValueSet[PropertyNames.Iid], Is.EqualTo(valueSets[0]));
            Assert.That((int) parameterValueSet[PropertyNames.RevisionNumber], Is.EqualTo(2));
            Assert.That((string) parameterValueSet[PropertyNames.ClassKind], Is.EqualTo("ParameterValueSet"));

            Assert.That((string) parameterValueSet[PropertyNames.ValueSwitch], Is.EqualTo("MANUAL"));

            const string emptyProperty = "[\"-\"]";
            Assert.That((string) parameterValueSet[PropertyNames.Published], Is.EqualTo(emptyProperty));
            Assert.That((string) parameterValueSet[PropertyNames.Formula], Is.EqualTo(emptyProperty));
            Assert.That((string) parameterValueSet[PropertyNames.Computed], Is.EqualTo(emptyProperty));
            Assert.That((string) parameterValueSet[PropertyNames.Manual], Is.EqualTo(emptyProperty));
            Assert.That((string) parameterValueSet[PropertyNames.Reference], Is.EqualTo(emptyProperty));

            Assert.That((string) parameterValueSet[PropertyNames.ActualState], Is.Null);
            Assert.That((string) parameterValueSet[PropertyNames.ActualOption], Is.Null);
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

            Assert.That((int) engineeeringModel[PropertyNames.RevisionNumber], Is.EqualTo(2));

            // get a specific ElementDefinition from the result by it's unique id
            var elementDefinition = jArray.Single(x => (string) x[PropertyNames.Iid] == "f73860b2-12f0-43e4-b8b2-c81862c0a159");

            Assert.That((int) elementDefinition[PropertyNames.RevisionNumber], Is.EqualTo(2));

            var expectedParameters = new[]
            {
                "3f05483f-66ff-4f21-bc76-45956779f66e",
                "2cd4eb9c-e92c-41b2-968c-f03ff7010bad"
            };

            var parametersArray = (JArray) elementDefinition[PropertyNames.Parameter];
            IList<string> parameters = parametersArray.Select(x => (string) x).ToList();
            Assert.That(parameters, Is.EquivalentTo(expectedParameters));

            // get the added Parameter from the result by it's unique id
            var parameter = jArray.Single(x => (string) x[PropertyNames.Iid] == "2cd4eb9c-e92c-41b2-968c-f03ff7010bad");

            // verify the amount of returned properties 
            Assert.That(parameter.Children().Count(), Is.EqualTo(14));

            // assert that the properties are what is expected
            Assert.That((string) parameter[PropertyNames.Iid], Is.EqualTo("2cd4eb9c-e92c-41b2-968c-f03ff7010bad"));
            Assert.That((int) parameter[PropertyNames.RevisionNumber], Is.EqualTo(2));
            Assert.That((string) parameter[PropertyNames.ClassKind], Is.EqualTo("Parameter"));

            Assert.That((string) parameter[PropertyNames.RequestedBy], Is.Null);
            Assert.That((bool) parameter[PropertyNames.AllowDifferentOwnerOfOverride], Is.False);
            Assert.That((bool) parameter[PropertyNames.ExpectsOverride], Is.False);
            Assert.That((string) parameter[PropertyNames.ParameterType], Is.EqualTo("35a9cf05-4eba-4cda-b60c-7cfeaac8f892"));
            Assert.That((string) parameter[PropertyNames.Scale], Is.Null);
            Assert.That((string) parameter[PropertyNames.StateDependence], Is.Null);
            Assert.That((string) parameter[PropertyNames.Group], Is.Null);
            Assert.That((bool) parameter[PropertyNames.IsOptionDependent], Is.False);
            Assert.That((string) parameter[PropertyNames.Owner], Is.EqualTo("0e92edde-fdff-41db-9b1d-f2e484f12535"));

            var expectedParameterSubscriptions = new string[] { };
            var parameterSubscriptionsArray = (JArray) parameter[PropertyNames.ParameterSubscription];
            IList<string> parameterSubscriptions = parameterSubscriptionsArray.Select(x => (string) x).ToList();
            Assert.That(parameterSubscriptions, Is.EquivalentTo(expectedParameterSubscriptions));

            // get the created ParameterValueSet as a side effect of creating Parameter from the result by it's unique id
            var valueSetsArray = (JArray) parameter[PropertyNames.ValueSet];
            IList<string> valueSets = valueSetsArray.Select(x => (string) x).ToList();
            Assert.That(valueSets.Count, Is.EqualTo(1));

            var parameterValueSet = jArray.Single(x => (string) x[PropertyNames.Iid] == valueSets[0]);

            // verify the amount of returned properties 
            Assert.That(parameterValueSet.Children().Count(), Is.EqualTo(11));

            // assert that the properties are what is expected
            Assert.That((string) parameterValueSet[PropertyNames.Iid], Is.EqualTo(valueSets[0]));
            Assert.That((int) parameterValueSet[PropertyNames.RevisionNumber], Is.EqualTo(2));
            Assert.That((string) parameterValueSet[PropertyNames.ClassKind], Is.EqualTo("ParameterValueSet"));

            Assert.That((string) parameterValueSet[PropertyNames.ValueSwitch], Is.EqualTo("MANUAL"));

            const string emptyProperty = "[\"-\"]";
            Assert.That((string) parameterValueSet[PropertyNames.Published], Is.EqualTo(emptyProperty));
            Assert.That((string) parameterValueSet[PropertyNames.Formula], Is.EqualTo(emptyProperty));
            Assert.That((string) parameterValueSet[PropertyNames.Computed], Is.EqualTo(emptyProperty));
            Assert.That((string) parameterValueSet[PropertyNames.Manual], Is.EqualTo(emptyProperty));
            Assert.That((string) parameterValueSet[PropertyNames.Reference], Is.EqualTo(emptyProperty));

            Assert.That((string) parameterValueSet[PropertyNames.ActualState], Is.Null);
            Assert.That((string) parameterValueSet[PropertyNames.ActualOption], Is.Null);
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

            Assert.That((int) engineeeringModel[PropertyNames.RevisionNumber], Is.EqualTo(2));

            // get a specific ElementDefinition from the result by it's unique id
            var elementDefinition = jArray.Single(x => (string) x[PropertyNames.Iid] == "f73860b2-12f0-43e4-b8b2-c81862c0a159");

            Assert.That((int) elementDefinition[PropertyNames.RevisionNumber], Is.EqualTo(2));

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
            Assert.That(parameter.Children().Count(), Is.EqualTo(14));

            // assert that the properties are what is expected
            Assert.That((string) parameter[PropertyNames.Iid], Is.EqualTo("2460b6a5-08ff-4cc3-a2cc-8fd5c5cf2736"));
            Assert.That((int) parameter[PropertyNames.RevisionNumber], Is.EqualTo(2));
            Assert.That((string) parameter[PropertyNames.ClassKind], Is.EqualTo("Parameter"));

            Assert.That((string) parameter[PropertyNames.RequestedBy], Is.Null);
            Assert.That((bool) parameter[PropertyNames.AllowDifferentOwnerOfOverride], Is.False);
            Assert.That((bool) parameter[PropertyNames.ExpectsOverride], Is.False);
            Assert.That((string) parameter[PropertyNames.ParameterType], Is.EqualTo("4a783624-b2bc-4e6d-95b3-11d036f6e917"));
            Assert.That((string) parameter[PropertyNames.Scale], Is.Null);
            Assert.That((string) parameter[PropertyNames.StateDependence], Is.Null);
            Assert.That((string) parameter[PropertyNames.Group], Is.Null);
            Assert.That((bool) parameter[PropertyNames.IsOptionDependent], Is.False);
            Assert.That((string) parameter[PropertyNames.Owner], Is.EqualTo("0e92edde-fdff-41db-9b1d-f2e484f12535"));

            var expectedParameterSubscriptions = new string[] { };
            var parameterSubscriptionsArray = (JArray) parameter[PropertyNames.ParameterSubscription];
            IList<string> parameterSubscriptions = parameterSubscriptionsArray.Select(x => (string) x).ToList();
            Assert.That(parameterSubscriptions, Is.EquivalentTo(expectedParameterSubscriptions));

            // get the created ParameterValueSet as a side effect of creating Parameter from the result by it's unique id
            var valueSetsArray = (JArray) parameter[PropertyNames.ValueSet];
            IList<string> valueSets = valueSetsArray.Select(x => (string) x).ToList();
            Assert.That(valueSets.Count, Is.EqualTo(1));

            var parameterValueSet = jArray.Single(x => (string) x[PropertyNames.Iid] == valueSets[0]);

            // verify the amount of returned properties 
            Assert.That(parameterValueSet.Children().Count(), Is.EqualTo(11));

            // assert that the properties are what is expected
            Assert.That((string) parameterValueSet[PropertyNames.Iid], Is.EqualTo(valueSets[0]));
            Assert.That((int) parameterValueSet[PropertyNames.RevisionNumber], Is.EqualTo(2));
            Assert.That((string) parameterValueSet[PropertyNames.ClassKind], Is.EqualTo("ParameterValueSet"));

            Assert.That((string) parameterValueSet[PropertyNames.ValueSwitch], Is.EqualTo("MANUAL"));

            const string emptyProperty = "[\"-\",\"-\"]";
            Assert.That((string) parameterValueSet[PropertyNames.Published], Is.EqualTo(emptyProperty));
            Assert.That((string) parameterValueSet[PropertyNames.Formula], Is.EqualTo(emptyProperty));
            Assert.That((string) parameterValueSet[PropertyNames.Computed], Is.EqualTo(emptyProperty));
            Assert.That((string) parameterValueSet[PropertyNames.Manual], Is.EqualTo(emptyProperty));
            Assert.That((string) parameterValueSet[PropertyNames.Reference], Is.EqualTo(emptyProperty));

            Assert.That((string) parameterValueSet[PropertyNames.ActualState], Is.Null);
            Assert.That((string) parameterValueSet[PropertyNames.ActualOption], Is.Null);
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

            Assert.That((int) engineeeringModel[PropertyNames.RevisionNumber], Is.EqualTo(2));

            // get a specific ElementDefinition from the result by it's unique id
            var elementDefinition = jArray.Single(x => (string) x[PropertyNames.Iid] == "f73860b2-12f0-43e4-b8b2-c81862c0a159");

            Assert.That((int) elementDefinition[PropertyNames.RevisionNumber], Is.EqualTo(2));

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
            Assert.That(parameter.Children().Count(), Is.EqualTo(14));

            // assert that the properties are what is expected
            Assert.That((string) parameter[PropertyNames.Iid], Is.EqualTo("9600b225-a4be-47b1-92b1-4dc2d8894ea3"));
            Assert.That((int) parameter[PropertyNames.RevisionNumber], Is.EqualTo(2));
            Assert.That((string) parameter[PropertyNames.ClassKind], Is.EqualTo("Parameter"));

            Assert.That((string) parameter[PropertyNames.RequestedBy], Is.Null);
            Assert.That((bool) parameter[PropertyNames.AllowDifferentOwnerOfOverride], Is.False);
            Assert.That((bool) parameter[PropertyNames.ExpectsOverride], Is.False);
            Assert.That((string) parameter[PropertyNames.ParameterType], Is.EqualTo("35a9cf05-4eba-4cda-b60c-7cfeaac8f892"));
            Assert.That((string) parameter[PropertyNames.Scale], Is.Null);
            Assert.That((string) parameter[PropertyNames.StateDependence], Is.Null);
            Assert.That((string) parameter[PropertyNames.Group], Is.Null);
            Assert.That((bool) parameter[PropertyNames.IsOptionDependent], Is.True);
            Assert.That((string) parameter[PropertyNames.Owner], Is.EqualTo("0e92edde-fdff-41db-9b1d-f2e484f12535"));

            var expectedParameterSubscriptions = new string[] { };
            var parameterSubscriptionsArray = (JArray) parameter[PropertyNames.ParameterSubscription];
            IList<string> parameterSubscriptions = parameterSubscriptionsArray.Select(x => (string) x).ToList();
            Assert.That(parameterSubscriptions, Is.EquivalentTo(expectedParameterSubscriptions));

            // get the created ParameterValueSet as a side effect of creating Parameter from the result by it's unique id
            var valueSetsArray = (JArray) parameter[PropertyNames.ValueSet];
            IList<string> valueSets = valueSetsArray.Select(x => (string) x).ToList();
            Assert.That(valueSets.Count, Is.EqualTo(1));

            var parameterValueSet = jArray.Single(x => (string) x[PropertyNames.Iid] == valueSets[0]);

            // verify the amount of returned properties 
            Assert.That(parameterValueSet.Children().Count(), Is.EqualTo(11));

            // assert that the properties are what is expected
            Assert.That((string) parameterValueSet[PropertyNames.Iid], Is.EqualTo(valueSets[0]));
            Assert.That((int) parameterValueSet[PropertyNames.RevisionNumber], Is.EqualTo(2));
            Assert.That((string) parameterValueSet[PropertyNames.ClassKind], Is.EqualTo("ParameterValueSet"));

            Assert.That((string) parameterValueSet[PropertyNames.ValueSwitch], Is.EqualTo("MANUAL"));

            const string emptyProperty = "[\"-\"]";
            Assert.That((string) parameterValueSet[PropertyNames.Published], Is.EqualTo(emptyProperty));
            Assert.That((string) parameterValueSet[PropertyNames.Formula], Is.EqualTo(emptyProperty));
            Assert.That((string) parameterValueSet[PropertyNames.Computed], Is.EqualTo(emptyProperty));
            Assert.That((string) parameterValueSet[PropertyNames.Manual], Is.EqualTo(emptyProperty));
            Assert.That((string) parameterValueSet[PropertyNames.Reference], Is.EqualTo(emptyProperty));

            Assert.That((string) parameterValueSet[PropertyNames.ActualState], Is.Null);

            Assert.That((string) parameterValueSet[PropertyNames.ActualOption], Is.EqualTo("bebcc9f4-ff20-4569-bbf6-d1acf27a8107"));
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

            Assert.That((int) engineeeringModel[PropertyNames.RevisionNumber], Is.EqualTo(2));

            var parameterValueSet = jArray.Single(x => (string) x[PropertyNames.ClassKind] == "ParameterValueSet");
            Assert.That((int) parameterValueSet[PropertyNames.RevisionNumber], Is.EqualTo(2));

            var parameterOverrideValueSet = jArray.Single(x => (string) x[PropertyNames.ClassKind] == "ParameterOverrideValueSet");

            Assert.That((int) parameterOverrideValueSet[PropertyNames.RevisionNumber], Is.EqualTo(2));

            Assert.That((string) parameterOverrideValueSet[PropertyNames.ParameterValueSet], Is.EqualTo((string) parameterValueSet[PropertyNames.Iid]));

            var parameterSubscriptionValueSet =
                jArray.Single(x => (string) x[PropertyNames.ClassKind] == "ParameterSubscriptionValueSet");

            Assert.That((int) parameterSubscriptionValueSet[PropertyNames.RevisionNumber], Is.EqualTo(2));

            Assert.That((string) parameterSubscriptionValueSet[PropertyNames.SubscribedValueSet], Is.EqualTo((string) parameterValueSet[PropertyNames.Iid]));

            var parameterSubscription = jArray.Single(
                x => (string) x[PropertyNames.Iid] == "f1f076c4-5307-42b8-a171-3263a9e7bb21");

            Assert.That((int) parameterSubscription[PropertyNames.RevisionNumber], Is.EqualTo(2));

            Assert.That((string) parameterSubscription[PropertyNames.ValueSet][0], Is.EqualTo((string) parameterSubscriptionValueSet[PropertyNames.Iid]));

            var parameter = jArray.Single(x => (string) x[PropertyNames.Iid] == "6c5aff74-f983-4aa8-a9d6-293b3429307c");
            Assert.That((int) parameter[PropertyNames.RevisionNumber], Is.EqualTo(2));
            var expectedValueSets = new[] { (string) parameterValueSet[PropertyNames.Iid] };
            var valueSetsArray = (JArray) parameter[PropertyNames.ValueSet];
            IList<string> valueSets = valueSetsArray.Select(x => (string) x).ToList();
            Assert.That(valueSets, Is.EquivalentTo(expectedValueSets));

            var parameterOverride = jArray.Single(
                x => (string) x[PropertyNames.Iid] == "93f767ed-4d22-45f6-ae97-d1dab0d36e1c");

            Assert.That((int) parameterOverride[PropertyNames.RevisionNumber], Is.EqualTo(2));
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

            Assert.That((int) engineeeringModel[PropertyNames.RevisionNumber], Is.EqualTo(2));

            var parameterValueSet = jArray.Single(x => (string) x[PropertyNames.ClassKind] == "ParameterValueSet");
            Assert.That((int) parameterValueSet[PropertyNames.RevisionNumber], Is.EqualTo(2));

            const string EmptyProperty = "[\"-\"]";
            Assert.That((string) parameterValueSet[PropertyNames.ValueSwitch], Is.EqualTo("MANUAL"));
            Assert.That((string) parameterValueSet[PropertyNames.Published], Is.EqualTo(EmptyProperty));
            Assert.That((string) parameterValueSet[PropertyNames.Formula], Is.EqualTo(EmptyProperty));
            Assert.That((string) parameterValueSet[PropertyNames.Computed], Is.EqualTo(EmptyProperty));
            Assert.That((string) parameterValueSet[PropertyNames.Manual], Is.EqualTo(EmptyProperty));
            Assert.That((string) parameterValueSet[PropertyNames.Reference], Is.EqualTo(EmptyProperty));

            Assert.That((string) parameterValueSet[PropertyNames.ActualState], Is.EqualTo("b91bfdbb-4277-4a03-b519-e4db839ef5d4"));

            Assert.That((string) parameterValueSet[PropertyNames.ActualOption], Is.Null);

            var parameterOverride = jArray.Single(x => (string) x[PropertyNames.ClassKind] == "ParameterOverride");
            Assert.That((int) parameterOverride[PropertyNames.RevisionNumber], Is.EqualTo(2));
            var valueSetsArray = (JArray) parameterOverride[PropertyNames.ValueSet];
            var valueSets = valueSetsArray.Select(x => (string) x).ToList();
            Assert.That(valueSets.Count, Is.EqualTo(1));
            Assert.That((string) parameterOverride[PropertyNames.Owner], Is.EqualTo("0e92edde-fdff-41db-9b1d-f2e484f12535"));
            Assert.That((string) parameterOverride[PropertyNames.Parameter], Is.EqualTo("6c5aff74-f983-4aa8-a9d6-293b3429307c"));

            var expectedParameterSubscriptions = new string[] { };
            var parameterSubscriptionsArray = (JArray) parameterOverride[PropertyNames.ParameterSubscription];
            IList<string> parameterSubscriptions = parameterSubscriptionsArray.Select(x => (string) x).ToList();
            Assert.That(parameterSubscriptions, Is.EquivalentTo(expectedParameterSubscriptions));

            var parameterOverrideValueSet = jArray.Single(
                x => (string) x[PropertyNames.Iid] == (string) parameterOverride[PropertyNames.ValueSet][0]);

            Assert.That((int) parameterOverrideValueSet[PropertyNames.RevisionNumber], Is.EqualTo(2));

            Assert.That((string) parameterOverrideValueSet[PropertyNames.ParameterValueSet], Is.EqualTo((string) parameterValueSet[PropertyNames.Iid]));

            Assert.That((string) parameterOverrideValueSet[PropertyNames.ValueSwitch], Is.EqualTo("MANUAL"));

            Assert.That((string) parameterOverrideValueSet[PropertyNames.Published], Is.EqualTo(EmptyProperty));
            Assert.That((string) parameterOverrideValueSet[PropertyNames.Formula], Is.EqualTo(EmptyProperty));
            Assert.That((string) parameterOverrideValueSet[PropertyNames.Computed], Is.EqualTo(EmptyProperty));
            Assert.That((string) parameterOverrideValueSet[PropertyNames.Manual], Is.EqualTo(EmptyProperty));
            Assert.That((string) parameterOverrideValueSet[PropertyNames.Reference], Is.EqualTo(EmptyProperty));

            var parameterSubscription = jArray.Single(
                x => (string) x[PropertyNames.Iid] == "f1f076c4-5307-42b8-a171-3263a9e7bb21");

            Assert.That((int) parameterSubscription[PropertyNames.RevisionNumber], Is.EqualTo(2));
            Assert.That((string) parameterOverride[PropertyNames.Owner], Is.EqualTo("0e92edde-fdff-41db-9b1d-f2e484f12535"));

            valueSetsArray = (JArray) parameterSubscription[PropertyNames.ValueSet];
            valueSets = valueSetsArray.Select(x => (string) x).ToList();
            Assert.That(valueSets.Count, Is.EqualTo(1));

            var parameterSubscriptionValueSet = jArray.Single(
                x => (string) x[PropertyNames.Iid] == (string) parameterSubscription[PropertyNames.ValueSet][0]);

            Assert.That((int) parameterSubscriptionValueSet[PropertyNames.RevisionNumber], Is.EqualTo(2));

            Assert.That((string) parameterSubscriptionValueSet[PropertyNames.SubscribedValueSet], Is.EqualTo((string) parameterValueSet[PropertyNames.Iid]));

            Assert.That((string) parameterSubscriptionValueSet[PropertyNames.SubscribedValueSet], Is.EqualTo((string) parameterValueSet[PropertyNames.Iid]));

            Assert.That((string) parameterSubscriptionValueSet[PropertyNames.Manual], Is.EqualTo(EmptyProperty));
            Assert.That((string) parameterSubscriptionValueSet[PropertyNames.ValueSwitch], Is.EqualTo("MANUAL"));

            var parameter = jArray.Single(x => (string) x[PropertyNames.Iid] == "6c5aff74-f983-4aa8-a9d6-293b3429307c");
            Assert.That((int) parameter[PropertyNames.RevisionNumber], Is.EqualTo(2));
            var expectedValueSets = new[] { (string) parameterValueSet[PropertyNames.Iid] };
            valueSetsArray = (JArray) parameter[PropertyNames.ValueSet];
            valueSets = valueSetsArray.Select(x => (string) x).ToList();
            Assert.That(valueSets, Is.EquivalentTo(expectedValueSets));
            Assert.That((string) parameter[PropertyNames.StateDependence], Is.EqualTo("db690d7d-761c-47fd-96d3-840d698a89dc"));
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

            Assert.That((int) engineeeringModel[PropertyNames.RevisionNumber], Is.EqualTo(2));

            var parameterValueSet = jArray.Single(x => (string) x[PropertyNames.ClassKind] == "ParameterValueSet");
            Assert.That((int) parameterValueSet[PropertyNames.RevisionNumber], Is.EqualTo(2));

            postBodyPath = this.GetPath("Tests/EngineeringModel/ActualFiniteStateList/PostNewActualFiniteStateList.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(iterationUri, postBody);

            engineeeringModel = jArray.Single(
                x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");

            Assert.That((int) engineeeringModel[PropertyNames.RevisionNumber], Is.EqualTo(3));

            var iteration = jArray.Single(x => (string) x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            Assert.That((int) iteration[PropertyNames.RevisionNumber], Is.EqualTo(3));

            postBodyPath = this.GetPath("Tests/EngineeringModel/Parameter/PostUpdateStateDependentParameterToAnotherState.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(iterationUri, postBody);

            engineeeringModel = jArray.Single(
                x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");

            Assert.That((int) engineeeringModel[PropertyNames.RevisionNumber], Is.EqualTo(4));

            parameterValueSet = jArray.Single(x => (string) x[PropertyNames.ClassKind] == "ParameterValueSet");
            Assert.That((int) parameterValueSet[PropertyNames.RevisionNumber], Is.EqualTo(4));

            const string EmptyProperty = "[\"-\"]";
            Assert.That((string) parameterValueSet[PropertyNames.ValueSwitch], Is.EqualTo("MANUAL"));
            Assert.That((string) parameterValueSet[PropertyNames.Published], Is.EqualTo(EmptyProperty));
            Assert.That((string) parameterValueSet[PropertyNames.Formula], Is.EqualTo(EmptyProperty));
            Assert.That((string) parameterValueSet[PropertyNames.Computed], Is.EqualTo(EmptyProperty));
            Assert.That((string) parameterValueSet[PropertyNames.Manual], Is.EqualTo(EmptyProperty));
            Assert.That((string) parameterValueSet[PropertyNames.Reference], Is.EqualTo(EmptyProperty));

            var parameter = jArray.Single(x => (string) x[PropertyNames.ClassKind] == "Parameter");
            Assert.That((int) parameter[PropertyNames.RevisionNumber], Is.EqualTo(4));

            var parameterSubscription = jArray.Single(x => (string) x[PropertyNames.ClassKind] == "ParameterSubscription");
            Assert.That((int) parameterSubscription[PropertyNames.RevisionNumber], Is.EqualTo(4));

            var parameterOverride = jArray.Single(x => (string) x[PropertyNames.ClassKind] == "ParameterOverride");
            Assert.That((int) parameterOverride[PropertyNames.RevisionNumber], Is.EqualTo(4));
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
                Assert.That(parameter.Children().Count(), Is.EqualTo(14));

                // assert that the properties are what is expected
                Assert.That((string) parameter[PropertyNames.Iid], Is.EqualTo("6c5aff74-f983-4aa8-a9d6-293b3429307c"));
                Assert.That((int) parameter[PropertyNames.RevisionNumber], Is.EqualTo(1));
                Assert.That((string) parameter[PropertyNames.ClassKind], Is.EqualTo("Parameter"));

                Assert.That((string) parameter[PropertyNames.RequestedBy], Is.Null);
                Assert.That((bool) parameter[PropertyNames.AllowDifferentOwnerOfOverride], Is.False);
                Assert.That((bool) parameter[PropertyNames.ExpectsOverride], Is.False);
                Assert.That((string) parameter[PropertyNames.ParameterType], Is.EqualTo("a21c15c4-3e1e-46b5-b109-5063dec1e254"));
                Assert.That((string) parameter[PropertyNames.Scale], Is.Null);
                Assert.That((string) parameter[PropertyNames.StateDependence], Is.Null);
                Assert.That("b739b3c6-9cc0-4e64-9cc4-ef7463edf559", Is.EqualTo((string) parameter[PropertyNames.Group]));
                Assert.That((bool) parameter[PropertyNames.IsOptionDependent], Is.False);
                Assert.That((string) parameter[PropertyNames.Owner], Is.EqualTo("0e92edde-fdff-41db-9b1d-f2e484f12535"));

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
                Assert.That(parameter.Children().Count(), Is.EqualTo(14));

                // assert that the properties are what is expected
                Assert.That((string) parameter[PropertyNames.Iid], Is.EqualTo("3f05483f-66ff-4f21-bc76-45956779f66e"));
                Assert.That((int) parameter[PropertyNames.RevisionNumber], Is.EqualTo(1));
                Assert.That((string) parameter[PropertyNames.ClassKind], Is.EqualTo("Parameter"));

                Assert.That((string) parameter[PropertyNames.RequestedBy], Is.Null);
                Assert.That((bool) parameter[PropertyNames.AllowDifferentOwnerOfOverride], Is.False);
                Assert.That((bool) parameter[PropertyNames.ExpectsOverride], Is.False);
                Assert.That((string) parameter[PropertyNames.ParameterType], Is.EqualTo("a21c15c4-3e1e-46b5-b109-5063dec1e254"));
                Assert.That((string) parameter[PropertyNames.Scale], Is.Null);
                Assert.That((string) parameter[PropertyNames.StateDependence], Is.Null);
                Assert.That("b739b3c6-9cc0-4e64-9cc4-ef7463edf559", Is.EqualTo((string) parameter[PropertyNames.Group]));
                Assert.That((bool) parameter[PropertyNames.IsOptionDependent], Is.False);
                Assert.That((string) parameter[PropertyNames.Owner], Is.EqualTo("eb759723-14b9-49f4-8611-544d037bb764"));

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
