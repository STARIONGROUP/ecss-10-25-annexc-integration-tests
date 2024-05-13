﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BinaryRelationShipTestFixture.cs" company="Starion Group S.A.">
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
    public class BinaryRelationShipTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var binaryRelationshipUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/relationship");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(binaryRelationshipUri);

            //check if there is required amount of BinaryRelationship object 
            Assert.AreEqual(2, jArray.Count);

            // get a specific BinaryRelationship from the result by it's unique id
            var binaryRelationship =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "320869e4-f6d6-4dd2-a696-1b1604f4c4b7");
            BinaryRelationShipTestFixture.VerifyProperties(binaryRelationship);

            // get a specific BinaryRelationship from the result by it's unique id
            var binaryRelationshipWithCategoryEntry =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "138f8a3e-69c6-4e21-b459-bc26b1319a2c");
            BinaryRelationShipTestFixture.VerifyPropertiesWithCategoryEntry(binaryRelationshipWithCategoryEntry);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var binaryRelationshipUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/relationship?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(binaryRelationshipUri);

            //check if there are 3 objects
            Assert.AreEqual(4, jArray.Count);

            // get a specific Iteration from the result by it's unique id
            var iteration =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific BinaryRelationship from the result by it's unique id
            var binaryRelationship =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "320869e4-f6d6-4dd2-a696-1b1604f4c4b7");
            BinaryRelationShipTestFixture.VerifyProperties(binaryRelationship);

            // get a specific BinaryRelationship from the result by it's unique id
            var binaryRelationshipWithCategoryEntry =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "138f8a3e-69c6-4e21-b459-bc26b1319a2c");
            BinaryRelationShipTestFixture.VerifyPropertiesWithCategoryEntry(binaryRelationshipWithCategoryEntry);
        }

        /// <summary>
        /// Verifies all properties of the BinaryRelationship <see cref="JToken"/>
        /// </summary>
        /// <param name="binaryRelationship">
        /// The <see cref="JToken"/> that contains the properties of
        /// the BinaryRelationship object
        /// </param>
        public static void VerifyProperties(JToken binaryRelationship)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(7, binaryRelationship.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("320869e4-f6d6-4dd2-a696-1b1604f4c4b7", (string)binaryRelationship[PropertyNames.Iid]);
            Assert.AreEqual(1, (int)binaryRelationship[PropertyNames.RevisionNumber]);
            Assert.AreEqual("BinaryRelationship", (string)binaryRelationship[PropertyNames.ClassKind]);

            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string)binaryRelationship[PropertyNames.Owner]);
            Assert.AreEqual("790b9e60-476b-4b6d-8aba-0af15178535e", (string)binaryRelationship[PropertyNames.Source]);
            Assert.AreEqual("ff6956dc-1882-4d61-8840-dedb3fba7b43", (string)binaryRelationship[PropertyNames.Target]);
            
            var expectedCategories = new string[] {};
            var categoriesArray = (JArray)binaryRelationship[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedCategories, categories);
        }

        public static void VerifyPropertiesWithCategoryEntry(JToken binaryRelationship)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(7, binaryRelationship.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("138f8a3e-69c6-4e21-b459-bc26b1319a2c", (string)binaryRelationship[PropertyNames.Iid]);
            Assert.AreEqual(1, (int)binaryRelationship[PropertyNames.RevisionNumber]);
            Assert.AreEqual("BinaryRelationship", (string)binaryRelationship[PropertyNames.ClassKind]);

            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string)binaryRelationship[PropertyNames.Owner]);
            Assert.AreEqual("67cdb7de-7721-40a0-9ca2-10a5cf7742fc", (string)binaryRelationship[PropertyNames.Source]);
            Assert.AreEqual("95bf0f17-1273-4338-98ae-839016242775", (string)binaryRelationship[PropertyNames.Target]);

            var expectedCategories = new string[]
            {
                "107fc408-7e6d-4f1a-895a-1b6a6025ac20"
            };
            var categoriesArray = (JArray)binaryRelationship[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedCategories, categories);
        }
    }
}
