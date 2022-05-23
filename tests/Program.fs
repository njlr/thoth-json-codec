module Thoth.Json.Codec.Tests.Entry

#if FABLE_COMPILER
open Fable.Mocha
#else
open Expecto
#endif

let tests =
  testList "Thoth.Json.Codec" [
    Primitives.tests
    Combinators.tests
    ObjectCodec.tests
    Variant.tests
  ]

[<EntryPoint>]
let main argv =
#if FABLE_COMPILER
  Mocha.runTests tests
#else
  runTestsWithCLIArgs [] argv tests
#endif
