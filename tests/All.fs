module Thoth.Json.Codec.Tests.All

#if FABLE_COMPILER
open Fable.Mocha
#else
open Expecto
#endif

let tests =
  testList
    "Thoth.Json.Codec"
    [
      Primitives.tests
      Combinators.tests
      Auto.tests
      ObjectCodec.tests
      Variant.tests
    ]
