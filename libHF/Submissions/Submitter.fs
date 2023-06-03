namespace libHF.Submissions

open libHF.Student

/// <summary>
/// Simple class wrapping a Student who has submitted something.
/// </summary>
type Submitter(who: Student) =
    /// <summary>
    /// The human name of the Submitter.
    /// </summary>
    member val Name = who.Name with get
    /// <summary>
    /// The Neptun code of the Submitter.
    /// Guaranteed to be at least syntactically valid.
    /// </summary>
    /// <seealso cref="libHF.Submissions.Neptun"/>
    member val Neptun = who.Neptun with get

    override this.GetHashCode() = hash who

    override this.Equals(obj) =
        match obj with
        | :? Submitter as s -> s.Neptun = this.Neptun
        | :? Student as s -> s = who
        | _ -> false
