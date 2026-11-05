namespace CSharpCodeRecipes.Recipes.CapabilityConstraint
{
    /// <summary>
    /// 領収書の番号を採番する能力を持つ実行環境
    /// </summary>
    public interface IHasReceiptNumbers
    {
        /// <summary>
        /// 未使用の領収書の番号の採番
        /// </summary>
        /// <returns>この環境でまだ使っていない領収書の番号</returns>
        public string NextReceiptNumber();
    }
}
