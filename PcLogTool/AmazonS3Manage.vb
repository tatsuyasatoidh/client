Imports Amazon.S3
Imports Amazon.S3.Model

Friend Class AmazonS3Manage

    Dim objS3Client As New AmazonS3Client
    Dim objS3PutRequest As New PutObjectRequest
    Dim objS3PutResponse As New PutObjectResponse

    'S3にログファイルをアップロードする処理
    Public Sub Upload(userId, CompanyId, dt)
        'バケットを指定
        objS3PutRequest.BucketName = "elasticbeanstalk-us-west-2-443316351375"
        'キーを指定
        objS3PutRequest.Key = "log/" & CompanyId & "/" & userId & "/log_" & dt & ".csv"
        'アップロードするローカルファイルのパスを指定
        objS3PutRequest.FilePath = "/tmp/PcLogTool/log/" & CompanyId & "/" & userId & "/log_" & dt & ".csv"
        'リクエストをパラメータに指定してPutObjectメソッドを実行
        objS3PutResponse = objS3Client.PutObject(objS3PutRequest)
    End Sub



End Class
