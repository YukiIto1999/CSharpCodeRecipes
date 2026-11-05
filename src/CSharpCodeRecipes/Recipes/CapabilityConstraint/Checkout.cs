namespace CSharpCodeRecipes.Recipes.CapabilityConstraint
{
    /// <summary>
    /// 領収書の発行までを含む会計
    /// </summary>
    public static class Checkout
    {
        /// <summary>
        /// 支払の受付と領収書の発行による会計の完了
        /// </summary>
        /// <typeparam name="TEnvironment"><see cref="Receipts.Issue{TEnvironment}"/> が要求する能力を全て持つ実行環境の型</typeparam>
        /// <param name="environment">会計で使う実行環境</param>
        /// <param name="amount">受け取った金額</param>
        /// <returns>会計の証明として渡す領収書</returns>
        public static Receipt Complete<TEnvironment>(TEnvironment environment, decimal amount)
            where TEnvironment : IHasClock, IHasReceiptNumbers
        {
            return Receipts.Issue(environment, amount);
        }
    }
}
