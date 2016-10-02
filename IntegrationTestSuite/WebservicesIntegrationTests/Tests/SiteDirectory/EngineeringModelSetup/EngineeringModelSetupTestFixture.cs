// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EngineeringModelSetupTestFixture.cs" company="RHEA S.A.">
//   Copyright (c) 2016 RHEA S.A.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebservicesIntegrationTests.SiteDirectory.EngineeringModelSetup
{
    using System;
    using NUnit.Framework;
    using WebservicesIntegrationTests.Net;

    /// <summary>
    /// The purpose of the <see cref="EngineeringModelSetupTestFixture"/> is to GET and POST person objects
    /// </summary>
    [TestFixture]
    public class EngineeringModelSetupTestFixture : WebClientTestFixtureBase
    {
        [Test]
        public void VerifyThatNewEngineeringModelCanBeCreatedWithWebApi()
        {
            var siteDirectoryUri = new Uri(string.Format(UriFormat, this.Settings.Hostname, "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294"));
            var postBodyPath = this.GetPath("Tests/SiteDirectory/EngineeringModelSetup/PostEngineeringModelSetup.json");

            var postBody = base.GetJsonFromFile(postBodyPath);
            var response = this.WebClient.PostDto(siteDirectoryUri, postBody);

            Console.WriteLine(response);
        }

        [Test]
        public void VerifyThatNewEngineeringModelCanBeCreatedBasedOnExistingModelWithWebApi()
        {
            var siteDirectoryUri = new Uri(string.Format(UriFormat, this.Settings.Hostname, "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294"));
            var postBodyPath = this.GetPath("Tests/SiteDirectory/EngineeringModelSetup/PostEngineeringModelSetupBasedOnExistingModel.json");

            var postBody = base.GetJsonFromFile(postBodyPath);
            var response = this.WebClient.PostDto(siteDirectoryUri, postBody);

            Console.WriteLine(response);
        }
    }
}
