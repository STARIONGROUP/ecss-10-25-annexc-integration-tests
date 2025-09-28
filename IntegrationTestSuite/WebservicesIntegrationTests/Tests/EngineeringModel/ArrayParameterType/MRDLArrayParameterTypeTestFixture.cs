// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MRDLArrayParameterTypeTestFixture.cs" company="Starion Group S.A.">
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
    using System.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class MRDLArrayParameterTypeTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("POST")]
        public void VerifyThatArrayParameterTypeCanBePosted()
        {
            var siteDirectoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");

            var postBodyPath = this.GetPath("Tests/EngineeringModel/ArrayParameterType/PostArrayParameterType.json");
            var postBody = this.GetJsonFromFile(postBodyPath);

            var jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            //Check the amount of objects 
            Assert.That(jArray.Count, Is.EqualTo(5));

            var siteDirectory = jArray.Single(x => (string) x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            Assert.That((int)siteDirectory[PropertyNames.RevisionNumber], Is.EqualTo(2));

            var arrayParameterType = jArray.Single(x => (string) x[PropertyNames.Iid] == "267b0d27-0421-453d-951a-4fcacc309a27");
            Assert.That((int)arrayParameterType[PropertyNames.RevisionNumber], Is.EqualTo(2));
            Assert.That((string)arrayParameterType[PropertyNames.ShortName], Is.EqualTo("cta"));
            Assert.That((string)arrayParameterType[PropertyNames.Name], Is.EqualTo("createTestArray"));
            Assert.That((string)arrayParameterType[PropertyNames.Symbol], Is.EqualTo("cta_s"));

            var component_1 = jArray.Single(x => (string) x[PropertyNames.Iid] == "f51de2a2-279e-4b5e-8f07-bbc7e9993a6b");
            Assert.That((int)component_1[PropertyNames.RevisionNumber], Is.EqualTo(2));
            Assert.That((string)component_1[PropertyNames.ParameterType], Is.EqualTo("4f9f3d9b-f3de-4ef5-b6cb-2e22199fab0d"));
            Assert.That((string)component_1[PropertyNames.Scale], Is.EqualTo("53e82aeb-c42c-475c-b6bf-a102af883471"));
            Assert.That((string)component_1[PropertyNames.ShortName], Is.EqualTo("{1;1}"));

            var component_2 = jArray.Single(x => (string) x[PropertyNames.Iid] == "0715f517-1f8b-462d-9189-b4ff20548266");
            Assert.That((int)component_2[PropertyNames.RevisionNumber], Is.EqualTo(2));
            Assert.That((string)component_2[PropertyNames.ParameterType], Is.EqualTo("35a9cf05-4eba-4cda-b60c-7cfeaac8f892"));
            Assert.That((string) component_2[PropertyNames.Scale], Is.Null);
            Assert.That((string)component_2[PropertyNames.ShortName], Is.EqualTo("{2;1}"));

            // Test created because of bug gh373 in github
            this.VerifyThatNewEngineeringModelCanBeCreatedBasedOnExistingModelWithWebApi();
        }

        public void VerifyThatNewEngineeringModelCanBeCreatedBasedOnExistingModelWithWebApi()
        {
            //GET old Iteration for checks later in this testfixture
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c?extent=deep");
            var jArray = this.WebClient.GetDto(iterationUri);
            Assert.That(jArray.Count, Is.EqualTo(50));

            var oldCommonFileStore = jArray.Single(x => (string)x[PropertyNames.Name] == "TestFileStore");
            Assert.That(oldCommonFileStore[PropertyNames.CreatedOn], Is.Not.Null); // Important for validity of later checks!

            var oldDomainFileStore = jArray.Single(x => (string)x[PropertyNames.Name] == "Test DomainFileStore");
            Assert.That(oldDomainFileStore[PropertyNames.CreatedOn], Is.Not.Null); // Important for validity of later checks!

            var siteDirectoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");
            var postBodyPath = this.GetPath("Tests/SiteDirectory/EngineeringModelSetup/PostEngineeringModelSetupBasedOnExistingModel.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            // Without new ArrayParameterType and its two components is would be a total of 9
            Assert.That(jArray.Count, Is.EqualTo(12));
        }
    }
}
