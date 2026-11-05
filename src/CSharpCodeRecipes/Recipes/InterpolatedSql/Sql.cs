namespace CSharpCodeRecipes.Recipes.InterpolatedSql
{
    /// <summary>
    /// 補間文字列からだけ作れる SQL 文の入口
    /// </summary>
    public static class Sql
    {
        /// <summary>
        /// 補間文字列からの穴をパラメータにした SQL 文の作成
        /// </summary>
        /// <param name="statement">穴に値を埋め込んだ SQL の補間文字列</param>
        /// <returns>穴をパラメータに置き換えた SQL 文</returns>
        public static SqlStatement Create(SqlInterpolatedStringHandler statement)
        {
            return statement.ToStatement();
        }
    }
}
