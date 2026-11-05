using CSharpCodeRecipes.Recipes;

namespace CSharpCodeRecipes.Tests.Recipes
{
    /// <summary>
    /// <see cref="RecipeCatalog"/> の仕様の検査
    /// </summary>
    public class RecipeCatalogTests
    {
        /// <summary>
        /// このテストアセンブリから検出した名前順のダミーレシピ
        /// </summary>
        private static readonly IRecipe[] DummyRecipes = RecipeCatalog.Discover(typeof(RecipeCatalogTests).Assembly);

        /// <summary>
        /// 定義順によらない名前順でのレシピの検出の検査
        /// </summary>
        [Fact]
        public void Discover_FindsRecipesSortedByName()
        {
            Assert.Equal(new[] { "Alpha", "Beta" }, DummyRecipes.Select(recipe => recipe.Name));
        }

        /// <summary>
        /// 大文字小文字を区別しない名前でのレシピの検索の検査
        /// </summary>
        /// <param name="key">大文字小文字が実際の名前と異なる検索キー</param>
        /// <param name="expectedName">検索で見つかるべきレシピの名前</param>
        [Theory]
        [InlineData("alpha", "Alpha")]
        [InlineData("BETA", "Beta")]
        public void Find_ByName_IgnoresCase(string key, string expectedName)
        {
            Assert.Equal(expectedName, RecipeCatalog.Find(DummyRecipes, key)?.Name);
        }

        /// <summary>
        /// 1始まりの番号での一覧の位置のレシピの検索の検査
        /// </summary>
        /// <param name="key">一覧の番号を表す検索キー</param>
        /// <param name="expectedName">検索で見つかるべきレシピの名前</param>
        [Theory]
        [InlineData("1", "Alpha")]
        [InlineData("2", "Beta")]
        public void Find_ByNumber_ReturnsRecipeAtPosition(string key, string expectedName)
        {
            Assert.Equal(expectedName, RecipeCatalog.Find(DummyRecipes, key)?.Name);
        }

        /// <summary>
        /// 該当するレシピがない検索での null の検査
        /// </summary>
        /// <param name="key">どのレシピの名前にも一覧の範囲の番号にも当たらない検索キー</param>
        [Theory]
        [InlineData("Unknown")]
        [InlineData("0")]
        [InlineData("3")]
        [InlineData("-1")]
        public void Find_Unknown_ReturnsNull(string key)
        {
            Assert.Null(RecipeCatalog.Find(DummyRecipes, key));
        }

        /// <summary>
        /// 大文字小文字を区別しない比較での全レシピの名前の一意性の検査
        /// </summary>
        [Fact]
        public void All_NamesAreUnique()
        {
            var duplicates = RecipeCatalog.All
                .GroupBy(recipe => recipe.Name, StringComparer.OrdinalIgnoreCase)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key);

            Assert.Empty(duplicates);
        }

        /// <summary>
        /// 全レシピの名前とフォルダ名を表す名前空間の末尾との一致の検査
        /// </summary>
        [Fact]
        public void All_NameMatchesFolderName()
        {
            Assert.All(RecipeCatalog.All, recipe =>
            {
                var folderName = recipe.GetType().Namespace?.Split('.').Last();
                Assert.Equal(folderName, recipe.Name);
            });
        }

        /// <summary>
        /// 全レシピの空白だけではない概要の検査
        /// </summary>
        [Fact]
        public void All_HasDescription()
        {
            Assert.All(RecipeCatalog.All, recipe => Assert.False(string.IsNullOrWhiteSpace(recipe.Description)));
        }

        // 定義順のままでも名前順になり並べ替えを検査できないため Alpha を先に置く定義順は不可
        /// <summary>
        /// 名前順で2番目になるダミーレシピ
        /// </summary>
        private sealed class BetaRecipe : IRecipe
        {
            /// <inheritdoc />
            string IRecipe.Name => "Beta";

            /// <inheritdoc />
            string IRecipe.Description => "テスト用";

            /// <inheritdoc />
            void IRecipe.Run()
            {
            }
        }

        /// <summary>
        /// 名前順で1番目になるダミーレシピ
        /// </summary>
        private sealed class AlphaRecipe : IRecipe
        {
            /// <inheritdoc />
            string IRecipe.Name => "Alpha";

            /// <inheritdoc />
            string IRecipe.Description => "テスト用";

            /// <inheritdoc />
            void IRecipe.Run()
            {
            }
        }
    }
}
