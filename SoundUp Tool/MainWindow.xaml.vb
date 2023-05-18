Option Explicit On
Imports System.IO
Imports System.Windows.Forms
Imports System.Xml
Imports System.Security
Imports System.Text

Imports SoundUp_Tool.INIReadWrite
Imports System.ComponentModel
Imports System.Threading

Class MainWindow

    Private INIFolderPath As String = Nothing
    Private XMLFolderPath As String = Nothing

    Private ConvertTo As String = "INI"
    Private XMlFile As New XmlDocument
    Private List_INIFilesPath As New List(Of String)
    Private List_FromXML_ININames As New List(Of String)
    Private List_FromINI_Items As New List(Of String)
    Private List_Items As New List(Of XmlNode)

    Private INITooltip As String = "The path to your INI files"
    Private XMLTooltip As String = "Click to select your XML File"
    Private INIText As String = "Click to select your INI File Directory"
    Private XMLText As String = "Click to select your XML File"
    Private CreateNew As Boolean = False
    Private Multiline As Boolean = False

    Private INISuccessful As Boolean = False
    Private XMLSuccessful As Boolean = False

    Private List_INIEntry As New List(Of INIEntry)

    Structure INIKey
        Public KeyName As String
        Public KeyValue As String
    End Structure

    Public Function New_INIKey(ByVal KeyName As String, ByVal KeyValue As String)
        Dim INIKey As New INIKey
        With INIKey
            .KeyName = KeyName
            .KeyValue = KeyValue
        End With
        Return INIKey
    End Function

    Structure INIEntry
        Public Path As String
        Public Filename As String
        Public Section As String
        Public Keylist As List(Of INIKey)
        Public Comment As String
    End Structure

    Public Function New_INIEntry(ByVal Path As String, ByVal Filename As String, ByVal Section As String, ByVal KeyList As List(Of INIKey), Optional ByVal Comment As String = vbNullString)
        Dim INIEntry As New INIEntry
        With INIEntry
            .Path = Path
            .Filename = Filename
            .Section = Section
            .Keylist = KeyList
            .Comment = Comment
        End With
        Return INIEntry
    End Function


    Private Sub ConvertToXML()
        INI_ListFactory()

        If XMLSuccessful = True Then
            MsgBox("Everything has been imported into the XML file", MsgBoxStyle.OkOnly, "XML Successful")
        End If
    End Sub


    Private Sub INI_ListFactory()

        Dim X As XmlNode = XMlFile.SelectSingleNode("Data")
        If X Is Nothing Then

            Dim settings As XmlWriterSettings = New XmlWriterSettings()
            settings.NewLineOnAttributes = True
            settings.Indent = True
            settings.IndentChars = ControlChars.Tab
            settings.Encoding = New UnicodeEncoding()

            Dim writer As XmlWriter = XmlWriter.Create(XMLFolderPath, settings)

            With writer
                .WriteStartDocument()
                .WriteComment("XML Database - Unicode")
                .WriteComment("SoundUP - Library - Version 1.0")
                .WriteComment("Author VampireMonkey - Contact http://www.nexusmods.com/games/users/4782829/")
                .WriteComment("Legal - SoundUP and this Tool are licensed under a Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License. Follow this link to learn more: http://creativecommons.org/licenses/by-nc-sa/4.0/")
                .WriteComment("Comment='Value' are now fully supported. When exporting and importing, comments will be overridden like any other value. Notice these are only the comments before a INI section, not inbetween key values.")
                .WriteStartElement("Data")
                .WriteEndElement()
                .WriteEndDocument()
            End With

            XMlFile.Save(writer)
            writer.Close()
            writer.Flush()
        End If

        ReadFromINIToList()

        If XMLTestLoad(XMLFolderPath) = True Then
            WriteToXMl()
        Else
            MsgBox("XML file is unreable. Parts might be missing or syntax poorly written.", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Error in file")
        End If

        List_INIEntry.Clear()
        List_FromINI_Items.Clear()

    End Sub


    Private Sub ReadFromINIToList()

        Debug.WriteLine("******ReadFromINIToList - Starting Parallel ForEach******")
        Parallel.ForEach(List_INIFilesPath,
                         Sub(XFile)
                             'Gets all section names
                             Dim SectionList As ArrayList = GetSections(XFile)

                             For Each SectionName As String In SectionList
                                 List_FromINI_Items.Add(SectionName)
                             Next

                             'Gets all section key names
                             For Each SectionName As String In List_FromINI_Items

                                 Dim XList As List(Of String) = GetKeys(XFile, SectionName)

                                 Dim ListKeys As New List(Of INIKey)

                                 For Each X As String In XList
                                     Dim KeyName As String = X
                                     Dim KeyValue As String = ReadINI(XFile, SectionName, X)
                                     Dim NewKey As INIKey = New_INIKey(KeyName, KeyValue)
                                     ListKeys.Add(NewKey)
                                 Next

                                 Dim NewEntry As INIEntry = New_INIEntry(XFile, Path.GetFileName(XFile), SectionName, ListKeys)
                                 'Final touch, add everything into a list
                                 List_INIEntry.Add(NewEntry)
                             Next

                             'Search file for comments and add them to each INIEntry
                             For iEntry = (List_INIEntry.Count - 1) To 0 Step -1
                                 Dim INIEntry As INIEntry = List_INIEntry(iEntry)
                                 Dim NameString As String = "[" + INIEntry.Section + "]"
                                 Dim FileString As ArrayList = New ArrayList(System.IO.File.ReadAllLines(XFile))
                                 Dim ExistingCommentString(0) As String

                                 For i = 0 To (FileString.Count - 1)
                                     If FileString(i).Contains(NameString) Then
                                         ReDim ExistingCommentString(0)
                                         Dim iBack As Integer = (i - 1)

                                         If iBack < 0 Then
                                             iBack = 0
                                         End If

                                         If FileString(iBack).StartsWith(";") Then
                                             For iStoredComment = 0 To (i - 1)
                                                 If FileString(iBack).StartsWith(";") Then
                                                     ReDim Preserve ExistingCommentString(iStoredComment)
                                                     ExistingCommentString(iStoredComment) = FileString(iBack)
                                                 Else
                                                     Exit For
                                                 End If

                                                 iBack -= 1
                                             Next
                                         End If
                                     End If
                                 Next

                                 Array.Reverse(ExistingCommentString)

                                 Dim CombinedString As String = String.Join(vbCr, ExistingCommentString, 0, ExistingCommentString.Length)

                                 Dim TempEntry As INIEntry = List_INIEntry(iEntry)
                                 TempEntry = New_INIEntry(TempEntry.Path, TempEntry.Filename, TempEntry.Section, TempEntry.Keylist, CombinedString)
                                 List_INIEntry.RemoveAt(iEntry)
                                 List_INIEntry.Insert(iEntry, TempEntry)
                             Next

                         End Sub)
    End Sub


    Private Sub WriteToXMl()

        XMlFile.Load(XMLFolderPath)

        Dim XBase As XmlNode = XMlFile.DocumentElement

        Debug.WriteLine("******WriteToXML- Starting Parallel ForEach******")
        'Parallel.ForEach(List_INIEntry, _
        '                Sub(Entry)
        For Each Entry As INIEntry In List_INIEntry
            Entry.Filename = AddValidation(Entry.Filename)

            Dim HostFile As XmlElement = XMlFile.SelectSingleNode("Data/" + Entry.Filename + "/UnOrganized")

            'If filename doesn't exist, create it
            If HostFile Is Nothing Then
                Dim NewNode As XmlElement = XMlFile.CreateElement(Entry.Filename)
                XBase.AppendChild(NewNode)

                Dim TempHost As XmlElement = XMlFile.SelectSingleNode("Data/" + Entry.Filename)
                Dim NewNode2 As XmlNode = XMlFile.CreateElement("UnOrganized")
                TempHost.AppendChild(NewNode2)

                Dim NewNodeElement As XmlElement = XMlFile.SelectSingleNode("Data/" + Entry.Filename)
                NewNodeElement.SetAttribute("FileToken", "True")

                Dim NewNodeElement2 As XmlElement = XMlFile.SelectSingleNode("Data/" + Entry.Filename + "/UnOrganized")

            End If

            Entry.Section = AddValidation(Entry.Section)

            Try
                Dim XHost As XmlElement = XMlFile.SelectSingleNode("Data/" + Entry.Filename + "/UnOrganized/" + Entry.Section)

                'Adds second layer and key
                If XHost Is Nothing Then
                    If (Entry.Keylist.Count > 0) Or (Entry.Comment.Length > 0) Then
                        Dim HostNode As XmlElement = XMlFile.SelectSingleNode("Data/" + Entry.Filename + "/UnOrganized")
                        Dim NewNode2 As XmlNode = XMlFile.CreateElement(Entry.Section)
                        HostNode.AppendChild(NewNode2)

                        'Sets XHost to the new node now
                        XHost = XMlFile.SelectSingleNode("Data/" + Entry.Filename + "/UnOrganized/" + Entry.Section)
                    End If
                End If

                If Not String.IsNullOrEmpty(Entry.Comment) Then
                    Dim Comment As String = Entry.Comment
                    Comment = SecurityElement.Escape(Comment)

                    XHost.SetAttribute("Comment", Comment)
                End If

                For Each XKey As INIKey In Entry.Keylist
                    Dim KeyName = XKey.KeyName
                    Dim KeyValue = XKey.KeyValue

                    KeyName = AddValidation(KeyName)
                    KeyValue = SecurityElement.Escape(KeyValue)

                    XHost.SetAttribute(KeyName, KeyValue)
                Next

            Catch Exc As System.Xml.XPath.XPathException
                MsgBox("Error Source: " + Entry.Section)
                'Catch Exc As NullReferenceException
                'Debug.WriteLine("******WriteToXML- NullReferenceException******")
            End Try
        Next
        'End Sub)

        'Adding indentations
        Dim settings As XmlWriterSettings = New XmlWriterSettings()

        If (Multiline = True) And (ConvertTo = "XML") Then
            settings.NewLineOnAttributes = True
        Else
            settings.NewLineOnAttributes = False
        End If

        settings.Indent = True
        settings.IndentChars = ControlChars.Tab
        settings.Encoding = New UnicodeEncoding()

        Dim writer As XmlWriter = XmlWriter.Create(XMLFolderPath, settings)

        XMlFile.Save(writer)
        writer.Close()
        writer.Flush()

        XMLSuccessful = True

    End Sub


    Private Function AddValidation(ByVal Name As String)
        Dim FirstCharSectionName As Char = Name.Substring(0, 1)

        If IsNumeric(FirstCharSectionName) Then
            Name = Name.Substring(0, 1).Replace(FirstCharSectionName, "Invalid_" + Name)
        End If

        Name = Name.Replace(" ", "___")
        Name = Name.Replace(":", ".COLON.")
        Name = SecurityElement.Escape(Name)

        Return Name
    End Function


    Private Sub ConvertToINI()
        Dim settings As XmlReaderSettings = New XmlReaderSettings()
        settings.IgnoreComments = True
        Dim reader As XmlReader = XmlReader.Create(XMLFolderPath, settings)

        If XMLTestLoad(XMLFolderPath) = False Then
            MsgBox("XML file is unreable. Parts might be missing or syntax poorly written.", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Error in file")
        Else
            XMlFile.Load(reader)

            Process(XMlFile)

            Buildlist_ININames()

            If (CheckIfFilesMatch() = True) Or (CreateNew = True) Then

                WriteToINI()

                List_Items.Clear()
                List_FromXML_ININames.Clear()

                If INISuccessful Then
                    MsgBox("Everything has been exported into the INI files", MsgBoxStyle.OkOnly, "INI Successful")
                End If

            Else
                MsgBox("No files exist", MsgBoxStyle.OkOnly, "Export Unsuccessful")
            End If
        End If
    End Sub

    Function CheckIfFilesMatch()
        For Each ININame As String In List_INIFilesPath
            ININame = IO.Path.GetFileName(ININame)

            For Each XML_ININame As String In List_FromXML_ININames
                If ININame.Equals(XML_ININame) Then
                    Return True
                End If
            Next
        Next

        Return False
    End Function

    Private Sub Buildlist_ININames()
        For Each X As XmlElement In XMlFile.DocumentElement
            If X.HasAttribute("FileToken") = "True" Then
                List_FromXML_ININames.Add(X.LocalName)
            End If
        Next
    End Sub

    Private Sub Process(node As XmlNode)
        Process(node, 0)
    End Sub


    Private Sub Process(node As XmlNode, level As Integer)
        For Each child As XmlNode In node.ChildNodes
            Process(child, level + 1)

            If child.Attributes IsNot Nothing Then
                For Each Attr As XmlAttribute In child.Attributes
                    If Attr.Name.Equals("FileToken") = False Then
                        List_Items.Add(child)
                    End If
                Next
            End If
        Next
    End Sub


    Private Sub WriteToINI()
        If CreateNew = False Then
            For Each INIFile As String In List_INIFilesPath
                For Each ININame As String In List_FromXML_ININames

                    Dim NewININame As String = RemoveValidation(ININame)
                    Dim list1 As XmlNodeList = XMlFile.DocumentElement.SelectSingleNode(ININame).SelectNodes(".//*")

                    If NewININame = IO.Path.GetFileName(INIFile) Then

                        For Each Y In list1

                            For Each Attr As XmlAttribute In Y.Attributes

                                If Not Attr.Name.Equals("Comment") Then

                                    'WriteINI(File, Section, Key, Value)
                                    WriteINI(INIFile, RemoveValidation(Y.LocalName), RemoveValidation(Attr.Name), Attr.Value)
                                End If
                            Next

                            If System.IO.File.Exists(INIFile) = True Then
                                For Each Attr As XmlAttribute In Y.Attributes
                                    If Attr.Name.Equals("Comment") Then

                                        Dim NameString As String = "[" + RemoveValidation(Y.LocalName) + "]"
                                        Dim FileString As ArrayList = New ArrayList(System.IO.File.ReadAllLines(INIFile))
                                        Dim AttrValue As String = Attr.Value

                                        For i = 0 To (FileString.Count - 1)
                                            If FileString(i).Contains(NameString) Then
                                                Dim iBack As Integer = (i - 1)

                                                If iBack < 0 Then
                                                    iBack = 0
                                                End If

                                                If AttrValue.Contains(";") Then
                                                    AttrValue = AttrValue.Replace(";", "")
                                                End If

                                                AttrValue = ";" + AttrValue
                                                AttrValue = AttrValue.Replace(vbCr, vbCr + ";")
                                                AttrValue = SecurityElement.Escape(AttrValue)

                                                If FileString(iBack).StartsWith(";") Then

                                                    Dim ExistingCommentString(0) As String

                                                    For iStoredComment = 0 To (i - 1)
                                                        If FileString(iBack).StartsWith(";") Then
                                                            ReDim Preserve ExistingCommentString(iStoredComment)
                                                            ExistingCommentString(iStoredComment) = FileString(iBack)
                                                        Else
                                                            Exit For
                                                        End If

                                                        iBack -= 1
                                                    Next

                                                    Array.Reverse(ExistingCommentString)

                                                    Dim CombinedString As String = String.Join(vbCr, ExistingCommentString, 0, ExistingCommentString.Length)

                                                    If Not (CombinedString = AttrValue) Then
                                                        FileString.RemoveRange(i - ExistingCommentString.Length, ExistingCommentString.Length)
                                                        FileString.Insert(i - ExistingCommentString.Length, AttrValue)
                                                    End If
                                                    Exit For

                                                    'If line above section does have text, override section and add comment and section again
                                                ElseIf (FileString(iBack) = "") = False Then
                                                    FileString(i) = Environment.NewLine + AttrValue + Environment.NewLine + NameString
                                                    Exit For

                                                    'If line above section does not have text, add comment only
                                                ElseIf (FileString(iBack) = "") = True Then
                                                    FileString(iBack) = Environment.NewLine + AttrValue
                                                    Exit For
                                                End If
                                            End If
                                        Next

                                        Dim SrWriter As New StreamWriter(INIFile)
                                        For Each SrLine As String In FileString
                                            SrWriter.WriteLine(SrLine)
                                        Next
                                        SrWriter.Close()

                                    End If
                                Next
                            End If

                        Next
                    End If
                Next
            Next

        ElseIf CreateNew = True Then

            For Each ININame As String In List_FromXML_ININames

                Dim NewININame As String = RemoveValidation(ININame)
                Dim FullINIPath As String = INIFolderPath + "/" + NewININame
                Dim list1 As XmlNodeList = XMlFile.DocumentElement.SelectSingleNode(ININame).SelectNodes(".//*")

                For Each Y In list1

                    For Each Attr As XmlAttribute In Y.Attributes
                        If Not Attr.Name.Equals("Comment") Then

                            'WriteINI(File, Section, Key, Value)
                            WriteINI(FullINIPath, RemoveValidation(Y.LocalName), RemoveValidation(Attr.Name), Attr.Value)
                        End If
                    Next

                    If System.IO.File.Exists(FullINIPath) = True Then
                        Dim LocalNameString As String = "[" + RemoveValidation(Y.LocalName) + "]"
                        Dim FilePathString() As String = System.IO.File.ReadAllLines(FullINIPath)

                        For i = 0 To (FilePathString.Length - 1)
                            If FilePathString(i).Contains(LocalNameString) Then

                                FilePathString(i) = Environment.NewLine + LocalNameString
                                Exit For
                            End If
                        Next
                        System.IO.File.WriteAllLines(FullINIPath, FilePathString)

                        For Each Attr As XmlAttribute In Y.Attributes
                            If Attr.Name.Equals("Comment") Then

                                Dim NameString As String = "[" + RemoveValidation(Y.LocalName) + "]"
                                Dim FileString() As String = System.IO.File.ReadAllLines(FullINIPath)

                                Dim AttrValue As String = Attr.Value

                                AttrValue = AttrValue.Replace(";", "")
                                AttrValue = ";" + AttrValue
                                AttrValue = AttrValue.Replace(vbCr, vbCr + ";")
                                AttrValue = SecurityElement.Escape(AttrValue)

                                For i = 0 To (FileString.Length - 1)
                                    If FileString(i).Contains(NameString) Then

                                        FileString(i) = AttrValue + Environment.NewLine + NameString
                                        Exit For
                                    End If
                                Next
                                System.IO.File.WriteAllLines(FullINIPath, FileString)
                            End If
                        Next
                    End If

                Next
            Next
        End If

        INISuccessful = True
    End Sub


    Private Function XMLTestLoad(Filepath As String) As Boolean
        Try
            Dim Xdoc As New XDocument()
            Xdoc = XDocument.Load(Filepath)
            Return True
        Catch exception As XmlException
            Return False
        End Try
    End Function

    Private Function RemoveValidation(ByVal Name As String)
        If Name.Length >= 8 Then
            If (Name.Substring(0, 8) = "Invalid_") = True Then
                Name = Name.Remove(0, 8)
            End If
        End If

        Name = Name.Replace("___", " ")
        Name = Name.Replace(".COLON.", ":")

        If ConvertTo = "XML" Then
            Name = SecurityElement.FromString(Name).Text()
        End If

        Return Name
    End Function

    'Start Button
    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        If (INIFolderPath IsNot Nothing) And (XMLFolderPath IsNot Nothing) Then
            If (CheckFilesExists() = True) Or (CreateNew = True) Then

                Dim YellowWorker As New Thread(
                    Sub()
                        Dim Gifp As New GifPlayer
                        Gifp.Owner = Me
                        Gifp.DoneYes = False
                        Gifp.Show()
                    End Sub)

                YellowWorker.SetApartmentState(ApartmentState.STA)
                YellowWorker.IsBackground = True
                YellowWorker.Start()

                Debug.WriteLine("YES")

                If ConvertTo = "INI" Then
                    ConvertToINI()
                ElseIf ConvertTo = "XML" Then
                    ConvertToXML()
                End If

                YellowWorker.Abort()

                'Gifp.DoneYes = True
                'Gifp.Close()

            Else
                MsgBox("There are no INI files in the directory you picked" + Environment.NewLine + "Check if files are readonly or in use.", MsgBoxStyle.Exclamation, "SoundUP Tool")
            End If
        Else
            If (INIFolderPath Is Nothing) And (XMLFolderPath IsNot Nothing) Then
                MsgBox("Please select a path to your INI files", MsgBoxStyle.Information, "SoundUP Tool")
            ElseIf (XMLFolderPath Is Nothing) And (INIFolderPath IsNot Nothing) Then
                MsgBox("Please select a XML file.", MsgBoxStyle.Information, "SoundUP Tool")
            ElseIf (XMLFolderPath Is Nothing) And (INIFolderPath Is Nothing) Then
                MsgBox("You haven't selected anything yet.", MsgBoxStyle.Information, "SoundUP Tool")
            End If
        End If
    End Sub

    Private Sub Button_Switch(sender As Object, e As RoutedEventArgs) Handles Btn_Switch.Click

        If ConvertTo = "INI" Then

            Btn_Switch.Content = "INI to XML"
            Btn_Switch.ToolTip = "Switch -> XML to INI"
            ConvertTo = "XML"
            ChBx_Multiline.IsEnabled = True

            If Not String.IsNullOrEmpty(XMLFolderPath) Then
                TxtB_INIPath.ToolTip = XMLFolderPath
                TxtB_INIPath.Text = XMLFolderPath
            Else
                TxtB_INIPath.ToolTip = XMLTooltip
                TxtB_INIPath.Text = XMLText
            End If

            If Not String.IsNullOrEmpty(INIFolderPath) Then
                TxtB_XMLPath.ToolTip = INIFolderPath
                TxtB_XMLPath.Text = INIFolderPath
            Else
                TxtB_XMLPath.ToolTip = INITooltip
                TxtB_XMLPath.Text = INIText
            End If

        ElseIf ConvertTo = "XML" Then

            Btn_Switch.Content = "XML to INI"
            Btn_Switch.ToolTip = "Switch -> INI to XML"
            ConvertTo = "INI"
            ChBx_Multiline.IsEnabled = False

            If Not String.IsNullOrEmpty(XMLFolderPath) Then
                TxtB_XMLPath.ToolTip = XMLFolderPath
                TxtB_XMLPath.Text = XMLFolderPath
            Else
                TxtB_XMLPath.ToolTip = XMLTooltip
                TxtB_XMLPath.Text = XMLText
            End If

            If Not String.IsNullOrEmpty(INIFolderPath) Then
                TxtB_INIPath.ToolTip = INIFolderPath
                TxtB_INIPath.Text = INIFolderPath
            Else
                TxtB_INIPath.ToolTip = INITooltip
                TxtB_INIPath.Text = INIText
            End If

        End If
    End Sub

    Private Function CheckFilesExists()
        Dim DirFilenames() As String = Nothing
        Dim INIextension As String = ".ini"
        Dim INIexists As Boolean = False

        DirFilenames = IO.Directory.GetFiles(INIFolderPath)

        For Each tempstring As String In DirFilenames
            If tempstring.Contains(INIextension) Then
                If ((GetAttr(tempstring) And vbReadOnly) = 0) And ((GetAttr(tempstring) And vbHidden) = 0) Then
                    If FileInUse(tempstring) = False Then
                        List_INIFilesPath.Add(tempstring)
                        INIexists = True
                    End If
                End If
            End If
        Next

        Return INIexists
    End Function

    Private Function FileInUse(FilePath As String)
        Try
            Dim Fs As FileStream = File.Open(FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None)
            Fs.Close()

        Catch Exception As IOException
            Return True
        End Try

        Return False
    End Function

    Private Sub TxtB_XmlPath_MouseDownClick(sender As Object, e As MouseButtonEventArgs)
        If ConvertTo = "INI" Then
            OpenFileDialog()
        ElseIf ConvertTo = "XML" Then
            OpenFolderBrowser()
        End If
    End Sub

    Private Sub TxtB_INIPath_MouseDownClick(sender As Object, e As MouseButtonEventArgs)
        If ConvertTo = "INI" Then
            OpenFolderBrowser()
        ElseIf ConvertTo = "XML" Then
            If CreateNew = False Then
                OpenFileDialog()
            Else
                SaveDialog()
            End If
        End If
    End Sub

    Public Sub SaveDialog()
        Dim SaveFileDialog1 As New SaveFileDialog
        SaveFileDialog1.RestoreDirectory = True
        SaveFileDialog1.Title = "Select Save Destination"
        SaveFileDialog1.Filter = "XMl file|*.xml"
        SaveFileDialog1.AddExtension = True
        SaveFileDialog1.FileName = "Tacitus"

        If SaveFileDialog1.ShowDialog() = Forms.DialogResult.OK Then
            XMLFolderPath = IO.Path.GetFullPath(SaveFileDialog1.FileName)

            If ConvertTo = "XML" Then
                TxtB_INIPath.Text = XMLFolderPath
                TxtB_INIPath.ToolTip = XMLFolderPath
            End If
        Else
            XMLFolderPath = Nothing
            If ConvertTo = "XML" Then
                TxtB_INIPath.Text = XMLText
                TxtB_INIPath.ToolTip = XMLTooltip
            End If
        End If
    End Sub

    Private Sub OpenFileDialog()
        Dim SelectFileDialog As New OpenFileDialog

        With SelectFileDialog
            .InitialDirectory = Environment.SpecialFolder.Desktop
            .FileName = Nothing
            .RestoreDirectory = True
            .Multiselect = False
            .Title = "Select A XML File To Use"
            .Filter = "XML file|*.xml"
        End With

        If SelectFileDialog.ShowDialog() = Forms.DialogResult.OK Then
            XMLFolderPath = IO.Path.GetFullPath(SelectFileDialog.FileName)

            If ConvertTo = "INI" Then
                TxtB_XMLPath.Text = XMLFolderPath
                TxtB_XMLPath.ToolTip = XMLFolderPath
            Else
                TxtB_INIPath.Text = XMLFolderPath
                TxtB_INIPath.ToolTip = XMLFolderPath
            End If
        Else
            XMLFolderPath = Nothing
            If ConvertTo = "INI" Then
                TxtB_XMLPath.Text = XMLText
                TxtB_XMLPath.ToolTip = XMLTooltip
            ElseIf ConvertTo = "XML" Then
                TxtB_INIPath.Text = XMLText
                TxtB_INIPath.ToolTip = XMLTooltip
            End If
        End If
    End Sub

    Private Sub OpenFolderBrowser()

        Dim App As Microsoft.Office.Interop.Excel.Application = New Microsoft.Office.Interop.Excel.Application()
        Dim Dialog As Microsoft.Office.Core.FileDialog = App.FileDialog(Microsoft.Office.Core.MsoFileDialogType.msoFileDialogFolderPicker)
        Dialog.InitialFileName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\"
        Dialog.AllowMultiSelect = False
        Dialog.ButtonName = "Select"

        If (CreateNew = True) And (ConvertTo = "INI") Then
            Dialog.Title = "Select the folder to save the exported INI files to"
        Else
            Dialog.Title = "Select the folder containing your INI files"
        End If

        If Dialog.Show() = -1 Then
            Dim SelectedItems As Microsoft.Office.Core.FileDialogSelectedItems = Dialog.SelectedItems
            Dim SelectedFolders As Array = SelectedItems.Cast(Of String).ToArray()

            If (SelectedFolders.Length > 0) Then
                INIFolderPath = SelectedFolders(0)

                If ConvertTo = "INI" Then
                    TxtB_INIPath.Text = INIFolderPath
                    TxtB_INIPath.ToolTip = INIFolderPath
                Else
                    TxtB_XMLPath.Text = INIFolderPath
                    TxtB_XMLPath.ToolTip = INIFolderPath
                End If
            End If

        Else
            INIFolderPath = Nothing
            If ConvertTo = "INI" Then
                TxtB_INIPath.Text = INIText
                TxtB_INIPath.ToolTip = INITooltip
            ElseIf ConvertTo = "XML" Then
                TxtB_XMLPath.Text = INIText
                TxtB_XMLPath.ToolTip = INITooltip
            End If
        End If
    End Sub

    Private Sub CheckBox_Checked(sender As Object, e As RoutedEventArgs)
        CreateNew = True
    End Sub

    Private Sub CheckBox_Unchecked(sender As Object, e As RoutedEventArgs)
        CreateNew = False
    End Sub

    Private Sub Label_MouseDown(sender As Object, e As MouseButtonEventArgs)
        Dim X As New AboutBoxForm
        X.ShowDialog()
    End Sub

    Private Sub CheckBox_ML_Checked(sender As Object, e As RoutedEventArgs)
        Multiline = True
    End Sub

    Private Sub CheckBox_ML_Unchecked(sender As Object, e As RoutedEventArgs)
        Multiline = False
    End Sub
End Class



