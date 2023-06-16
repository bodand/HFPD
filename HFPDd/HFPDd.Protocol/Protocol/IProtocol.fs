namespace HFPDd.Protocol.Protocol

open HFPDd.Core

/// <summary>
///     Interface that a given protocol implementation needs to implement.
/// </summary>
type IProtocol =
    /// <summary>
    ///     A virtual function that gives a string to the protocol to parse.
    /// </summary>
    /// <param name="read">
    ///     The string data as read from the wire including those the parser previously gave back as unfinished chunks.
    /// </param>
    /// <remarks>
    ///     <para>
    ///         The Process function takes a string and decides to parse it or not and returns it decision.
    ///         If it decides to not parse, or begins parsing with an algorithm that can pause its operations and does
    ///         not finish with a valid statement, the half-finished data is returned in a Parsing data construct, from
    ///         which the data part will be passed back in the next invocation prepended to the new string chunk.
    ///     </para>
    ///     <para>
    ///         If it decides to parse that can either succeed or failL: this information is reported in the
    ///         Success and Failed constructors of a ParsingStatus.
    ///         A successful parsing is to return a ProtocolCommand subclass's object which will then be executed by
    ///         the protocol loading framework and HFPDd runtime.
    ///         In case there is leftover data from the input string, for example after a delimited command, there is
    ///         the text of the next command, this can be returned as an optional second parameter of the aforementioned
    ///         constructors.
    ///     </para>
    /// </remarks>
    abstract Process: read: string -> ParsingStatus<ProtocolCommand>
