namespace CSharpCodeRecipes.Recipes.PlatformOnlyMember
{
    /// <summary>
    /// ドメイン側の集約が継承して業務上の出来事を記録する基底
    /// </summary>
    /// <remarks>
    /// 記録は継承した集約自身だけに許し取り出しは基盤のアセンブリだけに許す公開範囲
    /// </remarks>
    public abstract class AggregateRoot
    {
        /// <summary>
        /// 記録された順の未発行の出来事
        /// </summary>
        private readonly List<IDomainEvent> pendingEvents = [];

        /// <summary>
        /// 集約自身による業務上の出来事の記録
        /// </summary>
        /// <param name="domainEvent">集約の状態の変化として起きた出来事</param>
        protected void Raise(IDomainEvent domainEvent)
        {
            pendingEvents.Add(domainEvent);
        }

        /// <summary>
        /// 基盤の保存処理による未発行の出来事の取り出し
        /// </summary>
        /// <returns>記録された順の未発行の出来事</returns>
        /// <remarks>
        /// 取り出した出来事の集約からの消去
        /// </remarks>
        internal IReadOnlyList<IDomainEvent> DequeueEvents()
        {
            var events = pendingEvents.ToArray();
            pendingEvents.Clear();
            return events;
        }
    }
}
