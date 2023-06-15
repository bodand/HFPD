namespace HFPDd.Network

open System
open System.Net
open System.Net.Sockets

/// <summary>
/// Generic interface for the .NET Socket class.
/// </summary>
/// <remarks>
/// Encapsulates the basic interface of the Socket class in a way that it can be
/// used as a decoupled type.
/// The given properties and methods are not documented, for they directly map one-to-one to those of the
/// Socket class.
/// </remarks>
type ISocket =
    abstract Available: int
    abstract IsBound: bool
    abstract Connected: bool

    abstract Bind: EndPoint -> unit
    abstract Connect: EndPoint -> unit
    abstract Listen: unit -> unit
    abstract Listen: int -> unit
    abstract Accept: unit -> ISocket

    abstract Close: unit -> unit
    abstract Shutdown: SocketShutdown -> unit
    
    abstract Send: byte array * int option * int option -> int
    abstract Receive: byte array * int option * int option -> int

    abstract BeginAccept: AsyncCallback * obj -> IAsyncResult
    abstract BeginSend: byte array * int * int * SocketFlags * AsyncCallback * obj -> IAsyncResult
    abstract BeginReceive: byte array * int * int * SocketFlags * AsyncCallback * obj -> IAsyncResult
    
    abstract EndAccept: IAsyncResult -> ISocket
    abstract EndSend: IAsyncResult -> int
    abstract EndReceive: IAsyncResult -> int
