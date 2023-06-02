namespace libHF.Submissions

open System
open System.IO

/// <summary>
/// Module for the Submission related types.
/// </summary>
module Submission =
    /// <summary>
    /// A sum type representing the possible statuses of a Submission.
    /// </summary>
    type SubmissionStatus =
        /// <summary>
        /// A successful submission's status.
        /// </summary>
        /// <param name="message">The message about the successful submission.</param>
        /// <param name="points">The number of points received for the submission.</param>
        | Accepted of message: string * points: int
        /// <summary>
        /// The status for a submission that has been rejected by hand.
        /// </summary>
        /// <param name="message">The message for the rejection. Usually reason.</param>
        | Rejected of message: string
        /// <summary>
        /// The status for a submission that failed some automatic test.
        /// </summary>
        /// <param name="message">
        /// The message for the rejection. Usually the failed check's output.
        /// </param>
        | AutomaticFailure of message: string
        /// <summary>
        /// A temporary status while the configured automatic checks are running.
        /// </summary>
        | Running
        /// <summary>
        /// A status for submissions who have no concrete status.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is the catch-all status.
        /// If a specific Homework does not have automatic checks configured,
        /// and a user has not manually given the submission a status this
        /// will be used.
        /// </para>
        /// <para>
        /// In case there is some delay about running the automatic checks,
        /// this status represents that the checks have not yet started running.
        /// </para>
        /// </remarks>
        | Unknown

    /// <summary>
    /// The type representing a given submission for a Homework.
    /// </summary>
    /// <remarks>
    /// A type storing a given submitter and managing the current state of the
    /// submission.
    /// Stores a given submission's submitter and the date on which the submission was made.
    /// Additionally, if given, it can store a list of files, which represent the
    /// actual files submitted.
    /// </remarks>
    type Submission(submitter: Submitter, status: SubmissionStatus, date: DateTime, files: List<File>) =
        let mutable current_status = status

        /// <summary>
        /// Helper for creating a Submission that was submitted now.
        /// </summary>
        /// <param name="submitter">The submitter.</param>
        /// <param name="files">The list of files submitted.</param>
        static member public Make submitter files =
            Submission(submitter, Unknown, DateTime.Now, files)

        /// <summary>
        /// The submitter object, who submitted this submission.
        /// </summary>
        member val Submitter = submitter with get
        /// <summary>
        /// The date on which this submission was submitted.
        /// </summary>
        member val Date = date with get
        /// <summary>
        /// The list of files submitted in this submission.
        /// </summary>
        member val Files = files with get

        /// <summary>
        /// The current status of this submission.
        /// </summary>
        /// <remarks>
        /// This property reports the current status of a given submission.
        /// This may change, as a Submission type is not totally immutable.
        /// </remarks>
        member this.Status
            with get () = current_status
            and private set (value) = current_status <- value

        /// <summary>
        /// Sets the current status to Running.
        /// </summary>
        /// <see cref="libHF.Submissions.Submission.SubmissionStatus.Running"/>
        member this.StartRunning = this.Status <- Running
        /// <summary>
        /// Sets the current status to automatically failed.
        /// </summary>
        /// <param name="msg">The message from the automatic check.</param>
        /// <see cref="libHF.Submissions.Submission.SubmissionStatus.AutomaticFailure"/>
        member this.AutoFail msg = this.Status <- AutomaticFailure msg
        /// <summary>
        /// Sets the current status to rejected.
        /// </summary>
        /// <param name="msg">The message for the rejection.</param>
        /// <see cref="libHF.Submissions.Submission.SubmissionStatus.Rejected"/>
        member this.Reject msg = this.Status <- Rejected msg
        /// <summary>
        /// Sets the current status to accepted.
        /// </summary>
        /// <param name="msg">The message for the acceptance.</param>
        /// <param name="pts">The points received by this submission.</param>
        /// <see cref="libHF.Submissions.Submission.SubmissionStatus.Accepted"/>
        member this.Accept msg pts = this.Status <- Accepted(msg, pts)
