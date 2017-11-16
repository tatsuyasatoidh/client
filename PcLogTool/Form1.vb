Imports System.Runtime.InteropServices
'mysqlを使う

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
    'Mysql設定インスタンスを作成する
    Dim mysqlManage As New MysqlManage()
    'UserDaoのインスタンス
    Dim UserDao As New UserDao()
    'Userのインスタンス
    Dim UserManage As New User()
    'PcLogDaoのインスタンス
    Dim PcLogDao As New PcLogDao()
    '作業量
    Dim COUNT As Integer

    'プログラム起動時の処理
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Console.WriteLine("Form1")
        Try
            AutoRun()
            If CheckPc() Then
                MsgBox("このPCの操作は記録されています。")
                'グローバルフック起動
                Method1()
                If CheckTodayWork() Then
                    COUNT = CheckTodayWork()
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

            Console.WriteLine("Form1　end")
        Catch ex As Exception
            Console.WriteLine("formの読み込みに失敗しました。")
        End Try

    End Sub

    Public Shared Sub AutoRun()
        Console.WriteLine("AutoRun")
        'Runキーを開く
        Dim regkey As Microsoft.Win32.RegistryKey =
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
        "Software\Microsoft\Windows\CurrentVersion\Run", True)
        '値の名前に製品名、値のデータに実行ファイルのパスを指定し、書き込む
        regkey.SetValue(Application.ProductName, Application.ExecutablePath)
        '閉じる
        regkey.Close()
        Console.WriteLine("AutoRun end")
    End Sub

    '起動時PC認証
    Function CheckPc()
        Dim result As Boolean
        Console.WriteLine("CheckPc")
        result = UserDao.chkRecord(username, userspc)
        Console.WriteLine("CheckPc end")
        Return result
    End Function

    '今日作業したか？してある場合は作業量を引き継ぐ 
    '@return INT 作業量
    Function CheckTodayWork()
        Console.WriteLine("check_today_work")
        Dim result As String
        Dim dt = dtToday.ToString("yyyy-MM-dd")
        Dim userId As Integer
        userId = UserManage.GetId()
        result = PcLogDao.GetWorkCount(userId, dt)
        Console.WriteLine("check_today_work result　" + result)
        Return result
    End Function

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
        WriteLogToFile()
    End Sub

    'ログファイルに書き込む処理
    '
    Public Sub WriteLogToFile()
        Dim userId As Integer = UserManage.GetId()
        Dim CompanyId As Integer = UserManage.GetCompanyId()
        'ファイルパスの作成'
        Dim file_path = "/tmp/PcLogTool/log/" & CompanyId & "/" & userId & "/log_" & dtToday.ToString("yyyyMMdd") & ".csv"

        'ディレクトリの存在の確認
        If System.IO.Directory.Exists("/tmp/PcLogTool/log/" & CompanyId & "/" & userId) Then
        Else
            'ディレクトリがない場合、作成'
            System.IO.Directory.CreateDirectory("/tmp/PcLogTool/log/" & CompanyId & "/" & userId)
        End If

        'ファイルを上書きし、Shift JISで書き込む 
        Dim sw As New IO.StreamWriter(file_path,
            True,
            System.Text.Encoding.GetEncoding("shift_jis"))
        Dim InsertStr As String = Label1.Text + "," + "1"
        sw.WriteLine(InsertStr)
        '閉じる 
        sw.Close()
    End Sub

    Public Sub Method1()
        'マウスのグローバルフック開始
        M_clsMouseHook.HookMouse()
    End Sub

    'マウスダウン時のイベント部分
    Private Sub M_clsMouseHook_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles M_clsMouseHook.MouseDown
        '作業数をカウント
        COUNT = COUNT + 1
        '現在の時間を取得する
        Dim nowitme As String = DateTime.Now.ToString().Replace(" ", ",")
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
        WriteLogToFile()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim AmazonS3Manage As New AmazonS3Manage()
        Dim dt = dtToday.ToString("yyyyMMdd")
        Dim userId As Integer = UserManage.GetId()
        Dim CompanyId As Integer = UserManage.GetCompanyId()

        If CheckTodayWork() = 0 Then
            MsgBox("新しく今日の作業をデータベースに追加します")
            PcLogDao.Insertlog(COUNT)
        Else
            MsgBox("今日の作業データを更新します")
            PcLogDao.Updatelog(COUNT)
        End If

        AmazonS3Manage.Upload(userId, CompanyId, dt)

        MsgBox("ログをアップロードしました。")

    End Sub

    'グローバルフック検出用クラスを生成
    WithEvents M_clsMouseHook As New ClsMouseHook
    WithEvents KeyboardHooker1 As New KeyboardHooker
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
