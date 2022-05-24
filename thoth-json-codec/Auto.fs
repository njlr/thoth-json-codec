namespace Thoth.Json.Codec

#if FABLE_COMPILER
open Thoth.Json
#else
open Thoth.Json.Net
#endif

[<AutoOpen>]
module Auto =

  module Codec =

    type Auto private () =
      static member inline generateCodec(?caseStrategy : CaseStrategy, ?extra : ExtraCoders, ?skipNullField : bool) : Codec<'t> =
        Codec.create
          (Encode.Auto.generateEncoder(?caseStrategy=caseStrategy, ?extra=extra, ?skipNullField=skipNullField))
          (Decode.Auto.generateDecoder(?caseStrategy=caseStrategy, ?extra=extra))
