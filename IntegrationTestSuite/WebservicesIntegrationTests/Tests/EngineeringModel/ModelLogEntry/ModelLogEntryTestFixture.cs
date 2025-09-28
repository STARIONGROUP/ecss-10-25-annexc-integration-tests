// --------------------------------------------------------------------------------------------------------------------
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
            Assert.That(jArray.Count, Is.EqualTo(1));

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
            Assert.That(jArray.Count, Is.EqualTo(2));

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
            Assert.That(modelLogEntry.Children().Count(), Is.EqualTo(11));

            // assert that the properties are what is expected
            Assert.That((string) modelLogEntry[PropertyNames.Iid], Is.EqualTo("4e2375eb-8e37-4df2-9c7b-dd896683a891"));
            Assert.That((int) modelLogEntry[PropertyNames.RevisionNumber], Is.EqualTo(1));
            Assert.That((string) modelLogEntry[PropertyNames.ClassKind], Is.EqualTo("ModelLogEntry"));

            Assert.That((string) modelLogEntry[PropertyNames.Level], Is.EqualTo("TRACE"));
            Assert.That((string) modelLogEntry[PropertyNames.Author], Is.Null);
            Assert.That((string) modelLogEntry[PropertyNames.CreatedOn], Is.EqualTo("2016-11-07T09:08:36.186Z"));
            Assert.That((string) modelLogEntry[PropertyNames.Content], Is.EqualTo("testContent"));
            Assert.That((string) modelLogEntry[PropertyNames.LanguageCode], Is.EqualTo("en"));

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
