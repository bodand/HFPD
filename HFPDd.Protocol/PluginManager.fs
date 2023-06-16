namespace HFPDd.Protocol

open HFPDd.Protocol.Action
open HFPDd.Protocol.Protocol

/// <summary>
///     The class implementing the plugin management needs of the project.
/// </summary>
/// <remarks>
///     <para>
///         The class contains the two mappings between string identifiers and IProtocolFactory-es and IActionFactory-es.
///         Whenever a request is given for a protocol/action of a given id, if there exists a factory for it in the
///         registry, it will be used to construct it.
///     </para>
/// </remarks>
type PluginManager() =
    let mutable protocolMapping = Map<string, IProtocolFactory> []
    let mutable actionMapping = Map<string, IActionFactory> []

    interface IProtocolLoader with
        member this.AddFactory proto fact =
            protocolMapping <- protocolMapping.Add(proto, fact)

    interface IActionLoader with
        member this.AddFactory action fact =
            actionMapping <- actionMapping.Add(action, fact)

    interface IProtocolRegistry with
        member this.GetProtocol(pid) =
            protocolMapping.TryFind pid |> Option.map (fun s -> s.BuildProtocol(this))

    interface IActionRegistry with
        member this.GetAction(aid) =
            actionMapping.TryFind aid |> Option.map (fun s -> s.BuildAction())
