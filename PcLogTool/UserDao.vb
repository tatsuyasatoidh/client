'mysqlを使う
Imports MySql.Data.MySqlClient

Public Class UserDao

    'Mysql設定インスタンスを作成する
    '    Dim mysqlManage As New MysqlManage()

    Public Function GetAll(MachineUserName, MachineName)
        Dim Info As Hashtable = New Hashtable
        ' 2.データベースに接続するためのコネクションを準備して、実際につなぐ
        Dim Con As New MySqlConnection
        Con.ConnectionString = MysqlManage.Connect()
        Con.Open()

        ' 3.発行するSQL文を作成する
        Dim SqlStr As String
        SqlStr = "SELECT * FROM user WHERE user_name ='" + MachineUserName + "' and machine_name='" + MachineName + "'"
        ' 4.データ取得のためのアダプタの設定
        Dim Adapter = New MySqlDataAdapter(SqlStr, Con)

        ' 5.SQL文実行/gridに反映
        Dim Ds As New DataSet
        Adapter.Fill(Ds)

        If Ds.Tables(0).Rows.Count > 0 Then
            Info("id") = Ds.Tables(0).Rows(0)("id")
            Info("company_id") = Ds.Tables(0).Rows(0)("company_id")
            Return Info
        Else
            Return ""
        End If
        ' 6.データベースの切断
        Con.Close()
    End Function

    'Userテーブルにデータを格納する'
    Function InsertUser(companyId, username)
        Console.WriteLine("InsertUser")
        Dim Con As New MySqlConnection
        Dim machineName As String
        Try
            Con.ConnectionString = MysqlManage.Connect()
            Con.Open()

            machineName = System.Environment.MachineName

            'pclog_idを使ってファイルを格納
            Dim myInsertQuery As String = "INSERT INTO `user`(`user_name`, `machine_name`,`company_id`) VALUES(?val5,?val6,?val7)"
            Dim myCommand As New MySqlCommand(myInsertQuery, Con)
            'pclog_id
            myCommand.Parameters.AddWithValue("?val5", username)
            '
            myCommand.Parameters.AddWithValue("?val6", machineName)
            '企業ＩＤを格納
            myCommand.Parameters.AddWithValue("?val7", companyId)
            Console.WriteLine(myInsertQuery)
            Return myCommand.ExecuteNonQuery()

        Catch ex As Exception
            Console.WriteLine(ex)
        Finally
            Console.WriteLine("InsertUser end")
            Con.Close()
            'SQLを実行します
        End Try
        Return True

    End Function



    'レコードが存在するかを確認する関数
    '@param 端末情報　端末ユーザー名
    '@return boolean
    Public Function chkRecord(MachineUserName, MachineName)
        Dim result As Boolean
        ' 2.データベースに接続するためのコネクションを準備して、実際につなぐ
        Dim Con As New MySqlConnection
        Con.ConnectionString = MysqlManage.Connect()
        Con.Open()

        ' 3.発行するSQL文を作成する
        Dim SqlStr As String
        SqlStr = "SELECT * FROM user WHERE machine_name='" + MachineName + "' AND user_name ='" + MachineUserName + "'"
        Console.WriteLine(SqlStr)
        ' 4.データ取得のためのアダプタの設定
        Dim Adapter = New MySqlDataAdapter(SqlStr, Con)

        ' 5.SQL文実行/gridに反映
        Dim Ds As New DataSet
        Adapter.Fill(Ds)
        If Ds.Tables(0).Rows.Count > 0 Then
            result = True
        Else
            result = False
        End If

        ' 6.データベースの切断
        Con.Close()

        Return result

    End Function
End Class
