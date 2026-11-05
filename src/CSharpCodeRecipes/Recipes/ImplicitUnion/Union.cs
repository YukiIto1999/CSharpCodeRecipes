namespace CSharpCodeRecipes.Recipes.ImplicitUnion
{
    /// <summary>
    /// 型引数に挙げた二つの型のどちらか一方だけを持つ値
    /// </summary>
    /// <typeparam name="T1">一つ目の候補の型で <typeparamref name="T2"/> と異なる型</typeparam>
    /// <typeparam name="T2">二つ目の候補の型で <typeparamref name="T1"/> と異なる型</typeparam>
    /// <remarks>
    /// 候補の型の値からの暗黙の変換に限る構築
    /// </remarks>
    public abstract class Union<T1, T2>
        where T1 : notnull
        where T2 : notnull
    {
        /// <summary>
        /// ネストした二つのバリアントだけに許す構築
        /// </summary>
        private Union()
        {
        }

        /// <summary>
        /// 一つ目の候補の型の値からの変換
        /// </summary>
        /// <param name="value">保持する一つ目の候補の型の値</param>
        /// <returns>一つ目の候補の値を持つ和型の値</returns>
        /// <exception cref="ArgumentNullException">nullable の警告を無視して渡された null</exception>
        public static implicit operator Union<T1, T2>(T1 value)
        {
            ArgumentNullException.ThrowIfNull(value);
            return new First(value);
        }

        /// <summary>
        /// 二つ目の候補の型の値からの変換
        /// </summary>
        /// <param name="value">保持する二つ目の候補の型の値</param>
        /// <returns>二つ目の候補の値を持つ和型の値</returns>
        /// <exception cref="ArgumentNullException">nullable の警告を無視して渡された null</exception>
        public static implicit operator Union<T1, T2>(T2 value)
        {
            ArgumentNullException.ThrowIfNull(value);
            return new Second(value);
        }

        /// <summary>
        /// 保持する値の型に応じた二つの場合の処理
        /// </summary>
        /// <typeparam name="TOut">処理の結果の型</typeparam>
        /// <param name="onFirst">一つ目の候補の値を持つ場合の処理</param>
        /// <param name="onSecond">二つ目の候補の値を持つ場合の処理</param>
        /// <returns>保持する値に対応する一方の処理の結果</returns>
        public abstract TOut Match<TOut>(Func<T1, TOut> onFirst, Func<T2, TOut> onSecond);

        /// <summary>
        /// 一つ目の候補の値を持つバリアント
        /// </summary>
        /// <param name="value">保持する一つ目の候補の型の値</param>
        private sealed class First(T1 value) : Union<T1, T2>
        {
            /// <inheritdoc />
            public override TOut Match<TOut>(Func<T1, TOut> onFirst, Func<T2, TOut> onSecond) => onFirst(value);
        }

        /// <summary>
        /// 二つ目の候補の値を持つバリアント
        /// </summary>
        /// <param name="value">保持する二つ目の候補の型の値</param>
        private sealed class Second(T2 value) : Union<T1, T2>
        {
            /// <inheritdoc />
            public override TOut Match<TOut>(Func<T1, TOut> onFirst, Func<T2, TOut> onSecond) => onSecond(value);
        }
    }
}
