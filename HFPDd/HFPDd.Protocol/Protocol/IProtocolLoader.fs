namespace HFPDd.Protocol.Protocol

/// <summary>
///     The interface required by the protocol plugins to allow loading themselves
///     into the protocol system.
/// </summary>
/// <remarks>
///     Plugins are expected to expose a ProtocolInjector/ActionInjector class which can be default constructed,
///     and have a LoadInto method accepting a IProtocolLoader/IActionLoader.
///     Through this dance the plugin can initialize any internal state it wishes to upon loading and load itself into
///     the runtime.
/// </remarks>
type IProtocolLoader =
    /// <summary>
    ///     Adds a factory to the protocol system for the specified factory.
    /// </summary>
    /// <param name="proto">The name of the protocol to build.</param>
    /// <param name="fact">The factory building the <c>IProtocol</c> instances.</param>
    abstract AddFactory: proto: string -> fact: IProtocolFactory -> unit
