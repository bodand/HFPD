namespace HFPDd.Protocol.Action

open HFPDd.Core

/// <summary>
///     Interface for acting out the required actions for a given protocol.
/// </summary>
type IAction =
    abstract Process: payload: string -> Async<RunStatus>
