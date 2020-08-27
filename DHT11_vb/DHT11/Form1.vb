Imports System.IO.Ports
Imports System.IO

Public Class Form1

    Dim tempHumid() As String
    Dim receivedData As String = ""
    Dim tempUnits() As String = {"°C", "°F"}
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'nom de la fenetre
        Me.Text = "DHT11 DATA RECEIVE"
        Label10.Visible = False
        Label11.Visible = False

        'texte du combobox
        ComboBox1.Text = "Temp Units"

        'remplir le combobox avec un tabeau de string
        For i As Byte = 0 To tempUnits.Length - 1
            ComboBox1.Items.Add(tempUnits(i))
        Next
        'le timer execute les instruction qui sont en son sein selon son timing
        'ici il est utilise pour recevoir les donnees du port serie chaque une seconde
        'comme le blinking dans arduino
        Timer1.Enabled = False
        Try
            'on essaie de fermer le port s'il existe avant de l'ouvrir
            SerialPort1.Close()

        Catch ex As Exception
            MsgBox("Port doesn't exist")
        Finally
            'les configurations necessaires pour faire une communication serie
            SerialPort1.PortName = "COM4"
            SerialPort1.BaudRate = "9600"
            SerialPort1.DataBits = 8
            SerialPort1.Parity = Parity.None
            SerialPort1.StopBits = StopBits.One
            SerialPort1.Handshake = Handshake.None
            SerialPort1.Encoding = System.Text.Encoding.Default
            SerialPort1.ReadTimeout = 10000

            Try
                'ouvrir le port s'il existe et lancer le timer
                SerialPort1.Open()
                Timer1.Enabled = True
            Catch ex As Exception
                MsgBox("Port doesn't exist")
                Timer1.Stop()
                Label10.Text = "no data..."
                Label11.Text = "no data..."
                Label6.Visible = False
                Label7.Visible = False

            End Try

        End Try

    End Sub


    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        receivedData = "nothing"
        Try

            Try
                'lecture du buffer du port serie et lever une exception si le port est debranche
                receivedData = SerialPort1.ReadExisting()
            Catch ex As InvalidOperationException
                MsgBox("Port doesn't exit")
            End Try

            If receivedData Is Nothing Then
                Label10.Visible = True
                Label11.Visible = True
                Label10.Text = "transfering data..."
                Label11.Text = "transfering data..."
                Label5.Visible = False
                Label6.Visible = False
                Label7.Visible = False
                Label8.Visible = False
            Else

                tempHumid = receivedData.Split(",")
                Try

                    If ComboBox1.SelectedIndex = 1 Then
                        Label10.Visible = False
                        Label11.Visible = False
                        Label8.Visible = True
                        Label8.Text = CStr((tempHumid(0) * 1.8 + 32))
                    Else
                        Label8.Visible = True
                        Label8.Text = tempHumid(0)
                    End If
                    Label5.Visible = True
                    Label5.Text = tempHumid(1)
                    Label6.Visible = True
                    Label7.Visible = True
                    Label10.Visible = False
                    Label11.Visible = False
                Catch ex As IndexOutOfRangeException
                    Label10.Visible = True
                    Label11.Visible = True
                    Label10.Text = "transfering data..."
                    Label11.Text = "transfering data..."
                    Label5.Visible = False
                    Label8.Visible = False
                    Label6.Visible = False
                    Label7.Visible = False

                End Try

            End If

        Catch ex As TimeoutException
            MsgBox("timeout reading")

        End Try


    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged

        'handle selectedItemChanged
        Label6.Text = ComboBox1.SelectedItem

    End Sub

    Private Sub Label9_Click(sender As Object, e As EventArgs) Handles Label9.Click

    End Sub
End Class
