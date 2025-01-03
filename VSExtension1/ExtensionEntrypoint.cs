// ExtensionEntrypoint.cs
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

using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Extensibility;
using System.Reflection;


namespace VSExtension1
{
    /// <summary>
    /// Extension entrypoint for the VisualStudio.Extensibility extension.
    /// </summary>
    [VisualStudioContribution]
    internal class ExtensionEntrypoint : Extension
    {
        /// <summary>
        /// Creates and configures a new instance of the V8ScriptEngine.
        /// </summary>
        /// <param name="serviceProvider">The service provider to resolve dependencies.</param>
        /// <returns>A configured instance of the V8ScriptEngine.</returns>
        /// <exception cref="ScriptEngineException">Thrown when there is an error executing the script.</exception>
        /// <exception cref="FileLoadException">Thrown when there is an error loading a file.</exception>
        static public ScriptEngine CreateScriptEngine(IServiceProvider serviceProvider)
        {
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? ".";

            var scriptEngine = new V8ScriptEngine();
            scriptEngine.DocumentSettings.AccessFlags = DocumentAccessFlags.EnableFileLoading;
            scriptEngine.DocumentSettings.SearchPath = Path.Combine(basePath, "scripts");

            var globalObject = new GlobalObject(serviceProvider);
            try
            {
                scriptEngine.AddHostObject("extension", globalObject);
                scriptEngine.ExecuteDocument("__init__.js", ModuleCategory.Standard);
            }
            catch (ScriptEngineException e)
            {
                globalObject.Output.WriteLine("ScriptEngineException: " + e.Message);
                return scriptEngine;
            }
            catch (FileLoadException e)
            {
                // TODO
                return scriptEngine;
            }

            return scriptEngine;
        }

        /// <inheritdoc/>
        public override ExtensionConfiguration ExtensionConfiguration => new()
        {
            Metadata = new(
                    id: "VSExtension1.682fdf2e-183c-439e-8304-457aec4697b9",
                    version: this.ExtensionAssemblyVersion,
                    publisherName: "Kaz Nishimura",
                    displayName: "VSExtension1",
                    description: "Extension description"),
        };

        /// <inheritdoc />
        protected override void InitializeServices(IServiceCollection serviceCollection)
        {
            base.InitializeServices(serviceCollection);

            // You can configure dependency injection here by adding services to the serviceCollection.

            serviceCollection.AddScoped<ScriptEngine>(CreateScriptEngine);
        }
    }
}
