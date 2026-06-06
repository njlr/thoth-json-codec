module Thoth.Json.Codec.Tests.System.Text.Json.Entry

open Expecto
open Thoth.Json.Codec.Tests

[<EntryPoint>]
let main argv =
  runTestsWithCLIArgs [||] argv All.tests
