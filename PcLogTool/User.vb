
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
        SetProperty()
    End Sub

    'MysqlUserテーブルから対応のユーザー情報取得
    Private Sub SetProperty()
        Dim Info As Hashtable = New Hashtable

        'ログインユーザ名取得
        MachineUserName = System.Environment.UserName
        'マシン名取得
        MachineName = System.Environment.MachineName
        'ユーザーがデータベースに登録されているかを確認
        If UserDao.chkRecord(MachineUserName, MachineName) Then
            'ユーザーがデータベースに登録されている場合はクラス変数に値をセット
            Info = UserDao.GetAll(MachineUserName, MachineName)
            Me.Id = Info("id")
            Me.CompanyId = Info("company_id")
        End If
    End Sub

    '操作ユーザー名を返す
    Public Function GetUserName()
        Return Me.UserName
    End Function

    '端末情報を返す
    Public Function GetMachineName()
        Return Me.MachineName
    End Function

    'ユーザーＩＤを返す
    Public Function GetId()
        Return Me.Id
    End Function

    'ユーザーＩＤを返す
    Public Function GetCompanyId()
        Return Me.CompanyId
    End Function

End Class
