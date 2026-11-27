// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CometTaskStatusKind.cs" company="Starion Group S.A.">
//
//   Copyright 2016-2026 Starion Group S.A.
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
    /// <summary>
    /// The values of the statusKind property of a CometTask, as the CDP4-COMET WebServices serialize them
    /// (CDP4DalCommon.Protocol.Tasks.StatusKind)
    /// </summary>
    public static class CometTaskStatusKind
    {
        /// <summary>
        /// The work of the POST is still running
        /// </summary>
        public const int Processing = 0;

        /// <summary>
        /// The work of the POST has completed and its changes are stored
        /// </summary>
        public const int Succeeded = 1;

        /// <summary>
        /// The work of the POST has failed and nothing is stored
        /// </summary>
        public const int Failed = 2;
    }
}
