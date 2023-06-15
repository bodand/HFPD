namespace HFPDd.Core

open Microsoft.FSharp.Collections

type RuntimeManager() =
    // let actionsMap = Map<string, RunStatus>
    let mutable id: string option = None
    let mutable terminated: bool = false

    /// <summary>
    ///     The identifier of the runtime object.
    /// </summary>
    /// <value>
    ///     Optional value of the identifier of the object.
    ///     If none has been set yet, <c>None</c>, <c>Some id</c> otherwise.
    /// </value>
    member x.Id = id

    /// <summary>
    ///     Property whether the current object has been terminated.
    /// </summary>
    /// <value>The boolean value if the object is terminated.</value>
    member x.Terminated = terminated

    /// <summary>
    ///     Initializes the Runtime with the provided ID as received from the DaemonManager
    ///     running in HFPD.
    /// </summary>
    /// <param name="new_id">The new ID.</param>
    /// <exception cref="RuntimeAlreadyHasIdException">The ID has already been set on the runtime.</exception>
    /// <exception cref="RuntimeTerminatedException">
    ///     The runtime has already terminated before setting the ID.
    ///     If no ID has been set previously, the exception contains the "&lt;unknown-id&gt;" string as the ID.
    /// </exception>
    /// <remarks>
    ///     <list type="bullet">
    ///         <listheader>
    ///             <description>Preconditions</description>
    ///         </listheader>
    ///         <item>
    ///             <description>The object has not yet been terminated.</description>
    ///         </item>
    ///         <item>
    ///             <description>The object does not have a different identifier already set.</description>
    ///         </item>
    ///     </list>
    ///     <list type="bullet">
    ///         <listheader>
    ///             <description>Postconditions</description>
    ///         </listheader>
    ///         <item>
    ///             <description>The object has a non None identifier set.</description>
    ///         </item>
    ///     </list>
    ///     <para>
    ///         This method is the first that needs to be called by the main application.
    ///         All other methods have a precondition, that this has been called with a valid string.
    ///     </para>
    ///     <para>
    ///         The HDCP command word <c>INIT</c> is basically this method's equivalent.
    ///         Other protocols which may need some initialization step with data provided by the communication partner
    ///         may rely on this method for implementing this.
    ///         If a protocol does not need to be initialized, (such as a HTTP server backed REST API, which could
    ///         easily be implemented in a way that all initialization can happen without anything being sent through
    ///         the wire) can just set the ID to the empty string: the RuntimeManager only requires this to be set.
    ///     </para>
    /// </remarks>
    member x.Initialize new_id =
        if terminated then
            match id with
            | None -> raise (RuntimeTerminatedException "<unknown-id>")
            | Some value -> raise (RuntimeTerminatedException value)

        if id <> None && id <> Some new_id then
            raise (RuntimeAlreadyHasIdException(id.Value, new_id))

        id <- Some new_id

    /// <summary>
    ///     Terminates this runtime object if the given id matches the one set in the runtime.
    ///     This is not related to resource management, the RuntimeManager merely terminates as it finishes the required
    ///     lifetime as described in the used protocol.
    /// </summary>
    /// <param name="term_id">The id of the runtime to terminate.</param>
    /// <exception cref="RuntimeTerminatedException">The runtime has already been terminated before.</exception>
    /// <remarks>
    ///     <list type="bullet">
    ///         <listheader>
    ///             <description>Preconditions</description>
    ///         </listheader>
    ///         <item>
    ///             <description>The object has not yet been terminated.</description>
    ///         </item>
    ///     </list>
    ///     <list type="bullet">
    ///         <listheader>
    ///             <description>Postconditions</description>
    ///         </listheader>
    ///         <item>
    ///             <description>The object is in a terminated state.</description>
    ///         </item>
    ///     </list>
    ///     <para>
    ///         This method silently ignores attempts to terminate a runtime manager object with the wrong id.
    ///     </para>
    ///     <para>
    ///         The HDCP command word <c>RELEASE</c> has the same purpose as this method.
    ///         Notify the recipient not to behave as normal anymore, and prepare to cease operations.
    ///         Protocols who cannot properly relay termination orders from the daemon manager are allowed not to call
    ///         this method, but then they need to implement another way of terminating the daemon.
    ///     </para>
    /// </remarks>
    member x.Terminate term_id =
        if terminated then
            raise (RuntimeTerminatedException term_id)

        if id = Some term_id then
            terminated <- true
