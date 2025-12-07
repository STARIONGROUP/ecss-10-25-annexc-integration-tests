// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DomainOfExpertiseGroupTestFixture.cs" company="Starion Group S.A.">
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
            var domainOfExpertiseGroupUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/domainGroup");

            // Get the response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(domainOfExpertiseGroupUri);
           
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific DomainOfExpertiseGroup from the result by it's unique id
            var domainOfExpertiseGroup = jArray.Single(x => (string)x[PropertyNames.Iid] == "86992db5-8ce2-4431-8ff5-6fe794d14687");

            DomainOfExpertiseGroupTestFixture.VerifyProperties(domainOfExpertiseGroup);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedDomainOfExpertiseGroupWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var domainOfExpertiseGroupUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/domainGroup?includeAllContainers=true");

            // Get the response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(domainOfExpertiseGroupUri);
            
            Assert.That(jArray.Count, Is.EqualTo(2));

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
            Assert.That(domainOfExpertiseGroup.Children().Count(), Is.EqualTo(10));

            Assert.That((string)domainOfExpertiseGroup[PropertyNames.Iid], Is.EqualTo("86992db5-8ce2-4431-8ff5-6fe794d14687"));
            Assert.That((int)domainOfExpertiseGroup[PropertyNames.RevisionNumber], Is.EqualTo(1));
            Assert.That((string)domainOfExpertiseGroup[PropertyNames.ClassKind], Is.EqualTo("DomainOfExpertiseGroup"));

            // assert that the properties are what is expected
            Assert.That((string)domainOfExpertiseGroup[PropertyNames.Name], Is.EqualTo("Test Domain Of ExpertiseGroup"));
            Assert.That((string)domainOfExpertiseGroup[PropertyNames.ShortName], Is.EqualTo("TestDomainOfExpertiseGroup"));
            Assert.That((bool)domainOfExpertiseGroup[PropertyNames.IsDeprecated], Is.False);

            var expectedAliases = new string[] { };
            var aliasesArray = (JArray)domainOfExpertiseGroup[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string)x).ToList();
            Assert.That(aliases, Is.EquivalentTo(expectedAliases));

            var expectedDefinitions = new string[] { };
            var definitionsArray = (JArray)domainOfExpertiseGroup[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string)x).ToList();
            Assert.That(definitions, Is.EquivalentTo(expectedDefinitions));

            var expectedHyperlinks = new string[] { };
            var hyperlinksArray = (JArray)domainOfExpertiseGroup[PropertyNames.HyperLink];
            IList<string> h = hyperlinksArray.Select(x => (string)x).ToList();
            Assert.That(h, Is.EquivalentTo(expectedHyperlinks));
        }
    }
}
