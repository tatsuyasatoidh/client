Imports System.Runtime.InteropServices
'mysqlを使う
Imports MySql.Data.MySqlClient


Public Class User_Register_Form
    'ログインユーザ名取得
    Dim username As String = System.Environment.UserName
    'マシン名取得
    Dim userspc As String = System.Environment.MachineName
    '現在の日付を取得する
    Dim dtToday As DateTime = DateTime.Today

    Private Sub User_Register_Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label3.Text = username
        Label4.Text = userspc
    End Sub

    Private Sub register_submit_buttom_Click(sender As Object, e As EventArgs) Handles register_submit_buttom.Click

        ' 1.接続文字列を作成する
        Dim Builder = New MySqlConnectionStringBuilder()
        ' データベースに接続するために必要な情報をBuilderに与える
        Builder.Server = "localhost"
        Builder.Port = 3306
        Builder.UserID = "root"
        Builder.Database = "pclog"
        Dim ConStr = Builder.ToString()

        ' 2　　.データベースに接続するためのコネクションを準備して、実際につなぐ
        Dim Con As New MySqlConnection
        Con.ConnectionString = ConStr
        Con.Open()


        'pclog_idを使ってファイルを格納
        Dim myInsertQuery As String = "INSERT INTO `user`(`user_name`, `machine_name`) VALUES(?val5,?val6)"
        Dim myCommand As New MySqlCommand(myInsertQuery, Con)

        'pclog_id
        myCommand.Parameters.AddWithValue("?val5", Label3.Text)
        'logファイル
        myCommand.Parameters.AddWithValue("?val6", Label4.Text)

        'SQLを実行します
        myCommand.ExecuteNonQuery()

        ' 6.データベースの切断
        Con.Close()

        Dim form As New Form1()
        form.ShowDialog(Me)

    End Sub

End Class
