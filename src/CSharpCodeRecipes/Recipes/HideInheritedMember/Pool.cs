namespace CSharpCodeRecipes.Recipes.HideInheritedMember
{
    /// <summary>
    /// 借りた物を返して使い回すオブジェクトのプール
    /// </summary>
    /// <typeparam name="T">使い回す物の型</typeparam>
    /// <remarks>
    /// 派生して使う前提でライブラリが提供する基底クラスの代役
    /// </remarks>
    public class Pool<T>
        where T : class
    {
        /// <summary>
        /// 返されて次の貸出しを待つ物
        /// </summary>
        private readonly Stack<T> available = new();

        /// <summary>
        /// 待っている物がないときの新しい物の作り方
        /// </summary>
        private readonly Func<T> create;

        /// <summary>
        /// 新しい物の作り方を決めたプールの作成
        /// </summary>
        /// <param name="create">待っている物がないときに呼ぶ新しい物の作り方</param>
        public Pool(Func<T> create)
        {
            this.create = create;
        }

        /// <summary>
        /// 返された物か新しく作った物の貸出し
        /// </summary>
        /// <returns>最後に返された物か待っている物がない場合の新しい物</returns>
        public T Rent()
        {
            return available.TryPop(out var item) ? item : create();
        }

        /// <summary>
        /// 使い終えた物の次の貸出しへの返却
        /// </summary>
        /// <param name="item">このプールから借りて使い終えた物</param>
        public void Return(T item)
        {
            available.Push(item);
        }
    }
}
