namespace HFPDd.Protocol.Protocol

open HFPDd.Protocol

/// <summary>
///     Abstract factory interface for creating IProtocol objects.
/// </summary>
type IProtocolFactory =
    /// <summary>
    ///     Creates an IProtocol object as an appropriate subclass, that
    ///     may use the provided IActionRegistry
    /// </summary>
    /// <param name="areg">An action registry</param>
    abstract BuildProtocol: areg: IActionRegistry -> IProtocol
