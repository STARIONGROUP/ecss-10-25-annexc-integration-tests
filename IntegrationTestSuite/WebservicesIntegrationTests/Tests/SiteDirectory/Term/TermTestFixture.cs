// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TermTestFixture.cs" company="Starion Group S.A.">
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
    public class TermTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedTermIsReturnedFromWebApi()
        {
            var termUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/glossary/bb08686b-ae03-49eb-9f48-c196b5ad6bda/term");

            var jArray = this.WebClient.GetDto(termUri);
            
            Assert.AreEqual(1, jArray.Count);
            
            var term = jArray.Single(x => (string) x["iid"] == "18533006-1b9b-46c1-acc9-ae438ed4ebb2");

            TermTestFixture.VerifyProperties(term);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedTermWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var termUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/glossary/bb08686b-ae03-49eb-9f48-c196b5ad6bda/term/18533006-1b9b-46c1-acc9-ae438ed4ebb2?includeAllContainers=true");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(termUri);

            //check if there are 3 objects
            Assert.AreEqual(4, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary = jArray.Single(x => (string) x["iid"] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific Glossary from the result by it's unique id
            var glossary = jArray.Single(x => (string) x["iid"] == "bb08686b-ae03-49eb-9f48-c196b5ad6bda");
            GlossaryTestFixture.VerifyProperties(glossary);

            // get a specific Term from the result by it's unique id
            var term = jArray.Single(x => (string) x["iid"] == "18533006-1b9b-46c1-acc9-ae438ed4ebb2");
            TermTestFixture.VerifyProperties(term);
        }

        /// <summary>
        /// Verifies all properties of the Term <see cref="JToken"/>
        /// </summary>
        /// <param name="term">
        /// The <see cref="JToken"/> that contains the properties of
        /// the Term object
        /// </param>
        public static void VerifyProperties(JToken term)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(9, term.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("18533006-1b9b-46c1-acc9-ae438ed4ebb2", (string) term["iid"]);
            Assert.AreEqual(1, (int) term["revisionNumber"]);
            Assert.AreEqual("Term", (string) term["classKind"]);

            Assert.IsFalse((bool) term["isDeprecated"]);
            Assert.AreEqual("Test Term", (string) term["name"]);
            Assert.AreEqual("TestTerm", (string) term["shortName"]);

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) term["alias"];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) term["definition"];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) term["hyperLink"];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);
        }
    }
}
