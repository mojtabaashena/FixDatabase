Imports System.Data.OleDb
Public Class Form1

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim strCn As String = txtConnectionString.Text

        If IO.File.Exists(Application.StartupPath + "\cn.dll") Then
            strCn = IO.File.ReadAllText(Application.StartupPath + "\cn.dll")
            txtConnectionString.Text = "read from file"
        End If

        Dim connection As New OleDbConnection(strCn)
        Dim cmd As New OleDbCommand("", connection)

        connection.Open()
        Dim strErrorReport As String = ""
        Dim intCount As Integer = 0

        For Each str As String In txtQuery.Text.Split(New String() {vbCr & vbLf & "GO" & vbCr & vbLf}, StringSplitOptions.RemoveEmptyEntries)
            Try
                If str.Contains("@Year@") Then
                    Try
                        cmd.CommandText = str.Replace("@Year@", "_Year")
                        cmd.ExecuteNonQuery()
                    Catch ex As Exception
                        strErrorReport += intCount.ToString() & ": [ " & cmd.CommandText.Substring(0, 30) & " ]" & ex.Message & vbCr & vbLf
                    End Try
                End If

                cmd.CommandText = str.Replace("@Year@", "")
                strErrorReport += intCount.ToString() & ": (" & cmd.ExecuteNonQuery().ToString() & " rows affected)" & vbCr & vbLf
            Catch ex As Exception
                strErrorReport += intCount.ToString() & ": [ " & str.Substring(0, If(str.Length < 31, str.Length - 1, 30)) & " ]" & ex.Message & vbCr & vbLf
            End Try
            intCount += 1
        Next

        TextBox1.Text = strErrorReport
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If IO.File.Exists(Application.StartupPath + "\Query.txt") Then
            txtQuery.Text = IO.File.ReadAllText(Application.StartupPath + "\Query.txt")
        End If
        If IO.File.Exists(Application.StartupPath + "\cn.dll") Then
            txtConnectionString.Text = "read from file"
        End If
    End Sub


End Class
