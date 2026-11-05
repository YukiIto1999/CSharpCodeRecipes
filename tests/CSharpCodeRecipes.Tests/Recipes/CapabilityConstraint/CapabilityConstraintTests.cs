using CSharpCodeRecipes.Recipes.CapabilityConstraint;
using CSharpCodeRecipes.Tests.Compiling;

namespace CSharpCodeRecipes.Tests.Recipes.CapabilityConstraint
{
    /// <summary>
    /// ジェネリック制約による実行環境の能力の要求の検査
    /// </summary>
    public class CapabilityConstraintTests
    {
        /// <summary>
        /// 呼び出し先が要求する能力を宣言しない呼び出し側のコンパイルエラーの検査
        /// </summary>
        [Fact]
        public void CallerWithoutRequiredCapability_FailsToCompile()
        {
            const string source = """
                using CSharpCodeRecipes.Recipes.CapabilityConstraint;

                public static class Register
                {
                    public static Receipt Sell<TEnvironment>(TEnvironment environment)
                        where TEnvironment : IHasClock =>
                        Receipts.Issue(environment, 100m);
                }
                """;

            Assert.Equal(["CS0314"], ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// 要求する能力を持たない実行環境を渡すコードのコンパイルエラーの検査
        /// </summary>
        [Fact]
        public void EnvironmentWithoutRequiredCapability_FailsToCompile()
        {
            const string source = """
                using System;
                using CSharpCodeRecipes.Recipes.CapabilityConstraint;

                public sealed class ClockOnly : IHasClock
                {
                    public TimeProvider Clock => TimeProvider.System;
                }

                public static class Register
                {
                    public static Receipt Sell() => Receipts.Issue(new ClockOnly(), 100m);
                }
                """;

            Assert.Equal(["CS0311"], ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// 要求する能力を宣言して引き継ぐ呼び出し側の成功の検査
        /// </summary>
        [Fact]
        public void CallerDeclaringRequiredCapabilities_Compiles()
        {
            const string source = """
                using CSharpCodeRecipes.Recipes.CapabilityConstraint;

                public static class Register
                {
                    public static Receipt Sell<TEnvironment>(TEnvironment environment)
                        where TEnvironment : IHasClock, IHasReceiptNumbers =>
                        Checkout.Complete(environment, 100m);
                }
                """;

            Assert.Empty(ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// 型では防げない制約に現れない時計の直接の利用の成功の検査
        /// </summary>
        [Fact]
        public void DirectClockUse_StillCompiles()
        {
            const string source = """
                using System;
                using CSharpCodeRecipes.Recipes.CapabilityConstraint;

                public static class Register
                {
                    public static Receipt Sell<TEnvironment>(TEnvironment environment)
                        where TEnvironment : IHasReceiptNumbers =>
                        new Receipt(environment.NextReceiptNumber(), 100m, DateTimeOffset.UtcNow);
                }
                """;

            Assert.Empty(ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// テスト用の実行環境の時計と採番による領収書の内容の検査
        /// </summary>
        [Fact]
        public void Complete_UsesEnvironmentCapabilities()
        {
            var now = new DateTimeOffset(2026, 11, 5, 1, 0, 0, TimeSpan.Zero);
            var environment = new FixedEnvironment(now);

            var first = Checkout.Complete(environment, 1200m);
            var second = Checkout.Complete(environment, 800m);

            Assert.Equal(new Receipt("T-1", 1200m, now), first);
            Assert.Equal(new Receipt("T-2", 800m, now), second);
        }

        /// <summary>
        /// 固定の時刻と連番の採番を持つテスト用の実行環境
        /// </summary>
        /// <param name="now">時計が返し続ける UTC の時刻</param>
        private sealed class FixedEnvironment(DateTimeOffset now) : IHasClock, IHasReceiptNumbers
        {
            /// <summary>
            /// これまでに採番した領収書の数
            /// </summary>
            private int issuedCount;

            /// <inheritdoc />
            public TimeProvider Clock { get; } = new FixedTimeProvider(now);

            /// <inheritdoc />
            public string NextReceiptNumber()
            {
                issuedCount++;
                return $"T-{issuedCount}";
            }
        }

        /// <summary>
        /// 常に同じ時刻を返す時計
        /// </summary>
        /// <param name="now">返し続ける UTC の時刻</param>
        private sealed class FixedTimeProvider(DateTimeOffset now) : TimeProvider
        {
            /// <inheritdoc />
            public override DateTimeOffset GetUtcNow() => now;
        }
    }
}
