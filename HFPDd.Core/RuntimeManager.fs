namespace HFPDd.Core

open Microsoft.FSharp.Collections

type RuntimeManager() =
    let mutable actionsMap = Map<string, RunStatus> []
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

    /// <summary>
    ///     Adds an action with its Action ID (aid) to the Runtime for later querying.
    /// </summary>
    /// <param name="aid">The </param>
    /// <param name="stat"></param>
    /// <exception cref="RuntimeTerminatedException">The runtime has already been terminated before.</exception>
    /// <remarks>
    ///     <list type="bullet">
    ///         <listheader>
    ///             <description>Preconditions</description>
    ///         </listheader>
    ///         <item>
    ///             <description>The object has not yet been terminated.</description>
    ///         </item>
    ///         <item>
    ///             <description>The object does not contain the action id mapping to other status.</description>
    ///         </item>
    ///     </list>
    ///     <list type="bullet">
    ///         <listheader>
    ///             <description>Postconditions</description>
    ///         </listheader>
    ///         <item>
    ///             <description>The object contains the mapping from aid to stat.</description>
    ///         </item>
    ///         <item>
    ///             <description>The object contains all previous mappings.</description>
    ///         </item>
    ///     </list>
    ///     <para>
    ///         The Runtime will replace the aids which have been inserted prior and not removed before the call.
    ///         Therefore, if there already is a mapping from the given aid to some status, the function does not uphold
    ///         its postconditions.
    ///     </para>
    /// </remarks>
    member x.AddFinishedAction aid stat =
        if terminated then
            raise (RuntimeTerminatedException id.Value)

        actionsMap <- actionsMap.Add(aid, stat)

    /// <summary>
    ///     Retrieves and removes a mapping of an action id from the runtime or returns None.
    /// </summary>
    /// <param name="aid">The action id to search for.</param>
    /// <exception cref="RuntimeTerminatedException">The runtime has already been terminated before.</exception>
    /// <remarks>
    ///     <list type="bullet">
    ///         <listheader>
    ///             <description>Preconditions</description>
    ///         </listheader>
    ///         <item>
    ///             <description>The object has not yet been terminated.</description>
    ///         </item>
    ///         <item>
    ///             <description>The object does not contain the action id mapping to other status.</description>
    ///         </item>
    ///     </list>
    ///     <list type="bullet">
    ///         <listheader>
    ///             <description>Postconditions</description>
    ///         </listheader>
    ///         <item>
    ///             <description>The object does not contain the mapping from aid to stat.</description>
    ///         </item>
    ///         <item>
    ///             <description>The object contains all other mappings.</description>
    ///         </item>
    ///     </list>
    ///     <para>
    ///         Searches for the aid among the mappings and returns the found runtime status if it exists.
    ///     </para>
    /// </remarks>
    member x.GetFinishedAction aid =
        if terminated then
            raise (RuntimeTerminatedException id.Value)

        let stat = actionsMap.TryFind aid

        stat
        |> Option.map (fun x ->
            actionsMap <- actionsMap.Remove aid
            x)
