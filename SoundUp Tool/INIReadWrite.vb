Option Explicit On

Imports Microsoft.VisualBasic
Imports System.Runtime.InteropServices
Imports System.Text


'For reading and writing to an Ini file
Public Class INIReadWrite

    <DllImport("kernel32.dll", SetLastError:=True)> _
    Private Shared Function GetPrivateProfileString(ByVal lpAppName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As StringBuilder, ByVal nSize As Integer, ByVal lpFileName As String) As Integer
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)> _
    Private Shared Function WritePrivateProfileString(ByVal lpAppName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Boolean
    End Function

    'Source.text = ReadINI(File, Section, Key)
    Public Shared Function ReadINI(ByVal File As String, ByVal Section As String, ByVal Key As String) As String
        Dim sb As New StringBuilder(500)
        GetPrivateProfileString(Section, Key, "", sb, sb.Capacity, File)
        Return sb.ToString
    End Function

    'WriteINI(File, Section, Key, Value)
    Public Shared Sub WriteINI(ByVal File As String, ByVal Section As String, ByVal Key As String, ByVal Value As String)
        WritePrivateProfileString(Section, Key, Value, File)
    End Sub

    <DllImport("kernel32.dll")> _
    Public Shared Function GetPrivateProfileSection(lpAppName As String, lpszReturnBuffer As Byte(), nSize As Integer, lpFileName As String) As Integer
    End Function

    Public Shared Function GetKeys(iniFile As String, category As String) As List(Of String)

        Dim buffer As Byte() = New Byte(20479) {}

        GetPrivateProfileSection(category, buffer, 20480, iniFile)
        Dim tmp As [String]() = Encoding.ASCII.GetString(buffer).Trim(ControlChars.NullChar).Split(ControlChars.NullChar)

        Dim result As New List(Of String)()

        For Each entry As [String] In tmp
            If entry.IndexOf("=") >= 0 Then
                result.Add(entry.Substring(0, entry.IndexOf("=")))
            End If
        Next

        Return result
    End Function

    <DllImport("kernel32.dll")> _
    Public Shared Function GetPrivateProfileSectionNames(lpszReturnBuffer As Byte(), nSize As Integer, lpFileName As String) As Integer
    End Function


    Public Shared Function GetSections(iniFile As String) As ArrayList

        Try
            Dim buffer(20480) As Byte
            GetPrivateProfileSectionNames(buffer, 20480, iniFile)
            Dim parts() As String = Encoding.ASCII.GetString(buffer).Trim(ControlChars.NullChar).Split(ControlChars.NullChar)
            Return New ArrayList(parts)
        Catch
        End Try
        Return Nothing

    End Function

End Class

'to use add "INIReadWrite" before the sub example "INIReadWrite.WriteINI"