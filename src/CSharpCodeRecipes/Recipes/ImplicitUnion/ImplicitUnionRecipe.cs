namespace CSharpCodeRecipes.Recipes.ImplicitUnion
{
    /// <summary>
    /// 返せる型を型引数に限る和型を実演するレシピ
    /// </summary>
    internal sealed class ImplicitUnionRecipe : IRecipe
    {
        /// <inheritdoc />
        string IRecipe.Name => "ImplicitUnion";

        /// <inheritdoc />
        string IRecipe.Description => "返せる型を型引数に限る和型";

        /// <inheritdoc />
        void IRecipe.Run()
        {
            var directory = new MemberDirectory([new Member(1, "佐藤"), new Member(2, "鈴木")]);
            foreach (var id in new[] { 1, 3 })
            {
                var message = directory.Find(id).Match(
                    member => $"会員 {member.Id}: {member.Name}",
                    notFound => $"会員 {notFound.Id}: 該当なし");
                Console.WriteLine(message);
            }
        }
    }
}
