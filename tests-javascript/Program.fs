module Thoth.Json.Codec.Tests.System.Text.Json.Entry

open Fable.Mocha
open Thoth.Json.Codec.Tests

[<EntryPoint>]
let main _argv =
  Mocha.runTests All.tests
