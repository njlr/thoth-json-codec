module Thoth.Json.Codec.Tests.Combinators

#if FABLE_COMPILER
open Fable.Mocha
open Thoth.Json.Codec
#else
open Expecto
open Thoth.Json.Net.Codec
#endif

open Thoth.Json.Codec.Tests.Utils

let tests = testList "Combinators" [
  test "Codec.option works for simple case 1" {
    let codec = Codec.option Codec.string

    let expected = Some "abc"

    let actual = roundTrip codec expected

    Expect.equal actual expected "The decoded value must match the original"
  }

  test "Codec.option works for simple case 2" {
    let codec = Codec.option Codec.string

    let expected = None

    let actual = roundTrip codec expected

    Expect.equal actual expected "The decoded value must match the original"
  }

  test "Codec.list works for simple case 1" {
    let codec = Codec.list Codec.int

    let expected = [ 4; 6; 1; 2; 2; 8; 0; 5; 3 ]

    let actual = roundTrip codec expected

    Expect.equal actual expected "The decoded value must match the original"
  }

  test "Codec.array works for simple case 1" {
    let codec = Codec.array Codec.string

    let expected =
      [| "the"; "quick"; "brown"; "fox";
         "jumped"; "over"; "the"; "lazy"; "dog" |]

    let actual = roundTrip codec expected

    Expect.equal actual expected "The decoded value must match the original"
  }
]
