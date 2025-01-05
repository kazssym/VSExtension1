// Command1.cs
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
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using Microsoft.VisualStudio.Extensibility.Shell;
using System.Diagnostics;


namespace VSExtension1
{
    /// <summary>
    /// Handles the execution of Command1.
    /// </summary>
    [VisualStudioContribution]
    internal class Command1 : Command
    {
        private readonly TraceSource logger;

        /// <summary>
        /// Gets the script host used to manage the script engine.
        /// </summary>
        protected ScriptHost ScriptHost { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Command1"/> class.
        /// </summary>
        /// <param name="extensibility">The Visual Studio extensibility object.</param>
        /// <param name="traceSource">The trace source instance to utilize for logging.</param>
        /// <param name="scriptHost">The script host used to manage the script engine.</param>
        public Command1(VisualStudioExtensibility extensibility, TraceSource traceSource,
            ScriptHost scriptHost) :
            base(extensibility)
        {
            // This optional TraceSource can be used for logging in the command. You can use dependency injection to access
            // other services here as well.
            this.logger = Requires.NotNull(traceSource, nameof(traceSource));
            this.ScriptHost = Requires.NotNull(scriptHost);
        }

        /// <inheritdoc />
        public override CommandConfiguration CommandConfiguration => new("%VSExtension1.Command1.DisplayName%")
        {
            // Use this object initializer to set optional parameters for the command. The required parameter,
            // displayName, is set above. DisplayName is localized and references an entry in .vsextension\string-resources.json.
            Icon = new(ImageMoniker.KnownValues.Extension, IconSettings.IconAndText),
            Placements = [CommandPlacement.KnownPlacements.ExtensionsMenu],
        };

        /// <inheritdoc />
        public override Task InitializeAsync(CancellationToken cancellationToken)
        {
            // Use InitializeAsync for any one-time setup or initialization.
            return base.InitializeAsync(cancellationToken);
        }

        /// <inheritdoc />
        public override async Task ExecuteCommandAsync(IClientContext context, CancellationToken cancellationToken)
        {
            await this.Extensibility.Shell().ShowPromptAsync("Hello from an extension!", PromptOptions.OK, cancellationToken);
            try
            {
                this.ScriptHost.ScriptEngine.Evaluate("command1()");
            }
            catch (ScriptEngineException e)
            {
                await this.Extensibility.Shell().ShowPromptAsync(e.Message, PromptOptions.OK, cancellationToken);
                throw;
            }
        }
    }
}
