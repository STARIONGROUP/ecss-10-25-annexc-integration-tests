// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderedItem.cs" company="RHEA System">
//
//   Copyright 2016 RHEA System 
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
    /// Represents an ordered item in an array
    /// </summary>
    public class OrderedItem
    {
        /// <summary>
        /// Gets the key
        /// </summary>
        public int K { get; private set; }

        /// <summary>
        /// Gets the value
        /// </summary>
        public string V { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderedItem"/> class
        /// </summary>
        /// <param name="k"></param>
        /// <param name="v"></param>
        public OrderedItem(int k, string v)
        {
            this.K = k;
            this.V = v;
        }

        public override bool Equals(object obj)
        {
            var other = obj as OrderedItem;
            if (other == null) return false;
            return this.K == other.K && this.V == other.V;
        }

        public override int GetHashCode()
        {
            var hash = 27;
            hash = (13*hash) + this.K.GetHashCode();
            hash = (13*hash) + this.V.GetHashCode();
            return hash;
        }
    }
}