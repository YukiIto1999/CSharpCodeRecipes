namespace CSharpCodeRecipes.Recipes.Typestate
{
    /// <summary>
    /// これ以上の操作を持たない発送済みの注文
    /// </summary>
    public sealed class ShippedOrder
    {
        /// <summary>
        /// <see cref="PaidOrder.Ship"/> だけが行う発送済みの注文の構築
        /// </summary>
        /// <param name="orderNumber">支払済みの注文から引き継ぐ番号</param>
        /// <param name="amount">支払済みの注文から引き継ぐ金額</param>
        /// <param name="paidAt">支払済みの注文から引き継ぐ支払日時</param>
        /// <param name="trackingNumber">配送業者が発行した追跡番号</param>
        internal ShippedOrder(string orderNumber, decimal amount, DateTimeOffset paidAt, string trackingNumber)
        {
            OrderNumber = orderNumber;
            Amount = amount;
            PaidAt = paidAt;
            TrackingNumber = trackingNumber;
        }

        /// <summary>
        /// 注文を識別する番号
        /// </summary>
        /// <value>受注時に決めた番号</value>
        public string OrderNumber { get; }

        /// <summary>
        /// 支払われた金額
        /// </summary>
        /// <value>受注時に決めた金額</value>
        public decimal Amount { get; }

        /// <summary>
        /// 支払を受け付けた日時
        /// </summary>
        /// <value>支払の操作で渡された日時</value>
        public DateTimeOffset PaidAt { get; }

        /// <summary>
        /// 配送状況の照会に使う追跡番号
        /// </summary>
        /// <value>発送の操作で渡された追跡番号</value>
        public string TrackingNumber { get; }
    }
}
