// ScriptingURL.cs
// Copyright (C) 2025 Kaz Nishimura
//
// This program is free software: you can redistribute it and/or modify it
// under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or (at your
// option) any later version.
//
// This program is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
// FITNESS FOR A PARTICULAR PURPOSE.  See the GNU Affero General Public License
// for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Microsoft;


namespace VSExtension1.Scripting
{
    /// <summary>
    /// Implements a class to be used as the `URL` object in scripting languages.
    /// This class does not represent a URL itself but provides the implementation for the `URL` class.
    /// </summary>
    /// <remarks>
    /// This class provides multiple constructors to create an instance from different sources:
    /// - A string representing the URL.
    /// - Another ScriptingURL instance.
    /// - A string representing the URL and a base URL.
    /// - A string representing the URL and another ScriptingURL instance as the base URL.
    /// - Another ScriptingURL instance and a string representing the base URL.
    /// - Two ScriptingURL instances, one as the URL and one as the base URL.
    ///
    /// Note: This class is incomplete and shall be updated when an error is detected.
    /// </remarks>
#pragma warning disable S101 // Types should be named in PascalCase
    public class ScriptingURL
#pragma warning restore S101 // Types should be named in PascalCase
    {
        /// <summary>
        /// Gets the internal Uri object.
        /// </summary>
        protected Uri Uri { get; }

        // Other properties are to be implemented later.

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptingURL"/> class using an internal <see cref="Uri"/> object.
        /// </summary>
        /// <param name="uri">The internal <see cref="Uri"/> object to be used for initialization.</param>
        protected ScriptingURL(Uri uri)
        {
            this.Uri = Requires.NotNull(uri);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptingURL"/> class using a string URL.
        /// </summary>
        /// <param name="url">The string representation of the URL.</param>
        public ScriptingURL(String url) :
            this(new Uri(Requires.NotNull(url)))
        {
            // Empty.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptingURL"/> class using another <see cref="ScriptingURL"/> instance.
        /// </summary>
        /// <param name="url">The <see cref="ScriptingURL"/> instance to copy.</param>
        public ScriptingURL(ScriptingURL url) :
            this(Requires.NotNull(url).Uri)
        {
            // Empty.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptingURL"/> class using a string URL and a base URL.
        /// </summary>
        /// <param name="url">The string representation of the URL.</param>
        /// <param name="baseUrl">The string representation of the base URL.</param>
        public ScriptingURL(string url, string baseUrl) :
            this(new Uri(new Uri(Requires.NotNull(baseUrl)), Requires.NotNull(url)))
        {
            // Empty.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptingURL"/> class using a string URL and a base <see cref="ScriptingURL"/> instance.
        /// </summary>
        /// <param name="url">The string representation of the URL.</param>
        /// <param name="baseUrl">The base <see cref="ScriptingURL"/> instance.</param>
        public ScriptingURL(string url, ScriptingURL baseUrl) :
            this(new Uri(Requires.NotNull(baseUrl).Uri, Requires.NotNull(url)))
        {
            // Empty.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptingURL"/> class using another <see cref="ScriptingURL"/> instance and a base URL.
        /// </summary>
        /// <param name="url">The <see cref="ScriptingURL"/> instance to use as the URL.</param>
        /// <param name="baseUrl">The string representation of the base URL.</param>
        public ScriptingURL(ScriptingURL url, string baseUrl) :
            this(new Uri(new Uri(Requires.NotNull(baseUrl)), Requires.NotNull(url).Uri))
        {
            // Empty.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptingURL"/> class using another <see cref="ScriptingURL"/> instance and a base <see cref="ScriptingURL"/> instance.
        /// </summary>
        /// <param name="url">The <see cref="ScriptingURL"/> instance to use as the URL.</param>
        /// <param name="baseUrl">The base <see cref="ScriptingURL"/> instance.</param>
        public ScriptingURL(ScriptingURL url, ScriptingURL baseUrl) :
            this(new Uri(Requires.NotNull(baseUrl).Uri, Requires.NotNull(url).Uri))
        {
            // Empty.
        }
    }
}
