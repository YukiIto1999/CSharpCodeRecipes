using CSharpCodeRecipes.NoPermissionPublicMethod;

namespace CSharpCodeRecipes
{
    /// <summary>
    /// エントリークラス
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// エントリーポイント
        /// </summary>
        /// <param name="args">コマンドライン引数</param>
        static void Main(string[] args)
        {
            ExecuteNoPermissionPublicMethod();
        }

        /// <summary>
        /// Override以外のPublicメソッド作成禁止の例
        /// </summary>
        private static void ExecuteNoPermissionPublicMethod()
        {
            // インタフェース経由でのみメソッド呼び出しが可能なため、事実上Publicメソッドの作成を禁止できる
            var service = new ServiceImpl() as IService;
            service.Execute();
        }
    }
}
