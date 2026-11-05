namespace CSharpCodeRecipes.Recipes.RequiredOptions
{
    /// <summary>
    /// 基盤側のメール送信が使う SMTP サーバーへの接続の設定
    /// </summary>
    public sealed class SmtpOptions
    {
        /// <summary>
        /// 接続先の SMTP サーバーのホスト名
        /// </summary>
        /// <value>構築時に必ず設定するホスト名</value>
        public required string Host { get; init; }

        /// <summary>
        /// 接続先の SMTP サーバーのポート番号
        /// </summary>
        /// <value>構築時に必ず設定するポート番号</value>
        public required int Port { get; init; }

        /// <summary>
        /// 送信を諦めるまでの待ち時間
        /// </summary>
        /// <value>設定しなければ 30 秒の待ち時間</value>
        public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(30);
    }
}
