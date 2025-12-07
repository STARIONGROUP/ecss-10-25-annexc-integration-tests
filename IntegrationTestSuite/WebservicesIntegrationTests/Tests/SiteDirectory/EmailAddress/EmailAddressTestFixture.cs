// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmailAddressTestFixture.cs" company="Starion Group S.A.">
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
    public class EmailAddressTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedEmailAddressIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var emailAddressUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/person/77791b12-4c2c-4499-93fa-869df3692d22/emailAddress");
            
            // Get the response from the data-source as a JArray (JSON Array).
            var jArray = this.WebClient.GetDto(emailAddressUri);

            // assert that there are 2 EmailAddress objects.
            Assert.That(jArray.Count, Is.EqualTo(2));

            EmailAddressTestFixture.VerifyProperties(jArray);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedEmailAddressWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var emailAddressUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/person/77791b12-4c2c-4499-93fa-869df3692d22/emailAddress?includeAllContainers=true");
            
            // Get the response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(emailAddressUri);

            // assert that there are 4 objects
            Assert.That(jArray.Count, Is.EqualTo(4));

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific Person from the result by it's unique id
            var person = jArray.Single(x => (string) x[PropertyNames.Iid] == "77791b12-4c2c-4499-93fa-869df3692d22");
            PersonTestFixture.VerifyProperties(person);

            EmailAddressTestFixture.VerifyProperties(jArray);
        }

        /// <summary>
        /// Verifies the properties of the EmailAddress <see cref="JToken"/>
        /// </summary>
        /// <param name="emailAddress">
        /// The <see cref="JToken"/> that contains the properties of
        /// the EmailAddress object
        /// </param>
        public static void VerifyProperties(JToken emailAddress)
        {
            var emailAddressObject = emailAddress.Single(x => (string) x[PropertyNames.Iid] == "c855d849-62c6-447b-b4e4-db20ba836a91");
            Assert.That((string)emailAddressObject[PropertyNames.Iid], Is.EqualTo("c855d849-62c6-447b-b4e4-db20ba836a91"));
            Assert.That((int)emailAddressObject[PropertyNames.RevisionNumber], Is.EqualTo(1));
            Assert.That((string)emailAddressObject[PropertyNames.ClassKind], Is.EqualTo("EmailAddress"));
            Assert.That((string)emailAddressObject[PropertyNames.Value], Is.EqualTo("john.doe@gmail.com"));
            Assert.That((string)emailAddressObject[PropertyNames.VcardType], Is.EqualTo("HOME"));

            emailAddressObject = emailAddress.Single(x => (string) x[PropertyNames.Iid] == "325620cd-4354-4ddb-9c66-e75550da643a");
            Assert.That((string)emailAddressObject[PropertyNames.Iid], Is.EqualTo("325620cd-4354-4ddb-9c66-e75550da643a"));
            Assert.That((int)emailAddressObject[PropertyNames.RevisionNumber], Is.EqualTo(1));
            Assert.That((string)emailAddressObject[PropertyNames.ClassKind], Is.EqualTo("EmailAddress"));
            Assert.That((string)emailAddressObject[PropertyNames.Value], Is.EqualTo("john.doe@stariongrop.eu"));
            Assert.That((string)emailAddressObject[PropertyNames.VcardType], Is.EqualTo("WORK"));
        }
    }
}
