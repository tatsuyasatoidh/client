Imports System.Runtime.InteropServices
'mysqlを使う
Imports MySql.Data.MySqlClient

'Imports System
'Imports System.IO
'Imports System.Text


Public Delegate Function CallBack(
    ByVal nCode As Integer,
    ByVal wParam As IntPtr,
    ByVal lParam As IntPtr) As Integer


Public Class Form1
    Dim WH_MOUSE As Integer = 7
    Shared hHook As Integer = 0
    'ログインユーザ名取得
    Dim username As String = System.Environment.UserName
    'マシン名取得
    Dim userspc As String = System.Environment.MachineName
    '現在の日付を取得する
    Dim dtToday As DateTime = DateTime.Today
    'ファイルパスの作成'
    Dim file_path = "/tmp/PcLogTool/" & username & "/log_" & dtToday.ToString("yyyyMMdd") & ".txt"

    Dim COUNT As Integer

    'プログラム起動時の処理
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AutoRun()
        If check_pc() Then
            MsgBox("このPCの操作は記録されています。")
            Method1()
            If check_today_work() Then
                COUNT = check_today_work()
            End If
        Else
            MsgBox("このPCは登録されておりません。")
            'Form2クラスのインスタンスを作成する
            Dim form2 As New User_Register_Form()
            'Form2を表示する
            'ここではモーダルダイアログボックスとして表示する
            'オーナーウィンドウにMeを指定する
            form2.ShowDialog(Me)

        End If

        'ディレクトリの存在の確認
        If System.IO.Directory.Exists("/tmp/PcLogTool/" & username) Then
        Else
            'ディレクトリがない場合、作成'
            System.IO.Directory.CreateDirectory("/tmp/PcLogTool/" & username)
        End If

    End Sub

    Public Shared Sub AutoRun()
        'Runキーを開く
        Dim regkey As Microsoft.Win32.RegistryKey =
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
        "Software\Microsoft\Windows\CurrentVersion\Run", True)
        '値の名前に製品名、値のデータに実行ファイルのパスを指定し、書き込む
        regkey.SetValue(Application.ProductName, Application.ExecutablePath)
        '閉じる
        regkey.Close()
    End Sub

    Function db_con()

        '現在の日付を取得する
        Dim dtToday As DateTime = DateTime.Today

        ' 1.接続文字列を作成する
        Dim Builder = New MySqlConnectionStringBuilder()
        ' データベースに接続するために必要な情報をBuilderに与える
        Builder.Server = "localhost"
        Builder.Port = 3306
        Builder.UserID = "root"
        Builder.Database = "pclog"
        Dim ConStr = Builder.ToString()


        Return ConStr

    End Function
    '起動時PC認証
    Function check_pc()

        ' 2.データベースに接続するためのコネクションを準備して、実際につなぐ
        Dim Con As New MySqlConnection
        Con.ConnectionString = db_con()
        Con.Open()

        ' 3.発行するSQL文を作成する
        Dim SqlStr As String
        SqlStr = "SELECT * FROM user WHERE user_name ='" + username + "' and machine_name='" + userspc + "'"
        ' 4.データ取得のためのアダプタの設定
        Dim Adapter = New MySqlDataAdapter(SqlStr, Con)


        ' 5.SQL文実行/gridに反映
        Dim Ds As New DataSet
        Adapter.Fill(Ds)
        If Ds.Tables(0).Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
        ' 6.データベースの切断
        Con.Close()

    End Function
    'user_idを取得
    Function get_user_id()

        ' 2.データベースに接続するためのコネクションを準備して、実際につなぐ
        Dim Con As New MySqlConnection
        Con.ConnectionString = db_con()
        Con.Open()

        ' 3.発行するSQL文を作成する
        Dim SqlStr As String
        SqlStr = "SELECT * FROM user WHERE user_name ='" + username + "' and machine_name='" + userspc + "'"
        ' 4.データ取得のためのアダプタの設定
        Dim Adapter = New MySqlDataAdapter(SqlStr, Con)

        ' 5.SQL文実行/gridに反映
        Dim Ds As New DataSet
        Adapter.Fill(Ds)

        If Ds.Tables(0).Rows.Count > 0 Then
            Return Ds.Tables(0).Rows(0)("id")
        End If
        ' 6.データベースの切断
        Con.Close()

    End Function

    '今日作業したか？してある場合は作業量を引き継ぐ
    Function check_today_work()

        ' 2.データベースに接続するためのコネクションを準備して、実際につなぐ
        Dim Con As New MySqlConnection
        Con.ConnectionString = db_con()
        Con.Open()

        ' 3.発行するSQL文を作成する
        Dim SqlStr As String
        SqlStr = "SELECT * FROM pclog JOIN user ON pclog.user_id = user.id WHERE user_name ='" + username + "' and date='" + dtToday.ToString("yyyy-MM-dd") + "'"
        ' 4.データ取得のためのアダプタの設定
        Dim Adapter = New MySqlDataAdapter(SqlStr, Con)
        Console.Write(SqlStr)
        ' 5.SQL文実行/gridに反映
        Dim Dss As New DataSet
        Adapter.Fill(Dss)
        If Dss.Tables(0).Rows.Count > 0 Then
            MsgBox("今日のデータを引き継ぎます")
            Return Dss.Tables(0).Rows(0)("number_of_work")
        Else
            MsgBox("おはようございます")
            COUNT = 0
            Return False
        End If
        ' 6.データベースの切断
        Con.Close()

    End Function
    'INSERT処理
    Public Sub Insertlog()
        '現在の日付を取得する
        Dim dtToday As DateTime = DateTime.Today

        ' 2.データベースに接続するためのコネクションを準備して、実際につなぐ
        Dim Con As New MySqlConnection
        Con.ConnectionString = db_con()
        Con.Open()

        ' 3.発行するSQL文を作成する
        Dim myInsertQuery As String = "INSERT INTO `pclog` (`date`, `user_id`,`number_of_work`) VALUES (?val1, ?val2,?val3)"
        Dim myCommand As New MySqlCommand(myInsertQuery, Con)
        'プレースホルダーにバインドします。
        '操作日
        myCommand.Parameters.AddWithValue("?val1", dtToday.ToString("yyyy-MM-dd"))
        'user_id
        myCommand.Parameters.AddWithValue("?val2", get_user_id())
        '作業数
        myCommand.Parameters.AddWithValue("?val3", COUNT)

        'SQLを実行します
        myCommand.ExecuteNonQuery()

        'ファイルテーブルにファイルを格納
        'pclogテーブルのid取得
        Dim SqlStrGetID As String
        SqlStrGetID = "SELECT `id` FROM `pclog`WHERE `date` ='" + dtToday.ToString("yyyy-MM-dd") + "' "
        Dim Adapter = New MySqlDataAdapter(SqlStrGetID, Con)

        'SQL文実行/
        Dim Ds As New DataSet
        Adapter.Fill(Ds)
        Dim rowcount = (Ds.Tables(0).Rows.Count) - 1
        Dim pclog_id = (Ds.Tables(0).Rows(rowcount)("id"))

        '格納するファイルのテキスト情報を取得
        'Shift-JISコードとして開く
        Dim fileread As New System.IO.StreamReader("/tmp/PcLogTool/" & username & "/log_" & dtToday.ToString("yyyyMMdd") & ".txt",
         System.Text.Encoding.GetEncoding("shift_jis"))
        '内容をすべて読み込む
        Dim content As String = fileread.ReadToEnd()
        '閉じる
        fileread.Close()

        'pclog_idを使ってファイルを格納
        Dim myInsertQuery2 As String = "INSERT INTO `log_file`(`pclog_id`, `log`) VALUES(?val5,?val6)"
        Dim myCommand3 As New MySqlCommand(myInsertQuery2, Con)

        'pclog_id
        myCommand3.Parameters.AddWithValue("?val5", pclog_id)
        'logファイル
        myCommand3.Parameters.AddWithValue("?val6", content)

        'SQLを実行します
        myCommand3.ExecuteNonQuery()

        ' 6.データベースの切断
        Con.Close()


    End Sub

    'Updatelog()
    Public Sub Updatelog()
        '現在の日付を取得する
        Dim dtToday As DateTime = DateTime.Today

        ' 2.データベースに接続するためのコネクションを準備して、実際につなぐ
        Dim Con As New MySqlConnection
        Con.ConnectionString = db_con()
        Con.Open()

        ' 3.発行するSQL文を作成する
        Dim myInsertQuery As String = "UPDATE `pclog` SET number_of_work = ?val3 WHERE date=?val1 and user_id =?val2 "
        Dim myCommand As New MySqlCommand(myInsertQuery, Con)

        'プレースホルダーにバインドします。
        '操作日
        myCommand.Parameters.AddWithValue("?val1", dtToday.ToString("yyyy-MM-dd"))
        'user_id
        myCommand.Parameters.AddWithValue("?val2", get_user_id())
        '作業数
        myCommand.Parameters.AddWithValue("?val3", COUNT)
        'SQLを実行します
        myCommand.ExecuteNonQuery()

        'ファイルテーブルにファイルを格納
        'pclogテーブルのid取得
        Dim SqlStrGetID As String
        SqlStrGetID = "SELECT `id` FROM `pclog`WHERE `date` ='" + dtToday.ToString("yyyy-MM-dd") + "' "
        Dim Adapter = New MySqlDataAdapter(SqlStrGetID, Con)

        'SQL文実行/
        Dim Ds As New DataSet
        Adapter.Fill(Ds)
        Dim rowcount = (Ds.Tables(0).Rows.Count) - 1
        Dim pclog_id = (Ds.Tables(0).Rows(rowcount)("id"))

        '格納するファイルのテキスト情報を取得
        'Shift-JISコードとして開く
        Dim fileread As New System.IO.StreamReader("/tmp/PcLogTool/" & username & "/log_" & dtToday.ToString("yyyyMMdd") & ".txt",
         System.Text.Encoding.GetEncoding("shift_jis"))
        '内容をすべて読み込む
        Dim content As String = fileread.ReadToEnd()
        '閉じる
        fileread.Close()

        'pclog_idを使ってファイルを格納
        Dim myInsertQuery2 As String = "UPDATE `log_file` SET log = ?val6 WHERE pclog_id = ?val5"
        Dim myCommand3 As New MySqlCommand(myInsertQuery2, Con)

        'pclog_id
        myCommand3.Parameters.AddWithValue("?val5", pclog_id)
        'logファイル
        myCommand3.Parameters.AddWithValue("?val6", content)

        'SQLを実行します
        myCommand3.ExecuteNonQuery()

        ' 6.データベースの切断
        Con.Close()
    End Sub

    WithEvents KeyboardHooker1 As New KeyboardHooker
    'key押下時ログ出力
    Sub KeybordHooker1_KeyDown(sender As Object, e As KeyBoardHookerEventArgs) Handles KeyboardHooker1.KeyDown
        '作業数をカウント
        COUNT = COUNT + 1
        '現在の時間を取得する
        Dim nowitme As String = DateTime.Now.ToString().Replace(" ", ",")
        Select Case CStr(e.vkCode)

        '書き込む内容
            Case 27
                Label1.Text = username & " , " & nowitme & " , " & "'" & "esc" & "'"
            Case 112
                Label1.Text = username & "," & nowitme & "," & "'" & "F1" & "'"
            Case 113
                Label1.Text = username & "," & nowitme & "," & "'" & "F2" & "'"
            Case 114
                Label1.Text = username & "," & nowitme & "," & "'" & "F3" & "'"
            Case 115
                Label1.Text = username & "," & nowitme & "," & "'" & "F4" & "'"
            Case 116
                Label1.Text = username & "," & nowitme & "," & "'" & "F5" & "'"
            Case 117
                Label1.Text = username & "," & nowitme & "," & "'" & "F6" & "'"
            Case 118
                Label1.Text = username & "," & nowitme & "," & "'" & "F7" & "'"
            Case 119
                Label1.Text = username & "," & nowitme & "," & "'" & "F8" & "'"
            Case 120
                Label1.Text = username & "," & nowitme & "," & "'" & "F9" & "'"
            Case 121
                Label1.Text = username & "," & nowitme & "," & "'" & "F10" & "'"
            Case 122
                Label1.Text = username & "," & nowitme & "," & "'" & "F11" & "'"
            Case 123
                Label1.Text = username & "," & nowitme & "," & "'" & "F13" & "'"
            Case 45
                Label1.Text = username & "," & nowitme & "," & "'" & "insert" & "'"
            Case 44
                Label1.Text = username & "," & nowitme & "," & "'" & "prt scr" & "'"
            Case 46
                Label1.Text = username & "," & nowitme & "," & "'" & "delete" & "'"
            Case 9
                Label1.Text = username & "," & nowitme & "," & "'" & "Tab" & "'"
            Case 49
                Label1.Text = username & "," & nowitme & "," & "'" & "1(!)" & "'"
            Case 50
                Label1.Text = username & "," & nowitme & "," & "'" & "2('')" & "'"
            Case 51
                Label1.Text = username & "," & nowitme & "," & "'" & "3(#)" & "'"
            Case 52
                Label1.Text = username & "," & nowitme & "," & "'" & "4($)" & "'"
            Case 53
                Label1.Text = username & "," & nowitme & "," & "'" & "5(%)" & "'"
            Case 54
                Label1.Text = username & "," & nowitme & "," & "'" & "6(&)" & "'"
            Case 55
                Label1.Text = username & "," & nowitme & "," & "'" & "7(&)" & "'"
            Case 56
                Label1.Text = username & "," & nowitme & "," & "'" & "8(()" & "'"
            Case 57
                Label1.Text = username & "," & nowitme & "," & "'" & "9())" & "'"
            Case 48
                Label1.Text = username & "," & nowitme & "," & "'" & "0()" & "'"
            Case 189
                Label1.Text = username & "," & nowitme & "," & "'" & "-(=)" & "'"
            Case 222
                Label1.Text = username & "," & nowitme & "," & "'" & "^(~)" & "'"
            Case 220
                Label1.Text = username & "," & nowitme & "," & "'" & "\(|)" & "'"
            Case 8
                Label1.Text = username & "," & nowitme & "," & "'" & "backspace" & "'"
            Case 36
                Label1.Text = username & "," & nowitme & "," & "'" & "home" & "'"
            Case 192
                Label1.Text = username & "," & nowitme & "," & "'" & "@" & "'"
            Case 219
                Label1.Text = username & "," & nowitme & "," & "'" & "[({)" & "'"
            Case 13
                Label1.Text = username & "," & nowitme & "," & "'" & "enter" & "'"
            Case 33
                Label1.Text = username & "," & nowitme & "," & "'" & "pg up" & "'"
            Case 34
                Label1.Text = username & "," & nowitme & "," & "'" & "pg dn" & "'"
            Case 240
                Label1.Text = username & "," & nowitme & "," & "'" & "caps lock" & "'"
            Case 187
                Label1.Text = username & "," & nowitme & "," & "'" & ";(+)" & "'"
            Case 186
                Label1.Text = username & "," & nowitme & "," & "'" & ":(*)" & "'"
            Case 221
                Label1.Text = username & "," & nowitme & "," & "'" & "](})" & "'"
            Case 188
                Label1.Text = username & "," & nowitme & "," & "'" & ",(<)" & "'"
            Case 190
                Label1.Text = username & "," & nowitme & "," & "'" & ".(>)" & "'"
            Case 191
                Label1.Text = username & "," & nowitme & "," & "'" & "/(?)" & "'"
            Case 226
                Label1.Text = username & "," & nowitme & "," & "'" & "\(_)" & "'"
            Case 162
                Label1.Text = username & "," & nowitme & "," & "'" & "ctr" & "'"
            Case 91
                Label1.Text = username & "," & nowitme & "," & "'" & "windows" & "'"
            Case 29
                Label1.Text = username & "," & nowitme & "," & "'" & "無変換" & "'"
            Case 32
                Label1.Text = username & "," & nowitme & "," & "'" & "space" & "'"
            Case 28
                Label1.Text = username & "," & nowitme & "," & "'" & "変換" & "'"
            Case 242
                Label1.Text = username & "," & nowitme & "," & "'" & "カナ/かな/ローマ" & "'"

            Case 37
                Label1.Text = username & "," & nowitme & "," & "'" & "←" & "'"

            Case 38
                Label1.Text = username & "," & nowitme & "," & "'" & "↑" & "'"
            Case 39
                Label1.Text = username & "," & nowitme & "," & "'" & "→" & "'"
            Case 40
                Label1.Text = username & "," & nowitme & "," & "'" & "↓" & "'"
            Case Else
                Label1.Text = username & "," & nowitme & "," & "'" & Chr(CStr(e.vkCode)) & "'"
        End Select


        '同時押しショートカット
        Select Case Control.ModifierKeys
            ''コントロールボタン同時押しの場合
            Case Keys.Control
                Select Case e.vkCode
                    Case Keys.Insert, Keys.C
                        Label1.Text = username & "," & nowitme & "," & "'" & "Copy(Control+C)" & "'"
                    Case Keys.V
                        Label1.Text = username & "," & nowitme & "," & "'" & "Paste(Control+V)" & "'"
                    Case Keys.X
                        Label1.Text = username & "," & nowitme & "," & "'" & "Cut(Control+X)" & "'"
                    Case Else
                End Select
            'Shiftボタン同時押しの場合
            Case Keys.Shift
                If e.vkCode = Keys.Insert Then
                    Label1.Text = username & ":" & "Paste(Shift+Insert)"
                End If
            Case Else
        End Select
        'fileに書き込み
        write_file()
    End Sub

    Public Sub write_file()
        'ファイルを上書きし、Shift JISで書き込む 
        Dim sw As New IO.StreamWriter(file_path,
            True,
            System.Text.Encoding.GetEncoding("shift_jis"))
        sw.WriteLine(Label1.Text)
        '閉じる 
        sw.Close()
    End Sub

    'グローバルフック検出用クラスを生成
    WithEvents m_clsMouseHook As New ClsMouseHook

    Public Sub Method1()
        'マウスのグローバルフック開始
        m_clsMouseHook.HookMouse()

    End Sub

    'マウスダウン時のイベント部分
    Private Sub m_clsMouseHook_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles m_clsMouseHook.MouseDown
        '作業数をカウント
        COUNT = COUNT + 1
        '現在の時間を取得する
        Dim nowitme As DateTime = DateTime.Now
        '書き込む内容
        Select Case e.Button
            Case MouseButtons.Right
                Label1.Text = username & "," & nowitme & "," & "'" & "右クリック" & "'"
            Case MouseButtons.Left
                Label1.Text = username & "," & nowitme & "," & "'" & "左クリック" & "'"
            Case MouseButtons.Middle
                Label1.Text = username & "," & nowitme & "," & "'" & "中央ボタン" & "'"
            Case MouseButtons.XButton1
                'Windows2000から5ボタンサポート
                Label1.Text = username & "," & nowitme & "," & "'" & "ブラウザ戻る" & "'"
            Case MouseButtons.XButton2
                'Windows2000から5ボタンサポート
                Label1.Text = username & "," & nowitme & "," & "'" & "ブラウザ進む" & "'"
            Case MouseButtons.None
                Label1.Text = username & "," & nowitme & "," & "'" & "なし" & "'"
        End Select
        'fileに書き込み
        write_file()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If check_today_work() Then
            Updatelog()
        Else
            Insertlog()
        End If
        MsgBox("ログをアップロードしました。")
        'アップロードするファイル
        Dim upFile As String = "/tmp/PcLogTool/" & username & "/log_" & dtToday.ToString("yyyyMMdd") & ".txt"
        'アップロード先のURI
        Dim u As New Uri("ftp://sv2.webcrow-php.netowl.jp/test.txt")

        'FtpWebRequestの作成
        Dim ftpReq As System.Net.FtpWebRequest = CType(System.Net.WebRequest.Create(u), System.Net.FtpWebRequest)
        'ログインユーザー名とパスワードを設定
        ftpReq.Credentials = New System.Net.NetworkCredential("pclog.webcrow.jp", "****")
        'MethodにWebRequestMethods.Ftp.UploadFile("STOR")を設定
        ftpReq.Method = System.Net.WebRequestMethods.Ftp.UploadFile
        '要求の完了後に接続を閉じる
        ftpReq.KeepAlive = False
        'ASCIIモードで転送する
        ftpReq.UseBinary = False
        'PASVモードを無効にする
        ftpReq.UsePassive = False

        'ファイルをアップロードするためのStreamを取得
        Dim reqStrm As System.IO.Stream = ftpReq.GetRequestStream()
        'アップロードするファイルを開く
        Dim fs As New System.IO.FileStream(
    upFile, System.IO.FileMode.Open, System.IO.FileAccess.Read)
        'アップロードStreamに書き込む
        Dim buffer(1023) As Byte
        While True
            Dim readSize As Integer = fs.Read(buffer, 0, buffer.Length)
            If readSize = 0 Then
                Exit While
            End If
            reqStrm.Write(buffer, 0, readSize)
        End While
        fs.Close()
        reqStrm.Close()

        'FtpWebResponseを取得
        Dim ftpRes As System.Net.FtpWebResponse =
    CType(ftpReq.GetResponse(), System.Net.FtpWebResponse)
        'FTPサーバーから送信されたステータスを表示
        Console.WriteLine("{0}: {1}", ftpRes.StatusCode, ftpRes.StatusDescription)
        '閉じる
        ftpRes.Close()
    End Sub

End Class

Public Class KeyboardHooker

    Const WM_KEYDOWN As Integer = &H100
    Const WM_KEYUP As Integer = &H101

    Public Sub New()
        hookproc = AddressOf KeybordHookProc
        hHook = SetWindowsHookEx(WH_KEYBOARD_LL, hookproc, GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), 0)
        If hHook.Equals(0) Then
            MsgBox("SetWindowsHookEx Failed")
        End If
    End Sub

    Dim WH_KEYBOARD_LL As Integer = 13
    Shared hHook As Integer = 0

    Private hookproc As CallBack

    Public Delegate Function CallBack(
        ByVal nCode As Integer,
        ByVal wParam As IntPtr,
        ByVal lParam As IntPtr) As Integer

    <DllImport("User32.dll", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)>
    Public Overloads Shared Function SetWindowsHookEx _
          (ByVal idHook As Integer, ByVal HookProc As CallBack,
    ByVal hInstance As IntPtr, ByVal wParam As Integer) As Integer
    End Function

    <DllImport("kernel32.dll", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)>
    Public Overloads Shared Function GetModuleHandle _
    (ByVal lpModuleName As String) As IntPtr
    End Function

    <DllImport("User32.dll", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)>
    Public Overloads Shared Function CallNextHookEx _
          (ByVal idHook As Integer, ByVal nCode As Integer,
    ByVal wParam As IntPtr, ByVal lParam As IntPtr) As Integer
    End Function

    <DllImport("User32.dll", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)>
    Public Overloads Shared Function UnhookWindowsHookEx _
    (ByVal idHook As Integer) As Boolean
    End Function

    <StructLayout(LayoutKind.Sequential)> Public Structure KeyboardLLHookStruct
        Public vkCode As Integer
        Public scanCode As Integer
        Public flags As Integer
        Public time As Integer
        Public dwExtraInfo As Integer
    End Structure


    Public Function KeybordHookProc(
        ByVal nCode As Integer,
        ByVal wParam As IntPtr,
        ByVal lParam As IntPtr) As Integer

        If (nCode < 0) Then
            Return CallNextHookEx(hHook, nCode, wParam, lParam)
        End If

        Dim hookStruct As New KeyboardLLHookStruct()
        hookStruct = CType(Marshal.PtrToStructure(lParam, hookStruct.GetType()), KeyboardLLHookStruct)

        If wParam = New IntPtr(WM_KEYDOWN) Then
            Dim e As New KeyBoardHookerEventArgs
            e.vkCode = hookStruct.vkCode
            RaiseEvent KeyDown(Me, e)
            Return 0
        End If

        If wParam = New IntPtr(WM_KEYUP) Then
            Dim e As New KeyBoardHookerEventArgs
            e.vkCode = hookStruct.vkCode
            RaiseEvent KeyUp(Me, e)
            Return 0
        End If

        Return CallNextHookEx(hHook, nCode, wParam, lParam)
    End Function

    Public Event KeyDown(ByVal sender As Object, ByVal EventArgs As KeyBoardHookerEventArgs)
    Public Event KeyUp(ByVal sender As Object, ByVal EventArgs As KeyBoardHookerEventArgs)

    Public Sub Dispose()
        Dim ret As Boolean = UnhookWindowsHookEx(hHook)
        If ret.Equals(False) Then
        End If
    End Sub


End Class

Public Class KeyBoardHookerEventArgs
    Inherits EventArgs

    Dim _vkCode As Integer

    Public Property vkCode() As Integer
        Get
            Return _vkCode
        End Get
        Set(ByVal value As Integer)
            _vkCode = value
        End Set
    End Property

End Class

Public Class ClsMouseHook

    Private m_iDoubleClickTime As Integer = 0
    Private m_dtLastLMouseDown As DateTime = Now
    Private m_dtLastRMouseDown As DateTime = Now

    Private Shared m_iMouseHandle As Integer = 0
    Private m_clsHookCallback As MouseHookCallBack

    Private Const HC_ACTION As Integer = 0
    Private Const WH_MOUSE_LL As Integer = 14
    Private Const WM_MOUSEMOVE As Integer = &H200
    Private Const WM_LBUTTONDOWN As Integer = &H201
    Private Const WM_LBUTTONUP As Integer = &H202
    Private Const WM_LBUTTONDBLCLK As Integer = &H203
    Private Const WM_RBUTTONDOWN As Integer = &H204
    Private Const WM_RBUTTONUP As Integer = &H205
    Private Const WM_RBUTTONDBLCLK As Integer = &H206
    Private Const WM_MBUTTONDOWN As Integer = &H207
    Private Const WM_MBUTTONUP As Integer = &H208
    Private Const WM_MBUTTONDBLCLK As Integer = &H209
    Private Const WM_MOUSEWHEEL As Integer = &H20A

    Public Delegate Function MouseHookCallBack(ByVal nCode As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As Integer

    Public Declare Function GetModuleHandle Lib "kernel32.dll" Alias "GetModuleHandleA" (ByVal ModuleName As String) As IntPtr

    <DllImport("User32.dll", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)>
    Public Overloads Shared Function GetDoubleClickTime() As Integer
    End Function

    <DllImport("User32.dll", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)>
    Public Overloads Shared Function SetWindowsHookEx _
        (ByVal idHook As Integer, ByVal HookProc As MouseHookCallBack,
        ByVal hInstance As IntPtr, ByVal wParam As Integer) As Integer
    End Function

    <DllImport("User32.dll", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)>
    Public Overloads Shared Function CallNextHookEx _
        (ByVal idHook As Integer, ByVal nCode As Integer,
        ByVal wParam As IntPtr, ByVal lParam As IntPtr) As Integer
    End Function

    <DllImport("User32.dll", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)>
    Public Overloads Shared Function UnhookWindowsHookEx _
        (ByVal idHook As Integer) As Boolean
    End Function

    <StructLayout(LayoutKind.Sequential)>
    Public Structure Point
        Public x As Integer
        Public y As Integer
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Public Structure MouseHookStruct
        Public pt As Point
        Public hwnd As Integer
        Public wHitTestCode As Integer
        Public dwExtraInfo As Integer
    End Structure

    Public Event MouseUp As System.Windows.Forms.MouseEventHandler
    Public Event MouseDown As System.Windows.Forms.MouseEventHandler
    Public Event MouseDoubleClick As System.Windows.Forms.MouseEventHandler

    Private m_HookProc As MouseHookCallBack

    Private Function MouseProc(ByVal nCode As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As Integer

        If (nCode = HC_ACTION) Then
            Dim uInfo As MouseHookStruct = CType(Marshal.PtrToStructure(lParam, uInfo.GetType()), MouseHookStruct)
            Select Case wParam.ToInt32()
                Case WM_LBUTTONDOWN
                    RaiseEvent MouseDown(Me, New MouseEventArgs(MouseButtons.Left, 1, uInfo.pt.x, uInfo.pt.y, 0))
                    ' double-click hack...
                    If (m_dtLastLMouseDown.AddMilliseconds(m_iDoubleClickTime).CompareTo(Now) >= 0) Then
                        RaiseEvent MouseDoubleClick(Me, New MouseEventArgs(MouseButtons.Left, 2, uInfo.pt.x, uInfo.pt.y, 0))
                    Else
                        m_dtLastLMouseDown = Now
                    End If
                Case WM_RBUTTONDOWN
                    RaiseEvent MouseDown(Me, New MouseEventArgs(MouseButtons.Right, 1, uInfo.pt.x, uInfo.pt.y, 0))
                    ' double-click hack...
                    If (m_dtLastRMouseDown.AddMilliseconds(m_iDoubleClickTime).CompareTo(Now) >= 0) Then
                        RaiseEvent MouseDoubleClick(Me, New MouseEventArgs(MouseButtons.Right, 2, uInfo.pt.x, uInfo.pt.y, 0))
                    Else
                        m_dtLastRMouseDown = Now
                    End If
                Case WM_LBUTTONUP
                    RaiseEvent MouseUp(Me, New MouseEventArgs(MouseButtons.Left, 1, uInfo.pt.x, uInfo.pt.y, 0))
                Case WM_RBUTTONUP
                    RaiseEvent MouseUp(Me, New MouseEventArgs(MouseButtons.Right, 1, uInfo.pt.x, uInfo.pt.y, 0))
                Case Else
                    If (CType(wParam, Integer) <> WM_MOUSEMOVE) Then Debug.WriteLine(wParam)
            End Select
        End If

        Return CallNextHookEx(m_iMouseHandle, nCode, wParam, lParam)

    End Function

    Public Sub HookMouse()

        m_HookProc = New MouseHookCallBack(AddressOf MouseProc)

        m_iMouseHandle = SetWindowsHookEx(WH_MOUSE_LL _
, m_HookProc _
, GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName) _
, 0)
        If m_iMouseHandle = 0 Then
            Throw New System.Exception("Mouse hook failed.")
        Else
            m_iDoubleClickTime = GetDoubleClickTime()
        End If
    End Sub

    Public Sub UnhookMouse()
        If (m_iMouseHandle <> 0) Then UnhookWindowsHookEx(m_iMouseHandle)
        If Not m_HookProc Is Nothing Then m_HookProc = Nothing
    End Sub

End Class
