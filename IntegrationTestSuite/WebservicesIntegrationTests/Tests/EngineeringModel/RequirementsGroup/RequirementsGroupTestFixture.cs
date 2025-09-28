// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequirementsGroupTestFixture.cs" company="Starion Group S.A.">
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
    public class RequirementsGroupTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedRequirementsGroupIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var requirementsGroupUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/requirementsSpecification/bf0cde90-9086-43d5-bcff-32a2f8331800/group");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(requirementsGroupUri);

            //check if there is the only one RequirementsSpecification object 
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific RequirementsSpecification from the result by it's unique id
            var requirementsGroup = jArray.Single(x => (string)x[PropertyNames.Iid] == "d3474e6a-f9ac-4d1a-91d9-6f8be06a03b5");

            RequirementsGroupTestFixture.VerifyProperties(requirementsGroup);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedRequirementsGroupWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var requirementsGroupUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/requirementsSpecification/bf0cde90-9086-43d5-bcff-32a2f8331800/group?includeAllContainers=true");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(requirementsGroupUri);

            //check that the correct amount of objects is returned
            Assert.That(jArray.Count, Is.EqualTo(4));

            // get a specific Iteration from the result by it's unique id
            var iteration = jArray.Single(x => (string)x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific RequirementsSpecification from the result by it's unique id
            var requirementsSpecification = jArray.Single(x => (string)x[PropertyNames.Iid] == "bf0cde90-9086-43d5-bcff-32a2f8331800");
            RequirementsSpecificationTestFixture.VerifyProperties(requirementsSpecification);

            // get a specific RequirementsSpecification from the result by it's unique id
            var requirementsGroup = jArray.Single(x => (string)x[PropertyNames.Iid] == "d3474e6a-f9ac-4d1a-91d9-6f8be06a03b5");
            RequirementsGroupTestFixture.VerifyProperties(requirementsGroup);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatARequirementsGroupCanBeCreatedWithWebApi()
        {
            var iterationUri = new Uri(string.Format($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c"));
            var postBodyPath = this.GetPath("Tests/EngineeringModel/RequirementsGroup/PostNewRequirementsGroup.json");

            var postBody = base.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);
            
            var engineeeringModel = jArray.Single(x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.That((int) engineeeringModel[PropertyNames.RevisionNumber], Is.EqualTo(2));

            // get a specific RequirementsSpecification from the result by it's unique id
            var requirementsSpecification = jArray.Single(x => (string)x[PropertyNames.Iid] == "8d0734f4-ca4b-4611-9187-f6970e2b02bc");
            Assert.That((int)requirementsSpecification[PropertyNames.RevisionNumber], Is.EqualTo(2));

            var expectedRequirementsGroups = new string[] { "cffa1f05-41b8-4b89-922b-9a9505809601" };
            var requirementsGroupsArray = (JArray)requirementsSpecification[PropertyNames.Group];
            IList<string> groups = requirementsGroupsArray.Select(x => (string)x).ToList();
            Assert.That(groups, Is.EquivalentTo(expectedRequirementsGroups));

            // get the added RequirementsGroup from the result by it's unique id
            var requirementsGroup = jArray.Single(x => (string)x[PropertyNames.Iid] == "cffa1f05-41b8-4b89-922b-9a9505809601");

            // verify the amount of returned properties 
            Assert.That(requirementsGroup.Children().Count(), Is.EqualTo(10));

            // assert that the properties are what is expected
            Assert.That((string)requirementsGroup[PropertyNames.Iid], Is.EqualTo("cffa1f05-41b8-4b89-922b-9a9505809601"));
            Assert.That((int)requirementsGroup[PropertyNames.RevisionNumber], Is.EqualTo(2));
            Assert.That((string)requirementsGroup[PropertyNames.ClassKind], Is.EqualTo("RequirementsGroup"));

            Assert.That((string)requirementsGroup[PropertyNames.Name], Is.EqualTo("Test Requirements Group post test"));
            Assert.That((string)requirementsGroup[PropertyNames.ShortName], Is.EqualTo("TestRequirementsGroupPostTest"));

            Assert.That((string)requirementsGroup[PropertyNames.Owner], Is.EqualTo("0e92edde-fdff-41db-9b1d-f2e484f12535"));

            var expectedAliases = new string[] { };
            var aliasesArray = (JArray)requirementsGroup[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string)x).ToList();
            Assert.That(aliases, Is.EquivalentTo(expectedAliases));

            var expectedDefinitions = new string[] { };
            var definitionsArray = (JArray)requirementsGroup[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string)x).ToList();
            Assert.That(definitions, Is.EquivalentTo(expectedDefinitions));

            var expectedHyperlinks = new string[] { };
            var hyperlinksArray = (JArray)requirementsGroup[PropertyNames.HyperLink];
            IList<string> hyperlinks = hyperlinksArray.Select(x => (string)x).ToList();
            Assert.That(hyperlinks, Is.EquivalentTo(expectedHyperlinks));

            expectedRequirementsGroups = new string[] { };
            requirementsGroupsArray = (JArray)requirementsGroup[PropertyNames.Group];
            groups = requirementsGroupsArray.Select(x => (string)x).ToList();
            Assert.That(groups, Is.EquivalentTo(expectedRequirementsGroups));
        }

        [Test]
        [Category("POST")]
        public void VerifyThatRequirementsGroupCanBeDeletedAndContainedRequirementsReturnedFromWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/RequirementsGroup/PostDeleteRequirementsGroup.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            // check if there are appropriate amount of objects
            Assert.That(jArray.Count, Is.EqualTo(3));

            var engineeeringModel = jArray.Single(x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.That((int)engineeeringModel[PropertyNames.RevisionNumber], Is.EqualTo(2));

            // get a specific RequirementsSpecification from the result by it's unique id
            var requirementsSpecification = jArray.Single(x => (string)x[PropertyNames.Iid] == "bf0cde90-9086-43d5-bcff-32a2f8331800");
            Assert.That((int)requirementsSpecification[PropertyNames.RevisionNumber], Is.EqualTo(2));
            var expectedRequirementsGroups = new string[] { };
            var requirementsGroupsArray = (JArray)requirementsSpecification[PropertyNames.Group];
            IList<string> groups = requirementsGroupsArray.Select(x => (string)x).ToList();
            Assert.That(groups, Is.EquivalentTo(expectedRequirementsGroups));

            // get a specific Requirement from the result by it's unique id
            var requirement = jArray.Single(x => (string)x[PropertyNames.Iid] == "614e2a69-d602-46be-9311-2fb4d3273e88");
            Assert.That((int)requirement[PropertyNames.RevisionNumber], Is.EqualTo(2));
            Assert.That((string)requirement[PropertyNames.Group], Is.Null);
        }

        public static void VerifyProperties(JToken requirementsGroup)
        {
            // verify the amount of returned properties 
            Assert.That(requirementsGroup.Children().Count(), Is.EqualTo(10));

            // assert that the properties are what is expected
            Assert.That((string)requirementsGroup[PropertyNames.Iid], Is.EqualTo("d3474e6a-f9ac-4d1a-91d9-6f8be06a03b5"));
            Assert.That((int)requirementsGroup[PropertyNames.RevisionNumber], Is.EqualTo(1));
            Assert.That((string)requirementsGroup[PropertyNames.ClassKind], Is.EqualTo("RequirementsGroup"));

            Assert.That((string)requirementsGroup[PropertyNames.Name], Is.EqualTo("Test Requirements Group"));
            Assert.That((string)requirementsGroup[PropertyNames.ShortName], Is.EqualTo("TestRequirementsGroup"));

            Assert.That((string)requirementsGroup[PropertyNames.Owner], Is.EqualTo("0e92edde-fdff-41db-9b1d-f2e484f12535"));

            var expectedAliases = new string[] { };
            var aliasesArray = (JArray)requirementsGroup[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string)x).ToList();
            Assert.That(aliases, Is.EquivalentTo(expectedAliases));

            var expectedDefinitions = new string[] { };
            var definitionsArray = (JArray)requirementsGroup[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string)x).ToList();
            Assert.That(definitions, Is.EquivalentTo(expectedDefinitions));

            var expectedHyperlinks = new string[] { };
            var hyperlinksArray = (JArray)requirementsGroup[PropertyNames.HyperLink];
            IList<string> hyperlinks = hyperlinksArray.Select(x => (string)x).ToList();
            Assert.That(hyperlinks, Is.EquivalentTo(expectedHyperlinks));

            var expectedRequirementsGroups = new string[] { };
            var requirementsGroupsArray = (JArray)requirementsGroup[PropertyNames.Group];
            IList<string> groups = requirementsGroupsArray.Select(x => (string)x).ToList();
            Assert.That(groups, Is.EquivalentTo(expectedRequirementsGroups));
        }
    }
}
