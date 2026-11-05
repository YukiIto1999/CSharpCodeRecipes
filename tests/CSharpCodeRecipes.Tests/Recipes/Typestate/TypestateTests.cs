using CSharpCodeRecipes.Recipes.Typestate;
using CSharpCodeRecipes.Tests.Compiling;

namespace CSharpCodeRecipes.Tests.Recipes.Typestate
{
    /// <summary>
    /// 注文の状態ごとの型による操作の制限の検査
    /// </summary>
    public class TypestateTests
    {
        /// <summary>
        /// 支払前の発送のコンパイルエラーの検査
        /// </summary>
        [Fact]
        public void ShipBeforePay_FailsToCompile()
        {
            const string source = """
                using CSharpCodeRecipes.Recipes.Typestate;

                public static class Fulfillment
                {
                    public static ShippedOrder Ship(UnpaidOrder order) => order.Ship("TRK-1");
                }
                """;

            Assert.Equal(["CS1061"], ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// 支払を経ない支払済みの注文の構築のコンパイルエラーの検査
        /// </summary>
        [Fact]
        public void PaidOrderWithoutPay_FailsToCompile()
        {
            const string source = """
                using System;
                using CSharpCodeRecipes.Recipes.Typestate;

                public static class Fulfillment
                {
                    public static PaidOrder Forge() => new PaidOrder("A-1", 100m, DateTimeOffset.UnixEpoch);
                }
                """;

            Assert.Equal(["CS1729"], ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// 支払から発送までの正しい順序の操作の成功の検査
        /// </summary>
        [Fact]
        public void PayThenShip_Compiles()
        {
            const string source = """
                using System;
                using CSharpCodeRecipes.Recipes.Typestate;

                public static class Fulfillment
                {
                    public static ShippedOrder Complete(UnpaidOrder order) =>
                        order.Pay(DateTimeOffset.UnixEpoch).Ship("TRK-1");
                }
                """;

            Assert.Empty(ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// 型では防げない遷移前の値の再利用による二重の支払の成功の検査
        /// </summary>
        [Fact]
        public void PayTwice_StillCompiles()
        {
            const string source = """
                using System;
                using CSharpCodeRecipes.Recipes.Typestate;

                public static class Fulfillment
                {
                    public static (PaidOrder First, PaidOrder Second) PayTwice(UnpaidOrder order) =>
                        (order.Pay(DateTimeOffset.UnixEpoch), order.Pay(DateTimeOffset.UnixEpoch));
                }
                """;

            Assert.Empty(ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// 遷移での注文の情報の引継ぎの検査
        /// </summary>
        [Fact]
        public void Ship_CarriesOverOrderDetails()
        {
            var paidAt = new DateTimeOffset(2026, 11, 5, 10, 0, 0, TimeSpan.FromHours(9));

            var shipped = new UnpaidOrder("A-001", 3200m).Pay(paidAt).Ship("TRK-123");

            Assert.Equal("A-001", shipped.OrderNumber);
            Assert.Equal(3200m, shipped.Amount);
            Assert.Equal(paidAt, shipped.PaidAt);
            Assert.Equal("TRK-123", shipped.TrackingNumber);
        }
    }
}
