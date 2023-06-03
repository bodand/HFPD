namespace libHF.Student

open System

/// <summary>
/// Student type that wraps together a name and a NEPTUN code.
/// </summary>
/// <remarks>
/// <para>
/// The type implements custom ordering: a Student is sorted before another Student based on their
/// NEPTUN codes, completely disregarding the value of their names.
/// </para>
/// </remarks>
type Student(name: String, neptun: Neptun) =
    member val Name = name with get
    member val Neptun = neptun with get

    interface IComparable with
        member this.CompareTo(obj) =
            match obj with
            | :? Student as s -> (this.Neptun :> IComparable).CompareTo(s.Neptun)
            | _ -> 1
        
    override this.Equals(obj) = (this :> IComparable).CompareTo(obj) = 0
    override this.GetHashCode() = this.Neptun.GetHashCode()
