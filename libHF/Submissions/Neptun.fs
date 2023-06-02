namespace libHF.Submissions

open System

/// <summary>
/// The internal module for NEPTUN code related utilities.
/// </summary>
module private Neptun =
    /// <summary>
    /// The lenght expected of a NEPTUN code.
    /// </summary>
    /// <remarks>
    /// The constant 6.
    /// </remarks>
    let CodeLength = 6
    /// <summary>
    /// A char -> bool function that is used to validate a
    /// character in a NEPTUN code.
    /// </summary>
    /// <remarks>
    /// This is a function that maps each character into a boolean value
    /// depending on whether that character can appear in a syntactically
    /// valid NEPTUN code.
    /// This means only ASCII alpha characters and arabic numerals.
    /// </remarks>
    /// <seealso cref="Char.IsAsciiLetterOrDigit"/>
    let CharacterCheck = Char.IsAsciiLetterOrDigit

    /// <summary>
    /// Checks a string if it is a syntactically valid NEPTUN code.
    /// </summary>
    /// <param name="str">The string to check for validity.</param>
    /// <returns>True if the string could be a valid NEPTUN code, based on syntax.</returns>
    let validateNeptun (str: string) : bool =
        if str.Length <> CodeLength then
            false
        else
            let alnums = Seq.map CharacterCheck str
            Seq.fold (fun state elem -> state && elem) true alnums

/// <summary>The Struct representing a valid NEPTUN code.</summary>
/// <remarks>
/// <para>
/// This type wraps a string, with the additional information that
/// the stored string is a valid NEPTUN code.
/// The constructor validates strings passed to this object, so that a
/// NEPTUN code is syntactically valid if passed through this interface.
/// </para>
/// <para>
/// Note, that this does not mean that the code is **semantically** valid,
/// that is there exists a student who has this NEPTUN code.
/// </para>
/// </remarks>
[<Struct>]
type Neptun =
    /// <summary>
    /// The value of the Neptun object.
    /// </summary>
    /// <remarks>
    /// The value stored by the Neptun object.
    /// This string always contains an at least syntactically valid NEPTUN code.
    /// </remarks>
    val value: string

    /// <summary>
    /// Checks and possibly wraps a string in a Neptun object.
    /// </summary>
    /// <remarks>
    /// Checks a string for syntactical validity as a NEPTUN code.
    /// If it is not valid, an ArgumentException is thrown, else a new Neptun
    /// object is created, which wraps the input string.
    /// </remarks>
    /// <param name="value">The string to check and wrap.</param>
    /// <exception cref="ArgumentException">If the string is not a valid NEPTUN code.</exception>
    new(value: string) =
        { value = String.map Char.ToUpper value }
        then
            if not <| Neptun.validateNeptun value then
                raise <| ArgumentException $"Invalid NEPTUN code: {value}"
