using CSharpCodeRecipes.Recipes.InterpolatedSql;
using CSharpCodeRecipes.Tests.Compiling;

namespace CSharpCodeRecipes.Tests.Recipes.InterpolatedSql
{
    /// <summary>
    /// 補間文字列ハンドラによる SQL のパラメータ化の検査
    /// </summary>
    public class InterpolatedSqlTests
    {
        /// <summary>
        /// 補間の穴の値の本文からの切り離しとパラメータへの格納の検査
        /// </summary>
        [Fact]
        public void Holes_BecomeParameters()
        {
            var name = "x' OR '1'='1";
            var minimumAge = 20;

            var statement = Sql.Create($"SELECT * FROM Members WHERE Name = {name} AND Age >= {minimumAge}");

            Assert.Equal("SELECT * FROM Members WHERE Name = @p0 AND Age >= @p1", statement.Text);
            Assert.Equal([name, minimumAge], statement.Parameters);
        }

        /// <summary>
        /// 連結で作った文字列の SQL 文としての受け渡しのコンパイルエラーの検査
        /// </summary>
        [Fact]
        public void ConcatenatedString_FailsToCompile()
        {
            const string source = """
                using CSharpCodeRecipes.Recipes.InterpolatedSql;

                public static class Queries
                {
                    public static SqlStatement ByName(string name)
                    {
                        var text = "SELECT * FROM Members WHERE Name = '" + name + "'";
                        return Sql.Create(text);
                    }
                }
                """;

            Assert.Equal(["CS1503"], ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// 既定値での SQL 文の作成の nullable によるコンパイルエラーの検査
        /// </summary>
        [Fact]
        public void DefaultHandler_FailsToCompile()
        {
            const string source = """
                using CSharpCodeRecipes.Recipes.InterpolatedSql;

                public static class Queries
                {
                    public static SqlStatement Empty() => Sql.Create(default);
                }
                """;

            Assert.Equal(["CS8625"], ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// 穴のない補間文字列からの SQL 文の作成の成功の検査
        /// </summary>
        [Fact]
        public void InterpolatedStringWithoutHoles_Compiles()
        {
            const string source = """
                using CSharpCodeRecipes.Recipes.InterpolatedSql;

                public static class Queries
                {
                    public static SqlStatement All() => Sql.Create($"SELECT * FROM Members");
                }
                """;

            Assert.Empty(ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// 型では防げない手書きのハンドラによる本文への値の埋め込みの成功の検査
        /// </summary>
        [Fact]
        public void HandWrittenHandler_StillCompiles()
        {
            const string source = """
                using CSharpCodeRecipes.Recipes.InterpolatedSql;

                public static class Queries
                {
                    public static SqlStatement ByName(string name)
                    {
                        var handler = new SqlInterpolatedStringHandler(0, 0);
                        handler.AppendLiteral("SELECT * FROM Members WHERE Name = '" + name + "'");
                        return Sql.Create(handler);
                    }
                }
                """;

            Assert.Empty(ExternalCompilation.ErrorIds(source));
        }
    }
}
