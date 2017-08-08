'mysqlを使う
Imports MySql.Data.MySqlClient

Friend Class CompanyNameBox
    Private companyname As Hashtable = New Hashtable
    Dim CompanyDao As New CompanyDao()
    Dim MysqlManage As New MysqlManage()

    'コンストラクタ
    Public Sub New()

    End Sub

    Public Sub setComapanyId(ByVal id As String, ByVal campanyName As String)
        companyname(campanyName) = id
    End Sub

    Public Function getComapanyIdByName(ByVal campanyName As String)
        Return companyname(campanyName)
    End Function
End Class
