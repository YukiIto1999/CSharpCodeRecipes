namespace CSharpCodeRecipes.Recipes.CapabilityConstraint
{
    /// <summary>
    /// 必要な依存をジェネリック制約で型に出すレシピ
    /// </summary>
    internal sealed class CapabilityConstraintRecipe : IRecipe
    {
        /// <inheritdoc />
        string IRecipe.Name => "CapabilityConstraint";

        /// <inheritdoc />
        string IRecipe.Description => "必要な依存をジェネリック制約で型に出す";

        /// <inheritdoc />
        void IRecipe.Run()
        {
            var environment = new ApplicationEnvironment();
            foreach (var amount in new[] { 1200m, 800m })
            {
                var receipt = Checkout.Complete(environment, amount);
                Console.WriteLine($"{receipt.Number}: {receipt.Amount:N0} 円 {receipt.IssuedAt.ToLocalTime():yyyy-MM-dd HH:mm}");
            }
        }
    }
}
