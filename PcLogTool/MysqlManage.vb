'mysqlを使う
Imports MySql.Data.MySqlClient

'mysql接続関数
Friend Class MysqlManage

    Public Shared Function Connect()

        '現在の日付を取得する
        Dim dtToday As DateTime = DateTime.Today

        ' 1.接続文字列を作成する
        Dim Builder = New MySqlConnectionStringBuilder()
        ' データベースに接続するために必要な情報をBuilderに与える
        Builder.Server = "pclog.c5q2rhfkfpib.us-west-2.rds.amazonaws.com"
        Builder.Port = 3306
        Builder.Password = "idhpclogtool"
        Builder.UserID = "root"
        Builder.Database = "pclog"

        'Builder.Server = "localhost"
        'Builder.Port = 3306
        'Builder.UserID = "root"
        'Builder.Database = "pclog"
        Dim ConStr = Builder.ToString()

        Return ConStr

    End Function
End Class
