---
note: This uses DFM syntax, will probably render with minor inconvenience on GitHub.
---

# HFPDd Plugins

The HFPDd plugin system allows extending, or rather defining, the behavior of the HFPD daemon: the protocol and action
plugins.
These are both loaded dynamically by the HFPD daemon.

## The Protocol plugins

The protocol subsystem allows specifying the protocols that HFPDd uses to communicate with the DaemonWarlock in the
HFPD application.
Arguably this is the less interesting of the two orientations, since the HDCP (HFPD Daemon Control Protocol) is designed
in a way to provide the most flexible system with the least overhead on the wire when data is sent to the daemon.

> [!NOTE]
> To read more about the HDCP protocol see [here](HDCP.md).

### Loading a protocol plugin

For this the [`IProtocol`](xref:HFPDd.Protocol.Protocol.IProtocol) F# interface needs to be implemented, along with a 
matching factory with the [`IProtocolFactory`](xref:HFPDd.Protocol.Protocol.IProtocolFactory) and finally one 
implementing the [`IProtocolInjector`](xref:HFPDd.Protocol.Protocol.IProtocolInjector) interface. The latter will be 
searched and given an [`IProtocolLoader`](xref:HFPDd.Protocol.Protocol.IProtocolLoader) class which can then be used to 
load the protocol into the HFPD daemon.

### A protocol's responsibilities

A protocol's responsibilities are basically to parse a given text retrieved from a TCP socket connection in chunks and 
produce an executable command from this data.

For this, the protocol may just pass back a work in progress notation with the current data and wait until a complete
parsable set has arrived. Otherwise with some trickery, it is not forbidden from not wasting memory, like the HDCP
protocol's implementation does and parsing the data in an online manner.

After it has finished with parsing a given request it is to produce a subclass of the 
[`ProtocolCommand`](xref:HFPDd.Core.ProtocolCommand) class which will then be executed by the runtime.
In this command class--and its creating--can the protocol try and query a given action through the provided
[`IActionRegistry`](xref:HFPDd.Protocol.IActionRegistry) interface, and use the received 
[`IAction`](xref:HFPDd.Protocol.Action.IAction) interface whenever the command is executed.

> [!IMPORTANT]
> There is no limitation about what the [`IAction`](xref:HFPDd.Protocol.Action.IAction) plugins can do, so it is 
> imperative not to call them haphazardly.
> The loader system performs the usually required security measures about dynamically loading code, so anything you can 
> load can be considered safe, but you don't want to accidentally start compiling some C++ project while the HFPD client 
> on the other side is waiting for some response.
> In other words, do not call Action plugins from a Protocol plugin directly, just pass it the 
> [`IAction`](xref:HFPDd.Protocol.Action.IAction) through the command object, and the system will call it when it can.

