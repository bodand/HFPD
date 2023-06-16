namespace HFPDd.Protocol

open HFPDd.Protocol.Protocol

/// <summary>
///     The registry interface that allows the querying of the ProtocolRegistry.
/// </summary>
type IProtocolRegistry =
    /// <summary>
    ///     Creates and returns a given IProtocol, if it exists in the registry.
    /// </summary>
    /// <param name="pid">The identifier of the protocol.</param>
    abstract GetProtocol: pid: string -> IProtocol option