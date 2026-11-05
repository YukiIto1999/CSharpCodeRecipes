using CSharpCodeRecipes.Recipes.ImplicitUnion;
using CSharpCodeRecipes.Tests.Compiling;

namespace CSharpCodeRecipes.Tests.Recipes.ImplicitUnion
{
    /// <summary>
    /// <see cref="Union{T1, T2}"/> による返せる型の制限の検査
    /// </summary>
    public class ImplicitUnionTests
    {
        /// <summary>
        /// 型引数にない型の値を返すコードのコンパイルエラーの検査
        /// </summary>
        [Fact]
        public void ReturnUndeclaredType_FailsToCompile()
        {
            const string source = """
                using CSharpCodeRecipes.Recipes.ImplicitUnion;

                public static class Lookup
                {
                    public static Union<Member, MemberNotFound> Find(int id) => "該当なし";
                }
                """;

            Assert.Equal(["CS0029"], ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// 型引数に挙げた型の値を返すコードの成功の検査
        /// </summary>
        [Fact]
        public void ReturnDeclaredTypes_Compiles()
        {
            const string source = """
                using CSharpCodeRecipes.Recipes.ImplicitUnion;

                public static class Lookup
                {
                    public static Union<Member, MemberNotFound> Find(int id) =>
                        id == 1 ? new Member(1, "佐藤") : new MemberNotFound(id);
                }
                """;

            Assert.Empty(ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// null になり得る値の変換での nullable の警告の検査
        /// </summary>
        [Fact]
        public void MaybeNullValue_ReportsNullableWarning()
        {
            const string source = """
                using CSharpCodeRecipes.Recipes.ImplicitUnion;

                public static class Lookup
                {
                    public static Union<Member, MemberNotFound> Wrap(Member? member) => member;
                }
                """;

            Assert.Equal(["CS8604"], ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// 同じ型を二つ挙げた場合の曖昧な変換のコンパイルエラーの検査
        /// </summary>
        [Fact]
        public void SameTypeTwice_IsAmbiguous()
        {
            const string source = """
                using CSharpCodeRecipes.Recipes.ImplicitUnion;

                public static class Lookup
                {
                    public static Union<Member, Member> Wrap(Member member) => member;
                }
                """;

            Assert.Equal(["CS0457"], ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// 別アセンブリからの派生によるバリアントの追加のコンパイルエラーの検査
        /// </summary>
        [Fact]
        public void ExternalDerivation_FailsToCompile()
        {
            const string source = """
                using System;
                using CSharpCodeRecipes.Recipes.ImplicitUnion;

                public sealed class Third : Union<Member, MemberNotFound>
                {
                    public override TOut Match<TOut>(Func<Member, TOut> onFirst, Func<MemberNotFound, TOut> onSecond) =>
                        throw new NotSupportedException();
                }
                """;

            Assert.Equal(["CS1729"], ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// nullable の警告を無視して渡された null の実行時の拒否の検査
        /// </summary>
        [Fact]
        public void NullValue_ThrowsAtRuntime()
        {
            Member member = null!;

            Assert.Throws<ArgumentNullException>(() =>
            {
                Union<Member, MemberNotFound> union = member;
                return union;
            });
        }

        /// <summary>
        /// 保持する値の型に対応する処理だけの実行の検査
        /// </summary>
        [Fact]
        public void Match_CallsHandlerForHeldValue()
        {
            var directory = new MemberDirectory([new Member(1, "佐藤")]);

            Assert.Equal("佐藤", directory.Find(1).Match(member => member.Name, _ => "該当なし"));
            Assert.Equal("該当なし 3", directory.Find(3).Match(member => member.Name, notFound => $"該当なし {notFound.Id}"));
        }
    }
}
