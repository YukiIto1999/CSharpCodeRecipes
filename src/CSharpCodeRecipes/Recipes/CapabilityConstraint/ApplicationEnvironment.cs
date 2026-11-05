namespace CSharpCodeRecipes.Recipes.CapabilityConstraint
{
    /// <summary>
    /// 実行時の時計と連番の採番を持つアプリの実行環境
    /// </summary>
    internal sealed class ApplicationEnvironment : IHasClock, IHasReceiptNumbers
    {
        /// <summary>
        /// これまでに採番した領収書の数
        /// </summary>
        private int issuedCount;

        /// <inheritdoc />
        public TimeProvider Clock => TimeProvider.System;

        /// <inheritdoc />
        public string NextReceiptNumber()
        {
            issuedCount++;
            return $"R-{issuedCount:D4}";
        }
    }
}
