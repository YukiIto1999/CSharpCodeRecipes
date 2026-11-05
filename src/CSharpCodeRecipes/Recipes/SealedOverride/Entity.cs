namespace CSharpCodeRecipes.Recipes.SealedOverride
{
    /// <summary>
    /// 型と ID が同じなら同じものとみなすエンティティの基底
    /// </summary>
    /// <typeparam name="TId">エンティティを識別する値の型</typeparam>
    /// <remarks>
    /// 派生したエンティティによる等価性の上書きの禁止
    /// </remarks>
    public abstract class Entity<TId>
        where TId : notnull
    {
        /// <summary>
        /// ID を決めたエンティティの構築
        /// </summary>
        /// <param name="id">エンティティの生存期間を通じて変わらない識別子</param>
        protected Entity(TId id)
        {
            Id = id;
        }

        /// <summary>
        /// エンティティを識別する値
        /// </summary>
        /// <value>構築時に決めて変わらない識別子</value>
        public TId Id { get; }

        /// <summary>
        /// 型と ID による同じエンティティかの判定
        /// </summary>
        /// <param name="obj">比べる相手</param>
        /// <returns>相手が同じ型で同じ ID のエンティティなら true</returns>
        public sealed override bool Equals(object? obj)
        {
            return obj is Entity<TId> other
                && other.GetType() == GetType()
                && EqualityComparer<TId>.Default.Equals(Id, other.Id);
        }

        /// <summary>
        /// 型と ID から求めるハッシュ値
        /// </summary>
        /// <returns>同じ型で同じ ID のエンティティどうしで等しいハッシュ値</returns>
        public sealed override int GetHashCode()
        {
            return HashCode.Combine(GetType(), Id);
        }
    }
}
