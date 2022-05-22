module Thoth.Json.Codec.Tests.Utils

#if FABLE_COMPILER
open Fable.Mocha
open Thoth.Json
#else
open Thoth.Json.Net
open Expecto
#endif

open Thoth.Json.Codec

let roundTrip (codec : Codec<'t>) v =
  let encoded =
    v
    |> Encode.codec codec
    |> Encode.toString 2

  let decoded =
    encoded
    |> Decode.fromString (Decode.codec codec)

  Expect.wantOk decoded "Decoding must succeed"
