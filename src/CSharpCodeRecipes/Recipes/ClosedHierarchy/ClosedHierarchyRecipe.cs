namespace CSharpCodeRecipes.Recipes.ClosedHierarchy
{
    /// <summary>
    /// 別アセンブリからの派生を禁じる閉じた型階層を実演するレシピ
    /// </summary>
    internal sealed class ClosedHierarchyRecipe : IRecipe
    {
        /// <inheritdoc />
        string IRecipe.Name => "ClosedHierarchy";

        /// <inheritdoc />
        string IRecipe.Description => "別アセンブリからの派生を禁じる閉じた型階層";

        /// <inheritdoc />
        void IRecipe.Run()
        {
            PaymentMethod[] methods = [new PaymentMethod.Cash(), new PaymentMethod.BankTransfer("サンプル銀行")];
            foreach (var method in methods)
            {
                Console.WriteLine(method.Label);
            }
        }
    }
}
