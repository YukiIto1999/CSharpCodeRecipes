namespace CSharpCodeRecipes.Recipes.PlatformOnlyMember
{
    /// <summary>
    /// 注文が確定したという出来事
    /// </summary>
    /// <param name="OrderNumber">確定した注文の番号</param>
    internal sealed record OrderPlaced(string OrderNumber) : IDomainEvent;
}
