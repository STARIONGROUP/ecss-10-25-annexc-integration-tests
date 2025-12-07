// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrganizationTestFixture.cs" company="Starion Group S.A.">
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
            
            Assert.That(jArray.Count, Is.EqualTo(2));

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
            Assert.That(organization.Children().Count(), Is.EqualTo(6));

            Assert.That((string)organization[PropertyNames.Iid], Is.EqualTo("cd22fc45-d898-4fac-85fc-fbcb7d7b12a7"));
            Assert.That((int)organization[PropertyNames.RevisionNumber], Is.EqualTo(1));
            Assert.That((string)organization[PropertyNames.ClassKind], Is.EqualTo("Organization"));

            // assert that the properties are what is expected
            Assert.That((string)organization[PropertyNames.Name], Is.EqualTo("Test Organization"));
            Assert.That((string)organization[PropertyNames.ShortName], Is.EqualTo("TestOrganization"));
            Assert.That((bool)organization[PropertyNames.IsDeprecated], Is.False);
        }
    }
}
