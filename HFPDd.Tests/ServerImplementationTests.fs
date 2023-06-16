module HFPDd.Tests.ServerImplementationTests

open System
open System.IO
open System.Threading.Tasks
open HFPDd.Network
open Xunit

[<Fact>]
let ``ServerImplementation's Run waits for Task`` () =
    use strm = new MemoryStream()

    use server =
        new ServerImplementation(
            task {
                let buf: byte[] = Array.create 999 69uy
                do! Task.Delay 350
                let! read = strm.WriteAsync(buf)
                return read
            },
            { new IDisposable with
                member this.Dispose() = () }
        )

    server.Run()
    Assert.All(strm.GetBuffer(), Action<byte>(fun byt -> Assert.Equal(byt, 69uy)))

[<Fact>]
let ``ServerImplementation's Dispose waits for Task`` () =
    use strm = new MemoryStream()

    let server : IDisposable =
        new ServerImplementation(
            task {
                let buf: byte[] = Array.create 999 69uy
                do! Task.Delay 350
                let! read = strm.WriteAsync(buf)
                return read
            },
            { new IDisposable with
                member this.Dispose() = () }
        )
    server.Dispose()
        
    Assert.All(strm.GetBuffer(), Action<byte>(fun byt -> Assert.Equal(byt, 69uy)))

[<Fact>]
let ``ServerImplementation's Dispose calls inner Dispose`` () =
    let mutable called = 0

    let server: IDisposable =
        new ServerImplementation(
            task {
                do! Task.Delay 350
            },
            { new IDisposable with
                member this.Dispose() = called <- called + 1; () }
        )
    server.Dispose()
        
    Assert.Equal(called, 1)
