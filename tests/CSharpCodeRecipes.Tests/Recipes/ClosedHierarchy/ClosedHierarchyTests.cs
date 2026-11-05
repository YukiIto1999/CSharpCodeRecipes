using System.Reflection;
using CSharpCodeRecipes.Recipes.ClosedHierarchy;
using CSharpCodeRecipes.Tests.Compiling;

namespace CSharpCodeRecipes.Tests.Recipes.ClosedHierarchy
{
    /// <summary>
    /// <see cref="PaymentMethod"/> の階層の閉じ方の検査
    /// </summary>
    public class ClosedHierarchyTests
    {
        /// <summary>
        /// コピーコンストラクタ経由の別アセンブリからの派生のコンパイルエラーの検査
        /// </summary>
        [Fact]
        public void ExternalDerivation_FailsToCompile()
        {
            const string source = """
                using CSharpCodeRecipes.Recipes.ClosedHierarchy;

                public sealed record Points : PaymentMethod
                {
                    public Points(PaymentMethod original)
                        : base(original)
                    {
                    }
                }
                """;

            Assert.Equal(["CS0534"], ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// 宣言済みのバリアントの別アセンブリからの利用の成功の検査
        /// </summary>
        [Fact]
        public void ExternalUseOfVariants_Compiles()
        {
            const string source = """
                using CSharpCodeRecipes.Recipes.ClosedHierarchy;

                public static class Checkout
                {
                    public static string Describe(PaymentMethod method) => method.Label;

                    public static PaymentMethod Default() => new PaymentMethod.Cash();
                }
                """;

            Assert.Empty(ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// 外部からの派生を塞ぐ private protected の抽象メンバーの存在の検査
        /// </summary>
        [Fact]
        public void Base_DeclaresPrivateProtectedAbstractMember()
        {
            var gates = typeof(PaymentMethod)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(method => method.IsAbstract && method.IsFamilyAndAssembly);

            Assert.NotEmpty(gates);
        }

        /// <summary>
        /// 同じアセンブリ内の派生が基底にネストした sealed のバリアントに限られることの検査
        /// </summary>
        [Fact]
        public void Variants_AreNestedAndSealed()
        {
            var variants = typeof(PaymentMethod).Assembly.GetTypes()
                .Where(type => type != typeof(PaymentMethod) && typeof(PaymentMethod).IsAssignableFrom(type));

            Assert.All(variants, variant =>
            {
                Assert.Equal(typeof(PaymentMethod), variant.DeclaringType);
                Assert.True(variant.IsSealed, $"{variant.Name} を sealed にする。");
            });
        }

        /// <summary>
        /// バリアントごとの表示名の検査
        /// </summary>
        [Fact]
        public void Label_DependsOnVariant()
        {
            Assert.Equal("現金", new PaymentMethod.Cash().Label);
            Assert.Equal("銀行振込（サンプル銀行）", new PaymentMethod.BankTransfer("サンプル銀行").Label);
        }
    }
}
