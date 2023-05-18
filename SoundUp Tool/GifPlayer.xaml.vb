Option Explicit On

Public Class GifPlayer

    Private sTimer As Integer
    Public DoneYes As Boolean = False
    Private WindowMaxHeight As Double
    Private WindowMaxWidth As Double
    Private WindowMinHeight As Double
    Private WindowMinWidth As Double
    Private SizeRatio As Double

    Private Sub GifPlayerWindow_Loaded(sender As Object, e As RoutedEventArgs)
        Dim DispatcherTimer As New System.Windows.Threading.DispatcherTimer()
        AddHandler DispatcherTimer.Tick, AddressOf DispatcherTimer_Tick
        DispatcherTimer.Interval = New TimeSpan(0, 0, 1)
        DispatcherTimer.Start()

        Dim mMatrix As Matrix = PresentationSource.FromVisual(Application.Current.MainWindow).CompositionTarget.TransformToDevice
        Dim DubX As Double = mMatrix.M11
        Dim DubY As Double = mMatrix.M22

        Dim ScreenWidth As Double = System.Windows.SystemParameters.PrimaryScreenWidth * DubX
        Dim ScreenHeight As Double = System.Windows.SystemParameters.PrimaryScreenHeight * DubY

        SizeRatio = Math.Round(Me.MaxWidth / Me.MaxHeight, 2)

        Dim RelativeMaxHeight As Double = Math.Truncate(ScreenHeight / Me.MinHeight)
        Dim RelativeMaxWidth As Double = Math.Truncate(ScreenWidth / Me.MinWidth)

        If (RelativeMaxWidth > RelativeMaxHeight) Then
            RelativeMaxWidth = Me.MinWidth * RelativeMaxHeight
            RelativeMaxHeight = Me.MinHeight * RelativeMaxHeight
        Else
            RelativeMaxHeight = Me.MinHeight * RelativeMaxWidth
            RelativeMaxWidth = Me.MinWidth * RelativeMaxWidth
        End If

        WindowMaxHeight = Math.Truncate(RelativeMaxHeight / DubY)
        WindowMaxWidth = Math.Truncate(RelativeMaxWidth / DubX)
        WindowMinHeight = Me.MinHeight
        WindowMinWidth = Me.MinWidth

        sTimer = 0
    End Sub

    Private Sub DispatcherTimer_Tick(sender As Object, e As EventArgs)
        sTimer += 1
        TimerLabel.Content = GetTime(sTimer)
        CommandManager.InvalidateRequerySuggested()
    End Sub

    Public Function GetTime(Time As Integer) As String
        Dim nHour As Integer
        Dim nMin As Integer
        Dim nSec As Integer

        nSec = Time Mod 60
        nMin = ((Time - nSec) / 60) Mod 60
        nHour = ((Time - (nSec + (nMin * 60))) / 3600) Mod 60

        Return Format(nHour, "00") & ":" & Format(nMin, "00") & ":" & Format(nSec, "00")
    End Function

    Private Sub GifPlayerWindow_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        DragMove()
    End Sub

    Private Sub GifPlayerWindow_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs)
        If (Math.Truncate(Me.Height) >= WindowMaxHeight) AndAlso (Math.Truncate(Me.Width) >= WindowMaxWidth) Then
            Me.Height = WindowMinHeight
            Me.Width = WindowMinWidth
        Else
            Me.Height = WindowMaxHeight
            Me.Width = WindowMaxWidth
        End If
    End Sub

    Private Sub GifPlayerWindow_Closing(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Me.Closing
        If DoneYes = True Then
            Me.Close()
        Else
            e.Cancel = True
        End If
    End Sub

    Private Sub GifPlayerWindow_SizeChanged(sender As Object, e As SizeChangedEventArgs) Handles Me.SizeChanged, Me.SizeChanged
        GifPlayerWindow.Height = GifPicture.ActualHeight
        GifPlayerWindow.Width = GifPicture.ActualWidth
    End Sub
End Class