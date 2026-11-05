namespace CSharpCodeRecipes.Recipes.ImplicitUnion
{
    /// <summary>
    /// 会員番号で引ける登録済みの会員
    /// </summary>
    /// <param name="Id">会員を識別する番号</param>
    /// <param name="Name">表示に使う会員の名前</param>
    public sealed record Member(int Id, string Name);
}
