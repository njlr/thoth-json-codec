namespace Thoth.Json.Codec

#if FABLE_COMPILER
open Thoth.Json
#else
open Thoth.Json.Net
#endif

[<AutoOpen>]
module VariantCodecBuilder =

  [<NoComparison>]
  type VariantCase<'t, 'v> =
    {
      Value : 't
      Decoders : Map<string, Decoder<'v>>
    }

  type VariantCodecBuilder() =
    member this.MergeSources(a : VariantCase<'a, 'v>, b : VariantCase<'b, 'v>) : VariantCase<'a * 'b, 'v> =
      {
        Value = a.Value, b.Value
        Decoders =
          Map.ofSeq
            [
              yield! Map.toSeq a.Decoders
              yield! Map.toSeq b.Decoders
            ]
      }

    member this.BindReturn(x : VariantCase<'t, 'v>, f : 't -> Encoder<'v>) : Codec<'v> =
      Codec.create
        (fun (v : 'v) -> f x.Value v)
        (Decode.keys
          |> Decode.andThen
            (fun keys ->
              match List.tryExactlyOne keys with
              | Some tag ->
                match Map.tryFind tag x.Decoders with
                | Some decoder ->
                  Decode.field tag decoder
                | None ->
                  Decode.fail $"The tag \"{tag}\" was not recognized"
              | None ->
                Decode.fail $"Expected exactly one object key but found {keys}"))


  let variantCodec = VariantCodecBuilder()

  [<RequireQualifiedAccess>]
  module Codec =

    let case (tag : string) (caseConstructor : 't -> 'v) (caseCodec : Codec<'t>) : VariantCase<Encoder<'t>, 'v> =
      {
        Value = fun t -> Encode.object [ tag, caseCodec.Encoder t ]
        Decoders = Map.ofSeq [ tag, caseCodec.Decoder |> Decode.map caseConstructor ]
      }
