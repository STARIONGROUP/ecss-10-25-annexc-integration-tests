// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExclusiveOrExpressionTestFixture.cs" company="Starion Group S.A.">
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
    public class ExclusiveOrExpressionTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedExclusiveOrExpressionIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var exclusiveOrExpressionUri =
                new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/requirementsSpecification/bf0cde90-9086-43d5-bcff-32a2f8331800/requirement/614e2a69-d602-46be-9311-2fb4d3273e87/parametricConstraint/88200dbc-711a-47e0-a54a-dac4baca6e83/expression/8c6df21f-07ae-4d0b-ab9b-866dd1f90158");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(exclusiveOrExpressionUri);

            //check if there is the only one ExclusiveOrExpression object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific ExclusiveOrExpression from the result by it's unique id
            var exclusiveOrExpression =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "8c6df21f-07ae-4d0b-ab9b-866dd1f90158");
            ExclusiveOrExpressionTestFixture.VerifyProperties(exclusiveOrExpression);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedExclusiveOrExpressionWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var exclusiveOrExpressionUri =
                new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/requirementsSpecification/bf0cde90-9086-43d5-bcff-32a2f8331800/requirement/614e2a69-d602-46be-9311-2fb4d3273e87/parametricConstraint/88200dbc-711a-47e0-a54a-dac4baca6e83/expression/8c6df21f-07ae-4d0b-ab9b-866dd1f90158?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(exclusiveOrExpressionUri);

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

            // get a specific ExclusiveOrExpression from the result by it's unique id
            var exclusiveOrExpression =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "8c6df21f-07ae-4d0b-ab9b-866dd1f90158");
            ExclusiveOrExpressionTestFixture.VerifyProperties(exclusiveOrExpression);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatAnExclusiveOrExpressionCanBeCreatedWithWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/ExclusiveOrExpression/PostNewExclusiveOrExpression.json");

            var postBody = base.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeringModel =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");

            // Verify the amount of returned properties of the EngineeringModel
            Assert.AreEqual(8, engineeringModel.Children().Count());

            // Assert the properties of EngineeringModel have expected values
            var expectedIterations = new[] {"e163c5ad-f32b-4387-b805-f4b34600bc2c"};
            var iterationsArray = (JArray) engineeringModel[PropertyNames.Iteration];
            IList<string> iterations = iterationsArray.Select(x => (string) x).ToList();
            Assert.That(iterations, Is.EquivalentTo(expectedIterations));

            var expectedLogEntries = new[] {"4e2375eb-8e37-4df2-9c7b-dd896683a891"};
            var logEntriesArray = (JArray) engineeringModel[PropertyNames.LogEntry];
            IList<string> logEntries = logEntriesArray.Select(x => (string) x).ToList();
            Assert.That(logEntries, Is.EquivalentTo(expectedLogEntries));

            var expectedCommonFileStores = new[] {"8e5ca9cc-3da8-4e66-9172-7c3b2464a59c"};
            var commonFileStoresArray = (JArray) engineeringModel[PropertyNames.CommonFileStore];
            IList<string> commonFileStores = commonFileStoresArray.Select(x => (string) x).ToList();
            Assert.That(commonFileStores, Is.EquivalentTo(expectedCommonFileStores));

            Assert.AreEqual("EngineeringModel", (string) engineeringModel[PropertyNames.ClassKind]);
            Assert.AreEqual("116f6253-89bb-47d4-aa24-d11d197e43c9",
                (string) engineeringModel[PropertyNames.EngineeringModelSetup]);
            Assert.AreEqual("9ec982e4-ef72-4953-aa85-b158a95d8d56", (string) engineeringModel[PropertyNames.Iid]);
            Assert.AreEqual(2, (int) engineeringModel[PropertyNames.RevisionNumber]);

            // Get a specific ParametricConstraint from the result by it's unique id
            var parametricConstraint =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "88200dbc-711a-47e0-a54a-dac4baca6e83");

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
                "3160a8e3-a17d-4037-a834-4083c4333c2a"
            };
            var expressionArray = (JArray) parametricConstraint[PropertyNames.Expression];
            IList<string> expressions = expressionArray.Select(x => (string) x).ToList();
            Assert.That(expressions, Is.EquivalentTo(expectedExpressions));

            Assert.AreEqual("ParametricConstraint", (string) parametricConstraint[PropertyNames.ClassKind]);
            Assert.AreEqual("88200dbc-711a-47e0-a54a-dac4baca6e83", (string) parametricConstraint[PropertyNames.Iid]);
            Assert.AreEqual(2, (int) parametricConstraint[PropertyNames.RevisionNumber]);
            Assert.AreEqual("30cb785a-9e72-477f-ad1a-8df6ab623e3d",
                (string) parametricConstraint[PropertyNames.TopExpression]);

            // Get the added AndExpression from the result by it's unique id
            var exclusiveOrExpression =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "3160a8e3-a17d-4037-a834-4083c4333c2a");

            // verify the amount of returned properties 
            Assert.AreEqual(4, exclusiveOrExpression.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("3160a8e3-a17d-4037-a834-4083c4333c2a", (string) exclusiveOrExpression[PropertyNames.Iid]);
            Assert.AreEqual(2, (int) exclusiveOrExpression[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ExclusiveOrExpression", (string) exclusiveOrExpression[PropertyNames.ClassKind]);

            var expectedTerms = new string[]
            {
                "000484d0-cefd-47be-9317-a9eae72c94ce",
                "deaa2560-b704-4b2c-950b-aad02ff84052"
            };
            var termsArray = (JArray) exclusiveOrExpression[PropertyNames.Term];
            IList<string> terms = termsArray.Select(x => (string) x).ToList();
            Assert.That(terms, Is.EquivalentTo(expectedTerms));
        }

        /// <summary>
        /// Verifies all properties of the ExclusiveOrExpression <see cref="JToken"/>
        /// </summary>
        /// <param name="exclusiveOrExpression">
        /// The <see cref="JToken"/> that contains the properties of
        /// the ExclusiveOrExpression object
        /// </param>
        public static void VerifyProperties(JToken exclusiveOrExpression)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(4, exclusiveOrExpression.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("8c6df21f-07ae-4d0b-ab9b-866dd1f90158", (string) exclusiveOrExpression[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) exclusiveOrExpression[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ExclusiveOrExpression", (string) exclusiveOrExpression[PropertyNames.ClassKind]);

            var expectedTerms = new string[]
            {
                "000484d0-cefd-47be-9317-a9eae72c94ce",
                "deaa2560-b704-4b2c-950b-aad02ff84052"
            };
            var termsArray = (JArray) exclusiveOrExpression[PropertyNames.Term];
            IList<string> terms = termsArray.Select(x => (string) x).ToList();
            Assert.That(terms, Is.EquivalentTo(expectedTerms));
        }
    }
}
