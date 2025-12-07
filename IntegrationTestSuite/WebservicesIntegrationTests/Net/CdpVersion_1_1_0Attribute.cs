// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Cdp_1_1_0Attribute.cs" company="Starion Group S.A.">
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

namespace WebservicesIntegrationTests.Net
{
    /// <summary>
    /// The CDP version 1.1.0 attribute
    /// </summary>
    public class CdpVersion_1_1_0Attribute : CdpVersionAttribute
    {
        /// <summary>
        /// Gets the CDP version number as a string
        /// </summary>
        public override string Version { get; } = "1.1.0";
    }
}
