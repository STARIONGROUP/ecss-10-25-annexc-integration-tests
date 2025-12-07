// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CherryPickTestFixture.cs" company="Starion Group S.A.">
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

namespace WebservicesIntegrationTests.CherryPick
{
    using NUnit.Framework;

    using System;
    using System.Linq;

    [TestFixture]
    public class CherryPickTestFixture: WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedCherryPickedThingIsReturnedFromWebApi()
        {
            // GZsFz1wjvkiHo5qJQsjj4A is the shortguid for the categoryId cf059b19-235c-48be-87a3-9a8942c8e3e0
            var baseUri = $"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c";
            var cherryPickUri = new Uri($"{baseUri}?classkind=[ELEMENTDEFINITION]&category=[GZsFz1wjvkiHo5qJQsjj4A]&cherryPick=true");
            var jArray = this.WebClient.GetDto(cherryPickUri);

            Assert.Multiple(() =>
            {
                Assert.That(jArray, Has.Count.EqualTo(6));
                Assert.That(() => jArray.Single(x => (string)x[PropertyNames.Iid] == "f73860b2-12f0-43e4-b8b2-c81862c0a159"), Throws.Nothing);
            });

            var jsonPath = this.GetPath("Tests/CherryPick/PostNewThingsSRDL.json");
            var postUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");
            this.WebClient.PostDto(postUri, this.GetJsonFromFile(jsonPath));

            jsonPath = this.GetPath("Tests/CherryPick/PostNewThingsCherryPick.json");
            postUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            this.WebClient.PostDto(postUri, this.GetJsonFromFile(jsonPath));

            //3QV4ayC1TkObkCdSSmdvow is the shortguid for the categoryId 6b7805dd-b520-434e-9b90-27524a676fa3
            cherryPickUri = new Uri($"{baseUri}?classkind=[PARAMETER]&category=[3QV4ayC1TkObkCdSSmdvow]&cherryPick=true");
            jArray = this.WebClient.GetDto(cherryPickUri);

            Assert.Multiple(() =>
            {
                Assert.That(jArray, Has.Count.EqualTo(4));
                Assert.That(() => jArray.Single(x => (string)x[PropertyNames.Iid] == "ebc58281-173b-49ce-99a5-99daa45e09b0"), Throws.Nothing);
            });

            cherryPickUri = new Uri($"{baseUri}?classkind=[PARAMETER;ELEMENTDEFINITION]&category=[3QV4ayC1TkObkCdSSmdvow;GZsFz1wjvkiHo5qJQsjj4A]&cherryPick=true");
            jArray = this.WebClient.GetDto(cherryPickUri);

            Assert.Multiple(() =>
            {
                Assert.That(jArray, Has.Count.EqualTo(9));
                Assert.That(() => jArray.Single(x => (string)x[PropertyNames.Iid] == "ebc58281-173b-49ce-99a5-99daa45e09b0"), Throws.Nothing);
                Assert.That(() => jArray.Single(x => (string)x[PropertyNames.Iid] == "f73860b2-12f0-43e4-b8b2-c81862c0a159"), Throws.Nothing);
            });
        }
    }
}
