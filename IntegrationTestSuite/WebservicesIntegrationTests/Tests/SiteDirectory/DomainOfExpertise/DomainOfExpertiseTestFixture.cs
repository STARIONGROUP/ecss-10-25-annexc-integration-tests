// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DomainOfExpertiseTestFixture.cs" company="RHEA System">
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

    [TestFixture]
    public class DomainOfExpertiseTestFixture : WebClientTestFixtureBase
    {
        /// <summary>
        /// Verification that the DomainOfExpertise objects are returned from the data-source and that the 
        /// values of the DomainOfExpertise properties are equal to to expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedDomainOfExpertiseIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var domainOfExpertiseUri = new Uri(string.Format(UriFormat, this.Settings.Hostname, "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/domain"));

            // Get the response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(domainOfExpertiseUri);
           
            Assert.AreEqual(2, jArray.Count);

            // get a specific DomainOfExpertise from the result by it's unique id
            var domainOfExpertise = jArray.Single(x => (string)x["iid"] == "0e92edde-fdff-41db-9b1d-f2e484f12535");
            DomainOfExpertiseTestFixture.VerifyProperties(domainOfExpertise);

            // get a specific DomainOfExpertise from the result by it's unique id
            domainOfExpertise = jArray.Single(x => (string)x["iid"] == "eb759723-14b9-49f4-8611-544d037bb764");
            DomainOfExpertiseTestFixture.VerifyProperties(domainOfExpertise);
        }

        [Test]
        public void VerifyThatExpectedDomainOfExpertiseWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var domainOfExpertiseUri = new Uri(string.Format(UriFormat, this.Settings.Hostname, "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/domain?includeAllContainers=true"));

            // Get the response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(domainOfExpertiseUri);
            
            Assert.AreEqual(3, jArray.Count);

            var siteDirectory = jArray.Single(x => (string)x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific DomainOfExpertise from the result by it's unique id
            var domainOfExpertise = jArray.Single(x => (string)x["iid"] == "0e92edde-fdff-41db-9b1d-f2e484f12535");
            DomainOfExpertiseTestFixture.VerifyProperties(domainOfExpertise);
        }

        /// <summary>
        /// Verifies the properties of the DomainOfExpertise <see cref="JToken"/>
        /// </summary>
        /// <param name="domainOfExpertise">
        /// The <see cref="JToken"/> that contains the properties of
        /// the DomainOfExpertise object
        /// </param>
        public static void VerifyProperties(JToken domainOfExpertise)
        {
            // verify that the amount of returned properties 
            Assert.AreEqual(10, domainOfExpertise.Children().Count());

            if ((string)domainOfExpertise["iid"] == "0e92edde-fdff-41db-9b1d-f2e484f12535")
            {
                Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string)domainOfExpertise["iid"]);
                Assert.AreEqual(1, (int)domainOfExpertise["revisionNumber"]);
                Assert.AreEqual("DomainOfExpertise", (string)domainOfExpertise["classKind"]);

                // assert that the properties are what is expected
                Assert.AreEqual("Test Domain of Expertise", (string)domainOfExpertise["name"]);
                Assert.AreEqual("TST", (string)domainOfExpertise["shortName"]);
                Assert.IsFalse((bool)domainOfExpertise["isDeprecated"]);

                var expectedAliases = new string[] { };
                var aliasesArray = (JArray)domainOfExpertise["alias"];
                IList<string> aliases = aliasesArray.Select(x => (string)x).ToList();
                CollectionAssert.AreEquivalent(expectedAliases, aliases);

                var expectedDefinitions = new string[] { "23658615-a170-4c0f-ba71-da1a15c736ca" };
                var definitionsArray = (JArray)domainOfExpertise["definition"];
                IList<string> definitions = definitionsArray.Select(x => (string)x).ToList();
                CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

                var expectedHyperlinks = new string[] { };
                var hyperlinksArray = (JArray)domainOfExpertise["hyperLink"];
                IList<string> h = hyperlinksArray.Select(x => (string)x).ToList();
                CollectionAssert.AreEquivalent(expectedHyperlinks, h);

                var expectedCategories = new string[] { };
                var categoryArray = (JArray)domainOfExpertise["category"];
                IList<string> categories = categoryArray.Select(x => (string)x).ToList();
                CollectionAssert.AreEquivalent(expectedCategories, categories);
            }

            if ((string)domainOfExpertise["iid"] == "eb759723-14b9-49f4-8611-544d037bb764")
            {
                Assert.AreEqual("eb759723-14b9-49f4-8611-544d037bb764", (string)domainOfExpertise["iid"]);
                Assert.AreEqual(1, (int)domainOfExpertise["revisionNumber"]);
                Assert.AreEqual("DomainOfExpertise", (string)domainOfExpertise["classKind"]);

                // assert that the properties are what is expected
                Assert.AreEqual("Additional Test Domain of Expertise", (string)domainOfExpertise["name"]);
                Assert.AreEqual("ADD", (string)domainOfExpertise["shortName"]);
                Assert.IsFalse((bool)domainOfExpertise["isDeprecated"]);

                var expectedAliases = new string[] { };
                var aliasesArray = (JArray)domainOfExpertise["alias"];
                IList<string> aliases = aliasesArray.Select(x => (string)x).ToList();
                CollectionAssert.AreEquivalent(expectedAliases, aliases);

                var expectedDefinitions = new string[] {};
                var definitionsArray = (JArray)domainOfExpertise["definition"];
                IList<string> definitions = definitionsArray.Select(x => (string)x).ToList();
                CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

                var expectedHyperlinks = new string[] { };
                var hyperlinksArray = (JArray)domainOfExpertise["hyperLink"];
                IList<string> h = hyperlinksArray.Select(x => (string)x).ToList();
                CollectionAssert.AreEquivalent(expectedHyperlinks, h);

                var expectedCategories = new string[] { };
                var categoryArray = (JArray)domainOfExpertise["category"];
                IList<string> categories = categoryArray.Select(x => (string)x).ToList();
                CollectionAssert.AreEquivalent(expectedCategories, categories);
            }

        }
    }
}
