# Thoth.Json.Codec

Experimental codec support for [Thoth.Json](https://github.com/thoth-org/Thoth.Json) 🧪

Why use codecs?

 * Easier to keep encoding and decoding in sync
 * Less code in many cases
 * Clearer semantics when both encoding and decoding are required

## Install

Install from [NuGet](https://www.nuget.org/packages/Thoth.Json.Codec/):

```bash
dotnet add package Thoth.Json.Codec
paket add Thoth.Json.Codec
```

## Instructions

This library is built around a simple type definition:

```fsharp
type Codec<'t> =
  {
    Encoder : Encoder<'t>
    Decoder : Decoder<'t>
  }
```

Remember that a well-formed codec will allow an arbitary number of encoding-decoding round-trips.

First, open the namespace:

```fsharp
open Thoth.Json.Codec
```

Now you can create a codec from existing encoders and decoders like so:

```fsharp
let codec = Codec.create Encode.string Decode.string
```

However, it is recommended to use the built-in primitives.

```fsharp
Codec.int
Codec.bool
Codec.string

// etc...
```

You can encode values like this:

```fsharp
let json =
  123
  |> Encode.codec Codec.int
  |> Encode.toString 2
```

And decode JSON like this:

```fsharp
let decoded =
  "true"
  |> Decode.fromString (Decode.codec Codec.bool)
```

Object codecs, typically used for Records, can be constructed using the `objectCodec` Computation Expression:

```fsharp
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
```

The JSON looks like this:

```json
{
  "foo": 123,
  "bar": "abc"
}
```

Note the use of `and!`!

Variants, such as Discriminated Unions, should be constructed using the `variantCodec` Computation Expression:

```fsharp
type Shape =
  | Square of width : int
  | Rectangle of width : int * height : int

module Shape =

  let codec : Codec<Shape> =
    variantCodec {
      let! square = Codec.case "square" Square Codec.int
      and! rectangle = Codec.case "rectangle" Rectangle (Codec.tuple2 Codec.int Codec.int)

      return
        function
        | Square w -> square w
        | Rectangle (w, h) -> rectangle (w, h)
    }
```

With the above codec, the case value will be encoded to a property with the name of the tag.

In other words, the JSON will look like:

```json
{
  "square": 16
}
```

```json
{
  "rectangle": [
    3,
    4
  ]
}
```

If you prefer an object with `tag` and `value` properties, you can do the following:

```fsharp
module Shape =

  let codec : Codec<Shape> =
    variantCodecWithEncoding (TagAndValue ("tag", "value")) {
      let! square = Codec.case "square" Square Codec.int
      and! rectangle = Codec.case "rectangle" Rectangle (Codec.tuple2 Codec.int Codec.int)

      return
        function
        | Square w -> square w
        | Rectangle (w, h) -> rectangle (w, h)
    }
```

This gives JSON like so

```json
{
  "tag": "square",
  "value": 16
}
```

```json
{
  "tag": "rectangle",
  "value": [
    3,
    4
  ]
}
```
