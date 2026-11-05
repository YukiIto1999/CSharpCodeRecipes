using CSharpCodeRecipes.Recipes;

namespace CSharpCodeRecipes
{
    /// <summary>
    /// レシピを選んで実行するコンソールアプリの入口
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// 引数か対話入力で指定したレシピの実行
        /// </summary>
        /// <param name="args">先頭の要素に置く実行するレシピの名前か番号</param>
        /// <returns>該当するレシピがない場合だけ 1 になる終了コード</returns>
        /// <remarks>
        /// 引数がない場合の一覧の表示と標準入力からの指定の受付
        /// </remarks>
        static int Main(string[] args)
        {
            var key = args.Length > 0 ? args[0] : Prompt();
            if (string.IsNullOrWhiteSpace(key))
            {
                return 0;
            }

            var recipe = RecipeCatalog.Find(key.Trim());
            if (recipe is null)
            {
                Console.Error.WriteLine($"レシピが見つかりません: {key}");
                WriteRecipeList();
                return 1;
            }

            Console.WriteLine($"--- {recipe.Name}: {recipe.Description} ---");
            recipe.Run();
            return 0;
        }

        /// <summary>
        /// 一覧を表示した上での実行するレシピの対話入力
        /// </summary>
        /// <returns>入力された文字列か標準入力の終端での null</returns>
        private static string? Prompt()
        {
            WriteRecipeList();
            Console.Write("実行するレシピの番号または名前を入力してください: ");
            return Console.ReadLine();
        }

        /// <summary>
        /// 番号付きのレシピ一覧の標準出力への表示
        /// </summary>
        private static void WriteRecipeList()
        {
            Console.WriteLine("レシピ一覧:");
            for (var i = 0; i < RecipeCatalog.All.Count; i++)
            {
                var recipe = RecipeCatalog.All[i];
                Console.WriteLine($"  {i + 1}. {recipe.Name} - {recipe.Description}");
            }
        }
    }
}
