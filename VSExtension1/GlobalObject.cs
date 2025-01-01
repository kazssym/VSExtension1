using Microsoft.VisualStudio.Extensibility;


namespace VSExtension1
{
    internal class GlobalObject
    {
        private readonly VisualStudioExtensibility _extensibility;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalObject"/> class.
        /// </summary>
        /// <param name="extensibility">The Visual Studio extensibility object.</param>
        public GlobalObject(VisualStudioExtensibility extensibility)
        {
            this._extensibility = extensibility;
        }
    }
}
