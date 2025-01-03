// GlobalObject.cs
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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Documents;
using Microsoft.VisualStudio.Threading;


namespace VSExtension1
{
    /// <summary>
    /// Extension object for scripts.
    /// </summary>
    public class ExtensionObject
    {
        /// <summary>
        /// The joinable task factory object.
        /// </summary>
        protected JoinableTaskFactory JoinableTaskFactory { get; }

        /// <summary>
        /// The extensibility object.
        /// </summary>
        protected VisualStudioExtensibility Extensibility { get; }

        
        /// <summary>
        /// The output channel joinable task.
        /// </summary>
        private readonly JoinableTask<OutputChannel> _outputChannel;

        /// <summary>
        /// The `Output` property for scripts.
        /// </summary>
        public TextWriter Output
        {
            get => this.JoinableTaskFactory.Run(async () => await this._outputChannel).Writer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionObject"/> class.
        /// </summary>
        /// <param name="serviceProvider">service provider</param>
        public ExtensionObject(IServiceProvider serviceProvider)
        {
            this.JoinableTaskFactory = serviceProvider.GetRequiredService<JoinableTaskFactory>();
            this.Extensibility = serviceProvider.GetRequiredService<VisualStudioExtensibility>();
            this._outputChannel = this.JoinableTaskFactory.RunAsync(
                () => this.Extensibility.Views().Output
                    .CreateOutputChannelAsync(Strings.ScriptOutputChannelName, CancellationToken.None));
        }
    }
}
