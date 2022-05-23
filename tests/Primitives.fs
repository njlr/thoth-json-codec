module Thoth.Json.Codec.Tests.Primitives

open System

#if FABLE_COMPILER
open Fable.Mocha
#else
open Expecto
#endif

open Thoth.Json.Codec
open Thoth.Json.Codec.Tests.Utils

let tests = testList "Primitives" [
  test "Codec.int works for simple case 1" {
    let expected = 123
    let actual = roundTrip Codec.int expected

    Expect.equal actual expected "The decoded value must match the original"
  }

  test "Codec.string works for simple case 1" {
    let expected = "Hello, world. "
    let actual = roundTrip Codec.string expected

    Expect.equal actual expected "The decoded value must match the original"
  }

  test "Codec.bool works for simple case 1" {
    let expected = true
    let actual = roundTrip Codec.bool expected

    Expect.equal actual expected "The decoded value must match the original"
  }

  test "Codec.datetimeAsIs works for simple case 1" {
    let expected = DateTime.Parse "2022-05-23T07:45:39.700Z"
    let actual = roundTrip Codec.datetimeAsIs expected

    Expect.equal actual expected "The decoded value must match the original"
  }
]
