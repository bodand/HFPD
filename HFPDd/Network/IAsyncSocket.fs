namespace HFPDd.Network

open System.Net.Sockets

type IAsyncSocket =
    abstract AsyncAccept: unit -> Async<IAsyncSocket>
    abstract AsyncReceive: byte[] * int option * int option -> Async<int>
    abstract AsyncSend: byte[] * int option * int option -> Async<int>
    
    abstract Shutdown: SocketShutdown -> unit
    abstract Close: unit -> unit
