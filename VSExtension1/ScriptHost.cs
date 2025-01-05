// ScriptHost.cs
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
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using Microsoft.VisualStudio.Threading;
using System.Reflection;


namespace VSExtension1
{
    /// <summary>
    /// Represents the script host for the extension, providing functionality to initialize and manage the script engine.
    /// </summary>
    internal class ScriptHost
    {
        /// <summary>
        /// The name of the initialization file containing the initial JavaScript code to be executed by the ScriptEngine.
        /// </summary>
        protected const string InitializeFileName = "__init__.js";

        /// <summary>
        /// The joinable task factory used to manage asynchronous tasks, allowing them to be joined back to the main thread if necessary.
        /// </summary>
        protected JoinableTaskFactory JoinableTaskFactory { get; }

        private readonly JoinableTask<ScriptEngine> _scriptEngineJoinableTask;

        /// <summary>
        /// Provides synchronous access to the script engine, ensuring it is fully initialized before use.
        /// </summary>
        public ScriptEngine ScriptEngine => this.JoinableTaskFactory.Run(async () => await this._scriptEngineJoinableTask);

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptHost"/> class.
        /// </summary>
        /// <param name="joinableTaskFactory">The joinable task factory to manage asynchronous tasks.</param>
        /// <param name="extensionObject">The extension object to be used by the script engine.</param>
        public ScriptHost(JoinableTaskFactory joinableTaskFactory, ExtensionObject extensionObject)
        {
            this.JoinableTaskFactory = Requires.NotNull(joinableTaskFactory);
            this._scriptEngineJoinableTask = this.JoinableTaskFactory.RunAsync(
                () => CreateScriptEngineAsync(Requires.NotNull(extensionObject)));
        }

        /// <summary>
        /// Creates a meta object for a document, extracting and returning meta information from the provided <see cref="DocumentInfo"/> object.
        /// </summary>
        /// <param name="info">The document information.</param>
        /// <returns>A dictionary containing meta information about the document.</returns>
        protected static Dictionary<string, object> CreateMetaObject(DocumentInfo info)
        {
            return new Dictionary<string, object>()
                    {
                        {"url", info.Uri.ToString()},
                    };
        }

        /// <summary>
        /// Creates and configures a new instance of the <see cref="V8ScriptEngine"/>, setting it up with the necessary document settings and host objects.
        /// Attempts to execute an initialization script and handles any exceptions that occur during this process.
        /// </summary>
        /// <param name="extensionObject">The extension object to be used by the script engine.</param>
        /// <returns>A task representing the asynchronous operation, with a configured instance of the <see cref="V8ScriptEngine"/> as the result.</returns>
        /// <exception cref="ScriptEngineException">Thrown when there is an error executing the script.</exception>
        /// <exception cref="Exception">Thrown when there is a general error during script execution.</exception>
        protected static async Task<ScriptEngine> CreateScriptEngineAsync(ExtensionObject extensionObject)
        {
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? ".";

            var scriptEngine = new V8ScriptEngine();
            scriptEngine.DocumentSettings.AccessFlags = DocumentAccessFlags.EnableFileLoading;
            scriptEngine.DocumentSettings.SearchPath = Path.Combine(basePath, "scripts");
            scriptEngine.DocumentSettings.ContextCallback = CreateMetaObject;

            scriptEngine.AddHostType("URL", typeof(Scripting.ScriptingURL));
            scriptEngine.AddHostObject("extension", extensionObject);
            try
            {
                scriptEngine.ExecuteDocument(InitializeFileName, ModuleCategory.Standard);
            }
            catch (ScriptEngineException e)
            {
                await extensionObject.Output.WriteLineAsync("ScriptEngineException: " + e.Message);
                throw;
            }
            catch (Exception e)
            {
                await extensionObject.Output.WriteLineAsync("Exception: " + e.Message);
                throw;
            }

            return scriptEngine;
        }
    }
}
