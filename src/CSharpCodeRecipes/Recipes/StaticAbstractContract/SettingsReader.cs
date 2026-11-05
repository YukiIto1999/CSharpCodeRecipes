namespace CSharpCodeRecipes.Recipes.StaticAbstractContract
{
    /// <summary>
    /// 鍵ごとの文字列の設定からの型付きの値の読み出し
    /// </summary>
    public sealed class SettingsReader
    {
        /// <summary>
        /// 鍵ごとの設定に書かれたままの文字列
        /// </summary>
        private readonly IReadOnlyDictionary<string, string> texts;

        /// <summary>
        /// 読み出す設定の文字列の登録
        /// </summary>
        /// <param name="texts">鍵ごとの設定に書かれたままの文字列</param>
        public SettingsReader(IReadOnlyDictionary<string, string> texts)
        {
            this.texts = texts;
        }

        /// <summary>
        /// 鍵の文字列からの型付きの値の作成
        /// </summary>
        /// <typeparam name="T">設定の文字列から自分自身を作る契約を実装した型</typeparam>
        /// <param name="key">読み出す設定の鍵</param>
        /// <returns>鍵の文字列を <typeparamref name="T"/> の作り方で変換した値</returns>
        /// <exception cref="KeyNotFoundException">登録されていない鍵</exception>
        public T Get<T>(string key)
            where T : ISettingValue<T>
        {
            return T.FromSetting(texts[key]);
        }
    }
}
