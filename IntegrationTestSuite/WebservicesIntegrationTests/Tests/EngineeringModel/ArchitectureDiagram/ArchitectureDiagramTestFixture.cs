// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArchitectureDiagramTestFixture.cs" company="RHEA System S.A.">
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
    public class ArchitectureDiagramTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [CdpVersion_1_4_0]
        [Category("GET")]
        public void VerifyThatExpectedArchitectureDiagramIsReturnedFromWebApi()
        {
            //-----------------------------------------------------------------------------------------------------------------------------
            // Initial get works?
            //-----------------------------------------------------------------------------------------------------------------------------
            var architectureDiagramUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/diagramCanvas/05a0668b-64a0-438f-bf81-c917b9345592");

            var jArray = this.WebClient.GetDto(architectureDiagramUri);

            Assert.That(jArray.Count, Is.EqualTo(1));

            var architectureDiagram = jArray.Single(x => (string)x[PropertyNames.Iid] == "05a0668b-64a0-438f-bf81-c917b9345592");
            VerifyDefaultProperties(architectureDiagram);

            Assert.That((bool)architectureDiagram[PropertyNames.IsHidden], Is.EqualTo(false));
            Assert.That((string)architectureDiagram[PropertyNames.LockedBy], Is.EqualTo(null));
            Assert.That((int)architectureDiagram[PropertyNames.RevisionNumber], Is.EqualTo(1));

            //-----------------------------------------------------------------------------------------------------------------------------
            // Add 2nd user Jane that is in other DomainOfExpertise and run commands in her context
            //-----------------------------------------------------------------------------------------------------------------------------
            SiteDirectoryTestFixture.AddDomainExpertUserJane(this, out var userName, out var passWord);
            this.CreateNewWebClientForUser(userName, passWord);

            //-----------------------------------------------------------------------------------------------------------------------------
            // Is Jane allowed to read the ArchitectureDiagram?
            //-----------------------------------------------------------------------------------------------------------------------------
            architectureDiagramUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/diagramCanvas/05a0668b-64a0-438f-bf81-c917b9345592");

            jArray = this.WebClient.GetDto(architectureDiagramUri);

            Assert.That(jArray.Count, Is.EqualTo(1));
        }

        [Test]
        [CdpVersion_1_4_0]
        [Category("POST")]
        public void VerifyThatSetIsHiddenWithoutLockedByWorks()
        {
            //-----------------------------------------------------------------------------------------------------------------------------
            // Set only IsHidden on ArchitectureDiagram 
            //-----------------------------------------------------------------------------------------------------------------------------
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/ArchitectureDiagram/PostUpdateDiagramIsHidden.json");

            var postBody = this.GetJsonFromFile(postBodyPath);

            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            Assert.That(jArray.Count, Is.EqualTo(2));

            var architectureDiagram = jArray.Single(x => (string)x[PropertyNames.Iid] == "05a0668b-64a0-438f-bf81-c917b9345592");
            VerifyDefaultProperties(architectureDiagram);

            Assert.That((bool)architectureDiagram[PropertyNames.IsHidden], Is.EqualTo(true));
            Assert.That((string)architectureDiagram[PropertyNames.LockedBy], Is.EqualTo(null));
            Assert.That((int)architectureDiagram[PropertyNames.RevisionNumber], Is.EqualTo(2));

            //-----------------------------------------------------------------------------------------------------------------------------
            // Add 2nd user Jane that is in other DomainOfExpertise and run commands in her context
            //-----------------------------------------------------------------------------------------------------------------------------
            SiteDirectoryTestFixture.AddDomainExpertUserJane(this, out var userName, out var passWord);
            this.CreateNewWebClientForUser(userName, passWord);

            //-----------------------------------------------------------------------------------------------------------------------------
            // Check all access kinds (Read/Write/Delete) for Jane based on IsHidden (ArchitectureDiagram IsHidden means hidden to other domains)
            //-----------------------------------------------------------------------------------------------------------------------------
            var architectureDiagramUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/diagramCanvas/05a0668b-64a0-438f-bf81-c917b9345592");

            //No read for jane
            Assert.That(() => this.WebClient.GetDto(architectureDiagramUri), Throws.TypeOf<WebException>().With.Message.Contains("400"));

            iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            postBodyPath = this.GetPath("Tests/EngineeringModel/ArchitectureDiagram/PostUpdateDiagramIsHidden.json");

            postBody = this.GetJsonFromFile(postBodyPath);

            //No update for Jane
            Assert.That(() => this.WebClient.PostDto(iterationUri, postBody), Throws.TypeOf<WebException>().With.Message.Contains("400"));

            iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            postBodyPath = this.GetPath("Tests/EngineeringModel/ArchitectureDiagram/PostDeleteDiagram.json");

            postBody = this.GetJsonFromFile(postBodyPath);

            //No remove for Jane
            Assert.That(() => this.WebClient.PostDto(iterationUri, postBody), Throws.TypeOf<WebException>().With.Message.Contains("400"));

            //-----------------------------------------------------------------------------------------------------------------------------
            // Add Jane to the owner DomainOfExpertise of the ArchitectureDiagram
            //-----------------------------------------------------------------------------------------------------------------------------
            var siteDirectoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");
            postBodyPath = this.GetPath("Tests/EngineeringModel/ArchitectureDiagram/PostAddDomainForJane.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);
            Assert.That(jArray.Count, Is.EqualTo(2));

            //-----------------------------------------------------------------------------------------------------------------------------
            // Check all access kinds (Read/Write/Delete) for Jane now that she is added to the ArchitectureDiagram's owner DomainOfExpertise 
            //-----------------------------------------------------------------------------------------------------------------------------
            // Now Jane IS allowed to read as she is in the correct domain now
            architectureDiagramUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/diagramCanvas/05a0668b-64a0-438f-bf81-c917b9345592");

            // read access
            jArray = this.WebClient.GetDto(architectureDiagramUri);

            Assert.That(jArray.Count, Is.EqualTo(1));

            architectureDiagram = jArray.Single(x => (string)x[PropertyNames.Iid] == "05a0668b-64a0-438f-bf81-c917b9345592");
            VerifyDefaultProperties(architectureDiagram);

            iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            postBodyPath = this.GetPath("Tests/EngineeringModel/ArchitectureDiagram/PostUpdateDiagramIsHidden.json");

            postBody = this.GetJsonFromFile(postBodyPath);

            //Update allowed for Jane
            Assert.That(() => this.WebClient.PostDto(iterationUri, postBody), Throws.Nothing);

            iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            postBodyPath = this.GetPath("Tests/EngineeringModel/ArchitectureDiagram/PostDeleteDiagram.json");

            postBody = this.GetJsonFromFile(postBodyPath);

            //Delete allowed for Jane
            Assert.That(() => this.WebClient.PostDto(iterationUri, postBody), Throws.Nothing);
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
            var postBodyPath = this.GetPath("Tests/EngineeringModel/ArchitectureDiagram/PostDeleteDiagram.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            Assert.That(() => this.WebClient.PostDto(iterationUri, postBody), Throws.Nothing);

            var architectureDiagramUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/diagramCanvas/05a0668b-64a0-438f-bf81-c917b9345592");

            Assert.That(() => this.WebClient.GetDto(architectureDiagramUri), Throws.TypeOf<WebException>().With.Message.Contains("404"));
        }

        [Test]
        [CdpVersion_1_4_0]
        [Category("POST")]
        public void VerifyThatRemoveByNonDomainUserFails()
        {
            //-----------------------------------------------------------------------------------------------------------------------------
            // Add 2nd user Jane that is in other DomainOfExpertise and run commands in her context
            //-----------------------------------------------------------------------------------------------------------------------------
            SiteDirectoryTestFixture.AddDomainExpertUserJane(this, out var userName, out var passWord);
            this.CreateNewWebClientForUser(userName, passWord);

            //-----------------------------------------------------------------------------------------------------------------------------
            // Check if remove is allowed for user Jane 
            //-----------------------------------------------------------------------------------------------------------------------------
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/ArchitectureDiagram/PostDeleteDiagram.json");

            var postBody = this.GetJsonFromFile(postBodyPath);

            Assert.That(() => this.WebClient.PostDto(iterationUri, postBody), Throws.TypeOf<WebException>().With.Message.Contains("401"));
        }

        [Test]
        [CdpVersion_1_4_0]
        [Category("POST")]
        public void VerifyThatUpdateByNonDomainUserFails()
        {
            //-----------------------------------------------------------------------------------------------------------------------------
            // Add 2nd user Jane that is in other DomainOfExpertise and run commands in her context
            //-----------------------------------------------------------------------------------------------------------------------------
            SiteDirectoryTestFixture.AddDomainExpertUserJane(this, out var userName, out var passWord);
            this.CreateNewWebClientForUser(userName, passWord);

            //-----------------------------------------------------------------------------------------------------------------------------
            // Check if update fails for user Jane 
            //-----------------------------------------------------------------------------------------------------------------------------
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/ArchitectureDiagram/PostUpdateDiagramIsHidden.json");

            var postBody = this.GetJsonFromFile(postBodyPath);

            Assert.That(() => this.WebClient.PostDto(iterationUri, postBody), Throws.TypeOf<WebException>().With.Message.Contains("401"));
        }

        [Test]
        [CdpVersion_1_4_0]
        [Category("POST")]
        public void VerifyThatSetLockedByWorks()
        {
            //-----------------------------------------------------------------------------------------------------------------------------
            // Set only LockedBy on ArchitectureDiagram 
            //-----------------------------------------------------------------------------------------------------------------------------
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/ArchitectureDiagram/PostUpdateDiagramLockedBy.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var architectureDiagram = jArray.Single(x => (string)x[PropertyNames.Iid] == "05a0668b-64a0-438f-bf81-c917b9345592");
            VerifyDefaultProperties(architectureDiagram);

            // Custom/Non default properties
            Assert.That((bool)architectureDiagram[PropertyNames.IsHidden], Is.EqualTo(false));
            Assert.That((string)architectureDiagram[PropertyNames.LockedBy], Is.EqualTo("77791b12-4c2c-4499-93fa-869df3692d22"));
            Assert.That((int)architectureDiagram[PropertyNames.RevisionNumber], Is.EqualTo(2));

            //-----------------------------------------------------------------------------------------------------------------------------
            // Add 2nd user Jane that is in other DomainOfExpertise and run commands in her context
            //-----------------------------------------------------------------------------------------------------------------------------
            SiteDirectoryTestFixture.AddDomainExpertUserJane(this, out var userName, out var passWord);
            this.CreateNewWebClientForUser(userName, passWord);

            //-----------------------------------------------------------------------------------------------------------------------------
            // Check if Jane is allowed to read a locked ArchitectureDiagram (answer should be yes, because not hidden)
            //-----------------------------------------------------------------------------------------------------------------------------
            var architectureDiagramUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/diagramCanvas/05a0668b-64a0-438f-bf81-c917b9345592");

            jArray = this.WebClient.GetDto(architectureDiagramUri);

            Assert.That(jArray.Count, Is.EqualTo(1));

            architectureDiagram = jArray.Single(x => (string)x[PropertyNames.Iid] == "05a0668b-64a0-438f-bf81-c917b9345592");
            VerifyDefaultProperties(architectureDiagram);

            Assert.That((bool)architectureDiagram[PropertyNames.IsHidden], Is.EqualTo(false));
            Assert.That((string)architectureDiagram[PropertyNames.LockedBy], Is.EqualTo("77791b12-4c2c-4499-93fa-869df3692d22"));
            Assert.That((int)architectureDiagram[PropertyNames.RevisionNumber], Is.EqualTo(2));

            //-----------------------------------------------------------------------------------------------------------------------------
            // Check if Jane is allowed to update a locked ArchitectureDiagram (answer should be no, because not the right domain and user)
            //-----------------------------------------------------------------------------------------------------------------------------
            iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            postBodyPath = this.GetPath("Tests/EngineeringModel/ArchitectureDiagram/PostUpdateDiagramIsHidden.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            Assert.That(() => this.WebClient.PostDto(iterationUri, postBody), Throws.TypeOf<WebException>().With.Message.Contains("401"));

            //-----------------------------------------------------------------------------------------------------------------------------
            // Check if Jane is allowed to remove a locked ArchitectureDiagram (answer should be no, because not the right domain and user)
            //-----------------------------------------------------------------------------------------------------------------------------
            iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            postBodyPath = this.GetPath("Tests/EngineeringModel/ArchitectureDiagram/PostDeleteDiagram.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            Assert.That(() => this.WebClient.PostDto(iterationUri, postBody), Throws.TypeOf<WebException>().With.Message.Contains("401"));

            //-----------------------------------------------------------------------------------------------------------------------------
            // Add Jane to the owner DomainOfExpertise of the ArchitectureDiagram
            //-----------------------------------------------------------------------------------------------------------------------------
            var siteDirectoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");
            postBodyPath = this.GetPath("Tests/EngineeringModel/ArchitectureDiagram/PostAddDomainForJane.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);
            Assert.That(jArray.Count, Is.EqualTo(2));

            //-----------------------------------------------------------------------------------------------------------------------------
            // Check if Jane is allowed to update a locked ArchitectureDiagram (answer should be no, because not the right user)
            //-----------------------------------------------------------------------------------------------------------------------------
            iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            postBodyPath = this.GetPath("Tests/EngineeringModel/ArchitectureDiagram/PostUpdateDiagramIsHidden.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            Assert.That(() => this.WebClient.PostDto(iterationUri, postBody), Throws.TypeOf<WebException>().With.Message.Contains("401"));

            //-----------------------------------------------------------------------------------------------------------------------------
            // Check if Jane is allowed to delete a locked ArchitectureDiagram (answer should be no, because not the right user)
            //-----------------------------------------------------------------------------------------------------------------------------
            iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            postBodyPath = this.GetPath("Tests/EngineeringModel/ArchitectureDiagram/PostDeleteDiagram.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            Assert.That(() => this.WebClient.PostDto(iterationUri, postBody), Throws.TypeOf<WebException>().With.Message.Contains("401"));
        }

        [Test]
        [CdpVersion_1_4_0]
        [Category("POST")]
        public void VerifyThatSetIsHiddenAndIsLockedWorks()
        {
            //-----------------------------------------------------------------------------------------------------------------------------
            // Set LockedBy AND IsHidden on ArchitectureDiagram (Only lockedby user is allowed to update/remove and only correct domain user is allowed to read)
            //-----------------------------------------------------------------------------------------------------------------------------
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/ArchitectureDiagram/PostUpdateDiagramIsHiddenAndLockedBy.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var architectureDiagram = jArray.Single(x => (string)x[PropertyNames.Iid] == "05a0668b-64a0-438f-bf81-c917b9345592");
            VerifyDefaultProperties(architectureDiagram);

            Assert.That((bool)architectureDiagram[PropertyNames.IsHidden], Is.EqualTo(true));
            Assert.That((string)architectureDiagram[PropertyNames.LockedBy], Is.EqualTo("77791b12-4c2c-4499-93fa-869df3692d22"));
            Assert.That((int)architectureDiagram[PropertyNames.RevisionNumber], Is.EqualTo(2));

            //-----------------------------------------------------------------------------------------------------------------------------
            // Add 2nd user Jane that is in other DomainOfExpertise and run commands in her context
            //-----------------------------------------------------------------------------------------------------------------------------
            SiteDirectoryTestFixture.AddDomainExpertUserJane(this, out var userName, out var passWord);
            this.CreateNewWebClientForUser(userName, passWord);

            //-----------------------------------------------------------------------------------------------------------------------------
            // Check if Jane is allowed to read a locked ArchitectureDiagram (answer should be no, because hidden)
            //-----------------------------------------------------------------------------------------------------------------------------
            var architectureDiagramUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/diagramCanvas/05a0668b-64a0-438f-bf81-c917b9345592");

            Assert.That(() => this.WebClient.GetDto(architectureDiagramUri), Throws.TypeOf<WebException>().With.Message.Contains("400"));

            //-----------------------------------------------------------------------------------------------------------------------------
            // Check if Jane is allowed to update a locked ArchitectureDiagram (answer should be no, because hidden and locked by other person)
            //-----------------------------------------------------------------------------------------------------------------------------
            iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            postBodyPath = this.GetPath("Tests/EngineeringModel/ArchitectureDiagram/PostUpdateDiagramIsHidden.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            Assert.That(() => this.WebClient.PostDto(iterationUri, postBody), Throws.TypeOf<WebException>().With.Message.Contains("400"));

            //-----------------------------------------------------------------------------------------------------------------------------
            // Check if Jane is allowed to remove a locked ArchitectureDiagram (answer should be no, because hidden and locked by other person)
            //-----------------------------------------------------------------------------------------------------------------------------
            iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            postBodyPath = this.GetPath("Tests/EngineeringModel/ArchitectureDiagram/PostDeleteDiagram.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            Assert.That(() => this.WebClient.PostDto(iterationUri, postBody), Throws.TypeOf<WebException>().With.Message.Contains("400"));

            //-----------------------------------------------------------------------------------------------------------------------------
            // Add Jane to the owner DomainOfExpertise of the ArchitectureDiagram
            //-----------------------------------------------------------------------------------------------------------------------------
            var siteDirectoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");
            postBodyPath = this.GetPath("Tests/EngineeringModel/ArchitectureDiagram/PostAddDomainForJane.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);
            Assert.That(jArray.Count, Is.EqualTo(2));

            //-----------------------------------------------------------------------------------------------------------------------------
            // Check if Jane is allowed to update a locked ArchitectureDiagram (answer should be no, because lockedby other person)
            //-----------------------------------------------------------------------------------------------------------------------------
            iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            postBodyPath = this.GetPath("Tests/EngineeringModel/ArchitectureDiagram/PostUpdateDiagramIsHidden.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            Assert.That(() => this.WebClient.PostDto(iterationUri, postBody), Throws.TypeOf<WebException>().With.Message.Contains("401"));

            //-----------------------------------------------------------------------------------------------------------------------------
            // Check if Jane is allowed to delete a locked ArchitectureDiagram (answer should be no, because lockedby other person)
            //-----------------------------------------------------------------------------------------------------------------------------
            iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            postBodyPath = this.GetPath("Tests/EngineeringModel/ArchitectureDiagram/PostDeleteDiagram.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            Assert.That(() => this.WebClient.PostDto(iterationUri, postBody), Throws.TypeOf<WebException>().With.Message.Contains("401"));
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
        /// <param name="architectureDiagram">
        /// The <see cref="JToken"/> that contains the properties of
        /// the DiagramCanvas object
        /// </param>
        public static void VerifyDefaultProperties(JToken architectureDiagram)
        {
            // verify the amount of returned properties 
            Assert.That(architectureDiagram.Children().Count(), Is.EqualTo(16));

            // assert that the properties are what is expected
            Assert.That((string)architectureDiagram[PropertyNames.Iid], Is.EqualTo("05a0668b-64a0-438f-bf81-c917b9345592"));
            Assert.That((string)architectureDiagram[PropertyNames.ClassKind], Is.EqualTo("ArchitectureDiagram"));

            Assert.That((string)architectureDiagram[PropertyNames.Name], Is.EqualTo("Architecture 1"));
            Assert.That((string)architectureDiagram[PropertyNames.Description], Is.EqualTo("ArchitectureDiagram 1"));

            var expectedDiagramElement = new[]
            {
                "f4761bbf-6eb9-491e-9d21-a4ec1c24e33b"
            };

            var filesArray = (JArray)architectureDiagram[PropertyNames.DiagramElement];
            IList<string> files = filesArray.Select(x => (string)x).ToList();
            Assert.That(files, Is.EquivalentTo(expectedDiagramElement));
        }
    }
}
