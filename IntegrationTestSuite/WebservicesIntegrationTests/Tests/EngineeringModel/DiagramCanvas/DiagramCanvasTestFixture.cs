// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DiagramCanvasTestFixture.cs" company="RHEA System S.A.">
//
//   Copyright 2016-2024 RHEA System S.A.
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
    using System.Net;

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    using WebservicesIntegrationTests.Net;

    [TestFixture]
    public class DiagramCanvasTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [CdpVersion_1_4_0]
        [Category("GET")]
        public void VerifyThatExpectedDiagramCanvasIsReturnedFromWebApi()
        {
            //-----------------------------------------------------------------------------------------------------------------------------
            // Initial get works?
            //-----------------------------------------------------------------------------------------------------------------------------
            var diagramCanvasUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/diagramCanvas/131298db-5251-45f6-96fa-60a69d7c168a");

            var jArray = this.WebClient.GetDto(diagramCanvasUri);

            Assert.That(jArray.Count, Is.EqualTo(1));

            var diagramCanvas = jArray.Single(x => (string)x[PropertyNames.Iid] == "131298db-5251-45f6-96fa-60a69d7c168a");
            VerifyDefaultProperties(diagramCanvas);

            // Custom/Non default properties
            Assert.That((bool)diagramCanvas[PropertyNames.IsHidden], Is.EqualTo(false));
            Assert.That((string)diagramCanvas[PropertyNames.LockedBy], Is.EqualTo(null));
            Assert.That((int)diagramCanvas[PropertyNames.RevisionNumber], Is.EqualTo(1));

            //-----------------------------------------------------------------------------------------------------------------------------
            // Add 2nd user Jane that is in other DomainOfExpertise and run commands in her context
            //-----------------------------------------------------------------------------------------------------------------------------
            SiteDirectoryTestFixture.AddDomainExpertUserJane(this, out var userName, out var passWord);
            this.CreateNewWebClientForUser(userName, passWord);

            //-----------------------------------------------------------------------------------------------------------------------------
            // Is Jane allowed to read the DiagramCanvas?
            //-----------------------------------------------------------------------------------------------------------------------------
            diagramCanvasUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/diagramCanvas/131298db-5251-45f6-96fa-60a69d7c168a");

            jArray = this.WebClient.GetDto(diagramCanvasUri);

            Assert.That(jArray.Count, Is.EqualTo(1));
        }

        [Test]
        [CdpVersion_1_4_0]
        [Category("POST")]
        public void VerifyThatSetIsHiddenWithoutLockedByFails()
        {
            //-----------------------------------------------------------------------------------------------------------------------------
            // Set only IsHidden on DomainCanvas 
            //----------------------------------------------------------------------------------------------------------------------------
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/DiagramCanvas/PostUpdateDiagramIsHidden.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            Assert.That(() => this.WebClient.PostDto(iterationUri, postBody), Throws.TypeOf<WebException>().With.Message.Contains("400"));
        }

        [Test]
        [CdpVersion_1_4_0]
        [Category("POST")]
        public void VerifyThatRemoveWorks()
        {
            //-----------------------------------------------------------------------------------------------------------------------------
            // Check if remove is allowed for user John (Admin)
            //-----------------------------------------------------------------------------------------------------------------------------
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/DiagramCanvas/PostDeleteDiagram.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            Assert.That(() => this.WebClient.PostDto(iterationUri, postBody), Throws.Nothing);

            //-----------------------------------------------------------------------------------------------------------------------------
            // Check if DiagramCanvas is indeed deleted
            //-----------------------------------------------------------------------------------------------------------------------------
            var diagramCanvasUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/diagramCanvas/131298db-5251-45f6-96fa-60a69d7c168a");

            Assert.That(() => this.WebClient.GetDto(diagramCanvasUri), Throws.TypeOf<WebException>().With.Message.Contains("404"));
        }

        [Test]
        [CdpVersion_1_4_0]
        [Category("POST")]
        public void VerifyThatSetIsLockedWorks()
        {
            //-----------------------------------------------------------------------------------------------------------------------------
            // Set only LockedBy on DiagramCanvas
            //-----------------------------------------------------------------------------------------------------------------------------
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/DiagramCanvas/PostUpdateDiagramLockedBy.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            // get a specific DomainFileStore from the result by it's unique id
            var diagramCanvas = jArray.Single(x => (string)x[PropertyNames.Iid] == "131298db-5251-45f6-96fa-60a69d7c168a");
            VerifyDefaultProperties(diagramCanvas);

            // Custom/Non default properties
            Assert.That((bool)diagramCanvas[PropertyNames.IsHidden], Is.EqualTo(false));
            Assert.That((string)diagramCanvas[PropertyNames.LockedBy], Is.EqualTo("77791b12-4c2c-4499-93fa-869df3692d22"));
            Assert.That((int)diagramCanvas[PropertyNames.RevisionNumber], Is.EqualTo(2));

            //-----------------------------------------------------------------------------------------------------------------------------
            // Add 2nd user Jane that is in other DomainOfExpertise and run commands in her context
            //-----------------------------------------------------------------------------------------------------------------------------
            SiteDirectoryTestFixture.AddDomainExpertUserJane(this, out var userName, out var passWord);
            this.CreateNewWebClientForUser(userName, passWord);

            //-----------------------------------------------------------------------------------------------------------------------------
            // Check if Jane is allowed to read a locked DiagramCanvas (answer should be yes, because not hidden)
            //-----------------------------------------------------------------------------------------------------------------------------
            var diagramCanvasUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/diagramCanvas/131298db-5251-45f6-96fa-60a69d7c168a");

            jArray = this.WebClient.GetDto(diagramCanvasUri);

            Assert.That(jArray.Count, Is.EqualTo(1));

            diagramCanvas = jArray.Single(x => (string)x[PropertyNames.Iid] == "131298db-5251-45f6-96fa-60a69d7c168a");
            VerifyDefaultProperties(diagramCanvas);

            Assert.That((bool)diagramCanvas[PropertyNames.IsHidden], Is.EqualTo(false));
            Assert.That((string)diagramCanvas[PropertyNames.LockedBy], Is.EqualTo("77791b12-4c2c-4499-93fa-869df3692d22"));
            Assert.That((int)diagramCanvas[PropertyNames.RevisionNumber], Is.EqualTo(2));

            //-----------------------------------------------------------------------------------------------------------------------------
            // Check if Jane is allowed to update a locked DiagramCanvas (answer should be no, because not the right user)
            //-----------------------------------------------------------------------------------------------------------------------------
            iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            postBodyPath = this.GetPath("Tests/EngineeringModel/DiagramCanvas/PostUpdateDiagramIsHidden.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            Assert.That(() => this.WebClient.PostDto(iterationUri, postBody), Throws.TypeOf<WebException>().With.Message.Contains("401"));

            //-----------------------------------------------------------------------------------------------------------------------------
            // Check if Jane is allowed to delete a locked DiagramCanvas (answer should be no, because not the right user)
            //-----------------------------------------------------------------------------------------------------------------------------
            iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            postBodyPath = this.GetPath("Tests/EngineeringModel/DiagramCanvas/PostDeleteDiagram.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            Assert.That(() => this.WebClient.PostDto(iterationUri, postBody), Throws.TypeOf<WebException>().With.Message.Contains("401"));
        }

        [Test]
        [CdpVersion_1_4_0]
        [Category("POST")]
        public void VerifyThatSetIsHiddenAndIsLockedWorks()
        {
            //-----------------------------------------------------------------------------------------------------------------------------
            // Set LockedBy AND IsHidden on DiagramCanvas (Only lockedby user is allowed to update/remove and only correct user is allowed to read)
            //-----------------------------------------------------------------------------------------------------------------------------
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/DiagramCanvas/PostUpdateDiagramIsHiddenAndLockedBy.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var diagramCanvas = jArray.Single(x => (string)x[PropertyNames.Iid] == "131298db-5251-45f6-96fa-60a69d7c168a");
            VerifyDefaultProperties(diagramCanvas);

            Assert.That((bool)diagramCanvas[PropertyNames.IsHidden], Is.EqualTo(true));
            Assert.That((string)diagramCanvas[PropertyNames.LockedBy], Is.EqualTo("77791b12-4c2c-4499-93fa-869df3692d22"));
            Assert.That((int)diagramCanvas[PropertyNames.RevisionNumber], Is.EqualTo(2));

            //-----------------------------------------------------------------------------------------------------------------------------
            // Add 2nd user Jane that is in other DomainOfExpertise and run commands in her context
            //-----------------------------------------------------------------------------------------------------------------------------
            SiteDirectoryTestFixture.AddDomainExpertUserJane(this, out var userName, out var passWord);
            this.CreateNewWebClientForUser(userName, passWord);

            //-----------------------------------------------------------------------------------------------------------------------------
            // Check if Jane is allowed to read a locked DiagramCanvas (answer should be no, because hidden and locked by other person)
            //-----------------------------------------------------------------------------------------------------------------------------
            var diagramCanvasUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/diagramCanvas/131298db-5251-45f6-96fa-60a69d7c168a");

            Assert.That(() => this.WebClient.GetDto(diagramCanvasUri), Throws.TypeOf<WebException>().With.Message.Contains("400"));

            //-----------------------------------------------------------------------------------------------------------------------------
            // Check if Jane is allowed to update a locked ArchitectureDiagram (answer should be no, because hidden and locked by other person)
            //-----------------------------------------------------------------------------------------------------------------------------
            iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            postBodyPath = this.GetPath("Tests/EngineeringModel/DiagramCanvas/PostUpdateDiagramIsHidden.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            Assert.That(() => this.WebClient.PostDto(iterationUri, postBody), Throws.TypeOf<WebException>().With.Message.Contains("400"));

            //-----------------------------------------------------------------------------------------------------------------------------
            // Check if Jane is allowed to update a locked ArchitectureDiagram (answer should be no, because hidden and locked by other person)
            //-----------------------------------------------------------------------------------------------------------------------------
            iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            postBodyPath = this.GetPath("Tests/EngineeringModel/DiagramCanvas/PostDeleteDiagram.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            Assert.That(() => this.WebClient.PostDto(iterationUri, postBody), Throws.TypeOf<WebException>().With.Message.Contains("400"));
        }

        //[Test]
        //[Category("GET")]
        //public void VerifyThatExpectedDomainFileStoreWithContainerIsReturnedFromWebApi()
        //{
        //    // define the URI on which to perform a GET request
        //    var domainFileStoreUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/domainFileStore?includeAllContainers=true");

        //    // get a response from the data-source as a JArray (JSON Array)
        //    var jArray = this.WebClient.GetDto(domainFileStoreUri);

        //    //check if there are 3 objects
        //    Assert.AreEqual(3, jArray.Count);

        //    // get a specific Iteration from the result by it's unique id
        //    var iteration = jArray.Single(x => (string) x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
        //    IterationTestFixture.VerifyProperties(iteration);

        //    // get a specific DomainFileStore from the result by it's unique id
        //    var domainFileStore = jArray.Single(x => (string) x[PropertyNames.Iid] == "da7dddaa-02aa-4897-9935-e8d66c811a96");
        //    DomainFileStoreTestFixture.VerifyProperties(domainFileStore);
        //}

        //[Test]
        //[Category("POST")]
        //public void VerifyThatIsHiddenWorks()
        //{
        //    var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
        //    var postBodyPath = this.GetPath("Tests/EngineeringModel/DomainFileStore/PostUpdateDomainFileStoreIsHidden.json");

        //    var postBody = base.GetJsonFromFile(postBodyPath);
        //    var jArray = this.WebClient.PostDto(iterationUri, postBody);

        //    // define the URI on which to perform a domainFileStore GET request
        //    var domainFileStoreUri =
        //        new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/domainFileStore?extent=deep");

        //    // define the URI on which to perform a file GET request
        //    var fileUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/domainFileStore/da7dddaa-02aa-4897-9935-e8d66c811a96/file?extent=deep");

        //    // define the URI on which to perform a folder GET request
        //    var folderUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/domainFileStore/da7dddaa-02aa-4897-9935-e8d66c811a96/folder?extent=deep");

        //    // define the URI on which to perform a fileRevision GET request
        //    var fileRevisionUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/domainFileStore/da7dddaa-02aa-4897-9935-e8d66c811a96/file/95bf0f17-1273-4338-98ae-839016242775/fileRevision");

        //    // get a response from the data-source as a JArray (JSON Array)
        //    jArray = this.WebClient.GetDto(domainFileStoreUri);
        //    Assert.That(jArray.Count, Is.EqualTo(4));

        //    // get a specific DomainFileStore from the result by it's unique id
        //    var domainFileStore = jArray.Single(x => (string)x[PropertyNames.Iid] == "da7dddaa-02aa-4897-9935-e8d66c811a96");

        //    var file = jArray.Single(x => (string)x[PropertyNames.Iid] == "95bf0f17-1273-4338-98ae-839016242775");

        //    var folder = jArray.Single(x => (string)x[PropertyNames.Iid] == "67cdb7de-7721-40a0-9ca2-10a5cf7742fc");

        //    var fileRevision = jArray.Single(x => (string)x[PropertyNames.Iid] == "5544bb87-dc38-45d5-9d92-c580d3fe0442");

        //    Assert.That((string)domainFileStore[PropertyNames.IsHidden], Is.EqualTo("True"));

        //    jArray = this.WebClient.GetDto(fileUri);
        //    Assert.That(jArray.Count, Is.EqualTo(2));

        //    jArray = this.WebClient.GetDto(folderUri);
        //    Assert.That(jArray.Count, Is.EqualTo(1));

        //    jArray = this.WebClient.GetDto(fileRevisionUri);
        //    Assert.That(jArray.Count, Is.EqualTo(1));

        //    SiteDirectoryTestFixture.AddDomainExpertUserJane(this, out var userName, out var passWord);
        //    this.CreateNewWebClientForUser(userName, passWord);

        //    // Jane is not allowed to read it
        //    Assert.Throws<WebException>(() => this.WebClient.GetDto(domainFileStoreUri));

        //    // Jane is not allowed to read it
        //    Assert.Throws<WebException>(() => this.WebClient.GetDto(fileUri));

        //    // Jane is not allowed to read it
        //    Assert.Throws<WebException>(() => this.WebClient.GetDto(folderUri));

        //    // Jane is not allowed to read it
        //    Assert.Throws<WebException>(() => this.WebClient.GetDto(fileRevisionUri));

        //    jArray = this.WebClient.GetDto(iterationUri);

        //    domainFileStore = jArray.SingleOrDefault(x => (string)x[PropertyNames.Iid] == "da7dddaa-02aa-4897-9935-e8d66c811a96");

        //    file = jArray.SingleOrDefault(x => (string)x[PropertyNames.Iid] == "95bf0f17-1273-4338-98ae-839016242775");

        //    folder = jArray.SingleOrDefault(x => (string)x[PropertyNames.Iid] == "67cdb7de-7721-40a0-9ca2-10a5cf7742fc");

        //    fileRevision = jArray.SingleOrDefault(x => (string)x[PropertyNames.Iid] == "5544bb87-dc38-45d5-9d92-c580d3fe0442");

        //    Assert.That(domainFileStore, Is.Null);
        //    Assert.That(file, Is.Null);
        //    Assert.That(folder, Is.Null);
        //    Assert.That(fileRevision, Is.Null);
        //}

        /// <summary>
        /// Verifies all properties of the DiagramCanvas <see cref="JToken"/>
        /// </summary>
        /// <param name="diagramCanvas">
        /// The <see cref="JToken"/> that contains the properties of
        /// the DiagramCanvas object
        /// </param>
        public static void VerifyDefaultProperties(JToken diagramCanvas)
        {
            // verify the amount of returned properties 
            Assert.That(diagramCanvas.Children().Count(), Is.EqualTo(14));

            // assert that the properties are what is expected
            Assert.That((string)diagramCanvas[PropertyNames.Iid], Is.EqualTo("131298db-5251-45f6-96fa-60a69d7c168a"));
            Assert.That((string)diagramCanvas[PropertyNames.ClassKind], Is.EqualTo("DiagramCanvas"));

            Assert.That((string)diagramCanvas[PropertyNames.Name], Is.EqualTo("Canvas 1"));
            Assert.That((string)diagramCanvas[PropertyNames.Description], Is.EqualTo("DiagramCanvas 1"));

            var expectedDiagramElement = new[]
            {
                "d5b99fa2-ab7d-423b-ac31-8a7d79a30243"
            };

            var filesArray = (JArray)diagramCanvas[PropertyNames.DiagramElement];
            IList<string> files = filesArray.Select(x => (string)x).ToList();
            Assert.That(files, Is.EquivalentTo(expectedDiagramElement));
        }
    }
}
