module Thoth.Json.Codec.Tests.Variant

#if FABLE_COMPILER
open Thoth.Json
open Fable.Mocha
#else
open Thoth.Json.Net
open Expecto
#endif

open Thoth.Json.Codec
open Thoth.Json.Codec.Tests.Utils

type Shape =
  | Square of width : int
  | Rectangle of width : int * height : int
  | Circle of radius : int

module Shape =

  let codec2 : Codec<Shape> =
    Codec.create
      (
        function
        | Square w -> Encode.object [ "square", Encode.int w ]
        | Rectangle (w, h) -> Encode.object [ "rectangle", Encode.object [ "width", Encode.int w; "height", Encode.int h ] ]
        | Circle r -> Encode.object [ "circle", Encode.int r ]
      )
      (
        Decode.oneOf [
          Decode.field "square" Decode.int |> Decode.map Shape.Square
          Decode.field "rectangle" (Decode.object (fun get -> Rectangle (get.Required.Field "width" Decode.int, get.Required.Field "height" Decode.int)))
          Decode.field "circle" Decode.int |> Decode.map Shape.Circle
        ]
      )

  let codec : Codec<Shape> =
    variantCodec {
      let! square = Codec.case "square" Square Codec.int
      and! rectangle = Codec.case "rectangle" Rectangle (Codec.tuple2 Codec.int Codec.int)
      and! circle = Codec.case "circle" Circle Codec.int

      return
        function
        | Square w -> square w
        | Rectangle (w, h) -> rectangle (w, h)
        | Circle w -> circle w
    }

let tests = testList "VariantCodec" [
  test "variantCodec works for simple case 1" {
    let expected = Square 4
    let actual = roundTrip Shape.codec expected

    Expect.equal actual expected "The decoded value must match the original"

    let expected = Rectangle (7, 2)
    let actual = roundTrip Shape.codec expected

    Expect.equal actual expected "The decoded value must match the original"

    let expected = Circle 3
    let actual = roundTrip Shape.codec expected

    Expect.equal actual expected "The decoded value must match the original"
  }
]
