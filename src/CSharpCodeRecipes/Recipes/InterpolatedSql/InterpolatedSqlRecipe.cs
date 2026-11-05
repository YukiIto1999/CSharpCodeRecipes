namespace CSharpCodeRecipes.Recipes.InterpolatedSql
{
    /// <summary>
    /// 補間した値を必ずパラメータにする SQL を実演するレシピ
    /// </summary>
    internal sealed class InterpolatedSqlRecipe : IRecipe
    {
        /// <inheritdoc />
        string IRecipe.Name => "InterpolatedSql";

        /// <inheritdoc />
        string IRecipe.Description => "補間した値を必ずパラメータにする SQL";

        /// <inheritdoc />
        void IRecipe.Run()
        {
            var name = "x' OR '1'='1";
            var minimumAge = 20;
            var statement = Sql.Create($"SELECT * FROM Members WHERE Name = {name} AND Age >= {minimumAge}");

            Console.WriteLine(statement.Text);
            for (var i = 0; i < statement.Parameters.Count; i++)
            {
                Console.WriteLine($"  @p{i} = {statement.Parameters[i]}");
            }
        }
    }
}
