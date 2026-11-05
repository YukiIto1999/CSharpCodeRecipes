using System.Reflection;

namespace CSharpCodeRecipes.Recipes
{
    /// <summary>
    /// このアプリで実行できるレシピの一覧
    /// </summary>
    internal static class RecipeCatalog
    {
        /// <summary>
        /// このアプリのアセンブリから検出した全レシピ
        /// </summary>
        /// <value>名前の昇順に並んだレシピ</value>
        /// <exception cref="TypeInitializationException">引数なしのコンストラクタを持たないレシピの実装クラスの存在</exception>
        public static IReadOnlyList<IRecipe> All { get; } = Discover(typeof(IRecipe).Assembly);

        /// <summary>
        /// 実行時の引数か対話入力で指定されたレシピの検索
        /// </summary>
        /// <param name="key">大文字小文字を区別しないレシピ名か1始まりの一覧の番号</param>
        /// <returns>該当するレシピか該当がない場合の null</returns>
        public static IRecipe? Find(string key)
        {
            return Find(All, key);
        }

        /// <summary>
        /// 任意の一覧を対象とするレシピの検索
        /// </summary>
        /// <param name="recipes">番号の基準になる並び順のレシピ一覧</param>
        /// <param name="key">大文字小文字を区別しないレシピ名か1始まりの一覧の番号</param>
        /// <returns>該当するレシピか該当がない場合の null</returns>
        internal static IRecipe? Find(IReadOnlyList<IRecipe> recipes, string key)
        {
            if (int.TryParse(key, out var number))
            {
                return number >= 1 && number <= recipes.Count ? recipes[number - 1] : null;
            }

            return recipes.FirstOrDefault(recipe => string.Equals(recipe.Name, key, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// アセンブリ内の全レシピの実装クラスのインスタンス化
        /// </summary>
        /// <param name="assembly">レシピの実装クラスを探すアセンブリ</param>
        /// <returns>名前の昇順に並べたレシピのインスタンス</returns>
        /// <exception cref="MissingMethodException">引数なしのコンストラクタを持たないレシピの実装クラス</exception>
        internal static IRecipe[] Discover(Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(type => type is { IsClass: true, IsAbstract: false } && typeof(IRecipe).IsAssignableFrom(type))
                .Select(type => (IRecipe)Activator.CreateInstance(type, nonPublic: true)!)
                .OrderBy(recipe => recipe.Name, StringComparer.OrdinalIgnoreCase)
                .ToArray();
        }
    }
}
