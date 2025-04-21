// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BookTestFixture.cs" company="Starion Group S.A.">
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

    using Newtonsoft.Json;

    using NUnit.Framework;

    using WebservicesIntegrationTests.Net;

    /// <summary>
    /// Suite of tests for the Book ClassKind
    /// </summary>
    [TestFixture]
    public class BookTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("POST")]
        [CdpVersion_1_1_0]
        public void VerifyThatBookCanBeAddedAndDeleted()
        {
            SiteDirectoryTestFixture.AddDomainExpertUserJane(this, out var userName, out var passWord);
            this.CreateNewWebClientForUser(userName, passWord);
            
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/Book/PostNewBooks.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            //check if there are 3 classes returned
            Assert.That(jArray.Count, Is.EqualTo(3));

            var engineeringModel = jArray.Single(x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");

            // Verify the amount of returned properties of the EngineeringModel
            Assert.That(engineeringModel.Children().Count(), Is.EqualTo(14));

            var bookList = JsonConvert.DeserializeObject<List<OrderedItem>>(engineeringModel[PropertyNames.Book].ToString());

            var expectedBookList =
                new List<OrderedItem>
                {
                    new OrderedItem(1, "f84b5d72-be4d-418c-90db-19e311e75be3"),
                    new OrderedItem(2, "20a8f908-94c8-4093-8f74-7b6f25433826")
                };

            Assert.That(bookList, Is.EquivalentTo(expectedBookList));


            var book1 = jArray.Single(x => (string) x[PropertyNames.Iid] == "f84b5d72-be4d-418c-90db-19e311e75be3");
            var book2 = jArray.Single(x => (string) x[PropertyNames.Iid] == "20a8f908-94c8-4093-8f74-7b6f25433826");

            Assert.That(book1.Children().Count(), Is.EqualTo(12));
            Assert.That(book2.Children().Count(), Is.EqualTo(12));

            postBodyPath = this.GetPath("Tests/EngineeringModel/Book/PostDeleteBooks.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(iterationUri, postBody);

            engineeringModel = jArray.Single(x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");

            // Verify the amount of returned properties of the EngineeringModel
            Assert.That(engineeringModel.Children().Count(), Is.EqualTo(14));

            bookList = JsonConvert.DeserializeObject<List<OrderedItem>>(engineeringModel[PropertyNames.Book].ToString());

            expectedBookList =
                new List<OrderedItem>
                {
                    new OrderedItem(2, "20a8f908-94c8-4093-8f74-7b6f25433826")
                };

            Assert.That(bookList, Is.EquivalentTo(expectedBookList));
        }
    }
}
