namespace Thoth.Json.Codec

#if FABLE_COMPILER
open Thoth.Json
#else
open Thoth.Json.Net
#endif

[<AutoOpen>]
module VariantCodecBuilder =

  type VariantEncoding =
    | TagAndValue of tagName : string * valueName : string
    | OnTag

  [<NoComparison>]
  type VariantCase<'t, 'v> =
    {
      Value : 't
      Decoders : Map<string, Decoder<'v>>
    }

  module VariantCase =

    let zip (a : VariantCase<'a, 'v>) (b : VariantCase<'b, 'v>) : VariantCase<'a * 'b, 'v> =
      {
        Value = a.Value, b.Value
        Decoders =
          Map.ofSeq
            [
              yield! Map.toSeq a.Decoders
              yield! Map.toSeq b.Decoders
            ]
      }

    let complete (variantEncoding : VariantEncoding) (f : 't -> ('v -> VariantEncoding -> JsonValue)) (x : VariantCase<'t, 'v>) : Codec<'v> =
      let decodeForTag tag fieldName : Decoder<_> =
        match Map.tryFind tag x.Decoders with
        | Some decoder ->
          Decode.field fieldName decoder
        | None ->
          Decode.fail $"The tag \"{tag}\" was not recognized"

      Codec.create
        (fun (v : 'v) -> f x.Value v variantEncoding)
        (
          match variantEncoding with
          | TagAndValue (tagName, valueName) ->
            Decode.field tagName Decode.string
            |> Decode.andThen (fun tag -> decodeForTag tag valueName)
          | OnTag ->
            Decode.keys
            |> Decode.andThen
              (fun keys ->
                let recognizedKeys =
                  keys
                  |> Seq.filter (fun key -> x.Decoders |> Map.containsKey key)

                match Seq.tryExactlyOne recognizedKeys with
                | Some tag ->
                  decodeForTag tag tag
                | None ->
                  let found =
                    recognizedKeys
                    |> Seq.map (fun x -> $"\"{x}\"")
                    |> String.concat ", "

                  Decode.fail $"Expected exactly one object key but found: {found}"))

  type VariantCodecBuilder(variantEncoding : VariantEncoding) =
    member this.MergeSources(a, b) =
      VariantCase.zip a b

    member this.BindReturn(x, f) =
      VariantCase.complete variantEncoding f x

  let variantCodecWithEncoding variantEncoding = VariantCodecBuilder(variantEncoding)

  let variantCodec = VariantCodecBuilder(OnTag)

  [<RequireQualifiedAccess>]
  module Codec =

    let case (tag : string) (caseConstructor : 't -> 'v) (caseCodec : Codec<'t>) : VariantCase<'t -> VariantEncoding -> JsonValue, 'v> =
      {
        Value =
          fun t ->
            function
            | OnTag -> Encode.object [ tag, caseCodec.Encoder t ]
            | TagAndValue (tagName, valueName) ->
              Encode.object
                [
                  tagName, Encode.string tag
                  valueName, caseCodec.Encoder t
                ]
        Decoders = Map.ofSeq [ tag, caseCodec.Decoder |> Decode.map caseConstructor ]
      }
