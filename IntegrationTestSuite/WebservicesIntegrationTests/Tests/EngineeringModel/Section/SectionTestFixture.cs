// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SectionTestFixture.cs" company="RHEA System S.A.">
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

    using Newtonsoft.Json;

    using NUnit.Framework;

    using WebservicesIntegrationTests.Net;

    [TestFixture]
    public class SectionTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [CdpVersion_1_1_0]
        [Category("POST")]
        public void VerifyThatSectionCanBeAddedAndDeleted()
        {
            SiteDirectoryTestFixture.AddDomainExpertUserJane(this, out var userName, out var passWord);
            this.CreateNewWebClientForUser(userName, passWord);
            
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/Book/PostNewBooks.json");
            var postBody = this.GetJsonFromFile(postBodyPath);

            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            //check if there are 3 classes returned
            Assert.AreEqual(3, jArray.Count);

            postBodyPath = this.GetPath("Tests/EngineeringModel/Section/PostNewSections.json");
            postBody = this.GetJsonFromFile(postBodyPath);

            jArray = this.WebClient.PostDto(iterationUri, postBody);

            //check if there are 4 classes returned
            Assert.AreEqual(4, jArray.Count);

            var book = jArray.Single(x => (string) x[PropertyNames.Iid] == "f84b5d72-be4d-418c-90db-19e311e75be3");

            Assert.AreEqual(12, book.Children().Count());

            var sectionList = JsonConvert.DeserializeObject<List<OrderedItem>>(
                book[PropertyNames.Section].ToString());

            var expectedSectionList =
                new List<OrderedItem>
                {
                    new OrderedItem(1, "c2eccf19-a040-4756-8298-8678d7149c8f"),
                    new OrderedItem(2, "b47ccf37-caa8-4015-b0da-8620894aabce")
                };

            CollectionAssert.AreEquivalent(expectedSectionList, sectionList);

            var section1 = jArray.Single(x => (string) x[PropertyNames.Iid] == "c2eccf19-a040-4756-8298-8678d7149c8f");
            var section2 = jArray.Single(x => (string) x[PropertyNames.Iid] == "b47ccf37-caa8-4015-b0da-8620894aabce");

            Assert.AreEqual(12, section1.Children().Count());
            Assert.AreEqual(12, section2.Children().Count());

            postBodyPath = this.GetPath("Tests/EngineeringModel/Section/PostDeleteSection.json");
            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(iterationUri, postBody);

            book = jArray.Single(x => (string) x[PropertyNames.Iid] == "f84b5d72-be4d-418c-90db-19e311e75be3");

            Assert.AreEqual(12, book.Children().Count());

            sectionList = JsonConvert.DeserializeObject<List<OrderedItem>>(book[PropertyNames.Section].ToString());

            expectedSectionList =
                new List<OrderedItem>
                {
                    new OrderedItem(2, "b47ccf37-caa8-4015-b0da-8620894aabce")
                };

            CollectionAssert.AreEquivalent(
                expectedSectionList,
                sectionList);
        }
    }
}
