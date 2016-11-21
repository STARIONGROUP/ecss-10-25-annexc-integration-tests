// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmailAddressTestFixture.cs" company="RHEA System">
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
    public class EmailAddressTestFixture : WebClientTestFixtureBase
    {
        /// <summary>
        /// Verification that the EmailAddress objects are returned from the data-source and that the 
        /// values of the EmailAddress properties are equal to to expected value.
        /// </summary>
        [Test]
        public void VerifyThatExpectedEmailAddressIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var emailAddressUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/person/77791b12-4c2c-4499-93fa-869df3692d22/emailAddress"));

            // Get the response from the data-source as a JArray (JSON Array).
            var jArray = this.WebClient.GetDto(emailAddressUri);

            // assert that there are 2 EmailAddress objects.
            Assert.AreEqual(2, jArray.Count);

            EmailAddressTestFixture.VerifyProperties(jArray);
        }

        [Test]
        public void VerifyThatExpectedEmailAddressWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var emailAddressUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/person/77791b12-4c2c-4499-93fa-869df3692d22/emailAddress?includeAllContainers=true"));

            // Get the response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(emailAddressUri);

            // assert that there are 4 objects
            Assert.AreEqual(4, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
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
            var emailAddressObject =
                emailAddress.Single(x => (string) x[PropertyNames.Iid] == "c855d849-62c6-447b-b4e4-db20ba836a91");
            Assert.AreEqual("c855d849-62c6-447b-b4e4-db20ba836a91", (string) emailAddressObject[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) emailAddressObject[PropertyNames.RevisionNumber]);
            Assert.AreEqual("EmailAddress", (string) emailAddressObject[PropertyNames.ClassKind]);
            Assert.AreEqual("john.doe@gmail.com", (string) emailAddressObject[PropertyNames.Value]);
            Assert.AreEqual("HOME", (string) emailAddressObject[PropertyNames.VcardType]);

            emailAddressObject =
                emailAddress.Single(x => (string) x[PropertyNames.Iid] == "325620cd-4354-4ddb-9c66-e75550da643a");
            Assert.AreEqual("325620cd-4354-4ddb-9c66-e75550da643a", (string) emailAddressObject[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) emailAddressObject[PropertyNames.RevisionNumber]);
            Assert.AreEqual("EmailAddress", (string) emailAddressObject[PropertyNames.ClassKind]);
            Assert.AreEqual("john.doe@rhea.com", (string) emailAddressObject[PropertyNames.Value]);
            Assert.AreEqual("WORK", (string) emailAddressObject[PropertyNames.VcardType]);
        }
    }
}