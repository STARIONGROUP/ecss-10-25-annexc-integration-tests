﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModelLogEntryTestFixture.cs" company="Starion Group S.A.">
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
    public class ModelLogEntryTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedModelLogEntryIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var modelLogEntryUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/logEntry");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(modelLogEntryUri);

            //check if there is the only one ModelLogEntry object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific ModelLogEntry from the result by it's unique id
            var modelLogEntry = jArray.Single(x => (string) x[PropertyNames.Iid] == "4e2375eb-8e37-4df2-9c7b-dd896683a891");
            ModelLogEntryTestFixture.VerifyProperties(modelLogEntry);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedModelLogEntryWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var modelLogEntryUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/logEntry?includeAllContainers=true");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(modelLogEntryUri);

            //check if there are 2 objects
            Assert.AreEqual(2, jArray.Count);

            // get a specific ModelLogEntry from the result by it's unique id
            var modelLogEntry = jArray.Single(x => (string) x[PropertyNames.Iid] == "4e2375eb-8e37-4df2-9c7b-dd896683a891");
            ModelLogEntryTestFixture.VerifyProperties(modelLogEntry);
        }

        /// <summary>
        /// Verifies all properties of the ModelLogEntry <see cref="JToken"/>
        /// </summary>
        /// <param name="modelLogEntry">
        /// The <see cref="JToken"/> that contains the properties of
        /// the ModelLogEntry object
        /// </param>
        public static void VerifyProperties(JToken modelLogEntry)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(11, modelLogEntry.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("4e2375eb-8e37-4df2-9c7b-dd896683a891", (string) modelLogEntry[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) modelLogEntry[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ModelLogEntry", (string) modelLogEntry[PropertyNames.ClassKind]);

            Assert.AreEqual("TRACE", (string) modelLogEntry[PropertyNames.Level]);
            Assert.IsNull((string) modelLogEntry[PropertyNames.Author]);
            Assert.AreEqual("2016-11-07T09:08:36.186Z", (string) modelLogEntry[PropertyNames.CreatedOn]);
            Assert.AreEqual("testContent", (string) modelLogEntry[PropertyNames.Content]);
            Assert.AreEqual("en", (string) modelLogEntry[PropertyNames.LanguageCode]);

            var expectedCategories = new string[] {};
            var categoriesArray = (JArray) modelLogEntry[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string) x).ToList();
            Assert.That(categories, Is.EquivalentTo(expectedCategories));

            var expectedaffectedItemIids = new string[] {};
            var affectedItemIidsArray = (JArray) modelLogEntry[PropertyNames.AffectedItemIid];
            IList<string> affectedItemIids = affectedItemIidsArray.Select(x => (string) x).ToList();
            Assert.That(affectedItemIids, Is.EquivalentTo(expectedaffectedItemIids));
        }
    }
}
