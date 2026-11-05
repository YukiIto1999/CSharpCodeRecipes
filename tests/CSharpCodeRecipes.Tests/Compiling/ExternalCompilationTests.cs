namespace CSharpCodeRecipes.Tests.Compiling
{
    /// <summary>
    /// <see cref="ExternalCompilation"/> の仕様の検査
    /// </summary>
    public class ExternalCompilationTests
    {
        /// <summary>
        /// レシピの公開型だけを使うソースでのエラーの不在の検査
        /// </summary>
        [Fact]
        public void ErrorIds_PublicTypesOnly_ReturnsEmpty()
        {
            const string source = """
                using CSharpCodeRecipes.Recipes.NoPermissionPublicMethod;

                public sealed class Consumer : IService
                {
                    void IService.Execute()
                    {
                    }
                }
                """;

            Assert.Empty(ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// InternalsVisibleTo の対象外としてのレシピの内部型へのアクセスの拒否の検査
        /// </summary>
        [Fact]
        public void ErrorIds_InternalType_ReportsInaccessible()
        {
            const string source = """
                using CSharpCodeRecipes.Recipes.NoPermissionPublicMethod;

                public static class Consumer
                {
                    public static IService Create() => new ServiceImpl();
                }
                """;

            Assert.Equal(["CS0122"], ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// 警告のエラーとしての報告の検査
        /// </summary>
        [Fact]
        public void ErrorIds_Warning_ReportsAsError()
        {
            const string source = """
                public static class Consumer
                {
                    public static void Run()
                    {
                        var unused = 0;
                    }
                }
                """;

            Assert.Equal(["CS0219"], ExternalCompilation.ErrorIds(source));
        }
    }
}
