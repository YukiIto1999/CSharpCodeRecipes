using CSharpCodeRecipes.Recipes.PlatformOnlyMember;
using CSharpCodeRecipes.Tests.Compiling;

namespace CSharpCodeRecipes.Tests.Recipes.PlatformOnlyMember
{
    /// <summary>
    /// 集約の基底の公開範囲による基盤とドメインの役割分担の検査
    /// </summary>
    public class PlatformOnlyMemberTests
    {
        /// <summary>
        /// ドメイン側の集約の出来事の記録とそれを使う集約の定義
        /// </summary>
        private const string DomainSource = """
            using CSharpCodeRecipes.Recipes.PlatformOnlyMember;

            public sealed record InvoiceIssued(string InvoiceNumber) : IDomainEvent;

            public sealed class Invoice : AggregateRoot
            {
                public void Issue(string invoiceNumber) => Raise(new InvoiceIssued(invoiceNumber));
            }
            """;

        /// <summary>
        /// ドメイン側の集約自身による出来事の記録の成功の検査
        /// </summary>
        [Fact]
        public void DomainAggregateRaisingEvent_Compiles()
        {
            Assert.Empty(ExternalCompilation.ErrorIds(DomainSource));
        }

        /// <summary>
        /// ドメイン側からの未発行の出来事の取り出しのコンパイルエラーの検査
        /// </summary>
        [Fact]
        public void DequeueFromDomain_FailsToCompile()
        {
            const string source = DomainSource + """

                public static class Shortcut
                {
                    public static int Count(Invoice invoice) => invoice.DequeueEvents().Count;
                }
                """;

            Assert.Equal(["CS1061"], ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// 集約の外からの出来事の記録のコンパイルエラーの検査
        /// </summary>
        [Fact]
        public void RaiseFromOutsideAggregate_FailsToCompile()
        {
            const string source = DomainSource + """

                public static class Billing
                {
                    public static void Forge(Invoice invoice) => invoice.Raise(new InvoiceIssued("X-1"));
                }
                """;

            Assert.Equal(["CS0122"], ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// 保存での出来事の一度だけの発行の検査
        /// </summary>
        [Fact]
        public void Save_PublishesEventsOnce()
        {
            var store = new AggregateStore();
            var order = new Order("A-001");
            order.Place();

            store.Save(order);
            store.Save(order);

            Assert.Equal([new OrderPlaced("A-001")], store.PublishedEvents);
        }
    }
}
