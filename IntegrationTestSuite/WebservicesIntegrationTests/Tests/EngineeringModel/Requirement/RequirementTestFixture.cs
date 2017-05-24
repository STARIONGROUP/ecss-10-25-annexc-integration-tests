// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequirementTestFixture.cs" company="RHEA System">
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
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    
    /// <summary>
    /// Verification that the Requirement objects are returned from the data-source and that the 
    /// values of the Requirement properties are equal to the expected value
    /// </summary>
    [TestFixture]
    public class RequirementTestFixture : WebClientTestFixtureBase
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
        /// Verification that the Requirement objects are returned from the data-source and that the 
        /// values of the Requirement properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedRequirementIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var requirementUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/requirementsSpecification/bf0cde90-9086-43d5-bcff-32a2f8331800/requirement"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(requirementUri);

            //check if there is the only one Requirement object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific Requirement from the result by it's unique id
            var requirement = jArray.Single(x => (string) x[PropertyNames.Iid] == "614e2a69-d602-46be-9311-2fb4d3273e87");
            RequirementTestFixture.VerifyProperties(requirement);
        }

        [Test]
        public void VerifyThatExpectedRequirementWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var requirementUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/requirementsSpecification/bf0cde90-9086-43d5-bcff-32a2f8331800/requirement?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(requirementUri);

            // verify that the correct amount of objects is returned
            Assert.AreEqual(4, jArray.Count);

            // get a specific Iteration from the result by it's unique id
            var iteration = jArray.Single(x => (string) x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific RequirementsSpecification from the result by it's unique id
            var requirementsSpecification =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "bf0cde90-9086-43d5-bcff-32a2f8331800");
            RequirementsSpecificationTestFixture.VerifyProperties(requirementsSpecification);

            // get a specific Requirement from the result by it's unique id
            var requirement = jArray.Single(x => (string) x[PropertyNames.Iid] == "614e2a69-d602-46be-9311-2fb4d3273e87");
            RequirementTestFixture.VerifyProperties(requirement);
        }

        [Test]
        public void VerifyThatARequirementCanBeCreatedWithWebApi()
        {
            var iterationUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c"));
            var postBodyPath = this.GetPath("Tests/EngineeringModel/Requirement/PostNewRequirement.json");

            var postBody = base.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeeringModel =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.AreEqual(2, (int) engineeeringModel[PropertyNames.RevisionNumber]);

            // get a specific RequirementsSpecification from the result by it's unique id
            var requirementsSpecification =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "bf0cde90-9086-43d5-bcff-32a2f8331800");
            Assert.AreEqual(2, (int) requirementsSpecification[PropertyNames.RevisionNumber]);

            var expectedRequirements = new string[]
                {"614e2a69-d602-46be-9311-2fb4d3273e87", "09af5432-b7a5-4932-a983-b1065723efb7"};
            var requirementsArray = (JArray) requirementsSpecification[PropertyNames.Requirement];
            IList<string> requirements = requirementsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedRequirements, requirements);

            var requirement = jArray.Single(x => (string) x[PropertyNames.Iid] == "09af5432-b7a5-4932-a983-b1065723efb7");
            Assert.AreEqual(2, (int) requirement[PropertyNames.RevisionNumber]);
            Assert.AreEqual("create requirement", (string) requirement[PropertyNames.Name]);
            Assert.AreEqual("createrequirement", (string) requirement[PropertyNames.ShortName]);
            Assert.IsFalse((bool) requirement[PropertyNames.IsDeprecated]);
        }

        [Test]
        public void VerifyThatWhenARequirementIsMovedTheResponseIsComplete()
        {
            var iterationUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c"));
            var postBodyPath = this.GetPath("Tests/EngineeringModel/Requirement/PostMoveRequirement.json");

            var postBody = base.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            // verify that the correct amount of objects is returned
            Assert.AreEqual(4, jArray.Count);

            var engineeeringModel = jArray.Single(x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.AreEqual(2, (int)engineeeringModel[PropertyNames.RevisionNumber]);

            var requirementsSpecificationWithMovedRequirement = jArray.Single(x => (string)x[PropertyNames.Iid] == "8d0734f4-ca4b-4611-9187-f6970e2b02bc");
            Assert.AreEqual(2, (int)requirementsSpecificationWithMovedRequirement[PropertyNames.RevisionNumber]);

            var expectedRequirements = new string[] { "614e2a69-d602-46be-9311-2fb4d3273e87" };
            var requirementsArray = (JArray)requirementsSpecificationWithMovedRequirement[PropertyNames.Requirement];
            IList<string> requirements = requirementsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedRequirements, requirements);

            var requirementsSpecificationWithoutMovedRequirement = jArray.Single(x => (string)x[PropertyNames.Iid] == "bf0cde90-9086-43d5-bcff-32a2f8331800");
            Assert.AreEqual(2, (int)requirementsSpecificationWithoutMovedRequirement[PropertyNames.RevisionNumber]);

            expectedRequirements = new string[] { };
            requirementsArray = (JArray)requirementsSpecificationWithoutMovedRequirement[PropertyNames.Requirement];
            requirements = requirementsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedRequirements, requirements);

            var requirement = jArray.Single(x => (string)x[PropertyNames.Iid] == "614e2a69-d602-46be-9311-2fb4d3273e87");
            Assert.AreEqual(2, (int)requirement[PropertyNames.RevisionNumber]);
        }

        public static void VerifyProperties(JToken requirement)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(14, requirement.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("614e2a69-d602-46be-9311-2fb4d3273e87", (string) requirement[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) requirement[PropertyNames.RevisionNumber]);
            Assert.AreEqual("Requirement", (string) requirement[PropertyNames.ClassKind]);

            Assert.AreEqual("Test Requirement", (string) requirement[PropertyNames.Name]);
            Assert.AreEqual("TestRequirement", (string) requirement[PropertyNames.ShortName]);

            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string) requirement[PropertyNames.Owner]);

            Assert.IsFalse((bool) requirement[PropertyNames.IsDeprecated]);
            Assert.IsNull((string) requirement[PropertyNames.Group]);

            var expectedCategories = new string[] {"167b5cb0-766e-4ab2-b728-a9c9a662b017"};
            var categoriesArray = (JArray) requirement[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedCategories, categories);

            var expectedParameterValues = new string[]
            {
                "ef3b5740-6e0e-463c-99df-f255e38a32b6",
                "bcedefb0-b3ee-4a0b-8137-6561fa23b37f"
            };
            var parameterValuesArray = (JArray) requirement[PropertyNames.ParameterValue];
            IList<string> parameterValues = parameterValuesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedParameterValues, parameterValues);

            var expectedParametricConstraints = new List<OrderedItem>
            {
                new OrderedItem(1, "88200dbc-711a-47e0-a54a-dac4baca6e83")
            };
            var parametricConstraints = JsonConvert.DeserializeObject<List<OrderedItem>>(
                requirement[PropertyNames.ParametricConstraint].ToString());
            CollectionAssert.AreEquivalent(expectedParametricConstraints, parametricConstraints);

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) requirement[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) requirement[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) requirement[PropertyNames.HyperLink];
            IList<string> hyperlinks = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, hyperlinks);
        }
    }
}