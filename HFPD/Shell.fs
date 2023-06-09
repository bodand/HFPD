namespace HFPD

module Shell =
    open Avalonia
    open Avalonia.Input
    open Avalonia.Controls
    open Avalonia.FuncUI
    open Avalonia.FuncUI.DSL
    open Avalonia.FuncUI.Hosts

    let view =
        Component(fun _ ->
            DockPanel.create [
                DockPanel.children [
                ]
            ]
        )

    /// This is the main window of your application
    /// you can do all sort of useful things here like setting heights and widths
    /// as well as attaching your dev tools that can be super useful when developing with
    /// Avalonia
    type MainWindow() as this =
        inherit HostWindow()
        do
            base.Title <- "Full App"
            base.Width <- 800.0
            base.Height <- 600.0
            base.MinWidth <- 800.0
            base.MinHeight <- 600.0
            this.Content <- view

            //this.VisualRoot.VisualRoot.Renderer.DrawFps <- true
            //this.VisualRoot.VisualRoot.Renderer.DrawDirtyRects <- true

