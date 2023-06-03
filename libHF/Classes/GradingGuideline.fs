namespace libHF.Classes

open System

/// <summary>
/// A sum-type for the types of grading that results in points.
/// </summary>
/// <remarks>
/// Grading is a sum of positive and negative points.
/// Positive points are given for particular pieces of the answers that are expected to be present,
/// while negative points are for parts of code/answer that are so wrong, that they should be punished.
/// A Homework defines a set of Guidelines which together make up the points that may be gathered for that
/// homework with the errata that may be encountered.
/// </remarks>
type GradingGuideline =
    /// <summary>
    /// The positive grading object.
    /// </summary>
    /// <param name="name">The textual description of the points that can be gathered.</param>
    /// <param name="max">The maximum number of points that can be given for this grading guideline.</param>
    | PointGuideline of name: string * max: int
    /// <summary>
    /// The negative grading object.
    /// </summary>
    /// <param name="name">The textual description of why these point are subtracted.</param>
    /// <param name="max">The maximum number (maximum in absolute value) of points that can be subtracted for this errata.</param>
    | ErrataGuideline of name: string * max: int

    member this.AllowedPoints (x: int): bool =
        match this with
        | PointGuideline(name, max) -> 0 <= x && x <= max
        | ErrataGuideline(name, max) -> -Math.Abs(max) <= x && x <= 0