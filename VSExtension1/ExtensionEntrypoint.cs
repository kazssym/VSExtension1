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
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var scriptEngine = new V8ScriptEngine();
            scriptEngine.DocumentSettings.AccessFlags = DocumentAccessFlags.EnableFileLoading;
            scriptEngine.DocumentSettings.SearchPath = Path.Combine(basePath, "scripts");

            var extensibility = serviceProvider.GetRequiredService<VisualStudioExtensibility>();
            try
            {
                scriptEngine.AddHostObject("extension", new GlobalObject(extensibility));
                scriptEngine.ExecuteDocument("__init__.js", ModuleCategory.Standard);
            }
            catch (ScriptEngineException e)
            {
                // TODO
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
                    publisherName: "Publisher name",
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
