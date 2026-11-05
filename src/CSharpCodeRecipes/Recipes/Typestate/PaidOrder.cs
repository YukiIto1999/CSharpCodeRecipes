namespace CSharpCodeRecipes.Recipes.Typestate
{
    /// <summary>
    /// 発送の操作だけを持つ支払済みの注文
    /// </summary>
    public sealed class PaidOrder
    {
        /// <summary>
        /// <see cref="UnpaidOrder.Pay"/> だけが行う支払済みの注文の構築
        /// </summary>
        /// <param name="orderNumber">未払いの注文から引き継ぐ番号</param>
        /// <param name="amount">未払いの注文から引き継ぐ金額</param>
        /// <param name="paidAt">支払を受け付けた日時</param>
        internal PaidOrder(string orderNumber, decimal amount, DateTimeOffset paidAt)
        {
            OrderNumber = orderNumber;
            Amount = amount;
            PaidAt = paidAt;
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
        /// 発送による発送済みの注文への遷移
        /// </summary>
        /// <param name="trackingNumber">配送業者が発行した追跡番号</param>
        /// <returns>番号と金額と支払日時を引き継いだ発送済みの注文</returns>
        public ShippedOrder Ship(string trackingNumber)
        {
            return new ShippedOrder(OrderNumber, Amount, PaidAt, trackingNumber);
        }
    }
}
