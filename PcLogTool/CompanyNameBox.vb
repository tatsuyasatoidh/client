Friend Class CompanyNameBox
    Private companyname As String = ""

    'コンストラクタ
    Public Sub New(ByVal name As String)
        companyname = name
    End Sub

    '表示名称
    '（このプロパティはこのサンプルでは使わないのでなくても良い）
    Public ReadOnly Property Name() As String
        Get
            Return companyname
        End Get
    End Property

    'オーバーライドしたメソッド
    'これがコンボボックスに表示される
    Public Overrides Function ToString() As String
        Return companyname
    End Function

End Class
