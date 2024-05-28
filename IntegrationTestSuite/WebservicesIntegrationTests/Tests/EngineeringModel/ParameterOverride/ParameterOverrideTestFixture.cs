// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterOverrideTestFixture.cs" company="Starion Group S.A.">
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

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class ParameterOverrideTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedParameterOverrideIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var parameterOverrideUri =
                new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/fe9295c5-af99-494e-86ff-e715837806ae/containedElement/75399754-ee45-4bca-b033-63e2019870d1/parameterOverride");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(parameterOverrideUri);

            //check if there is the only one ParameterOverride object 
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific ParameterOverride from the result by it's unique id
            var parameterOverride = jArray.Single(x => (string) x[PropertyNames.Iid] == "93f767ed-4d22-45f6-ae97-d1dab0d36e1c");

            ParameterOverrideTestFixture.VerifyProperties(parameterOverride);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedParameterOverrideWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var parameterOverrideUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/fe9295c5-af99-494e-86ff-e715837806ae/containedElement/75399754-ee45-4bca-b033-63e2019870d1/parameterOverride?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(parameterOverrideUri);

            //check if there are 5 objects
            Assert.That(jArray.Count, Is.EqualTo(5));

            // get a specific Iteration from the result by it's unique id
            var iteration = jArray.Single(x => (string) x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific ElementDefinition from the result by it's unique id
            ElementDefinitionTestFixture.VerifyProperties(jArray);

            // get a specific ElementUsage from the result by it's unique id
            ElementUsageTestFixture.VerifyProperties(jArray);

            // get a specific ParameterOverride from the result by it's unique id
            var parameterOverride = jArray.Single(x => (string) x[PropertyNames.Iid] == "93f767ed-4d22-45f6-ae97-d1dab0d36e1c");
            ParameterOverrideTestFixture.VerifyProperties(parameterOverride);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatAParameterOverrideCanBeCreatedWithWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/ParameterOverride/PostNewParameterOverride.json");

            var postBody = base.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeeringModel = jArray.Single(x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.That((int)engineeeringModel[PropertyNames.RevisionNumber], Is.EqualTo(2));

            var elementUsage = jArray.Single(x => (string) x[PropertyNames.Iid] == "75399754-ee45-4bca-b033-63e2019870d1");
            Assert.That((int)elementUsage[PropertyNames.RevisionNumber], Is.EqualTo(2));

            var expectedParameterOverrides = new string[]
            {
                "93f767ed-4d22-45f6-ae97-d1dab0d36e1c",
                "3587bb05-0db4-4741-b5e3-da43393e13ed"
            };
            var parameterOverridesArray = (JArray) elementUsage[PropertyNames.ParameterOverride];
            IList<string> parameterOverrides = parameterOverridesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedParameterOverrides, parameterOverrides);

            // get the added ParameterOverride from the result by it's unique id
            var parameterOverride = jArray.Single(x => (string)x[PropertyNames.Iid] == "3587bb05-0db4-4741-b5e3-da43393e13ed");

            // verify the amount of properties
            Assert.AreEqual(7, parameterOverride.Children().Count());

            // assert that the properties are what is expected
            Assert.That((string)parameterOverride[PropertyNames.Iid], Is.EqualTo("3587bb05-0db4-4741-b5e3-da43393e13ed"));
            Assert.That((string)parameterOverride[PropertyNames.Owner], Is.EqualTo("0e92edde-fdff-41db-9b1d-f2e484f12535"));
            Assert.That((string)parameterOverride[PropertyNames.ClassKind], Is.EqualTo("ParameterOverride"));
            Assert.That((int)parameterOverride[PropertyNames.RevisionNumber], Is.EqualTo(2));

            var expectedParameterOverrideSubscriptions = new string[] { };
            var expectedParameterOverrideSubscriptionsArray = (JArray)parameterOverride[PropertyNames.ParameterSubscription];
            IList<string> parameterSubscriptions = expectedParameterOverrideSubscriptionsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedParameterOverrideSubscriptions, parameterSubscriptions);
            
            var valueSetsArray = (JArray)parameterOverride[PropertyNames.ValueSet];
            IList<string> valueSets = valueSetsArray.Select(x => (string)x).ToList();
            Assert.That(valueSets.Count, Is.EqualTo(1));

            var parameterOverrideValueSet = jArray.Single(x => (string)x[PropertyNames.Iid] == valueSets[0]);
            Assert.That(parameterOverrideValueSet.Children().Count(), Is.EqualTo(10));

            Assert.That((string)parameterOverrideValueSet[PropertyNames.ParameterValueSet], Is.EqualTo("af5c88c6-301f-497b-81f7-53748c3900ed"));
            
            Assert.That((int)parameterOverrideValueSet[PropertyNames.RevisionNumber], Is.EqualTo(2));
            Assert.That((string)parameterOverrideValueSet[PropertyNames.ClassKind], Is.EqualTo("ParameterOverrideValueSet"));

            Assert.That((string)parameterOverrideValueSet[PropertyNames.ValueSwitch], Is.EqualTo("MANUAL"));

            const string emptyProperty = "[\"-\"]";
            Assert.That((string)parameterOverrideValueSet[PropertyNames.Published], Is.EqualTo(emptyProperty));
            Assert.That((string)parameterOverrideValueSet[PropertyNames.Formula], Is.EqualTo(emptyProperty));
            Assert.That((string)parameterOverrideValueSet[PropertyNames.Computed], Is.EqualTo(emptyProperty));
            Assert.That((string)parameterOverrideValueSet[PropertyNames.Manual], Is.EqualTo(emptyProperty));
            Assert.That((string)parameterOverrideValueSet[PropertyNames.Reference], Is.EqualTo(emptyProperty));
        }

        [Test]
        [Category("POST")]
        public void VerifyThatChangeOwnershipOfOptionDependentParameterOverrideLeavesParameterValuesetValuesInTact()
        {
            //----------------------------------------------------------------------------------------------------
            // Create ParameterOverride
            //----------------------------------------------------------------------------------------------------
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/ParameterOverride/PostNewParameterOverride.json");

            var postBody = base.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeeringModel = jArray.Single(x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.That((int)engineeeringModel[PropertyNames.RevisionNumber], Is.EqualTo(2));

            //----------------------------------------------------------------------------------------------------
            // Post new option
            //----------------------------------------------------------------------------------------------------
            postBodyPath = this.GetPath("Tests/EngineeringModel/Option/PostNewOption.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(iterationUri, postBody);

            engineeeringModel = jArray.Single(x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.That((int)engineeeringModel[PropertyNames.RevisionNumber], Is.EqualTo(3));

            //----------------------------------------------------------------------------------------------------
            // Make Parameter and ParameterOverride Option Dependent
            //----------------------------------------------------------------------------------------------------
            postBodyPath = this.GetPath("Tests/EngineeringModel/ParameterOverride/PostUpdateParameterToOptionDependent.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(iterationUri, postBody);

            engineeeringModel = jArray.Single(x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.That((int)engineeeringModel[PropertyNames.RevisionNumber], Is.EqualTo(4));

            var parameterOverrideValueSet1 = jArray.First(x => (string)x[PropertyNames.ClassKind] == "ParameterOverrideValueSet");
            var parameterOverrideValueSet2 = jArray.Where(x => (string)x[PropertyNames.ClassKind] == "ParameterOverrideValueSet").Skip(1).First();

            //----------------------------------------------------------------------------------------------------
            // Create ValueSets for both Options on ParameterOverride
            //----------------------------------------------------------------------------------------------------
            var valueSetUpdatePath = this.GetPath("Tests/EngineeringModel/ParameterOverride/PostUpdateParameterOverrideValueSetTemplate.json");

            //override valueset 1
            var initialValueSetContent = this.GetJsonFromFile(valueSetUpdatePath).Replace("<INNERIID>", $"\"{(string)parameterOverrideValueSet1[PropertyNames.Iid]}\"");

            postBody = initialValueSetContent.Replace("<INNERVALUE>", JsonConvert.SerializeObject("1"));
            jArray = this.WebClient.PostDto(iterationUri, postBody);

            var povs1 = jArray.Single(x => (string)x[PropertyNames.Iid] == (string)parameterOverrideValueSet1[PropertyNames.Iid]);

            Assert.That((int)povs1[PropertyNames.RevisionNumber], Is.EqualTo(5));
            Assert.That((string)povs1[PropertyNames.Manual], Is.EqualTo("[\"1\"]"));
            
            //override valueset 2
            initialValueSetContent = this.GetJsonFromFile(valueSetUpdatePath).Replace("<INNERIID>", $"\"{(string)parameterOverrideValueSet2[PropertyNames.Iid]}\"");

            postBody = initialValueSetContent.Replace("<INNERVALUE>", JsonConvert.SerializeObject("2"));
            jArray = this.WebClient.PostDto(iterationUri, postBody);

            var povs2 = jArray.Single(x => (string)x[PropertyNames.Iid] == (string)parameterOverrideValueSet2[PropertyNames.Iid]);

            Assert.That((int)povs2[PropertyNames.RevisionNumber], Is.EqualTo(6));
            Assert.That((string)povs2[PropertyNames.Manual], Is.EqualTo("[\"2\"]"));

            //----------------------------------------------------------------------------------------------------
            // Change ownership on ParameterOverride
            //----------------------------------------------------------------------------------------------------
            postBodyPath = this.GetPath("Tests/EngineeringModel/ParameterOverride/PostChangeOwnerOnParameterOverride.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(iterationUri, postBody);

            var parameterOverride = jArray.Single(x => (string)x[PropertyNames.Iid] == "3587bb05-0db4-4741-b5e3-da43393e13ed");
            Assert.That((int)parameterOverride[PropertyNames.RevisionNumber], Is.EqualTo(7));
            Assert.That((string)parameterOverride[PropertyNames.Owner], Is.EqualTo("eb759723-14b9-49f4-8611-544d037bb764"));

            // No Subscriptions should be there
            var expectedParameterOverrideSubscriptions = new string[] { };
            var expectedParameterOverrideSubscriptionsArray = (JArray)parameterOverride[PropertyNames.ParameterSubscription];
            IList<string> parameterSubscriptions = expectedParameterOverrideSubscriptionsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedParameterOverrideSubscriptions, parameterSubscriptions);
            Assert.That(parameterSubscriptions.Count, Is.EqualTo(0));

            //----------------------------------------------------------------------------------------------------
            // Get and check EU containing the ParameterOverride and all related objects
            //----------------------------------------------------------------------------------------------------
            var elementUsageUri =
                new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/fe9295c5-af99-494e-86ff-e715837806ae/containedElement/75399754-ee45-4bca-b033-63e2019870d1?extent=deep");

            // get a response from the data-source as a JArray (JSON Array)
            jArray = this.WebClient.GetDto(elementUsageUri);

            //check if there is the only one ParameterOverride object 
            Assert.AreEqual(7, jArray.Count);

            povs1 = jArray.Single(x => (string)x[PropertyNames.Iid] == (string)parameterOverrideValueSet1[PropertyNames.Iid]);

            Assert.That((int)povs1[PropertyNames.RevisionNumber], Is.EqualTo(5));
            Assert.That((string)povs1[PropertyNames.Manual], Is.EqualTo("[\"1\"]"));

            povs2 = jArray.Single(x => (string)x[PropertyNames.Iid] == (string)parameterOverrideValueSet2[PropertyNames.Iid]);

            Assert.That((int)povs2[PropertyNames.RevisionNumber], Is.EqualTo(6));
            Assert.That((string)povs2[PropertyNames.Manual], Is.EqualTo("[\"2\"]"));
        }

        /// <summary>
        /// Verifies all properties of the ParameterOverride <see cref="JToken"/>
        /// </summary>
        /// <param name="parameterOverride">
        /// The <see cref="JToken"/> that contains the properties of
        /// the ParameterOverride object
        /// </param>
        public static void VerifyProperties(JToken parameterOverride)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(7, parameterOverride.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("93f767ed-4d22-45f6-ae97-d1dab0d36e1c",
                (string) parameterOverride[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) parameterOverride[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ParameterOverride", (string) parameterOverride[PropertyNames.ClassKind]);

            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string) parameterOverride[PropertyNames.Owner]);
            Assert.AreEqual("6c5aff74-f983-4aa8-a9d6-293b3429307c",
                (string) parameterOverride[PropertyNames.Parameter]);
            
            var expectedParameterSubscriptions = new string[] {};
            var parameterSubscriptionsArray = (JArray) parameterOverride[PropertyNames.ParameterSubscription];
            IList<string> parameterSubscriptions = parameterSubscriptionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedParameterSubscriptions, parameterSubscriptions);

            var expectedValueSets = new string[]
            {
                "985db346-a297-4ce6-956b-e675d53d415e"
            };
            var valueSetsArray = (JArray) parameterOverride[PropertyNames.ValueSet];
            IList<string> valueSets = valueSetsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedValueSets, valueSets);
        }
    }
}
