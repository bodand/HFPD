namespace HFPDd.Network

open System
open System.Net.Sockets

type SocketAdapter(inner: Socket) =
    interface IDisposable with
        member this.Dispose() = inner.Dispose()

    interface ISocket with
        member this.Available = inner.Available
        member this.Connected = inner.Connected
        member this.IsBound = inner.IsBound

        member this.Accept() = new SocketAdapter(inner.Accept())
        member this.Bind(ep) = inner.Bind(ep)
        member this.Connect(ep) = inner.Connect(ep)

        member this.Listen() = inner.Listen()
        member this.Listen(backlog) = inner.Listen(backlog)

        member this.Close() = inner.Close()
        member this.Shutdown(shutdown) = inner.Shutdown(shutdown)

        member this.Receive(buf, offset, count) =
            match (offset, count) with
            | None, None -> inner.Receive(buf)
            | None, Some count -> inner.Receive(buf, 0, count, SocketFlags.None)
            | Some offset, None -> inner.Receive(buf, offset, SocketFlags.None)
            | Some offset, Some count -> inner.Receive(buf, offset, count, SocketFlags.None)

        member this.Send(buf, offset, count) =
            match (offset, count) with
            | None, None -> inner.Send(buf)
            | None, Some count -> inner.Send(buf, 0, count, SocketFlags.None)
            | Some offset, None -> inner.Send(buf, offset, SocketFlags.None)
            | Some offset, Some count -> inner.Send(buf, offset, count, SocketFlags.None)

        member this.BeginAccept(callback, data) = inner.BeginAccept(callback, data)

        member this.BeginReceive(buf, offset, count, flags, callback, data) =
            inner.BeginReceive(buf, offset, count, flags, callback, data)

        member this.BeginSend(buf, offset, count, flags, callback, data) =
            inner.BeginSend(buf, offset, count, flags, callback, data)

        member this.EndAccept(result) =
            new SocketAdapter(inner.EndAccept(result))

        member this.EndReceive(result) = inner.EndReceive(result)
        member this.EndSend(result) = inner.EndSend(result)
