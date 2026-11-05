namespace CSharpCodeRecipes.Recipes.ClosedHierarchy
{
    /// <summary>
    /// 現金と銀行振込に限る支払方法
    /// </summary>
    /// <remarks>
    /// 別アセンブリでは実装できない <see cref="GetLabel"/> による外部からの派生の禁止
    /// </remarks>
    public abstract record PaymentMethod
    {
        /// <summary>
        /// ネストしたバリアントだけに許す基底の構築
        /// </summary>
        private PaymentMethod()
        {
        }

        /// <summary>
        /// 画面や明細に表示する支払方法の名前
        /// </summary>
        /// <value>バリアントごとに決まる空でない表示名</value>
        public string Label => GetLabel();

        /// <summary>
        /// バリアントごとの表示名の決定
        /// </summary>
        /// <returns>空でない表示名</returns>
        private protected abstract string GetLabel();

        /// <summary>
        /// 現金での支払
        /// </summary>
        public sealed record Cash : PaymentMethod
        {
            /// <inheritdoc />
            private protected override string GetLabel() => "現金";
        }

        /// <summary>
        /// 銀行口座への振込での支払
        /// </summary>
        /// <param name="BankName">振込先の口座がある銀行の名前</param>
        public sealed record BankTransfer(string BankName) : PaymentMethod
        {
            /// <inheritdoc />
            private protected override string GetLabel() => $"銀行振込（{BankName}）";
        }
    }
}
