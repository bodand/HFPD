namespace HFPDd.Core

open System.Runtime.InteropServices

/// <summary>
///     Abstract baseclass for the specific commands that may be executed by a protocol.
/// </summary>
/// <remarks>
///     The specific protocols instantiate objects derived from this type to implement completely decoupled
///     protocols from the runtime.
/// </remarks>
[<AbstractClass>]
type ProtocolCommand() =
    /// <summary>
    ///     Performs the specific action implemented in the subclass.
    /// </summary>
    /// <param name="rt">The RuntimeManager object associated with the protocol.</param>
    abstract Execute: rt: RuntimeManager -> unit
