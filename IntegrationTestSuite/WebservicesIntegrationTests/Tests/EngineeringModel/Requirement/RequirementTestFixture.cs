// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequirementTestFixture.cs" company="Starion Group S.A.">
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
    public class RequirementTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedRequirementIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var requirementUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/requirementsSpecification/bf0cde90-9086-43d5-bcff-32a2f8331800/requirement");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(requirementUri);

            // check if there are only two Requirement object 
            Assert.AreEqual(2, jArray.Count);

            VerifyProperties(jArray);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedRequirementWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var requirementUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/requirementsSpecification/bf0cde90-9086-43d5-bcff-32a2f8331800/requirement?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(requirementUri);

            // verify that the correct amount of objects is returned
            Assert.AreEqual(5, jArray.Count);

            // get a specific Iteration from the result by it's unique id
            var iteration = jArray.Single(x => (string) x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific RequirementsSpecification from the result by it's unique id
            var requirementsSpecification = jArray.Single(x => (string) x[PropertyNames.Iid] == "bf0cde90-9086-43d5-bcff-32a2f8331800");
            RequirementsSpecificationTestFixture.VerifyProperties(requirementsSpecification);

            VerifyProperties(jArray);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatARequirementCanBeCreatedWithWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/Requirement/PostNewRequirement.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeeringModel = jArray.Single(x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.AreEqual(2, (int) engineeeringModel[PropertyNames.RevisionNumber]);

            // get a specific RequirementsSpecification from the result by it's unique id
            var requirementsSpecification = jArray.Single(x => (string) x[PropertyNames.Iid] == "bf0cde90-9086-43d5-bcff-32a2f8331800");
            Assert.AreEqual(2, (int) requirementsSpecification[PropertyNames.RevisionNumber]);

            var expectedRequirements = new[]
            {
                "614e2a69-d602-46be-9311-2fb4d3273e87",
                "614e2a69-d602-46be-9311-2fb4d3273e88",
                "09af5432-b7a5-4932-a983-b1065723efb7"
            };

            var requirementsArray = (JArray) requirementsSpecification[PropertyNames.Requirement];
            IList<string> requirements = requirementsArray.Select(x => (string) x).ToList();
            Assert.That(requirements, Is.EquivalentTo(expectedRequirements));

            var requirement = jArray.Single(
                x => (string) x[PropertyNames.Iid] == "09af5432-b7a5-4932-a983-b1065723efb7");

            Assert.AreEqual(2, (int) requirement[PropertyNames.RevisionNumber]);
            Assert.AreEqual("create requirement", (string) requirement[PropertyNames.Name]);
            Assert.AreEqual("createrequirement", (string) requirement[PropertyNames.ShortName]);
            Assert.IsFalse((bool) requirement[PropertyNames.IsDeprecated]);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatWhenARequirementIsMovedTheResponseIsComplete()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/Requirement/PostMoveRequirement.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            // verify that the correct amount of objects is returned
            Assert.AreEqual(4, jArray.Count);

            var engineeeringModel = jArray.Single(x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.AreEqual(2, (int) engineeeringModel[PropertyNames.RevisionNumber]);

            var requirementsSpecificationWithMovedRequirement = jArray.Single(x => (string) x[PropertyNames.Iid] == "8d0734f4-ca4b-4611-9187-f6970e2b02bc");
            Assert.AreEqual(2, (int) requirementsSpecificationWithMovedRequirement[PropertyNames.RevisionNumber]);

            var expectedRequirements = new[] { "614e2a69-d602-46be-9311-2fb4d3273e87" };
            var requirementsArray = (JArray) requirementsSpecificationWithMovedRequirement[PropertyNames.Requirement];
            IList<string> requirements = requirementsArray.Select(x => (string) x).ToList();
            Assert.That(requirements, Is.EquivalentTo(expectedRequirements));

            var requirementsSpecificationWithoutMovedRequirement = jArray.Single(x => (string) x[PropertyNames.Iid] == "bf0cde90-9086-43d5-bcff-32a2f8331800");
            Assert.AreEqual(2, (int) requirementsSpecificationWithoutMovedRequirement[PropertyNames.RevisionNumber]);

            expectedRequirements = new string[] { "614e2a69-d602-46be-9311-2fb4d3273e88" };
            requirementsArray = (JArray) requirementsSpecificationWithoutMovedRequirement[PropertyNames.Requirement];
            requirements = requirementsArray.Select(x => (string) x).ToList();
            Assert.That(requirements, Is.EquivalentTo(expectedRequirements));

            var requirement = jArray.Single(x => (string) x[PropertyNames.Iid] == "614e2a69-d602-46be-9311-2fb4d3273e87");
            Assert.AreEqual(2, (int) requirement[PropertyNames.RevisionNumber]);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatParametricConstraintDeletionAsPropertyFromRequirementCanBeDoneFromWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/Requirement/PostDeleteConstraintAsProperty.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            // check if there are 2 objects
            Assert.AreEqual(2, jArray.Count);

            var engineeeringModel = jArray.Single(x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.AreEqual(2, (int) engineeeringModel[PropertyNames.RevisionNumber]);

            // get a specific Requirement from the result by it's unique id
            var requirement = jArray.Single(x => (string) x[PropertyNames.Iid] == "614e2a69-d602-46be-9311-2fb4d3273e87");
            Assert.AreEqual(2, (int) requirement[PropertyNames.RevisionNumber]);

            var expectedParametricConstraints = new List<OrderedItem>();

            var parametricConstraints =
                JsonConvert.DeserializeObject<List<OrderedItem>>(
                    requirement[PropertyNames.ParametricConstraint].ToString());

            Assert.That(parametricConstraints, Is.EquivalentTo(expectedParametricConstraints));

            // define the URI on which to perform a GET request 
            var constraintUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/requirementsSpecification/bf0cde90-9086-43d5-bcff-32a2f8331800/requirement/614e2a69-d602-46be-9311-2fb4d3273e87/parametricConstraint/88200dbc-711a-47e0-a54a-dac4baca6e83");
            Assert.That(() => this.WebClient.GetDto(constraintUri), Throws.Exception.TypeOf<WebException>());
        }

        /// <summary>
        /// Verifies properties of supplied Requirements.
        /// </summary>
        /// <param name="jArray">
        /// The JSON array.
        /// </param>
        public static void VerifyProperties(JArray jArray)
        {
            // get a specific Requirement from the result by it's unique id
            var requirement = jArray.Single(
                x => (string) x[PropertyNames.Iid] == "614e2a69-d602-46be-9311-2fb4d3273e87");

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

            var expectedCategories = new[] { "167b5cb0-766e-4ab2-b728-a9c9a662b017" };
            var categoriesArray = (JArray) requirement[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string) x).ToList();
            Assert.That(categories, Is.EquivalentTo(expectedCategories));

            var expectedParameterValues = new[]
            {
                "ef3b5740-6e0e-463c-99df-f255e38a32b6",
                "bcedefb0-b3ee-4a0b-8137-6561fa23b37f"
            };

            var parameterValuesArray = (JArray) requirement[PropertyNames.ParameterValue];
            IList<string> parameterValues = parameterValuesArray.Select(x => (string) x).ToList();
            Assert.That(parameterValues, Is.EquivalentTo(expectedParameterValues));

            var expectedParametricConstraints =
                new List<OrderedItem> { new OrderedItem(1, "88200dbc-711a-47e0-a54a-dac4baca6e83") };

            var parametricConstraints =
                JsonConvert.DeserializeObject<List<OrderedItem>>(
                    requirement[PropertyNames.ParametricConstraint].ToString());

            Assert.That(parametricConstraints, Is.EquivalentTo(expectedParametricConstraints));

            var expectedAliases = new string[] { };
            var aliasesArray = (JArray) requirement[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            Assert.That(aliases, Is.EquivalentTo(expectedAliases));

            var expectedDefinitions = new string[] { };
            var definitionsArray = (JArray) requirement[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            Assert.That(definitions, Is.EquivalentTo(expectedDefinitions));

            var expectedHyperlinks = new string[] { };
            var hyperlinksArray = (JArray) requirement[PropertyNames.HyperLink];
            IList<string> hyperlinks = hyperlinksArray.Select(x => (string) x).ToList();
            Assert.That(hyperlinks, Is.EquivalentTo(expectedHyperlinks));

            // get a specific Requirement from the result by it's unique id
            requirement = jArray.SingleOrDefault(x => (string) x[PropertyNames.Iid] == "614e2a69-d602-46be-9311-2fb4d3273e88");

            if (requirement != null)
            {
                // verify the amount of returned properties 
                Assert.AreEqual(14, requirement.Children().Count());

                // assert that the properties are what is expected
                Assert.AreEqual("614e2a69-d602-46be-9311-2fb4d3273e88", (string) requirement[PropertyNames.Iid]);
                Assert.AreEqual(1, (int) requirement[PropertyNames.RevisionNumber]);
                Assert.AreEqual("Requirement", (string) requirement[PropertyNames.ClassKind]);

                Assert.AreEqual("Test Requirement 2", (string) requirement[PropertyNames.Name]);
                Assert.AreEqual("TestRequirementTwo", (string) requirement[PropertyNames.ShortName]);

                Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string) requirement[PropertyNames.Owner]);

                Assert.IsFalse((bool) requirement[PropertyNames.IsDeprecated]);
                Assert.AreEqual("d3474e6a-f9ac-4d1a-91d9-6f8be06a03b5", (string) requirement[PropertyNames.Group]);

                expectedCategories = new string[] { };
                categoriesArray = (JArray) requirement[PropertyNames.Category];
                categories = categoriesArray.Select(x => (string) x).ToList();
                Assert.That(categories, Is.EquivalentTo(expectedCategories));

                expectedParameterValues = new string[] { };
                parameterValuesArray = (JArray) requirement[PropertyNames.ParameterValue];
                parameterValues = parameterValuesArray.Select(x => (string) x).ToList();
                Assert.That(parameterValues, Is.EquivalentTo(expectedParameterValues));

                expectedParametricConstraints = new List<OrderedItem> { };

                parametricConstraints =
                    JsonConvert.DeserializeObject<List<OrderedItem>>(
                        requirement[PropertyNames.ParametricConstraint].ToString());

                Assert.That(parametricConstraints, Is.EquivalentTo(expectedParametricConstraints));

                expectedAliases = new string[] { };
                aliasesArray = (JArray) requirement[PropertyNames.Alias];
                aliases = aliasesArray.Select(x => (string) x).ToList();
                Assert.That(aliases, Is.EquivalentTo(expectedAliases));

                expectedDefinitions = new string[] { };
                definitionsArray = (JArray) requirement[PropertyNames.Definition];
                definitions = definitionsArray.Select(x => (string) x).ToList();
                Assert.That(definitions, Is.EquivalentTo(expectedDefinitions));

                expectedHyperlinks = new string[] { };
                hyperlinksArray = (JArray) requirement[PropertyNames.HyperLink];
                hyperlinks = hyperlinksArray.Select(x => (string) x).ToList();
                Assert.That(hyperlinks, Is.EquivalentTo(expectedHyperlinks));
            }
        }

        [Test]
        [Category("POST")]
        public void VerifyThatASameAsContainerObjectCannotBeUpdatedWhenParticipantIsNotTheOwnerWithWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/Requirement/PostNewRequirement.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var definition = jArray.Single(x => (string) x[PropertyNames.Iid] == "3d8fa7f7-5235-4fe4-a026-207015e5822c");
            Assert.AreEqual(2, (int) definition[PropertyNames.RevisionNumber]);

            SiteDirectoryTestFixture.AddDomainExpertUserJane(this, out var userName, out var passWord);
            this.CreateNewWebClientForUser(userName, passWord);

            iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            postBodyPath = this.GetPath("Tests/EngineeringModel/Requirement/PostUpdateDefinition.json");

            postBody = this.GetJsonFromFile(postBodyPath);

            // Jane is not allowed to update
            var exception = Assert.Catch<WebException>(() => this.WebClient.PostDto(iterationUri, postBody));
            var errorMessage = this.WebClient.ExtractExceptionStringFromResponse(exception.Response);
            Assert.AreEqual(HttpStatusCode.Unauthorized, ((HttpWebResponse) exception.Response).StatusCode);
            Assert.IsTrue(errorMessage.Contains("The person Jane does not have an appropriate update permission for Definition."));
        }
    }
}
