Option Explicit On


Public Class AboutBoxForm

    Dim UpdateTime As Integer = 120
    Dim TibLevel As Integer = 0

    Private Sub AboutBoxForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        About_Timer1.Enabled = True
    End Sub

    Private Sub About_Timer1_Tick(sender As Object, e As EventArgs) Handles About_Timer1.Tick

        If UpdateTime = 0 Then
            If TibLevel = 1 Then
                LogoPictureBox.Image = My.Resources.E2
            ElseIf TibLevel = 2 Then
                LogoPictureBox.Image = My.Resources.E3
                About_Timer1.Enabled = False
            End If
            TibLevel += 1
            UpdateTime = 120
        Else
            UpdateTime -= 1
        End If
    End Sub


    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles About_OKButton.Click
        Me.Close()
    End Sub

    Private Sub LabelProductName_Click(sender As Object, e As EventArgs) Handles LabelProductName.Click
        Dim Contact As String = "http://www.nexusmods.com/skyrim/users/4782829/?"
        Process.Start(Contact)
    End Sub


End Class
