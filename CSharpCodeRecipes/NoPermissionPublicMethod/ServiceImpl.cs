namespace CSharpCodeRecipes.NoPermissionPublicMethod
{
    /// <summary>
    /// サービス実装クラス
    /// </summary>
    internal sealed class ServiceImpl: IService
    {
        /// <summary>
        /// 実行メソッド
        /// </summary>
        void IService.Execute()
        {
            Console.WriteLine("Hello World!");
        }
    }
}
