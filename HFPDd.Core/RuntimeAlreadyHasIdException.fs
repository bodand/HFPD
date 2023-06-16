namespace HFPDd.Core

/// <summary>
///     Exception thrown whenever a Runtime that already has and ID specified
///     is specified again.
/// </summary>
/// <remarks>
///     This exception arises for example a protocol that is initialized twice and thus tries to initialize the
///     runtime twice.
///     Such a protocol is HDCP which has the command word <c>INIT</c>.
/// </remarks>
exception RuntimeAlreadyHasIdException of
    /// <summary>
    ///     The already present ID of the runtime.
    /// </summary>
    old: string *
    /// <summary>
    ///     The ID that was provided to initialize the runtime again.
    /// </summary>
    ``new``: string