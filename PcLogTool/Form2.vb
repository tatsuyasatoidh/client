'mysqlを使う
Imports MySql.Data.MySqlClient

Public Class User_Register_Form
    'ログインユーザ名取得
    Dim username As String = System.Environment.UserName
    'マシン名取得
    Dim userspc As String = System.Environment.MachineName
    '現在の日付を取得する
    Dim dtToday As DateTime = DateTime.Today

    'Mysql設定インスタンスを作成する
    Dim mysqlManage As New MysqlManage()

    '企業名設定インスタンスを作成する
    Dim CompanyDao As New CompanyDao()

    Dim companyNameBoxManage As New CompanyNameBox()

    Private Sub User_Register_Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'PC名をデフォルトで入力させておく
        Label4.Text = userspc
        '企業名コンボボックスに初期値を入れておく
        companyNameBoxDefaultValue()

    End Sub

    '企業名コンボボックスに初期値を入れておく処理
    Function companyNameBoxDefaultValue()
        Dim Con As New MySqlConnection
        Dim result As String
        Dim resultArray As New ArrayList
        Dim dr As MySqlDataReader
        Con.ConnectionString = mysqlManage.Connect()

        Try
            Console.WriteLine("companyNameBoxDefaultValue start")

            Con.ConnectionString = mysqlManage.Connect()

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

            While dr.Read()
                Dim id As String = CStr(dr("id"))
                Dim companyName As String = dr("company_name")
                Console.WriteLine(id, CompanyName)
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

        Dim companyName As String
        Dim companyId As Integer

        'フォームで選択された企業名
        companyName = companyNameBox.SelectedItem
        'フォームで選択された企業名から企業ＩＤを取得
        companyId = companyNameBoxManage.getComapanyIdByName(companyName)
        Console.WriteLine("companyId:" & companyId)
        'ユーザー名と企業ＩＤをmysqlに保存
        insertUser(companyId)

        Dim form As New Form1()
        form.ShowDialog(Me)

    End Sub

    Function insertUser(companyId)
        Console.WriteLine("insertUser")
        Dim Con As New MySqlConnection
        Try
            Con.ConnectionString = mysqlManage.Connect()
            Con.Open()
            'pclog_idを使ってファイルを格納
            Dim myInsertQuery As String = "INSERT INTO `user`(`user_name`, `machine_name`,`company_id`) VALUES(?val5,?val6,?val7)"
            Dim myCommand As New MySqlCommand(myInsertQuery, Con)
            'pclog_id
            myCommand.Parameters.AddWithValue("?val5", box_username.Text)
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
        Dim Con As New MySqlConnection
        Con.ConnectionString = mysqlManage.Connect()
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

    Private Sub box_username_TextChanged(sender As Object, e As EventArgs) Handles box_username.TextChanged

    End Sub

    Private Sub companyNameBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles companyNameBox.SelectedIndexChanged
    End Sub
End Class
