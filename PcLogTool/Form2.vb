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

    Private Sub User_Register_Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'PC名をデフォルトで入力させておく
        Label4.Text = userspc
        '企業名コンボボックスに初期値を入れておく
        companyNameBoxDefaultValue()

    End Sub

    '企業名コンボボックスに初期値を入れておく
    Function companyNameBoxDefaultValue()
        Dim Con As New MySqlConnection
        Dim result As String
        Dim resultArray As New ArrayList
        Con.ConnectionString = mysqlManage.Connect()

        Try
            Console.WriteLine("companyNameBoxDefaultValue start")
            Dim item As CompanyNameBox

            'SQL文実行 
            resultArray = CompanyDao.getCompanyInfo

            '結果を表示 
            For Each value As String In resultArray
                Console.WriteLine(value)
                item = New CompanyNameBox(value)
                companyNameBox.Items.Add(value)
            Next
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
        companyId = CompanyDao.getCompanyId(companyName)
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
