// "__init__.js"

// This file will be used to initialize the ScriptEngine.

import { pipeline } from './transformers.js'

extension.Output.WriteLine("Hello from __init__.js")

globalThis.command1 = function () {
    extension.Output.WriteLine("command1 called")
}
