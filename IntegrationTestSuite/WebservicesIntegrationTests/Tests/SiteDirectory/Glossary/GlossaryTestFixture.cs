// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GlossaryTestFixture.cs" company="RHEA System">
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
    public class GlossaryTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        /// <summary>
        /// Verification that the Glossary objects are returned from the data-source and that the 
        /// values of the Glossary properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedGlossaryIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var glossaryUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/glossary"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(glossaryUri);

            //check if there is the only one Glossary object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific Glossary from the result by it's unique id
            var glossary =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "bb08686b-ae03-49eb-9f48-c196b5ad6bda");

            GlossaryTestFixture.VerifyProperties(glossary);
        }

        [Test]
        public void VerifyThatExpectedGlossaryWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var glossaryUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/glossary?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(glossaryUri);

            //check if there are 3 objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific Glossary from the result by it's unique id
            var glossary =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "bb08686b-ae03-49eb-9f48-c196b5ad6bda");
            GlossaryTestFixture.VerifyProperties(glossary);
        }

        /// <summary>
        /// Verifies all properties of the Glossary <see cref="JToken"/>
        /// </summary>
        /// <param name="glossary">
        /// The <see cref="JToken"/> that contains the properties of
        /// the Glossary object
        /// </param>
        public static void VerifyProperties(JToken glossary)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(11, glossary.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("bb08686b-ae03-49eb-9f48-c196b5ad6bda", (string) glossary[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) glossary[PropertyNames.RevisionNumber]);
            Assert.AreEqual("Glossary", (string) glossary[PropertyNames.ClassKind]);

            Assert.IsFalse((bool) glossary[PropertyNames.IsDeprecated]);
            Assert.AreEqual("Test Glossary", (string) glossary[PropertyNames.Name]);
            Assert.AreEqual("TestGlossary", (string) glossary[PropertyNames.ShortName]);

            var expectedTerms = new string[]
            {
                "18533006-1b9b-46c1-acc9-ae438ed4ebb2"
            };
            var termsArray = (JArray) glossary["term"];
            IList<string> terms = termsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedTerms, terms);

            var expectedCategories = new string[] {};
            var categoriesArray = (JArray) glossary[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedCategories, categories);

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) glossary[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) glossary[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) glossary[PropertyNames.HyperLink];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);
        }
        
        [Test]
        public void VerifyThatWhenAGlossaryIsDeprecatedTheContainedTermsAreDeprecated()
        {
            var postBodyPath = this.GetPath("Tests/SiteDirectory/Glossary/PostGlossaryUpdate.json");
            var postBody = base.GetJsonFromFile(postBodyPath);

            // define the URI on which to perform a GET request
            var siteDirectoryUri = new Uri(string.Format(UriFormat, this.Settings.Hostname, "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294"));
            var jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            var siteDirectory = jArray.Single(x => (string)x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            Assert.AreEqual(2, (int)siteDirectory[PropertyNames.RevisionNumber]);

            var glossary = jArray.Single(x => (string)x[PropertyNames.Iid] == "bb08686b-ae03-49eb-9f48-c196b5ad6bda");
            Assert.AreEqual(2, (int)glossary[PropertyNames.RevisionNumber]);
            Assert.IsTrue((bool)glossary[PropertyNames.IsDeprecated]);

            var term = jArray.Single(x => (string)x[PropertyNames.Iid] == "18533006-1b9b-46c1-acc9-ae438ed4ebb2");
            Assert.AreEqual(2, (int)term[PropertyNames.RevisionNumber]);
            Assert.IsTrue((bool)term[PropertyNames.IsDeprecated]);

            Assert.AreEqual(3, jArray.Count);
        }
    }
}
