// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AndExpressionTestFixture.cs" company="RHEA System S.A.">
//
//   Copyright 2016-2021 RHEA System S.A.
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
    public class AndExpressionTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedAndExpressionIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var andExpressionUri =
                new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/requirementsSpecification/bf0cde90-9086-43d5-bcff-32a2f8331800/requirement/614e2a69-d602-46be-9311-2fb4d3273e87/parametricConstraint/88200dbc-711a-47e0-a54a-dac4baca6e83/expression/000484d0-cefd-47be-9317-a9eae72c94ce");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(andExpressionUri);

            //check if there is the only one AndExpression object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific AndExpression from the result by it's unique id
            var andExpression =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "000484d0-cefd-47be-9317-a9eae72c94ce");

            AndExpressionTestFixture.VerifyProperties(andExpression);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedAndExpressionWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var andExpressionUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/requirementsSpecification/bf0cde90-9086-43d5-bcff-32a2f8331800/requirement/614e2a69-d602-46be-9311-2fb4d3273e87/parametricConstraint/88200dbc-711a-47e0-a54a-dac4baca6e83/expression/000484d0-cefd-47be-9317-a9eae72c94ce?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(andExpressionUri);

            // verify that the correct amount of objects is returned
            Assert.AreEqual(6, jArray.Count);

            // get a specific Iteration from the result by it's unique id
            var iteration = jArray.Single(x => (string) x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific RequirementsSpecification from the result by it's unique id
            var requirementsSpecification =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "bf0cde90-9086-43d5-bcff-32a2f8331800");
            RequirementsSpecificationTestFixture.VerifyProperties(requirementsSpecification);

            // get a specific Requirement from the result by it's unique id
            RequirementTestFixture.VerifyProperties(jArray);

            // get a specific ParametricConstraint from the result by it's unique id
            var parametricConstraint =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "88200dbc-711a-47e0-a54a-dac4baca6e83");
            ParametricConstraintTestFixture.VerifyProperties(parametricConstraint);

            // get a specific AndExpression from the result by it's unique id
            var andExpression =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "000484d0-cefd-47be-9317-a9eae72c94ce");
            AndExpressionTestFixture.VerifyProperties(andExpression);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatAnAndExpressionCanBeCreatedWithWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/AndExpression/PostNewAndExpression.json");

            var postBody = base.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeringModel = jArray.Single(x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");

            // Verify the amount of returned properties of the EngineeringModel
            Assert.AreEqual(8, engineeringModel.Children().Count());

            // Assert the properties of EngineeringModel have expected values
            var expectedIterations = new[] { "e163c5ad-f32b-4387-b805-f4b34600bc2c" };
            var iterationsArray = (JArray)engineeringModel[PropertyNames.Iteration];
            IList<string> iterations = iterationsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedIterations, iterations);

            var expectedLogEntries = new[] { "4e2375eb-8e37-4df2-9c7b-dd896683a891" };
            var logEntriesArray = (JArray)engineeringModel[PropertyNames.LogEntry];
            IList<string> logEntries = logEntriesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedLogEntries, logEntries);

            var expectedCommonFileStores = new[] { "8e5ca9cc-3da8-4e66-9172-7c3b2464a59c" };
            var commonFileStoresArray = (JArray)engineeringModel[PropertyNames.CommonFileStore];
            IList<string> commonFileStores = commonFileStoresArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedCommonFileStores, commonFileStores);

            Assert.AreEqual("EngineeringModel", (string)engineeringModel[PropertyNames.ClassKind]);
            Assert.AreEqual("116f6253-89bb-47d4-aa24-d11d197e43c9", (string)engineeringModel[PropertyNames.EngineeringModelSetup]);
            Assert.AreEqual("9ec982e4-ef72-4953-aa85-b158a95d8d56", (string)engineeringModel[PropertyNames.Iid]);
            Assert.AreEqual(2, (int)engineeringModel[PropertyNames.RevisionNumber]);

            // Get a specific ParametricConstraint from the result by it's unique id
            var parametricConstraint = jArray.Single(x => (string)x[PropertyNames.Iid] == "88200dbc-711a-47e0-a54a-dac4baca6e83");

            // verify the amount of returned properties of ParametricConstraint
            Assert.AreEqual(5, parametricConstraint.Children().Count());

            // assert that the properties of ParametricConstraint are what is expected
            var expectedExpressions = new string[]
            {
                "000484d0-cefd-47be-9317-a9eae72c94ce",
                "30cb785a-9e72-477f-ad1a-8df6ab623e3d",
                "5f90327f-95a2-4c5a-9efe-581f8daf08ed",
                "8c6df21f-07ae-4d0b-ab9b-866dd1f90158",
                "deaa2560-b704-4b2c-950b-aad02ff84052",
                "a6e44651-7c4a-4a57-bdf9-c0290497f392",
                "c99662c1-4f80-4f98-b6c1-0008980f930d"
            };
            var expressionArray = (JArray)parametricConstraint[PropertyNames.Expression];
            IList<string> expressions = expressionArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedExpressions, expressions);

            Assert.AreEqual("ParametricConstraint", (string)parametricConstraint[PropertyNames.ClassKind]);
            Assert.AreEqual("88200dbc-711a-47e0-a54a-dac4baca6e83", (string)parametricConstraint[PropertyNames.Iid]);
            Assert.AreEqual(2, (int)parametricConstraint[PropertyNames.RevisionNumber]);
            Assert.AreEqual("30cb785a-9e72-477f-ad1a-8df6ab623e3d", (string)parametricConstraint[PropertyNames.TopExpression]);

            // Get the added AndExpression from the result by it's unique id
            var andExpression = jArray.Single(x => (string)x[PropertyNames.Iid] == "c99662c1-4f80-4f98-b6c1-0008980f930d");

            // Verify the amount of returned properties 
            Assert.AreEqual(4, andExpression.Children().Count());

            // Assert that the properties are equal to expected values   
            var expectedTerms = new string[]
            {
                "deaa2560-b704-4b2c-950b-aad02ff84052",
                "a6e44651-7c4a-4a57-bdf9-c0290497f392"
            };
            var termsArray = (JArray)andExpression[PropertyNames.Term];
            IList<string> terms = termsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedTerms, terms);

            Assert.AreEqual("AndExpression", (string)andExpression[PropertyNames.ClassKind]);
            Assert.AreEqual("c99662c1-4f80-4f98-b6c1-0008980f930d", (string)andExpression[PropertyNames.Iid]);
            Assert.AreEqual(2, (int)andExpression[PropertyNames.RevisionNumber]);
        }

        /// <summary>
        /// Verifies all properties of the AndExpression <see cref="JToken"/>
        /// </summary>
        /// <param name="andExpression">
        /// The <see cref="JToken"/> that contains the properties of
        /// the AndExpression object
        /// </param>
        public static void VerifyProperties(JToken andExpression)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(4, andExpression.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("000484d0-cefd-47be-9317-a9eae72c94ce", (string) andExpression[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) andExpression[PropertyNames.RevisionNumber]);
            Assert.AreEqual("AndExpression", (string) andExpression[PropertyNames.ClassKind]);

            var expectedTerms = new string[]
            {
                "deaa2560-b704-4b2c-950b-aad02ff84052",
                "a6e44651-7c4a-4a57-bdf9-c0290497f392"
            };
            var termsArray = (JArray) andExpression[PropertyNames.Term];
            IList<string> terms = termsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedTerms, terms);
        }
    }
}
