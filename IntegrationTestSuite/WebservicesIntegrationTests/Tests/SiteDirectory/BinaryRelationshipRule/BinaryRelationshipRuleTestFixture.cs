// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BinaryRelationshipRuleTestFixture.cs" company="Starion Group S.A.">
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
    public class BinaryRelationshipRuleTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedRuleIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var binaryRelationshipRuleUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/rule/8569bd5c-de3c-4d92-855f-b2c0ca94de0e");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(binaryRelationshipRuleUri);

            //check if there is the only one BinaryRelationshipRule object 
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific BinaryRelationshipRule from the result by it's unique id
            var binaryRelationshipRule = jArray.Single(x => (string) x["iid"] == "8569bd5c-de3c-4d92-855f-b2c0ca94de0e");

            BinaryRelationshipRuleTestFixture.VerifyProperties(binaryRelationshipRule);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedRuleWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var binaryRelationshipRuleUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/rule/8569bd5c-de3c-4d92-855f-b2c0ca94de0e?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(binaryRelationshipRuleUri);

            //check if there are 3 objects
            Assert.That(jArray.Count, Is.EqualTo(3));

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary = jArray.Single(x => (string) x["iid"] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific BinaryRelationshipRule from the result by it's unique id
            var binaryRelationshipRule = jArray.Single(x => (string) x["iid"] == "8569bd5c-de3c-4d92-855f-b2c0ca94de0e");
            BinaryRelationshipRuleTestFixture.VerifyProperties(binaryRelationshipRule);
        }

        /// <summary>
        /// Verifies all properties of the BinaryRelationshipRule <see cref="JToken"/>
        /// </summary>
        /// <param name="binaryRelationshipRule">
        /// The <see cref="JToken"/> that contains the properties of
        /// the BinaryRelationshipRule object
        /// </param>
        public static void VerifyProperties(JToken binaryRelationshipRule)
        {
            // verify the amount of returned properties 
            Assert.That(binaryRelationshipRule.Children().Count(), Is.EqualTo(14));

            // assert that the properties are what is expected
            Assert.That((string)binaryRelationshipRule["iid"], Is.EqualTo("8569bd5c-de3c-4d92-855f-b2c0ca94de0e"));
            Assert.That((int)binaryRelationshipRule["revisionNumber"], Is.EqualTo(1));
            Assert.That((string)binaryRelationshipRule["classKind"], Is.EqualTo("BinaryRelationshipRule"));

            Assert.That((bool) binaryRelationshipRule["isDeprecated"], Is.False);
            Assert.That((string)binaryRelationshipRule["name"], Is.EqualTo("Test Binary Relationship Rule"));
            Assert.That((string)binaryRelationshipRule["shortName"], Is.EqualTo("TestBinaryRelationshipRule"));

            Assert.That((string)binaryRelationshipRule["relationshipCategory"], Is.EqualTo("107fc408-7e6d-4f1a-895a-1b6a6025ac20"));
            Assert.That((string)binaryRelationshipRule["sourceCategory"], Is.EqualTo("cf059b19-235c-48be-87a3-9a8942c8e3e0"));
            Assert.That((string)binaryRelationshipRule["targetCategory"], Is.EqualTo("cf059b19-235c-48be-87a3-9a8942c8e3e0"));
            Assert.That((string)binaryRelationshipRule["forwardRelationshipName"], Is.EqualTo("forward"));
            Assert.That((string)binaryRelationshipRule["inverseRelationshipName"], Is.EqualTo("inverse"));

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) binaryRelationshipRule["alias"];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            Assert.That(aliases, Is.EquivalentTo(expectedAliases));

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) binaryRelationshipRule["definition"];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            Assert.That(definitions, Is.EquivalentTo(expectedDefinitions));

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) binaryRelationshipRule["hyperLink"];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            Assert.That(h, Is.EquivalentTo(expectedHyperlinks));
        }
    }
}
