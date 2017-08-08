'mysqlを使う
Imports MySql.Data.MySqlClient

Friend Class PcLogDao

    'Mysql設定インスタンスを作成する
    Dim mysqlManage As New MysqlManage()
    'Userのインスタンス
    Dim UserManage As New User()

    Public Sub New()
    End Sub

    '作業量を取得
    '@param ユーザーＩＤ　今日の日付
    '@Return INT
    Public Function GetWorkCount(ByVal userId As String, ByVal datetime As String)

        Dim result As Integer

        Dim Con As New MySqlConnection
        Con.ConnectionString = mysqlManage.Connect()
        Con.Open()

        ' 3.発行するSQL文を作成する
        Dim SqlStr As String
        SqlStr = "SELECT * FROM pclog WHERE user_id ='" + userId + "' and date='" + datetime + "'"
        ' 4.データ取得のためのアダプタの設定
        Dim Adapter = New MySqlDataAdapter(SqlStr, Con)
        Console.Write(SqlStr)
        ' 5.SQL文実行/gridに反映
        Dim Dss As New DataSet
        Adapter.Fill(Dss)

        If Dss.Tables(0).Rows.Count > 0 Then
            Console.WriteLine("check_today_work end")
            result = Dss.Tables(0).Rows(0)("number_of_work")
        Else
            result = 0
        End If
        ' 6.データベースの切断
        Con.Close()
        Return result
    End Function

    '今日の作業量を新しく格納
    '@param 作業量
    Public Sub Insertlog(ByVal work As Integer)
        '現在の日付を取得する
        Dim dtToday As DateTime = DateTime.Today
        Dim userId As Integer = UserManage.getId()

        ' 2.データベースに接続するためのコネクションを準備して、実際につなぐ
        Dim Con As New MySqlConnection
        Con.ConnectionString = mysqlManage.Connect()
        Con.Open()

        ' 3.発行するSQL文を作成する
        Dim myInsertQuery As String = "INSERT INTO `pclog` (`date`, `user_id`,`number_of_work`) VALUES (?val1, ?val2,?val3)"
        Dim myCommand As New MySqlCommand(myInsertQuery, Con)
        'プレースホルダーにバインドします。
        '操作日
        myCommand.Parameters.AddWithValue("?val1", dtToday.ToString("yyyy-MM-dd"))
        'user_id
        myCommand.Parameters.AddWithValue("?val2", userId)
        '作業数
        myCommand.Parameters.AddWithValue("?val3", work)

        'SQLを実行します
        myCommand.ExecuteNonQuery()

        ' 6.データベースの切断
        Con.Close()

    End Sub

    '今日の作業量を更新
    '@param 作業量
    Public Sub Updatelog(ByVal work As Integer)
        '現在の日付を取得する
        Dim dtToday As DateTime = DateTime.Today
        Dim userId As Integer = UserManage.getId()

        ' 2.データベースに接続するためのコネクションを準備して、実際につなぐ
        Dim Con As New MySqlConnection
        Con.ConnectionString = mysqlManage.Connect()
        Con.Open()

        ' 3.発行するSQL文を作成する
        Dim myInsertQuery As String = "UPDATE `pclog` Set `number_of_work`=?val1 WHERE `id`=?val2 And `Date`=?val3"
        Dim myCommand As New MySqlCommand(myInsertQuery, Con)
        'プレースホルダーにバインドします。
        '操作日
        myCommand.Parameters.AddWithValue("?val1", work)
        'user_id
        myCommand.Parameters.AddWithValue("?val2", userId)
        '作業数
        myCommand.Parameters.AddWithValue("?val3", dtToday.ToString("yyyy-MM-dd"))

        'SQLを実行します
        myCommand.ExecuteNonQuery()

        ' 6.データベースの切断
        Con.Close()

    End Sub

End Class
