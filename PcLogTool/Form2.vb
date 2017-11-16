'mysqlを使う
Imports MySql.Data.MySqlClient

Public Class User_Register_Form
    Dim Con As New MySqlConnection

    'ログインユーザ名取得
    Dim username As String = System.Environment.UserName
    'マシン名取得
    Dim userspc As String = System.Environment.MachineName
    '現在の日付を取得する
    Dim dtToday As DateTime = DateTime.Today

    'Mysql設定インスタンスを作成する
    '    Dim mysqlManage As New MysqlManage()

    '企業名設定インスタンスを作成する
    Dim CompanyDao As New CompanyDao(Con)

    Dim companyNameBoxManage As New CompanyNameBox()

    Private Sub User_Register_Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Con.ConnectionString = MysqlManage.Connect()
        'user名をデフォルトで入力させておく
        Label3.Text = username
        'PC名をデフォルトで入力させておく
        Label4.Text = userspc
        '企業名コンボボックスに初期値を入れておく
        companyNameBoxDefaultValue()

    End Sub

    '企業名コンボボックスに初期値を入れておく処理
    Function companyNameBoxDefaultValue()
        '        Dim Con As New MySqlConnection
        Dim result As String
        Dim resultArray As New ArrayList
        Dim dr As MySqlDataReader

        Try
            Console.WriteLine("companyNameBoxDefaultValue start")

            '            Con.ConnectionString = MysqlManage.Connect()

            Dim cmd As MySqlCommand

            Dim sqlStr As String

            '接続 
            Con.Open()
            'SQL文 
            sqlStr = "SELECT id,company_name FROM company"

            'MySQLCommand作成 
            cmd = New MySqlCommand(sqlStr, Con)

            'SQL文実行 
            dr = cmd.ExecuteReader

            Dim id As String
            Dim companyName As String

            While dr.Read()
                id = CStr(dr("id"))
                companyName = dr("company_name")
                Console.WriteLine(id, companyName)
                companyNameBoxManage.setComapanyId(id, companyName)
                companyNameBox.Items.Add(CompanyName)
            End While

            result = True
        Catch ex As Exception
            Console.WriteLine(ex.ToString())
            MsgBox("企業名リスト取得に失敗しました。")
            result = False
        Finally
            Console.WriteLine("companyNameBoxDefaultValue　end")
        End Try
        Return result
    End Function

    Private Sub register_submit_buttom_Click(sender As Object, e As EventArgs) Handles register_submit_buttom.Click
        Try
            Dim companyName As String
            Dim companyId As Integer
            MsgBox("ユーザー情報を登録します。")
            'フォームで選択された企業名
            companyName = companyNameBox.SelectedItem
            'フォームで選択された企業名から企業ＩＤを取得
            companyId = companyNameBoxManage.getComapanyIdByName(companyName)
            Console.WriteLine("companyId:" & companyId)
            'ユーザー名と企業ＩＤをmysqlに保存
            insertUser(companyId)
            Dim form As New Form1()
            Dim form2 As New User_Register_Form()

            form2.Close()
            MsgBox("ユーザー情報を登録しました")
            form.ShowDialog(Me)
        Catch ex As Exception
            MsgBox("ユーザー情報に失敗しました")
        Finally

        End Try
    End Sub

    Function insertUser(companyId)
        Console.WriteLine("insertUser")
        '       Dim Con As New MySqlConnection
        Try
            '            Con.ConnectionString = MysqlManage.Connect()
            Con.Open()
            'pclog_idを使ってファイルを格納
            Dim myInsertQuery As String = "INSERT INTO `user`(`user_name`, `machine_name`,`company_id`) VALUES(?val5,?val6,?val7)"
            Dim myCommand As New MySqlCommand(myInsertQuery, Con)
            'pclog_id
            myCommand.Parameters.AddWithValue("?val5", Label3.Text)
            '
            myCommand.Parameters.AddWithValue("?val6", Label4.Text)
            '企業ＩＤを格納
            myCommand.Parameters.AddWithValue("?val7", companyId)
            Console.WriteLine(myInsertQuery)
            Return myCommand.ExecuteNonQuery()

        Catch ex As Exception
            Console.WriteLine(ex)
        Finally
            Console.WriteLine("insertUser end")
            Con.Close()
            'SQLを実行します

        End Try

        Return True

    End Function



    Function InsertCompany()
        '        Dim Con As New MySqlConnection
        '        Con.ConnectionString = MysqlManage.Connect()
        Con.Open()
        'pclog_idを使ってファイルを格納
        Dim myInsertQuery As String = "INSERT INTO `company`(`company_name`) VALUES(?val1)"
        Dim myCommand As New MySqlCommand(myInsertQuery, Con)

        'pclog_id
        myCommand.Parameters.AddWithValue("?val1", companyNameBox.SelectedItem)

        'SQLを実行します
        myCommand.ExecuteNonQuery()

        Con.Close()
        Return True
    End Function

    Private Sub box_username_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub companyNameBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles companyNameBox.SelectedIndexChanged
    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click

    End Sub
End Class
