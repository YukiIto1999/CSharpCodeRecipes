namespace CSharpCodeRecipes.Recipes.InterpolatedSql
{
    /// <summary>
    /// 値をパラメータとして本文から切り離した SQL 文
    /// </summary>
    public sealed class SqlStatement
    {
        /// <summary>
        /// <see cref="SqlInterpolatedStringHandler"/> だけが行う SQL 文の構築
        /// </summary>
        /// <param name="text">補間の穴を @p0 からの連番の名前に置き換えた本文</param>
        /// <param name="parameters">本文の連番と同じ順の値</param>
        internal SqlStatement(string text, IReadOnlyList<object?> parameters)
        {
            Text = text;
            Parameters = parameters;
        }

        /// <summary>
        /// データベースへ送る SQL の本文
        /// </summary>
        /// <value>補間した値を含まずパラメータ名だけを含む本文</value>
        public string Text { get; }

        /// <summary>
        /// 本文のパラメータ名に対応する値
        /// </summary>
        /// <value>添字が @p の後の連番と一致する値の並び</value>
        public IReadOnlyList<object?> Parameters { get; }
    }
}
