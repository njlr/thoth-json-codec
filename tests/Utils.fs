module Thoth.Json.Codec.Tests.Utils

#if FABLE_COMPILER
open Fable.Mocha
open Thoth.Json
open Thoth.Json.Codec
#else
open Expecto
open Thoth.Json.Net
open Thoth.Json.Net.Codec
#endif

let roundTrip (codec : Codec<'t>) v =
  let encoded =
    v
    |> Encode.codec codec
    |> Encode.toString 2

  let decoded =
    encoded
    |> Decode.fromString (Decode.codec codec)

  Expect.wantOk decoded "Decoding must succeed"
