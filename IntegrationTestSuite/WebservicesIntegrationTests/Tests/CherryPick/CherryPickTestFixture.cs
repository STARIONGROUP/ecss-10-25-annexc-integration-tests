// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CherryPickTestFixture.cs" company="RHEA System S.A.">
// 
//    Copyright (c) 2023 RHEA System S.A.
// 
//    Authors: Sam Gerené, Antoine Théate, Jaime Bernar, Omar Elebiary
// 
//    This file is part of AP1067 application
//    The  AP1067 application is a Web Application implementation of ECSS-E-TM-10-25 for AP1067.
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
