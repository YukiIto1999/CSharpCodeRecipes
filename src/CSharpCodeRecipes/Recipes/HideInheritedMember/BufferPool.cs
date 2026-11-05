namespace CSharpCodeRecipes.Recipes.HideInheritedMember
{
    /// <summary>
    /// 返すときに中身を消す固定長のバイト配列のプール
    /// </summary>
    public sealed class BufferPool : Pool<byte[]>
    {
        /// <summary>
        /// 貸し出すバッファの長さを決めたプールの作成
        /// </summary>
        /// <param name="length">新しく作るバッファのバイト数</param>
        public BufferPool(int length)
            : base(() => new byte[length])
        {
        }

        /// <summary>
        /// 中身を消した上でのバッファの返却
        /// </summary>
        /// <param name="buffer">このプールから借りて使い終えたバッファ</param>
        public void ClearAndReturn(byte[] buffer)
        {
            Array.Clear(buffer);
            base.Return(buffer);
        }

        /// <summary>
        /// 中身を残したままの返却の禁止
        /// </summary>
        /// <param name="buffer">返そうとしたバッファ</param>
        /// <exception cref="NotSupportedException">リフレクションのようなコンパイラの検査を経ない全ての呼出し</exception>
        [Obsolete("中身を残したまま返すと次に借りた処理に前の内容が見えるので ClearAndReturn を使う。", error: true)]
        public new void Return(byte[] buffer)
        {
            throw new NotSupportedException("ClearAndReturn を使う。");
        }
    }
}
