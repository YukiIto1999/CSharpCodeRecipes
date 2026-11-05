namespace CSharpCodeRecipes.Recipes.StaticAbstractContract
{
    /// <summary>
    /// 設定の文字列から自分自身を作る静的メンバーを持つ型
    /// </summary>
    /// <typeparam name="TSelf">この契約を実装する型自身</typeparam>
    public interface ISettingValue<TSelf>
        where TSelf : ISettingValue<TSelf>
    {
        /// <summary>
        /// 設定の文字列からの値の作成
        /// </summary>
        /// <param name="text">設定に書かれたままの文字列</param>
        /// <returns>文字列が表す値</returns>
        public static abstract TSelf FromSetting(string text);
    }
}
