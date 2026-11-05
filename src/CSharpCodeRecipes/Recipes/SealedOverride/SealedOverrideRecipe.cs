namespace CSharpCodeRecipes.Recipes.SealedOverride
{
    /// <summary>
    /// 基底クラスの振る舞いをドメイン側に上書きさせないレシピ
    /// </summary>
    internal sealed class SealedOverrideRecipe : IRecipe
    {
        /// <inheritdoc />
        string IRecipe.Name => "SealedOverride";

        /// <inheritdoc />
        string IRecipe.Description => "基底クラスの振る舞いをドメイン側に上書きさせない";

        /// <inheritdoc />
        void IRecipe.Run()
        {
            var before = new Customer(1, "佐藤商店");
            var after = new Customer(1, "佐藤商店");
            after.Rename("株式会社サトウ");

            var customers = new HashSet<Customer> { before, after };
            Console.WriteLine($"同じ顧客: {before.Equals(after)} 集合の要素数: {customers.Count}");
        }
    }
}
