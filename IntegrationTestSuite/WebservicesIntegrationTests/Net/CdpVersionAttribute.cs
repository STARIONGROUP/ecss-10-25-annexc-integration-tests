// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CdpTestAttribute.cs" company="RHEA System">
//
//   Copyright 2016-2020 RHEA System 
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

namespace WebservicesIntegrationTests.Net
{
    using NUnit.Framework;

    /// <summary>
    /// Class that is the base class for Cdp version attributes
    /// </summary>
    public abstract class CdpVersionAttribute : CategoryAttribute
    {
        /// <summary>
        /// Gets the CDP version number as a string
        /// </summary>
        public abstract string Version { get; }
    }
}
