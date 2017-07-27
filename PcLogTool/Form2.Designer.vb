<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class User_Register_Form
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.register_submit_buttom = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.box_username = New System.Windows.Forms.TextBox()
        Me.companyNameBox = New System.Windows.Forms.ComboBox()
        Me.SuspendLayout()
        '
        'register_submit_buttom
        '
        Me.register_submit_buttom.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.register_submit_buttom.Location = New System.Drawing.Point(104, 254)
        Me.register_submit_buttom.Name = "register_submit_buttom"
        Me.register_submit_buttom.Size = New System.Drawing.Size(100, 23)
        Me.register_submit_buttom.TabIndex = 4
        Me.register_submit_buttom.Text = "ユーザー情報登録"
        Me.register_submit_buttom.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(24, 107)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(63, 12)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "企業名　　："
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label2.Location = New System.Drawing.Point(24, 44)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(63, 12)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "ユーザー名："
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label5.Location = New System.Drawing.Point(26, 73)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(61, 12)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "ＰＣ名　　 ："
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label4.Location = New System.Drawing.Point(102, 73)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(38, 12)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Label4"
        '
        'box_username
        '
        Me.box_username.Location = New System.Drawing.Point(104, 37)
        Me.box_username.Name = "box_username"
        Me.box_username.Size = New System.Drawing.Size(100, 19)
        Me.box_username.TabIndex = 9
        '
        'companyNameBox
        '
        Me.companyNameBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.companyNameBox.FormattingEnabled = True
        Me.companyNameBox.Location = New System.Drawing.Point(104, 104)
        Me.companyNameBox.Name = "companyNameBox"
        Me.companyNameBox.Size = New System.Drawing.Size(121, 20)
        Me.companyNameBox.TabIndex = 11
        '
        'User_Register_Form
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 326)
        Me.Controls.Add(Me.companyNameBox)
        Me.Controls.Add(Me.box_username)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.register_submit_buttom)
        Me.Controls.Add(Me.Label4)
        Me.ForeColor = System.Drawing.SystemColors.ControlLight
        Me.Name = "User_Register_Form"
        Me.Text = "Form2"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents register_submit_buttom As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents box_username As TextBox
    Friend WithEvents companyNameBox As ComboBox
End Class
