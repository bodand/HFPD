namespace libHF.Submissions

open libHF.Student

/// <summary>
/// Simple class wrapping a Student who has submitted something.
/// </summary>
type Submitter(who: Student) =
    /// <summary>
    /// The human name of the Submitter.
    /// </summary>
    member val Name = who.name with get, set
    /// <summary>
    /// The Neptun code of the Submitter.
    /// Guaranteed to be at least syntactically valid.
    /// </summary>
    /// <seealso cref="libHF.Submissions.Neptun"/>
    member val Neptun = who.neptun with get, set
