using System.Runtime.CompilerServices;
using System.Text;

namespace CSharpCodeRecipes.Recipes.InterpolatedSql
{
    /// <summary>
    /// 補間文字列の穴をパラメータに置き換える SQL 文の組み立て
    /// </summary>
    /// <remarks>
    /// コンパイラが生成するコードからの利用を前提とする公開の構築と追加
    /// </remarks>
    [InterpolatedStringHandler]
    public sealed class SqlInterpolatedStringHandler
    {
        /// <summary>
        /// 組み立て中の SQL の本文
        /// </summary>
        private readonly StringBuilder text;

        /// <summary>
        /// 穴に渡された順の値
        /// </summary>
        private readonly List<object?> parameters;

        /// <summary>
        /// 補間文字列の長さと穴の数に合わせた組み立ての開始
        /// </summary>
        /// <param name="literalLength">補間文字列の穴を除く文字数</param>
        /// <param name="formattedCount">補間文字列の穴の数</param>
        public SqlInterpolatedStringHandler(int literalLength, int formattedCount)
        {
            text = new StringBuilder(literalLength + (formattedCount * 4));
            parameters = new List<object?>(formattedCount);
        }

        /// <summary>
        /// 補間文字列の穴でない部分の本文への追加
        /// </summary>
        /// <param name="literal">ソースに書かれた SQL の断片</param>
        public void AppendLiteral(string literal)
        {
            text.Append(literal);
        }

        /// <summary>
        /// 補間文字列の穴の値のパラメータとしての追加
        /// </summary>
        /// <typeparam name="T">穴に渡された値の型</typeparam>
        /// <param name="value">本文に埋め込まずにパラメータとして渡す値</param>
        public void AppendFormatted<T>(T value)
        {
            text.Append("@p").Append(parameters.Count);
            parameters.Add(value);
        }

        /// <summary>
        /// 組み立て終えた SQL 文の取得
        /// </summary>
        /// <returns>本文とパラメータの組</returns>
        internal SqlStatement ToStatement()
        {
            return new SqlStatement(text.ToString(), parameters.ToArray());
        }
    }
}
