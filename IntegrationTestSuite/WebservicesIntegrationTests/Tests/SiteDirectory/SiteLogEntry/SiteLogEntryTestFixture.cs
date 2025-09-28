// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SiteLogEntryTestFixture.cs" company="Starion Group S.A.">
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
    public class SiteLogEntryTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedSiteLogEntriesIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var siteLogEntryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/logEntry");
            
            // Get the response from the data-source as a JArray (JSON Array).
            var jArray = this.WebClient.GetDto(siteLogEntryUri);

            // assert that there are 2 SiteLogEntry objects.
            Assert.That(jArray.Count, Is.EqualTo(2));

            SiteLogEntryTestFixture.VerifyProperties(jArray);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedSiteLogEntryWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var siteLogEntryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/logEntry?includeAllContainers=true");

            // Get the response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(siteLogEntryUri);

            // assert that there are 3 objects
            Assert.That(jArray.Count, Is.EqualTo(3));

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            SiteLogEntryTestFixture.VerifyProperties(jArray);
        }

        /// <summary>
        /// Verifies the properties of the SiteLogEntry <see cref="JToken"/>
        /// </summary>
        /// <param name="siteLogEntry">
        /// The <see cref="JToken"/> that contains the properties of
        /// the SiteLogEntry object
        /// </param>
        public static void VerifyProperties(JToken siteLogEntry)
        {
            var siteLogEntryObject = siteLogEntry.Single(x => (string) x[PropertyNames.Iid] == "98ba7b8a-1a1b-4569-a17c-b1ff620246a5");
            Assert.That((string) siteLogEntryObject[PropertyNames.Iid], Is.EqualTo("98ba7b8a-1a1b-4569-a17c-b1ff620246a5"));
            Assert.That((int) siteLogEntryObject[PropertyNames.RevisionNumber], Is.EqualTo(1));
            Assert.That((string) siteLogEntryObject[PropertyNames.ClassKind], Is.EqualTo("SiteLogEntry"));
            Assert.That((string) siteLogEntryObject[PropertyNames.Level], Is.EqualTo("TRACE"));

            // verify that there are no affectedItemIid for this SiteLogEntry
            var affectedItemIids = (JArray) siteLogEntryObject[PropertyNames.AffectedItemIid];
            IList<string> affectedItemIidsList = affectedItemIids.Select(x => (string) x).ToList();
            Assert.IsEmpty(affectedItemIidsList);

            Assert.That((string) siteLogEntryObject[PropertyNames.Author], Is.EqualTo("77791b12-4c2c-4499-93fa-869df3692d22"));

            // verify that there are no categories for this SiteLogEntry
            var categories = (JArray) siteLogEntryObject[PropertyNames.Category];
            IList<string> categoriesList = categories.Select(x => (string) x).ToList();
            Assert.IsEmpty(categoriesList);

            Assert.That((string) siteLogEntryObject[PropertyNames.Content], Is.EqualTo("TestLogEntry"));
            Assert.That((string) siteLogEntryObject[PropertyNames.LanguageCode], Is.EqualTo("en-GB"));
            Assert.That((string) siteLogEntryObject[PropertyNames.CreatedOn], Is.EqualTo("2016-10-19T11:15:32.186Z"));

            //Second logEntry
            siteLogEntryObject = siteLogEntry.Single(x => (string) x[PropertyNames.Iid] == "66220289-e6ee-43cb-8fcd-d8e59a3dbf97");
            Assert.That((string) siteLogEntryObject[PropertyNames.Iid], Is.EqualTo("66220289-e6ee-43cb-8fcd-d8e59a3dbf97"));
            Assert.That((int) siteLogEntryObject[PropertyNames.RevisionNumber], Is.EqualTo(1));
            Assert.That((string) siteLogEntryObject[PropertyNames.ClassKind], Is.EqualTo("SiteLogEntry"));
            Assert.That((string) siteLogEntryObject[PropertyNames.Level], Is.EqualTo("DEBUG"));

            // verify that there are no affectedItemIid for this SiteLogEntry
            affectedItemIids = (JArray) siteLogEntryObject[PropertyNames.AffectedItemIid];
            affectedItemIidsList = affectedItemIids.Select(x => (string) x).ToList();
            Assert.IsEmpty(affectedItemIidsList);

            Assert.That((string) siteLogEntryObject[PropertyNames.Author], Is.EqualTo("77791b12-4c2c-4499-93fa-869df3692d22"));

            // verify that there are no categories for this SiteLogEntry
            categories = (JArray) siteLogEntryObject[PropertyNames.Category];
            categoriesList = categories.Select(x => (string) x).ToList();
            Assert.IsEmpty(categoriesList);

            Assert.That((string) siteLogEntryObject[PropertyNames.Content], Is.EqualTo("TestLogEntry"));
            Assert.That((string) siteLogEntryObject[PropertyNames.LanguageCode], Is.EqualTo("en-GB"));
            Assert.That((string) siteLogEntryObject[PropertyNames.CreatedOn], Is.EqualTo("2016-10-19T11:15:42.384Z"));
        }
    }
}