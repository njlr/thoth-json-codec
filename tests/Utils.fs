namespace Thoth.Json.Codec.Tests

open Thoth.Json.Codec

[<AutoOpen>]
module Utils =

#if FABLE_COMPILER
  open Fable.Mocha
  open Thoth.Json.JavaScript
#else
  open Expecto
  open Thoth.Json.System.Text.Json
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
