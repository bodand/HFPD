namespace HFPDd.Protocol

open HFPDd.Protocol.Action

/// <summary>
///     The registry interface that allows the querying of the ActionRegistry.
/// </summary>
type IActionRegistry =
    /// <summary>
    ///     Creates and returns a given IAction, if it exists in the registry.
    /// </summary>
    /// <param name="aid">The identifier of the action.</param>
    abstract GetAction: aid: string -> IAction option