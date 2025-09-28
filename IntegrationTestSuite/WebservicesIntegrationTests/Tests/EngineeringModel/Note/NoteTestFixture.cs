// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NoteTestFixture.cs" company="Starion Group S.A.">
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

    using NUnit.Framework;

    using WebservicesIntegrationTests.Net;

    [TestFixture]
    public class NoteTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Ignore("Doen't work because Note ordered implementation is not OK. See https://github.com/STARIONGROUP/CDP4-WebServices-Community-Edition/issues/126")]
        [CdpVersion_1_1_0]
        [Category("POST")]
        [Test]
        public void VerifyThatNotesCanBeAddedAndDeleted()
        {
            SiteDirectoryTestFixture.AddDomainExpertUserJane(this, out var userName, out var passWord);
            this.CreateNewWebClientForUser(userName, passWord);

            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c"); 
            var postBodyPath = this.GetPath("Tests/EngineeringModel/Book/PostNewBooks.json");
            var postBody = this.GetJsonFromFile(postBodyPath);

            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            //check if there are 3 classes returned
            Assert.That(jArray.Count, Is.EqualTo(3));

            postBodyPath = this.GetPath("Tests/EngineeringModel/Section/PostNewSections.json");
            postBody = this.GetJsonFromFile(postBodyPath);

            jArray = this.WebClient.PostDto(iterationUri, postBody);

            //check if there are 4 classes returned
            Assert.That(jArray.Count, Is.EqualTo(4));

            postBodyPath = this.GetPath("Tests/EngineeringModel/Page/PostNewPages.json");
            postBody = this.GetJsonFromFile(postBodyPath);

            jArray = this.WebClient.PostDto(iterationUri, postBody);

            //check if there are 4 classes returned
            Assert.That(jArray.Count, Is.EqualTo(4));

            postBodyPath = this.GetPath("Tests/EngineeringModel/Note/PostNewNotes.json");
            postBody = this.GetJsonFromFile(postBodyPath);

            jArray = this.WebClient.PostDto(iterationUri, postBody);

            //check if there are 4 classes returned
            Assert.That(jArray.Count, Is.EqualTo(4));

            var page = jArray.Single(x => (string) x[PropertyNames.Iid] == "663114f6-9bb1-4751-a3ed-60bad354913e");

            Assert.That(page.Children().Count(), Is.EqualTo(12));

            var noteList = JsonConvert.DeserializeObject<List<OrderedItem>>(
                page[PropertyNames.Note].ToString());

            var expectedNoteList =
                new List<OrderedItem>
                {
                    new OrderedItem(1, "ab5ddcfb-6302-400b-bd71-17d64a43c747"),
                    new OrderedItem(2, "85ab7b55-6af5-48fb-a14a-86d16dfe1b57")
                };

            Assert.That(noteList, Is.EquivalentTo(expectedNoteList));

            var note1 = jArray.Single(x => (string) x[PropertyNames.Iid] == "ab5ddcfb-6302-400b-bd71-17d64a43c747");
            var note2 = jArray.Single(x => (string) x[PropertyNames.Iid] == "85ab7b55-6af5-48fb-a14a-86d16dfe1b57");

            Assert.That(note1.Children().Count(), Is.EqualTo(12));
            Assert.That(note2.Children().Count(), Is.EqualTo(12));

            postBodyPath = this.GetPath("Tests/EngineeringModel/Note/PostDeleteNote.json");
            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(iterationUri, postBody);

            page = jArray.Single(x => (string) x[PropertyNames.Iid] == "663114f6-9bb1-4751-a3ed-60bad354913e");

            Assert.That(page.Children().Count(), Is.EqualTo(12));

            noteList = JsonConvert.DeserializeObject<List<OrderedItem>>(
                page[PropertyNames.Note].ToString());

            expectedNoteList =
                new List<OrderedItem>
                {
                    new OrderedItem(2, "85ab7b55-6af5-48fb-a14a-86d16dfe1b57")
                };

            Assert.That(
                noteList,
                Is.EquivalentTo(expectedNoteList));
        }
    }
}
