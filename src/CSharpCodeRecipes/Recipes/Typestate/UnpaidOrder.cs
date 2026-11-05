namespace CSharpCodeRecipes.Recipes.Typestate
{
    /// <summary>
    /// 支払の操作だけを持つ未払いの注文
    /// </summary>
    public sealed class UnpaidOrder
    {
        /// <summary>
        /// 受注した時点の未払いの注文の作成
        /// </summary>
        /// <param name="orderNumber">注文を識別する番号</param>
        /// <param name="amount">支払うべき金額</param>
        public UnpaidOrder(string orderNumber, decimal amount)
        {
            OrderNumber = orderNumber;
            Amount = amount;
        }

        /// <summary>
        /// 注文を識別する番号
        /// </summary>
        /// <value>受注時に決めた番号</value>
        public string OrderNumber { get; }

        /// <summary>
        /// 支払うべき金額
        /// </summary>
        /// <value>受注時に決めた金額</value>
        public decimal Amount { get; }

        /// <summary>
        /// 支払による支払済みの注文への遷移
        /// </summary>
        /// <param name="paidAt">支払を受け付けた日時</param>
        /// <returns>番号と金額を引き継いだ支払済みの注文</returns>
        public PaidOrder Pay(DateTimeOffset paidAt)
        {
            return new PaidOrder(OrderNumber, Amount, paidAt);
        }
    }
}
