namespace CSharpCodeRecipes.Recipes.RequiredOptions
{
    /// <summary>
    /// 設定に従って SMTP サーバーへメールを送る基盤側の送信
    /// </summary>
    public sealed class MailSender
    {
        /// <summary>
        /// 接続の設定を受け取った送信の準備
        /// </summary>
        /// <param name="options">必須の項目を全て設定した接続の設定</param>
        public MailSender(SmtpOptions options)
        {
            Endpoint = $"smtp://{options.Host}:{options.Port}";
            Timeout = options.Timeout;
        }

        /// <summary>
        /// 送信に使う接続先
        /// </summary>
        /// <value>設定のホスト名とポート番号から作った SMTP の URL</value>
        public string Endpoint { get; }

        /// <summary>
        /// 送信を諦めるまでの待ち時間
        /// </summary>
        /// <value>設定から引き継いだ待ち時間</value>
        public TimeSpan Timeout { get; }
    }
}
