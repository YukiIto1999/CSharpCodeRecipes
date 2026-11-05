namespace CSharpCodeRecipes.Recipes.ImplicitUnion
{
    /// <summary>
    /// 検索した会員番号に該当する会員の不在
    /// </summary>
    /// <param name="Id">検索に使った会員番号</param>
    public sealed record MemberNotFound(int Id);
}
