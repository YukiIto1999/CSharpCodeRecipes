namespace CSharpCodeRecipes.Recipes.SealedOverride
{
    /// <summary>
    /// 名前が変わっても同じ顧客として扱う顧客のエンティティ
    /// </summary>
    /// <remarks>
    /// 実際の構成ではドメイン側のアセンブリに置くエンティティの例
    /// </remarks>
    internal sealed class Customer : Entity<int>
    {
        /// <summary>
        /// 顧客番号と名前を決めた顧客の作成
        /// </summary>
        /// <param name="id">顧客を識別する番号</param>
        /// <param name="name">表示に使う顧客の名前</param>
        public Customer(int id, string name)
            : base(id)
        {
            Name = name;
        }

        /// <summary>
        /// 表示に使う顧客の名前
        /// </summary>
        /// <value>作成時か最後の改名で決めた名前</value>
        public string Name { get; private set; }

        /// <summary>
        /// 顧客の改名
        /// </summary>
        /// <param name="name">新しい名前</param>
        public void Rename(string name)
        {
            Name = name;
        }
    }
}
