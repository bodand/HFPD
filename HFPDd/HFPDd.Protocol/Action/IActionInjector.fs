namespace HFPDd.Protocol.Action

/// <summary>
///     Interface whose implementations are searched in plugin dll-s.
/// </summary>
/// <remarks>
///     For an action plugin DLL, there needs to be implementations of the IActionInjector which can register a given
///     action handle in the action registry.
/// </remarks>
type IActionInjector =
    /// <summary>
    ///     Handles injecting the plugin this IProtocolInjector implementation is responsible into the IActionLoader.
    ///     Also handles any necessary plugin-level initialization.
    /// </summary>
    /// <param name="loader">The protocol handle to load ourselves into.</param>
    abstract LoadInto: loader: IActionLoader -> unit
