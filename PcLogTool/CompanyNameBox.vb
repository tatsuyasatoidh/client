Friend Class CompanyNameBox
    Private companyname As Hashtable = New Hashtable

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
