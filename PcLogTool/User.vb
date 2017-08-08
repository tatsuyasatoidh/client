
Public Class User
    'UserDao設定インスタンスを作成する
    Dim UserDao As New UserDao()

    Private Id
    Private UserName
    Private MachineUserName
    Private MachineName
    Private CompanyId

    'コンストラクタ
    Public Sub New()
        setProperty()
    End Sub

    'MysqlUserテーブルから対応のユーザー情報取得
    Private Sub setProperty()
        MsgBox("setProperty")
        Dim Info As Hashtable = New Hashtable

        'ログインユーザ名取得
        MachineUserName = System.Environment.UserName
        'マシン名取得
        MachineName = System.Environment.MachineName

        Info = UserDao.getAll(MachineUserName, MachineName)

        Me.Id = Info("id")
        Me.CompanyId = Info("company_id")
    End Sub

    '操作ユーザー名を返す
    Public Function getUserName()
        Return Me.UserName
    End Function

    '端末情報を返す
    Public Function getMachineName()
        Return Me.MachineName
    End Function

    'ユーザーＩＤを返す
    Public Function getId()
        Return Me.Id
    End Function

    'ユーザーＩＤを返す
    Public Function getCompanyId()
        Return Me.CompanyId
    End Function

End Class
