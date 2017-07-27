'mysqlを使う
Imports MySql.Data.MySqlClient

Friend Class CompanyDao

    Private MysqlManage

    Public Sub New()
        Dim Mysqlinstance As New MysqlManage()
        MysqlManage = Mysqlinstance
    End Sub

    '企業名取得関数
    Public Function getCompanyInfo()
        Dim Con As New MySqlConnection
        Dim dr As MySqlDataReader
        Dim resultArray As New ArrayList
        Try
            Con.ConnectionString = MysqlManage.Connect()

            Dim cmd As MySqlCommand

            Dim sqlStr As String

            '接続 
            Con.Open()
            'SQL文 
            sqlStr = "SELECT company_name FROM company"

            'MySQLCommand作成 
            cmd = New MySqlCommand(sqlStr, Con)
            'SQL文実行 
            dr = cmd.ExecuteReader

            '結果を表示 
            While dr.Read()
                resultArray.Add(CStr(dr("company_name")))
            End While
        Catch ex As Exception
            MsgBox("getComapnyNameでエラー")
        Finally
            'クローズ 
            Con.Close()
        End Try

        Return resultArray

    End Function

    '企業名を使って企業ＩＤを取得'
    Function getCompanyId(ByVal companyName As String)
        Console.WriteLine("getCompanyId start")
        Dim Con As New MySqlConnection
        Dim result As String
        Dim resultArray As New ArrayList

        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        Dim sqlStr As String


        Con.ConnectionString = MysqlManage.Connect()
        result = ""
        Try
            Console.WriteLine(companyName)
            '接続 
            Con.Open()
            'SQL文 
            sqlStr = "SELECT id FROM company" & " WHERE company.company_name = companyName"
            Console.WriteLine(sqlStr)
            'MySQLCommand作成 
            cmd = New MySqlCommand(sqlStr, Con)

            'SQL文実行 
            dr = cmd.ExecuteReader
            '結果を表示 
            While dr.Read()
                result = dr.Item(0)
            End While
            Console.WriteLine(result)
        Catch ex As Exception
            Console.WriteLine(ex)
            Console.WriteLine(ex.ToString)
            Console.WriteLine("企業ＩＤの取得に失敗しました")
            result = False
        Finally
            Con.Close()
            Console.WriteLine("getCompanyId end")
        End Try

        Return result

    End Function

End Class
