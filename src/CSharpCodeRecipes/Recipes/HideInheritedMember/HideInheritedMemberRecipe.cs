namespace CSharpCodeRecipes.Recipes.HideInheritedMember
{
    /// <summary>
    /// 継承したメソッドの呼出しをコンパイルエラーにするレシピ
    /// </summary>
    internal sealed class HideInheritedMemberRecipe : IRecipe
    {
        /// <inheritdoc />
        string IRecipe.Name => "HideInheritedMember";

        /// <inheritdoc />
        string IRecipe.Description => "継承したメソッドの呼出しをコンパイルエラーにする";

        /// <inheritdoc />
        void IRecipe.Run()
        {
            var pool = new BufferPool(4);
            var buffer = pool.Rent();
            buffer[0] = 42;
            pool.ClearAndReturn(buffer);

            var reused = pool.Rent();
            Console.WriteLine($"同じバッファの再利用: {ReferenceEquals(buffer, reused)} 先頭のバイト: {reused[0]}");
        }
    }
}
