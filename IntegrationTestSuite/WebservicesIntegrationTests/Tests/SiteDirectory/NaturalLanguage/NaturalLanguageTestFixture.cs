// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NaturalLanguageTestFixture.cs" company="Starion Group S.A.">
//
//   Copyright 2016-2025 Starion Group S.A.
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
    using System.Linq;

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;
    
    public class NaturalLanguageTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedNaturalLanguageIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var naturalLanguageUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/naturalLanguage");

            // Get the response from the data-source as a JArray (JSON Array).
            var jArray = this.WebClient.GetDto(naturalLanguageUri);

            // assert that the returned naturalLanguageUri count = 1.
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific naturalLanguage from the result by it's unique id.
            var naturalLanguage = jArray.Single(x => (string)x["iid"] == "73bf30cc-3573-488f-8746-6c03ba47973e");

            NaturalLanguageTestFixture.VerifyProperties(naturalLanguage);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedNaturalLanguageWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var naturalLanguageUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/naturalLanguage?includeAllContainers=true");

            // Get the response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(naturalLanguageUri);

            // assert that the returned object count = 2
            Assert.That(jArray.Count, Is.EqualTo(2));

            var siteDirectory = jArray.Single(x => (string)x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific natural language from the result by it's unique id
            var naturalLanguage = jArray.Single(x => (string)x["iid"] == "73bf30cc-3573-488f-8746-6c03ba47973e");
            NaturalLanguageTestFixture.VerifyProperties(naturalLanguage);
        }

        /// <summary>
        /// Verifies the properties of the Person <see cref="JToken"/>
        /// </summary>
        /// <param name="naturalLanguage">
        /// The <see cref="JToken"/> that contains the properties of
        /// the naturalLanguage object
        /// </param>
        public static void VerifyProperties(JToken naturalLanguage)
        {
            // verify that the amount of returned properties 
            Assert.That(naturalLanguage.Children().Count(), Is.EqualTo(6));

            Assert.That((string)naturalLanguage["iid"], Is.EqualTo("73bf30cc-3573-488f-8746-6c03ba47973e"));
            Assert.That((int)naturalLanguage["revisionNumber"], Is.EqualTo(1));
            Assert.That((string)naturalLanguage["classKind"], Is.EqualTo("NaturalLanguage"));
            Assert.That((string)naturalLanguage["name"], Is.EqualTo("Test Natural Language"));
            Assert.That((string)naturalLanguage["languageCode"], Is.EqualTo("TNL"));
            Assert.That((string)naturalLanguage["nativeName"], Is.EqualTo("test naive name"));
        }
    }
}
