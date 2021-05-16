// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DomainOfExpertiseGroupTestFixture.cs" company="RHEA System S.A.">
//
//   Copyright 2016-2021 RHEA System S.A.
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
    public class DomainOfExpertiseGroupTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedDomainOfExpertiseGroupIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var domainOfExpertiseGroupUri = new Uri(string.Format(UriFormat, this.Settings.Hostname, "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/domainGroup"));

            // Get the response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(domainOfExpertiseGroupUri);
           
            Assert.AreEqual(1, jArray.Count);

            // get a specific DomainOfExpertiseGroup from the result by it's unique id
            var domainOfExpertiseGroup = jArray.Single(x => (string)x[PropertyNames.Iid] == "86992db5-8ce2-4431-8ff5-6fe794d14687");

            DomainOfExpertiseGroupTestFixture.VerifyProperties(domainOfExpertiseGroup);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedDomainOfExpertiseGroupWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var domainOfExpertiseGroupUri = new Uri(string.Format(UriFormat, this.Settings.Hostname, "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/domainGroup?includeAllContainers=true"));

            // Get the response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(domainOfExpertiseGroupUri);
            
            Assert.AreEqual(2, jArray.Count);

            var siteDirectory = jArray.Single(x => (string)x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific domain from the result by it's unique id
            var domainOfExpertiseGroup = jArray.Single(x => (string)x[PropertyNames.Iid] == "86992db5-8ce2-4431-8ff5-6fe794d14687");
            DomainOfExpertiseGroupTestFixture.VerifyProperties(domainOfExpertiseGroup);
        }

        /// <summary>
        /// Verifies the properties of the DomainOfExpertiseGroup <see cref="JToken"/>
        /// </summary>
        /// <param name="domainOfExpertiseGroup">
        /// The <see cref="JToken"/> that contains the properties of
        /// the DomainOfExpertiseGroup object
        /// </param>
        public static void VerifyProperties(JToken domainOfExpertiseGroup)
        {
            // verify that the amount of returned properties 
            Assert.AreEqual(10, domainOfExpertiseGroup.Children().Count());

            Assert.AreEqual("86992db5-8ce2-4431-8ff5-6fe794d14687", (string)domainOfExpertiseGroup[PropertyNames.Iid]);
            Assert.AreEqual(1, (int)domainOfExpertiseGroup[PropertyNames.RevisionNumber]);
            Assert.AreEqual("DomainOfExpertiseGroup", (string)domainOfExpertiseGroup[PropertyNames.ClassKind]);

            // assert that the properties are what is expected
            Assert.AreEqual("Test Domain Of ExpertiseGroup", (string)domainOfExpertiseGroup[PropertyNames.Name]);
            Assert.AreEqual("TestDomainOfExpertiseGroup", (string)domainOfExpertiseGroup[PropertyNames.ShortName]);
            Assert.IsFalse((bool)domainOfExpertiseGroup[PropertyNames.IsDeprecated]);

            var expectedAliases = new string[] { };
            var aliasesArray = (JArray)domainOfExpertiseGroup[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] { };
            var definitionsArray = (JArray)domainOfExpertiseGroup[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] { };
            var hyperlinksArray = (JArray)domainOfExpertiseGroup[PropertyNames.HyperLink];
            IList<string> h = hyperlinksArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);
        }
    }
}
