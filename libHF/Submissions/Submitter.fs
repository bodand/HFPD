namespace libHF.Submissions

/// <summary>
/// Simple class joining together a human name and a Neptun object
/// for a given Submitter.
/// </summary>
type Submitter(name: string, neptun: Neptun) =
    /// <summary>
    /// The human name of the Submitter.
    /// </summary>
    member val Name = name with get, set
    /// <summary>
    /// The Neptun code of the Submitter.
    /// Guaranteed to be at least syntactically valid.
    /// </summary>
    /// <seealso cref="libHF.Submissions.Neptun"/>
    member val Neptun = neptun with get, set
