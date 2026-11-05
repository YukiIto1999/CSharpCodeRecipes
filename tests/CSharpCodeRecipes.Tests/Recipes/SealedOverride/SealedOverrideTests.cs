using CSharpCodeRecipes.Recipes.SealedOverride;
using CSharpCodeRecipes.Tests.Compiling;

namespace CSharpCodeRecipes.Tests.Recipes.SealedOverride
{
    /// <summary>
    /// sealed override による基底クラスの等価性の固定の検査
    /// </summary>
    public class SealedOverrideTests
    {
        /// <summary>
        /// ドメイン側のエンティティによる等価性の上書きのコンパイルエラーの検査
        /// </summary>
        [Fact]
        public void OverrideEquals_FailsToCompile()
        {
            const string source = """
                using CSharpCodeRecipes.Recipes.SealedOverride;

                public sealed class Product : Entity<int>
                {
                    public Product(int id)
                        : base(id)
                    {
                    }

                    public override bool Equals(object? obj) => ReferenceEquals(this, obj);

                    public override int GetHashCode() => 0;
                }
                """;

            Assert.Equal(["CS0239"], ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// 等価性に触れないドメイン側のエンティティの成功の検査
        /// </summary>
        [Fact]
        public void DomainEntity_Compiles()
        {
            const string source = """
                using CSharpCodeRecipes.Recipes.SealedOverride;

                public sealed class Product : Entity<int>
                {
                    public Product(int id, string name)
                        : base(id)
                    {
                        Name = name;
                    }

                    public string Name { get; }
                }
                """;

            Assert.Empty(ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// 型では防げない new による Equals の隠蔽の成功の検査
        /// </summary>
        [Fact]
        public void HidingEqualsWithNew_StillCompiles()
        {
            const string source = """
                using CSharpCodeRecipes.Recipes.SealedOverride;

                public sealed class Product : Entity<int>
                {
                    public Product(int id)
                        : base(id)
                    {
                    }

                    public new bool Equals(object? obj) => ReferenceEquals(this, obj);
                }
                """;

            Assert.Empty(ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// 名前が違っても同じ ID なら同じ顧客とみなすことの検査
        /// </summary>
        [Fact]
        public void SameId_IsSameEntity()
        {
            var before = new Customer(1, "佐藤商店");
            var after = new Customer(1, "株式会社サトウ");

            Assert.Equal(before, after);
            Assert.Single(new HashSet<Customer> { before, after });
        }

        /// <summary>
        /// ID が同じでも型の違うエンティティを別とみなすことの検査
        /// </summary>
        [Fact]
        public void SameIdDifferentType_IsDifferentEntity()
        {
            Assert.NotEqual<object>(new Customer(1, "佐藤商店"), new Supplier(1));
        }

        /// <summary>
        /// 隠蔽した Equals を使わない基底の型を経由した比較の検査
        /// </summary>
        [Fact]
        public void HiddenEquals_IsIgnoredThroughBaseType()
        {
            var first = new Hiding(1);
            var second = new Hiding(1);

            Assert.False(first.Equals(second));
            Assert.True(((object)first).Equals(second));
        }

        /// <summary>
        /// 型の違いの検査に使う仕入先のエンティティ
        /// </summary>
        /// <param name="id">仕入先を識別する番号</param>
        private sealed class Supplier(int id) : Entity<int>(id);

        /// <summary>
        /// new で Equals を隠蔽したエンティティ
        /// </summary>
        /// <param name="id">エンティティを識別する番号</param>
        private sealed class Hiding(int id) : Entity<int>(id)
        {
            /// <summary>
            /// 参照による比較での基底の等価性の隠蔽
            /// </summary>
            /// <param name="obj">比べる相手</param>
            /// <returns>相手が同じインスタンスなら true</returns>
            public new bool Equals(object? obj) => ReferenceEquals(this, obj);
        }
    }
}
