namespace Thoth.Json.Codec

open Thoth.Json.Core.Auto

[<AutoOpen>]
module Auto =

  module Codec =

    type Auto private () =
      static member inline generateCodec
        (?caseStrategy : CaseStrategy, ?extra : ExtraCoders, ?skipNullField : bool)
        : Codec<'t> =
        Codec.create
          (Encode.Auto.generateEncoder (?caseStrategy = caseStrategy, ?extra = extra, ?skipNullField = skipNullField))
          (Decode.Auto.generateDecoder (?caseStrategy = caseStrategy, ?extra = extra))
