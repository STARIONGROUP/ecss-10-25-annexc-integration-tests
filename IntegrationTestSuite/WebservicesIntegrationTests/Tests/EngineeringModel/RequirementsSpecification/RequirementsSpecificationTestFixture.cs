// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequirementsSpecificationTestFixture.cs" company="Starion Group S.A.">
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

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class RequirementsSpecificationTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedRequirementsSpecificationIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var requirementsSpecificationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/requirementsSpecification");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(requirementsSpecificationUri);

            // verify the number of RequirementsSpecification object 
            Assert.AreEqual(2, jArray.Count);

            // get a specific RequirementsSpecification from the result by it's unique id
            var requirementsSpecification = jArray.Single(x => (string)x[PropertyNames.Iid] == "bf0cde90-9086-43d5-bcff-32a2f8331800");

            RequirementsSpecificationTestFixture.VerifyProperties(requirementsSpecification);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedRequirementsSpecificationWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var requirementsSpecificationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/requirementsSpecification?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(requirementsSpecificationUri);

            // verify that the correct amount of objects is returned
            Assert.AreEqual(4, jArray.Count);

            // get a specific Iteration from the result by it's unique id
            var iteration = jArray.Single(x => (string)x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific RequirementsSpecification from the result by it's unique id
            var requirementsSpecification = jArray.Single(x => (string)x[PropertyNames.Iid] == "bf0cde90-9086-43d5-bcff-32a2f8331800");
            RequirementsSpecificationTestFixture.VerifyProperties(requirementsSpecification);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatARequirementsSpecificationCanBeCreatedWithWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");

            var postBodyPath = this.GetPath("Tests/EngineeringModel/RequirementsSpecification/PostNewRequirementsSpecification.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeeringModel = jArray.Single(x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.AreEqual(2, (int)engineeeringModel[PropertyNames.RevisionNumber]);

            var iteration = jArray.Single(x => (string)x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            Assert.AreEqual(2, (int)iteration[PropertyNames.RevisionNumber]);

            var expectedRequirementsSpecifications = new[]
                                                         {
                                                             "bf0cde90-9086-43d5-bcff-32a2f8331800",
                                                             "8d0734f4-ca4b-4611-9187-f6970e2b02bc",
                                                             "272e59f8-267a-4e5f-84eb-6d1b495bf4c7"
                                                         };
            var requirementsSpecificationsArray = (JArray)iteration[PropertyNames.RequirementsSpecification];
            IList<string> requirementsSpecifications = requirementsSpecificationsArray.Select(x => (string)x).ToList();
            Assert.That(requirementsSpecifications, Is.EquivalentTo(expectedRequirementsSpecifications));

            var requirementsSpecification = jArray.Single(x => (string)x[PropertyNames.Iid] == "272e59f8-267a-4e5f-84eb-6d1b495bf4c7");

            // verify the amount of returned properties 
            Assert.AreEqual(12, requirementsSpecification.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("272e59f8-267a-4e5f-84eb-6d1b495bf4c7", (string)requirementsSpecification[PropertyNames.Iid]);
            Assert.AreEqual(2, (int)requirementsSpecification[PropertyNames.RevisionNumber]);
            Assert.AreEqual("RequirementsSpecification", (string)requirementsSpecification[PropertyNames.ClassKind]);

            Assert.AreEqual("Test Requirements Specification 3", (string)requirementsSpecification[PropertyNames.Name]);
            Assert.AreEqual("TestRequirementsSpecification3", (string)requirementsSpecification[PropertyNames.ShortName]);

            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string)requirementsSpecification[PropertyNames.Owner]);

            Assert.IsFalse((bool)requirementsSpecification[PropertyNames.IsDeprecated]);

            var expectedAliases = new string[] { };
            var aliasesArray = (JArray)requirementsSpecification[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string)x).ToList();
            Assert.That(aliases, Is.EquivalentTo(expectedAliases));

            var expectedDefinitions = new string[] { };
            var definitionsArray = (JArray)requirementsSpecification[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string)x).ToList();
            Assert.That(definitions, Is.EquivalentTo(expectedDefinitions));

            var expectedHyperlinks = new string[] { };
            var hyperlinksArray = (JArray)requirementsSpecification[PropertyNames.HyperLink];
            IList<string> hyperlinks = hyperlinksArray.Select(x => (string)x).ToList();
            Assert.That(hyperlinks, Is.EquivalentTo(expectedHyperlinks));

            var expectedRequirementsGroups = new string[] { };
            var requirementsGroupsArray = (JArray)requirementsSpecification[PropertyNames.Group];
            IList<string> groups = requirementsGroupsArray.Select(x => (string)x).ToList();
            Assert.That(groups, Is.EquivalentTo(expectedRequirementsGroups));

            var expectedRequirements = new string[] { };
            var requirementsArray = (JArray)requirementsSpecification[PropertyNames.Requirement];
            IList<string> requirements = requirementsArray.Select(x => (string)x).ToList();
            Assert.That(requirements, Is.EquivalentTo(expectedRequirements));
        }

        public static void VerifyProperties(JToken requirementsSpecification)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(12, requirementsSpecification.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual(
                "bf0cde90-9086-43d5-bcff-32a2f8331800",
                (string)requirementsSpecification[PropertyNames.Iid]);
            Assert.AreEqual(1, (int)requirementsSpecification[PropertyNames.RevisionNumber]);
            Assert.AreEqual("RequirementsSpecification", (string)requirementsSpecification[PropertyNames.ClassKind]);

            Assert.AreEqual("Test Requirements Specification", (string)requirementsSpecification[PropertyNames.Name]);
            Assert.AreEqual(
                "TestRequirementsSpecification",
                (string)requirementsSpecification[PropertyNames.ShortName]);

            Assert.AreEqual(
                "0e92edde-fdff-41db-9b1d-f2e484f12535",
                (string)requirementsSpecification[PropertyNames.Owner]);

            Assert.IsFalse((bool)requirementsSpecification[PropertyNames.IsDeprecated]);

            var expectedAliases = new string[] { };
            var aliasesArray = (JArray)requirementsSpecification[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string)x).ToList();
            Assert.That(aliases, Is.EquivalentTo(expectedAliases));

            var expectedDefinitions = new string[] { };
            var definitionsArray = (JArray)requirementsSpecification[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string)x).ToList();
            Assert.That(definitions, Is.EquivalentTo(expectedDefinitions));

            var expectedHyperlinks = new string[] { };
            var hyperlinksArray = (JArray)requirementsSpecification[PropertyNames.HyperLink];
            IList<string> hyperlinks = hyperlinksArray.Select(x => (string)x).ToList();
            Assert.That(hyperlinks, Is.EquivalentTo(expectedHyperlinks));

            var expectedRequirementsGroups = new[] { "d3474e6a-f9ac-4d1a-91d9-6f8be06a03b5" };
            var requirementsGroupsArray = (JArray)requirementsSpecification[PropertyNames.Group];
            IList<string> groups = requirementsGroupsArray.Select(x => (string)x).ToList();
            Assert.That(groups, Is.EquivalentTo(expectedRequirementsGroups));

            var expectedRequirements = new[]
                                           {
                                               "614e2a69-d602-46be-9311-2fb4d3273e87",
                                               "614e2a69-d602-46be-9311-2fb4d3273e88"
                                           };
            var requirementsArray = (JArray)requirementsSpecification[PropertyNames.Requirement];
            IList<string> requirements = requirementsArray.Select(x => (string)x).ToList();
            Assert.That(requirements, Is.EquivalentTo(expectedRequirements));
        }
    }
}
