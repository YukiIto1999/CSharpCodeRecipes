namespace CSharpCodeRecipes.Recipes.Typestate
{
    /// <summary>
    /// 状態ごとの型で誤った操作を禁じるレシピ
    /// </summary>
    internal sealed class TypestateRecipe : IRecipe
    {
        /// <inheritdoc />
        string IRecipe.Name => "Typestate";

        /// <inheritdoc />
        string IRecipe.Description => "状態ごとの型で誤った操作を禁じる";

        /// <inheritdoc />
        void IRecipe.Run()
        {
            var unpaid = new UnpaidOrder("A-001", 3200m);
            var paid = unpaid.Pay(new DateTimeOffset(2026, 11, 5, 10, 0, 0, TimeSpan.FromHours(9)));
            var shipped = paid.Ship("TRK-123");
            Console.WriteLine($"{shipped.OrderNumber}: {shipped.Amount:N0} 円 支払 {shipped.PaidAt:yyyy-MM-dd HH:mm} 追跡番号 {shipped.TrackingNumber}");
        }
    }
}
