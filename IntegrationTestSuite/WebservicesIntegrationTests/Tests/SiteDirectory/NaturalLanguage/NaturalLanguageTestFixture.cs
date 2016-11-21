// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NaturalLanguageTestFixture.cs" company="RHEA System">
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
    using System.Linq;
    using NUnit.Framework;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The purpose of the <see cref="NaturalLanguageTestFixture"/> is to execute integration tests using the GET and POST
    /// verbs on NaturalLanguage objects.
    /// </summary>   
    public class NaturalLanguageTestFixture : WebClientTestFixtureBase
    {
        /// <summary>
        /// Verification that the Person objects are returned from the data-source and that the 
        /// values of the person properties are equal to to expected value.
        /// </summary>
        [Test]
        public void VerifyThatExpectedNaturalLanguageIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var naturalLanguageUri = new Uri(string.Format(UriFormat, this.Settings.Hostname, "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/naturalLanguage"));

            // Get the response from the data-source as a JArray (JSON Array).
            var jArray = this.WebClient.GetDto(naturalLanguageUri);

            // assert that the returned naturalLanguageUri count = 1.
            Assert.AreEqual(1, jArray.Count);

            // get a specific naturalLanguage from the result by it's unique id.
            var naturalLanguage = jArray.Single(x => (string)x["iid"] == "73bf30cc-3573-488f-8746-6c03ba47973e");

            NaturalLanguageTestFixture.VerifyProperties(naturalLanguage);
        }

        [Test]
        public void VerifyThatExpectedNaturalLanguageWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var naturalLanguageUri = new Uri(string.Format(UriFormat, this.Settings.Hostname, "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/naturalLanguage?includeAllContainers=true"));

            // Get the response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(naturalLanguageUri);

            // assert that the returned object count = 2
            Assert.AreEqual(2, jArray.Count);

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
            Assert.AreEqual(6, naturalLanguage.Children().Count());

            Assert.AreEqual("73bf30cc-3573-488f-8746-6c03ba47973e", (string)naturalLanguage["iid"]);
            Assert.AreEqual(1, (int)naturalLanguage["revisionNumber"]);
            Assert.AreEqual("NaturalLanguage", (string)naturalLanguage["classKind"]);
            Assert.AreEqual("Test Natural Language", (string)naturalLanguage["name"]);
            Assert.AreEqual("TNL", (string)naturalLanguage["languageCode"]);
            Assert.AreEqual("test naive name", (string)naturalLanguage["nativeName"]);
        }
    }
}
