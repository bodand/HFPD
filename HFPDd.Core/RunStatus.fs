namespace HFPDd.Core

/// <summary>
///     The type representing the result of an action after running it.
/// </summary>
/// <remarks>
///     This is a sum type representing either success or failure of an action.
///     An action can either succeed with a message and optionally output and error data,
///     or fail with a message.
/// </remarks>
type RunStatus =
    /// <summary>
    ///     The constructor for creating RunStatus objects for successful runs of an action.
    /// </summary>
    /// <param name="message">The message about the action's run.</param>
    /// <param name="output">Optional output from the action; its meaning is action specific.</param>
    /// <param name="error">Optional error output from the action; its meaning is action specific.</param>
    | Success of message: string * output: string option * error: string option
    /// <summary>
    ///     The constructor for creating RunStatus objects for failed runs on an action.
    /// </summary>
    /// <param name="message">The message about the action's run.</param>
    | Failure of message: string
