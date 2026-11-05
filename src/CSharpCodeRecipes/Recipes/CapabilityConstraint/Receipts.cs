namespace CSharpCodeRecipes.Recipes.CapabilityConstraint
{
    /// <summary>
    /// 時計と採番の能力を要求する領収書の発行
    /// </summary>
    public static class Receipts
    {
        /// <summary>
        /// 環境の時計と採番による領収書の発行
        /// </summary>
        /// <typeparam name="TEnvironment">時計と採番の両方の能力を持つ実行環境の型</typeparam>
        /// <param name="environment">発行の日時と番号を決める実行環境</param>
        /// <param name="amount">受け取った金額</param>
        /// <returns>環境の時計の UTC の現在時刻で発行した領収書</returns>
        public static Receipt Issue<TEnvironment>(TEnvironment environment, decimal amount)
            where TEnvironment : IHasClock, IHasReceiptNumbers
        {
            return new Receipt(environment.NextReceiptNumber(), amount, environment.Clock.GetUtcNow());
        }
    }
}
