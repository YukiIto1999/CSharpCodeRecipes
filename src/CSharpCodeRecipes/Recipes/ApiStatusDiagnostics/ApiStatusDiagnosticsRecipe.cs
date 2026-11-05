namespace CSharpCodeRecipes.Recipes.ApiStatusDiagnostics
{
    /// <summary>
    /// 基盤 API の試験的と非推奨を診断 ID で伝えるレシピ
    /// </summary>
    internal sealed class ApiStatusDiagnosticsRecipe : IRecipe
    {
        /// <inheritdoc />
        string IRecipe.Name => "ApiStatusDiagnostics";

        /// <inheritdoc />
        string IRecipe.Description => "基盤 API の試験的と非推奨を診断 ID で伝える";

        /// <inheritdoc />
        void IRecipe.Run()
        {
            var notifier = new Notifier();
            notifier.Notify("佐藤", "受注しました");
#pragma warning disable RECIPE0001
            notifier.NotifyAll(["鈴木", "高橋"], "メンテナンスのお知らせ");
#pragma warning restore RECIPE0001

            foreach (var notification in notifier.Sent)
            {
                Console.WriteLine(notification);
            }
        }
    }
}
