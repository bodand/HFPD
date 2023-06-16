namespace HFPDd.Protocol.Protocol

/// <summary>
///     Interface whose implementations are searched in plugin dll-s.
/// </summary>
/// <remarks>
///     For a protocol plugin DLL, there needs to be implementations of the IProtocolInjector which can register a given
///     protocol in the protocol handler.
/// </remarks>
type IProtocolInjector =
    /// <summary>
    ///     Handles injecting the plugin this IProtocolInjector implementation is responsible into the IProtocolLoader.
    ///     Also handles any necessary plugin-level initialization.
    /// </summary>
    /// <param name="loader">The protocol handle to load ourselves into.</param>
    abstract LoadInto: loader: IProtocolLoader -> unit
