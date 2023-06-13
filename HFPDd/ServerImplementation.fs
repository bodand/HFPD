namespace HFPDd

open System
open System.Threading.Tasks

/// <summary>
/// Helper class that wraps a task and an IDisposable object.
/// This is returned by the TcpServer implementation and serves as the
/// main server object for HFPDd.
/// </summary>
/// <remarks>
/// <para>
/// The class can be disposed through the IDisposable interface, but note that
/// this will also wait for the given task to finish.
/// </para>
/// </remarks>
type ServerImplementation(wait: Task<unit>, server: IDisposable) =
    interface IDisposable with
        member this.Dispose() =
            wait.Wait()
            server.Dispose()

    /// <summary>
    /// Waits until the given task finishes.
    /// </summary>
    member this.Run() = wait.Wait()
