# Thoth.Json.Codec

Experimental codec support for [Thoth.Json](https://github.com/thoth-org/Thoth.Json) 🧪

Why use codecs?

 * Easier to keep encoding and decoding in sync
 * Less code in many cases
 * Clearer semantics when both encoding and decoding are required

## Install

Install from NuGet:

```bash
dotnet add package Thoth.Json.Codec
paket add Thoth.Json.Codec
```

## Quick Demo

```fsharp
open Thoth.Json.Codec

type FooBar =
  {
    Foo : int
    Bar : string
  }

module FooBar =

  let codec : Codec<FooBar> =
    objectCodec {
      let! foo = Codec.field "foo" (fun x -> x.Foo) Codec.int
      and! bar = Codec.field "bar" (fun x -> x.Bar) Codec.string

      return
        {
          Foo = foo
          Bar = bar
        }
    }

let fooBar =
  {
    Foo = 123
    Bar = "abc"
  }

let json =
  fooBar
  |> Encode.codec FooBar.codec
  |> Encode.toString 2

(*
JSON looks like this:

{
  "foo": 123,
  "bar": "abc"
}

*)

let decoded =
  encoded
  |> Decode.fromString (Decode.codec FooBar.codec)
```
