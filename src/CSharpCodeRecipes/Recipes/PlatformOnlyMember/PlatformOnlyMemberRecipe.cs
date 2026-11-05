namespace CSharpCodeRecipes.Recipes.PlatformOnlyMember
{
    /// <summary>
    /// 基盤だけが呼べるメンバーをドメインが継承する型に置くレシピ
    /// </summary>
    internal sealed class PlatformOnlyMemberRecipe : IRecipe
    {
        /// <inheritdoc />
        string IRecipe.Name => "PlatformOnlyMember";

        /// <inheritdoc />
        string IRecipe.Description => "基盤だけが呼べるメンバーをドメインが継承する型に置く";

        /// <inheritdoc />
        void IRecipe.Run()
        {
            var store = new AggregateStore();
            var order = new Order("A-001");
            order.Place();

            store.Save(order);
            store.Save(order);

            Console.WriteLine($"発行した出来事の数: {store.PublishedEvents.Count}");
            foreach (var domainEvent in store.PublishedEvents)
            {
                Console.WriteLine($"  {domainEvent}");
            }
        }
    }
}
