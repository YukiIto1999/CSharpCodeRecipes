namespace CSharpCodeRecipes.Recipes.NoPermissionPublicMethod
{
    /// <summary>
    /// Override以外のPublicメソッドの作成禁止を実演するレシピ
    /// </summary>
    internal sealed class NoPermissionPublicMethodRecipe : IRecipe
    {
        /// <inheritdoc />
        string IRecipe.Name => "NoPermissionPublicMethod";

        /// <inheritdoc />
        string IRecipe.Description => "Override以外のPublicメソッドの作成禁止";

        /// <inheritdoc />
        void IRecipe.Run()
        {
            // 明示的なインタフェース実装のため ServiceImpl 型のままでの Execute の呼出しは不可
            var service = new ServiceImpl() as IService;
            service.Execute();
        }
    }
}
