// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QuantityKindFactorTestFixture.cs" company="Starion Group S.A.">
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

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class QuantityKindFactorTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedKindFactorIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var quantityKindFactorUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/parameterType/74d9c38f-5ace-4f90-8841-d0f9942e9d09/quantityKindFactor");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(quantityKindFactorUri);

            //check if there is the only one quantityKindFactor object 
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific QuantityKindFactor from the result by it's unique id
            var quantityKindFactor = jArray.Single(x => (string) x[PropertyNames.Iid] == "ab7e80da-6bc9-427f-b1fb-b97faeeca4c6");

            VerifyProperties(quantityKindFactor);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedKindFactorWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var quantityKindFactorUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/parameterType/74d9c38f-5ace-4f90-8841-d0f9942e9d09/quantityKindFactor?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(quantityKindFactorUri);

            //check if there are 4 objects
            Assert.That(jArray.Count, Is.EqualTo(4));

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");

            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary = jArray.Single(x => (string) x[PropertyNames.Iid] == "c454c687-ba3e-44c4-86bc-44544b2c7880");

            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific DerivedQuantityKind from the result by it's unique id
            var derivedQuantityKind = jArray.Single(x => (string) x[PropertyNames.Iid] == "74d9c38f-5ace-4f90-8841-d0f9942e9d09");

            DerivedQuantityKindTestFixture.VerifyProperties(derivedQuantityKind);

            // get a specific QuantityKindFactor from the result by it's unique id
            var quantityKindFactor = jArray.Single(x => (string) x[PropertyNames.Iid] == "ab7e80da-6bc9-427f-b1fb-b97faeeca4c6");

            VerifyProperties(quantityKindFactor);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatDerivedQuantityKindCanBeAddedAndReordered()
        {
            var siteDirectoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");
            var postBodyPath = this.GetPath("Tests/SiteDirectory/QuantityKindFactor/PostNewQuantityKindFactor.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            Assert.That(jArray.Count, Is.EqualTo(3));

            var siteDirectory = jArray.Single(x => (string) x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            Assert.That((int) siteDirectory[PropertyNames.RevisionNumber], Is.EqualTo(2));

            var quantityKindFactor = jArray.Single(x => (string) x[PropertyNames.Iid] == "6b1b9a7b-8a57-412e-a823-bcc4fd8b67e9");
            Assert.That((int) quantityKindFactor[PropertyNames.RevisionNumber], Is.EqualTo(2));

            var derivedQuantityKind = jArray.Single(x => (string) x[PropertyNames.Iid] == "74d9c38f-5ace-4f90-8841-d0f9942e9d09");
            Assert.That((int) derivedQuantityKind[PropertyNames.RevisionNumber], Is.EqualTo(2));

            var expectedQuantityKindFactorArray = new List<OrderedItem> { new OrderedItem(2, "6b1b9a7b-8a57-412e-a823-bcc4fd8b67e9"), new OrderedItem(2948121, "ab7e80da-6bc9-427f-b1fb-b97faeeca4c6") };
            var quantityKindFactorArray = JsonConvert.DeserializeObject<List<OrderedItem>>(derivedQuantityKind[PropertyNames.QuantityKindFactor].ToString());
            Assert.That(quantityKindFactorArray, Is.EquivalentTo(expectedQuantityKindFactorArray));

            postBodyPath = this.GetPath("Tests/SiteDirectory/QuantityKindFactor/PostReorderQuantityKindFactor.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            Assert.That(jArray.Count, Is.EqualTo(4));

            siteDirectory = jArray.Single(x => (string) x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            Assert.That((int) siteDirectory[PropertyNames.RevisionNumber], Is.EqualTo(3));

            quantityKindFactor = jArray.Single(x => (string) x[PropertyNames.Iid] == "6b1b9a7b-8a57-412e-a823-bcc4fd8b67e9");
            Assert.That((int) quantityKindFactor[PropertyNames.RevisionNumber], Is.EqualTo(3));

            var quantityKindFactor2 = jArray.Single(x => (string) x[PropertyNames.Iid] == "ab7e80da-6bc9-427f-b1fb-b97faeeca4c6");
            Assert.That((int) quantityKindFactor2[PropertyNames.RevisionNumber], Is.EqualTo(3));

            derivedQuantityKind = jArray.Single(x => (string) x[PropertyNames.Iid] == "74d9c38f-5ace-4f90-8841-d0f9942e9d09");
            Assert.That((int) derivedQuantityKind[PropertyNames.RevisionNumber], Is.EqualTo(3));

            expectedQuantityKindFactorArray = new List<OrderedItem> { new OrderedItem(1, "ab7e80da-6bc9-427f-b1fb-b97faeeca4c6"), new OrderedItem(3, "6b1b9a7b-8a57-412e-a823-bcc4fd8b67e9") };
            quantityKindFactorArray = JsonConvert.DeserializeObject<List<OrderedItem>>(derivedQuantityKind[PropertyNames.QuantityKindFactor].ToString());
            Assert.That(quantityKindFactorArray, Is.EquivalentTo(expectedQuantityKindFactorArray));
        }

        /// <summary>
        /// Verifies all properties of the QuantityKindFactor <see cref="JToken"/>
        /// </summary>
        /// <param name="quantityKindFactor">
        /// The <see cref="JToken"/> that contains the properties of
        /// the QuantityKindFactor object
        /// </param>
        public static void VerifyProperties(JToken quantityKindFactor)
        {
            // verify the amount of returned properties 
            Assert.That(quantityKindFactor.Children().Count(), Is.EqualTo(5));

            // assert that the properties are what is expected
            Assert.That((string) quantityKindFactor[PropertyNames.Iid], Is.EqualTo("ab7e80da-6bc9-427f-b1fb-b97faeeca4c6"));
            Assert.That((int) quantityKindFactor[PropertyNames.RevisionNumber], Is.EqualTo(1));
            Assert.That((string) quantityKindFactor[PropertyNames.ClassKind], Is.EqualTo("QuantityKindFactor"));
            Assert.That((string) quantityKindFactor[PropertyNames.QuantityKind], Is.EqualTo("4f9f3d9b-f3de-4ef5-b6cb-2e22199fab0d"));
            Assert.That((string) quantityKindFactor[PropertyNames.Exponent], Is.EqualTo("-1"));
        }
    }
}
