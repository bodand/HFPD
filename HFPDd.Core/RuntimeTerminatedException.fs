namespace HFPDd.Core

/// <summary>
///     The exception class thrown when an runtime which has already terminated is accessed.
/// </summary>
exception RuntimeTerminatedException of
    /// <summary>
    ///     The runtime's identifier.
    /// </summary>
    id: string
