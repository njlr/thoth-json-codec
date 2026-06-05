module Thoth.Json.Codec.Tests.Variant

#if FABLE_COMPILER
open Fable.Mocha
#else
open Expecto
#endif

open Thoth.Json.Codec
open Thoth.Json.Codec.Tests.Utils

type Shape =
  | Square of width : int
  | Rectangle of width : int * height : int
  | Circle of radius : int

module Shape =

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

  let codec' : Codec<Shape> =
    variantCodecWithEncoding (TagAndValue ("type", "value")) {
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

  test "variantCodec works for simple case 2" {
    let expected = Square 4
    let actual = roundTrip Shape.codec' expected

    Expect.equal actual expected "The decoded value must match the original"

    let expected = Rectangle (7, 2)
    let actual = roundTrip Shape.codec' expected

    Expect.equal actual expected "The decoded value must match the original"

    let expected = Circle 3
    let actual = roundTrip Shape.codec' expected

    Expect.equal actual expected "The decoded value must match the original"
  }
]
