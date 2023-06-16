namespace HFPDd.Protocol.Protocol

/// <summary>
///     Protocol's return type representing the current state of parsing.
/// </summary>
type ParsingStatus<'T> =
    /// <summary>
    ///     A ParsingStatus constructor representing a failed parsing attempt.
    /// </summary>
    /// <param name="msg">
    ///     The message reported by the protocol/parser, send back to the request's sender as a response.
    /// </param>
    /// <remarks>
    ///     Since the returned data is sent back to the sender verbatim, the protocol MUST ensure that it is a valid
    ///     response according the to implemented protocol's specification.
    ///     For example the followings show some considerations to check when implementing a protocol. Note that this is
    ///     in no means a comprehensive list.
    ///     <list type="bullet">
    ///         <item>
    ///             <description>
    ///                 If the protocol requires escaping special characters it MUST be done by the implementation.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 The response MUST be a valid response the the parsed request, if it was well-formed as specified
    ///                 by the implemented protocol.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 If the implemented protocol specifies the error response format(s), for ill-formed requests this
    ///                 MUST be well-formed according to that specification and MUST be semantically correct.
    ///             </description>
    ///         </item>
    ///     </list>
    /// </remarks>
    | Failed of msg: string
    /// <summary>
    ///     A ParsingStatus constructor representing a parsing still in progress.
    /// </summary>
    /// <param name="data">
    ///     String data, which has not yet been parsed. The next parts of the input will be appended to this when the
    ///     next invocation happens.
    /// </param>
    /// <remarks>
    ///     This below described practice is not prohibited, but not actively encouraged.
    ///     It is possible for a protocol to add custom data in this, but if it does so it MUST be prepared to parse
    ///     it again when the next parse invocation happens.
    ///     For example a protocol may start parsing the data chunk as-is and then output a serialized AST to this which
    ///     it can just read back again and continue parsing the new data.
    ///     This also means, a protocol can perform preprocessing on the chunks before commencing the final processing
    ///     when all data has arrived.
    /// </remarks>
    | Parsing of data: string
    /// <summary>
    ///     A ParsingStatus constructor representing a successful parsing attempt.
    /// </summary>
    /// <param name="parsed">The parsed data.</param>
    /// <param name="remainder">Optional string data, that will be passed to the next invocation.</param>
    /// <remarks>
    ///     <para>
    ///         The first parameter (<c>parsed</c>) is a value of arbitrary type as returned by the parser.
    ///         The HFPDd protocol loading framework requires it to be a <c>ProtocolCommand</c> subclass, but in general
    ///         it could be any data.
    ///         This data MUST be a valid value (non null) and MUST only perform operations that can be decoded from the
    ///         request.
    ///         That is, the protocol loading framework SHOULD assume that the protocol supplied deterministically
    ///         returns the same <c>ProtocolCommand</c> (at least one that behaves equivalently to it) and MAY cache it.
    ///     </para>
    ///     <para>
    ///         The <c>remainder</c> parameter MUST be <c>None</c> or it MUST contain a string which can 1) be appended
    ///         to when the next chunk arrives; and 2) can be passed to the same protocol implementation as-is.
    ///         Note that the protocol loading framework MAY OR MAY NOT wait for the next chunk to arrive, and see if
    ///         the protocol can do anything with this data as-is.
    ///         The framework implementation SHOULD wait with a relatively small (unspecified) timeout for a next chunk
    ///         to arrive and if it times out, then pass this data as-is to the protocol implementation.
    ///         This allows optimizing a call to a dynamic function in highly contested environments, when a new chunk
    ///         is expected to arrive, or has actually arrived and not lose data in low contested environments at the
    ///         cost of little bit of a delay. (An implementation may choose to alter its behavior regarding this at
    ///         runtime.) 
    ///     </para>
    /// </remarks>
    | Success of parsed: 'T * remainder: string option
