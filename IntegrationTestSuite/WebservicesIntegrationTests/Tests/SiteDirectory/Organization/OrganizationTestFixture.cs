// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrganizationTestFixture.cs" company="Starion Group S.A.">
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
    using System.Linq;

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class OrganizationTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedPersonIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var organizationUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/organization");

            // Get the response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(organizationUri);
           
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific organization from the result by it's unique id
            var organization = jArray.Single(x => (string)x[PropertyNames.Iid] == "cd22fc45-d898-4fac-85fc-fbcb7d7b12a7");

            OrganizationTestFixture.VerifyProperties(organization);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedPersonWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var personsUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/organization?includeAllContainers=true");

            // Get the response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(personsUri);
            
            Assert.AreEqual(2, jArray.Count);

            var siteDirectory = jArray.Single(x => (string)x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific Organization from the result by it's unique id
            var organization = jArray.Single(x => (string)x[PropertyNames.Iid] == "cd22fc45-d898-4fac-85fc-fbcb7d7b12a7");
            OrganizationTestFixture.VerifyProperties(organization);
        }

        /// <summary>
        /// Verifies the properties of the Organization <see cref="JToken"/>
        /// </summary>
        /// <param name="organization">
        /// The <see cref="JToken"/> that contains the properties of
        /// the Organization object
        /// </param>
        public static void VerifyProperties(JToken organization)
        {
            // verify that the amount of returned properties 
            Assert.AreEqual(6, organization.Children().Count());

            Assert.AreEqual("cd22fc45-d898-4fac-85fc-fbcb7d7b12a7", (string)organization[PropertyNames.Iid]);
            Assert.AreEqual(1, (int)organization[PropertyNames.RevisionNumber]);
            Assert.AreEqual("Organization", (string)organization[PropertyNames.ClassKind]);

            // assert that the properties are what is expected
            Assert.AreEqual("Test Organization", (string)organization[PropertyNames.Name]);
            Assert.AreEqual("TestOrganization", (string)organization[PropertyNames.ShortName]);
            Assert.IsFalse((bool)organization[PropertyNames.IsDeprecated]);
        }
    }
}
