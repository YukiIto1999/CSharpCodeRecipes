namespace CSharpCodeRecipes.Recipes.PlatformOnlyMember
{
    /// <summary>
    /// 集約の保存に伴って出来事を発行する基盤側の保存先
    /// </summary>
    public sealed class AggregateStore
    {
        /// <summary>
        /// 発行した順の出来事
        /// </summary>
        private readonly List<IDomainEvent> published = [];

        /// <summary>
        /// これまでに発行した出来事
        /// </summary>
        /// <value>発行した順の出来事</value>
        public IReadOnlyList<IDomainEvent> PublishedEvents => published;

        /// <summary>
        /// 集約の保存に伴う未発行の出来事の発行
        /// </summary>
        /// <param name="aggregate">保存する集約</param>
        public void Save(AggregateRoot aggregate)
        {
            published.AddRange(aggregate.DequeueEvents());
        }
    }
}
