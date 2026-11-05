namespace CSharpCodeRecipes.Recipes.CapabilityConstraint
{
    /// <summary>
    /// 現在時刻を読む能力を持つ実行環境
    /// </summary>
    public interface IHasClock
    {
        /// <summary>
        /// 処理が現在時刻を読むときに使う時計
        /// </summary>
        /// <value>本番では <see cref="TimeProvider.System"/> でテストでは固定の時刻を返す時計</value>
        public TimeProvider Clock { get; }
    }
}
