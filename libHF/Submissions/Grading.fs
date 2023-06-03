namespace libHF.Submissions

open System
open libHF.Classes

/// <summary>
/// Concrete points received on a GradingGuideline.
/// </summary>
/// <remarks>
/// Stores a given GradingGuideline and the amount of points received on that guideline.
/// The type guarantees the received value is appropriate in the context of the GradingGuideline.
/// </remarks>
type Grading(guideline: GradingGuideline, received: int) =
    do
        if not <| guideline.AllowedPoints received then
            raise
            <| ArgumentException $"Illegal amount of points received on grade: {received}"

    /// <summary>
    /// The GradingGuideline the grading is given for.
    /// </summary>
    member val Guideline = guideline with get
    /// <summary>
    /// The amount of points received for the GradingGuideline.
    /// </summary>
    member val ReceivedPoints = received with get
