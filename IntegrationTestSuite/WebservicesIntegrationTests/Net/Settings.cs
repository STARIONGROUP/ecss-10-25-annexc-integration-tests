// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebClient.cs" company="RHEA S.A.">
//   Copyright (c) 2016 RHEA S.A.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebservicesIntegrationTests.Net
{
    /// <summary>
    /// The purpose of the <see cref="Settings"/> class is to
    /// read the test settings from disk
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Gets or sets the User name that is used to authenticate
        /// against the data-source
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password that is used to authenticate
        /// against the data-source
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the hostname where the data-source is hosted
        /// </summary>
        public string Hostname { get; set; }
    }
}
