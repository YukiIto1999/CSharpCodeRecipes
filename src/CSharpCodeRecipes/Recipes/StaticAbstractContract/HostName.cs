namespace CSharpCodeRecipes.Recipes.StaticAbstractContract
{
    /// <summary>
    /// 空白を含まない接続先のホスト名
    /// </summary>
    public sealed record HostName : ISettingValue<HostName>
    {
        /// <summary>
        /// 空白がないことを確かめた名前からのホスト名の構築
        /// </summary>
        /// <param name="value">空でなく空白を含まない名前</param>
        private HostName(string value)
        {
            Value = value;
        }

        /// <summary>
        /// 接続に使うホストの名前
        /// </summary>
        /// <value>空でなく空白を含まない名前</value>
        public string Value { get; }

        /// <summary>
        /// 前後の空白を除いた文字列からのホスト名の作成
        /// </summary>
        /// <param name="text">前後に空白があってもよいホスト名の文字列</param>
        /// <returns>前後の空白を除いたホスト名</returns>
        /// <exception cref="FormatException">空の名前か途中に空白を含む名前</exception>
        public static HostName FromSetting(string text)
        {
            var trimmed = text.Trim();
            if (trimmed.Length == 0 || trimmed.Any(char.IsWhiteSpace))
            {
                throw new FormatException($"ホスト名として使えない文字列: '{text}'");
            }

            return new HostName(trimmed);
        }
    }
}
