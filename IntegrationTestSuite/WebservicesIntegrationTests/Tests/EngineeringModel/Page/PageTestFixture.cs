// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageTestFixture.cs" company="RHEA System S.A.">
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
    public class PageTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [CdpVersion_1_1_0]
        [Category("POST")]
        public void VerifyThatPagesCanBeAddedAndDeleted()
        {
            SiteDirectoryTestFixture.AddDomainExpertUserJane(this, out var userName, out var passWord);
            this.CreateNewWebClientForUser(userName, passWord);

            var pageUri = new Uri(string.Format(UriFormat, this.Settings.Hostname, "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56"));
            var postBodyPath = this.GetPath("Tests/EngineeringModel/Book/PostNewBooks.json");
            var postBody = this.GetJsonFromFile(postBodyPath);

            var jArray = this.WebClient.PostDto(pageUri, postBody);

            //check if there are 3 classes returned
            Assert.AreEqual(3, jArray.Count);

            postBodyPath = this.GetPath("Tests/EngineeringModel/Section/PostNewSections.json");
            postBody = this.GetJsonFromFile(postBodyPath);

            jArray = this.WebClient.PostDto(pageUri, postBody);

            //check if there are 4 classes returned
            Assert.AreEqual(4, jArray.Count);

            postBodyPath = this.GetPath("Tests/EngineeringModel/Page/PostNewPages.json");
            postBody = this.GetJsonFromFile(postBodyPath);

            jArray = this.WebClient.PostDto(pageUri, postBody);

            //check if there are 4 classes returned
            Assert.AreEqual(4, jArray.Count);

            var section = jArray.Single(x => (string) x[PropertyNames.Iid] == "c2eccf19-a040-4756-8298-8678d7149c8f");

            Assert.AreEqual(12, section.Children().Count());

            var pageList = JsonConvert.DeserializeObject<List<OrderedItem>>(
                section[PropertyNames.Page].ToString());

            var expectedPageList =
                new List<OrderedItem>
                {
                    new OrderedItem(1, "663114f6-9bb1-4751-a3ed-60bad354913e"),
                    new OrderedItem(2, "ad20d5de-54ae-40de-98fb-bb8d2cd7a4b8")
                };

            CollectionAssert.AreEquivalent(
                expectedPageList,
                pageList);

            var section1 = jArray.Single(x => (string) x[PropertyNames.Iid] == "663114f6-9bb1-4751-a3ed-60bad354913e");
            var section2 = jArray.Single(x => (string) x[PropertyNames.Iid] == "ad20d5de-54ae-40de-98fb-bb8d2cd7a4b8");

            Assert.AreEqual(12, section1.Children().Count());
            Assert.AreEqual(12, section2.Children().Count());

            postBodyPath = this.GetPath("Tests/EngineeringModel/Page/PostDeletePage.json");
            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(pageUri, postBody);

            section = jArray.Single(x => (string) x[PropertyNames.Iid] == "c2eccf19-a040-4756-8298-8678d7149c8f");

            Assert.AreEqual(12, section.Children().Count());

            pageList = JsonConvert.DeserializeObject<List<OrderedItem>>(
                section[PropertyNames.Page].ToString());

            expectedPageList =
                new List<OrderedItem>
                {
                    new OrderedItem(2, "ad20d5de-54ae-40de-98fb-bb8d2cd7a4b8")
                };

            CollectionAssert.AreEquivalent(
                expectedPageList,
                pageList);
        }
    }
}
