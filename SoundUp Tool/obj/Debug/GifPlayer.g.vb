﻿#ExternalChecksum("..\..\GifPlayer.xaml","{406ea660-64cf-4c82-b6f0-42d48172a799}","C6F201E88D26020171857176475D0AED")
'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System
Imports System.Diagnostics
Imports System.Windows
Imports System.Windows.Automation
Imports System.Windows.Controls
Imports System.Windows.Controls.Primitives
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Forms.Integration
Imports System.Windows.Ink
Imports System.Windows.Input
Imports System.Windows.Markup
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Media.Effects
Imports System.Windows.Media.Imaging
Imports System.Windows.Media.Media3D
Imports System.Windows.Media.TextFormatting
Imports System.Windows.Navigation
Imports System.Windows.Shapes
Imports System.Windows.Shell
Imports XamlAnimatedGif


'''<summary>
'''GifPlayer
'''</summary>
<Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>  _
Partial Public Class GifPlayer
    Inherits System.Windows.Window
    Implements System.Windows.Markup.IComponentConnector
    
    
    #ExternalSource("..\..\GifPlayer.xaml",5)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents GifPlayerWindow As GifPlayer
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\GifPlayer.xaml",39)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents GifPicture As System.Windows.Controls.Viewbox
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\GifPlayer.xaml",43)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents TimerLabel As System.Windows.Controls.Label
    
    #End ExternalSource
    
    Private _contentLoaded As Boolean
    
    '''<summary>
    '''InitializeComponent
    '''</summary>
    <System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")>  _
    Public Sub InitializeComponent() Implements System.Windows.Markup.IComponentConnector.InitializeComponent
        If _contentLoaded Then
            Return
        End If
        _contentLoaded = true
        Dim resourceLocater As System.Uri = New System.Uri("/SoundUp Tool;component/gifplayer.xaml", System.UriKind.Relative)
        
        #ExternalSource("..\..\GifPlayer.xaml",1)
        System.Windows.Application.LoadComponent(Me, resourceLocater)
        
        #End ExternalSource
    End Sub
    
    <System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0"),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never),  _
     System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes"),  _
     System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity"),  _
     System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")>  _
    Sub System_Windows_Markup_IComponentConnector_Connect(ByVal connectionId As Integer, ByVal target As Object) Implements System.Windows.Markup.IComponentConnector.Connect
        If (connectionId = 1) Then
            Me.GifPlayerWindow = CType(target,GifPlayer)
            
            #ExternalSource("..\..\GifPlayer.xaml",15)
            AddHandler Me.GifPlayerWindow.Loaded, New System.Windows.RoutedEventHandler(AddressOf Me.GifPlayerWindow_Loaded)
            
            #End ExternalSource
            
            #ExternalSource("..\..\GifPlayer.xaml",16)
            AddHandler Me.GifPlayerWindow.MouseDoubleClick, New System.Windows.Input.MouseButtonEventHandler(AddressOf Me.GifPlayerWindow_MouseDoubleClick)
            
            #End ExternalSource
            
            #ExternalSource("..\..\GifPlayer.xaml",17)
            AddHandler Me.GifPlayerWindow.MouseLeftButtonDown, New System.Windows.Input.MouseButtonEventHandler(AddressOf Me.GifPlayerWindow_MouseLeftButtonDown)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 2) Then
            Me.GifPicture = CType(target,System.Windows.Controls.Viewbox)
            Return
        End If
        If (connectionId = 3) Then
            Me.TimerLabel = CType(target,System.Windows.Controls.Label)
            Return
        End If
        Me._contentLoaded = true
    End Sub
End Class

