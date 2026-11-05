namespace CSharpCodeRecipes.Recipes.StaticAbstractContract
{
    /// <summary>
    /// 静的メンバーの規約を static abstract で型に出すレシピ
    /// </summary>
    internal sealed class StaticAbstractContractRecipe : IRecipe
    {
        /// <inheritdoc />
        string IRecipe.Name => "StaticAbstractContract";

        /// <inheritdoc />
        string IRecipe.Description => "静的メンバーの規約を static abstract で型に出す";

        /// <inheritdoc />
        void IRecipe.Run()
        {
            var settings = new SettingsReader(new Dictionary<string, string>
            {
                ["Host"] = " db.example.com ",
                ["Port"] = "5432",
            });

            var host = settings.Get<HostName>("Host");
            var port = settings.Get<Port>("Port");
            Console.WriteLine($"{host.Value}:{port.Number}");
        }
    }
}
