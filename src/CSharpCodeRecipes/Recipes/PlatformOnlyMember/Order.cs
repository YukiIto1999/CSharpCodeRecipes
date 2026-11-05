namespace CSharpCodeRecipes.Recipes.PlatformOnlyMember
{
    /// <summary>
    /// 確定の操作で出来事を記録する注文の集約
    /// </summary>
    /// <remarks>
    /// 実際の構成ではドメイン側のアセンブリに置く集約の例
    /// </remarks>
    internal sealed class Order : AggregateRoot
    {
        /// <summary>
        /// 番号を決めた未確定の注文の作成
        /// </summary>
        /// <param name="orderNumber">注文を識別する番号</param>
        public Order(string orderNumber)
        {
            OrderNumber = orderNumber;
        }

        /// <summary>
        /// 注文を識別する番号
        /// </summary>
        /// <value>作成時に決めた番号</value>
        public string OrderNumber { get; }

        /// <summary>
        /// 注文の確定と確定の出来事の記録
        /// </summary>
        public void Place()
        {
            Raise(new OrderPlaced(OrderNumber));
        }
    }
}
