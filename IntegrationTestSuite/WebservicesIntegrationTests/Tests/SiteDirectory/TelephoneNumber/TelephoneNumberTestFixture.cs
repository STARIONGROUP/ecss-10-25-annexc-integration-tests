// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TelephoneNumberTestFixture.cs" company="RHEA System">
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
    public class TelephoneNumberTestFixture : WebClientTestFixtureBase
    {
        /// <summary>
        /// Verification that the TelephoneNumber objects are returned from the data-source and that the 
        /// values of the TelephoneNumber properties are equal to to expected value.
        /// </summary>
        [Test]
        public void VerifyThatExpectedTelephoneNumbersIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var telephoneNumbersUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/person/77791b12-4c2c-4499-93fa-869df3692d22/telephoneNumber"));

            // Get the response from the data-source as a JArray (JSON Array).
            var jArray = this.WebClient.GetDto(telephoneNumbersUri);

            // assert that there are 2 TelephoneNumber objects.
            Assert.AreEqual(2, jArray.Count);

            TelephoneNumberTestFixture.VerifyProperties(jArray);
        }

        [Test]
        public void VerifyThatExpectedTelephoneNumbersWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var telephoneNumbersUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/person/77791b12-4c2c-4499-93fa-869df3692d22/telephoneNumber?includeAllContainers=true"));

            // Get the response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(telephoneNumbersUri);

            // assert that there are 4 objects
            Assert.AreEqual(4, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific Person from the result by it's unique id
            var person = jArray.Single(x => (string) x[PropertyNames.Iid] == "77791b12-4c2c-4499-93fa-869df3692d22");
            PersonTestFixture.VerifyProperties(person);

            TelephoneNumberTestFixture.VerifyProperties(jArray);
        }

        /// <summary>
        /// Verifies the properties of the TelephoneNumber <see cref="JToken"/>
        /// </summary>
        /// <param name="telephoneNumber">
        /// The <see cref="JToken"/> that contains the properties of
        /// the TelephoneNumber object
        /// </param>
        public static void VerifyProperties(JToken telephoneNumber)
        {
            var telephoneNumberObject =
                telephoneNumber.Single(x => (string) x[PropertyNames.Iid] == "7f85a641-1844-4064-b19d-c6a447543ab3");
            Assert.AreEqual("7f85a641-1844-4064-b19d-c6a447543ab3", (string) telephoneNumberObject[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) telephoneNumberObject[PropertyNames.RevisionNumber]);
            Assert.AreEqual("TelephoneNumber", (string) telephoneNumberObject[PropertyNames.ClassKind]);
            Assert.AreEqual("+38-044-444-44-44", (string) telephoneNumberObject[PropertyNames.Value]);

            var expectedVcardTypes = new string[]
            {
                "HOME"
            };
            var vcardTypesArray = (JArray) telephoneNumberObject[PropertyNames.VcardType];
            IList<string> vcardTypes = vcardTypesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedVcardTypes, vcardTypes);

            telephoneNumberObject =
                telephoneNumber.Single(x => (string) x[PropertyNames.Iid] == "0367167c-80cb-4f99-a24b-e713efd15436");
            Assert.AreEqual("0367167c-80cb-4f99-a24b-e713efd15436", (string) telephoneNumberObject[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) telephoneNumberObject[PropertyNames.RevisionNumber]);
            Assert.AreEqual("TelephoneNumber", (string) telephoneNumberObject[PropertyNames.ClassKind]);
            Assert.AreEqual("+38-066-666-66-66", (string) telephoneNumberObject[PropertyNames.Value]);

            expectedVcardTypes = new string[]
            {
                "WORK",
                "CELL"
            };
            vcardTypesArray = (JArray) telephoneNumberObject[PropertyNames.VcardType];
            vcardTypes = vcardTypesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedVcardTypes, vcardTypes);
        }
    }
}