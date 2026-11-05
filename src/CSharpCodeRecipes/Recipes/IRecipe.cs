namespace CSharpCodeRecipes.Recipes
{
    /// <summary>
    /// 一覧からの選択と実行の対象になるレシピ
    /// </summary>
    /// <remarks>
    /// 引数なしのコンストラクタを前提とする <see cref="RecipeCatalog"/> の自動検出の対象
    /// </remarks>
    internal interface IRecipe
    {
        /// <summary>
        /// 実行時の引数と一覧表示で使うレシピの識別名
        /// </summary>
        /// <value>レシピのフォルダ名と一致する名前</value>
        /// <remarks>
        /// 大文字小文字を区別しない比較での全レシピ間の一意性
        /// </remarks>
        public string Name { get; }

        /// <summary>
        /// 一覧でレシピ名に添えて表示する概要
        /// </summary>
        /// <value>空白だけではない一行の説明</value>
        public string Description { get; }

        /// <summary>
        /// レシピが示すパターンの実演
        /// </summary>
        /// <remarks>
        /// 実演結果の標準出力への書き出し
        /// </remarks>
        public void Run();
    }
}
