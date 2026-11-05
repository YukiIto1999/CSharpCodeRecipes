namespace CSharpCodeRecipes.Recipes.RequiredOptions
{
    /// <summary>
    /// 基盤の設定項目の設定漏れをコンパイルエラーにするレシピ
    /// </summary>
    internal sealed class RequiredOptionsRecipe : IRecipe
    {
        /// <inheritdoc />
        string IRecipe.Name => "RequiredOptions";

        /// <inheritdoc />
        string IRecipe.Description => "基盤の設定項目の設定漏れをコンパイルエラーにする";

        /// <inheritdoc />
        void IRecipe.Run()
        {
            var sender = new MailSender(new SmtpOptions { Host = "smtp.example.com", Port = 587 });
            Console.WriteLine($"接続先: {sender.Endpoint} 待ち時間: {sender.Timeout.TotalSeconds} 秒");
        }
    }
}
