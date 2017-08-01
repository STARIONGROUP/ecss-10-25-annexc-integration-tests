// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequirementsGroupTestFixture.cs" company="RHEA System">
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
    using Newtonsoft.Json;
    
    [TestFixture]
    public class RequirementsGroupTestFixture : WebClientTestFixtureBase
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
        /// Verification that the RequirementsGroup objects are returned from the data-source and that the 
        /// values of the RequirementsGroup properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedRequirementsGroupIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var requirementsGroupUri = new Uri(string.Format(UriFormat, this.Settings.Hostname, "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/requirementsSpecification/bf0cde90-9086-43d5-bcff-32a2f8331800/group"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(requirementsGroupUri);

            //check if there is the only one RequirementsSpecification object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific RequirementsSpecification from the result by it's unique id
            var requirementsGroup = jArray.Single(x => (string)x[PropertyNames.Iid] == "d3474e6a-f9ac-4d1a-91d9-6f8be06a03b5");

            RequirementsGroupTestFixture.VerifyProperties(requirementsGroup);
        }

        [Test]
        public void VerifyThatExpectedRequirementsGroupWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var requirementsGroupUri = new Uri(string.Format(UriFormat, this.Settings.Hostname, "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/requirementsSpecification/bf0cde90-9086-43d5-bcff-32a2f8331800/group?includeAllContainers=true"));
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(requirementsGroupUri);

            //check that the correct amount of objects is returned
            Assert.AreEqual(4, jArray.Count);

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
        public void VerifyThatARequirementsGroupCanBeCreatedWithWebApi()
        {
            var iterationUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c"));
            var postBodyPath = this.GetPath("Tests/EngineeringModel/RequirementsGroup/PostNewRequirementsGroup.json");

            var postBody = base.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);
            
            var engineeeringModel =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.AreEqual(2, (int) engineeeringModel[PropertyNames.RevisionNumber]);

            // get a specific RequirementsSpecification from the result by it's unique id
            var requirementsSpecification =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "8d0734f4-ca4b-4611-9187-f6970e2b02bc");
            Assert.AreEqual(2, (int)requirementsSpecification[PropertyNames.RevisionNumber]);

            var expectedRequirementsGroups = new string[] { "cffa1f05-41b8-4b89-922b-9a9505809601" };
            var requirementsGroupsArray = (JArray)requirementsSpecification[PropertyNames.Group];
            IList<string> groups = requirementsGroupsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedRequirementsGroups, groups);

            // get the added RequirementsGroup from the result by it's unique id
            var requirementsGroup = jArray.Single(x => (string)x[PropertyNames.Iid] == "cffa1f05-41b8-4b89-922b-9a9505809601");

            // verify the amount of returned properties 
            Assert.AreEqual(10, requirementsGroup.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("cffa1f05-41b8-4b89-922b-9a9505809601", (string)requirementsGroup[PropertyNames.Iid]);
            Assert.AreEqual(2, (int)requirementsGroup[PropertyNames.RevisionNumber]);
            Assert.AreEqual("RequirementsGroup", (string)requirementsGroup[PropertyNames.ClassKind]);

            Assert.AreEqual("Test Requirements Group post test", (string)requirementsGroup[PropertyNames.Name]);
            Assert.AreEqual("TestRequirementsGroupPostTest", (string)requirementsGroup[PropertyNames.ShortName]);

            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string)requirementsGroup[PropertyNames.Owner]);

            var expectedAliases = new string[] { };
            var aliasesArray = (JArray)requirementsGroup[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] { };
            var definitionsArray = (JArray)requirementsGroup[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] { };
            var hyperlinksArray = (JArray)requirementsGroup[PropertyNames.HyperLink];
            IList<string> hyperlinks = hyperlinksArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, hyperlinks);

            expectedRequirementsGroups = new string[] { };
            requirementsGroupsArray = (JArray)requirementsGroup[PropertyNames.Group];
            groups = requirementsGroupsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedRequirementsGroups, groups);
        }

        [Test]
        public void VerifyThatRequirementsGroupCanBeDeletedAndContainedRequirementsReturnedFromWebApi()
        {
            var iterationUri = new Uri(
                string.Format(
                    UriFormat,
                    this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c"));
            var postBodyPath = this.GetPath("Tests/EngineeringModel/RequirementsGroup/PostDeleteRequirementsGroup.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            Console.WriteLine(jArray);

            // check if there are appropriate amount of objects
            Assert.AreEqual(3, jArray.Count);

            var engineeeringModel = jArray.Single(
                x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.AreEqual(2, (int)engineeeringModel[PropertyNames.RevisionNumber]);

            // get a specific RequirementsSpecification from the result by it's unique id
            var requirementsSpecification = jArray.Single(
                x => (string)x[PropertyNames.Iid] == "bf0cde90-9086-43d5-bcff-32a2f8331800");
            Assert.AreEqual(2, (int)requirementsSpecification[PropertyNames.RevisionNumber]);
            var expectedRequirementsGroups = new string[] { };
            var requirementsGroupsArray = (JArray)requirementsSpecification[PropertyNames.Group];
            IList<string> groups = requirementsGroupsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedRequirementsGroups, groups);

            // get a specific Requirement from the result by it's unique id
            var requirement = jArray.Single(x => (string)x[PropertyNames.Iid] == "614e2a69-d602-46be-9311-2fb4d3273e88");
            Assert.AreEqual(2, (int)requirement[PropertyNames.RevisionNumber]);
            Assert.IsNull((string)requirement[PropertyNames.Group]);
        }

        public static void VerifyProperties(JToken requirementsGroup)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(10, requirementsGroup.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("d3474e6a-f9ac-4d1a-91d9-6f8be06a03b5", (string)requirementsGroup[PropertyNames.Iid]);
            Assert.AreEqual(1, (int)requirementsGroup[PropertyNames.RevisionNumber]);
            Assert.AreEqual("RequirementsGroup", (string)requirementsGroup[PropertyNames.ClassKind]);

            Assert.AreEqual("Test Requirements Group", (string)requirementsGroup[PropertyNames.Name]);
            Assert.AreEqual("TestRequirementsGroup", (string)requirementsGroup[PropertyNames.ShortName]);

            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string)requirementsGroup[PropertyNames.Owner]);

            var expectedAliases = new string[] { };
            var aliasesArray = (JArray)requirementsGroup[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] { };
            var definitionsArray = (JArray)requirementsGroup[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] { };
            var hyperlinksArray = (JArray)requirementsGroup[PropertyNames.HyperLink];
            IList<string> hyperlinks = hyperlinksArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, hyperlinks);

            var expectedRequirementsGroups = new string[] { };
            var requirementsGroupsArray = (JArray)requirementsGroup[PropertyNames.Group];
            IList<string> groups = requirementsGroupsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedRequirementsGroups, groups);
        }
    }
}
