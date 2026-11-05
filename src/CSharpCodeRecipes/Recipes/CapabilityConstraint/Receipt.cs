namespace CSharpCodeRecipes.Recipes.CapabilityConstraint
{
    /// <summary>
    /// 支払を受けた証明として渡す領収書
    /// </summary>
    /// <param name="Number">採番された領収書の番号</param>
    /// <param name="Amount">受け取った金額</param>
    /// <param name="IssuedAt">発行した UTC の日時</param>
    public sealed record Receipt(string Number, decimal Amount, DateTimeOffset IssuedAt);
}
