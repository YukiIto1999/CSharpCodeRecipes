using CSharpCodeRecipes.Recipes.HideInheritedMember;
using CSharpCodeRecipes.Tests.Compiling;

namespace CSharpCodeRecipes.Tests.Recipes.HideInheritedMember
{
    /// <summary>
    /// new と Obsolete による継承したメソッドの封じ方の検査
    /// </summary>
    public class HideInheritedMemberTests
    {
        /// <summary>
        /// 派生型からの隠したメソッドの呼出しのコンパイルエラーの検査
        /// </summary>
        [Fact]
        public void ReturnOnDerivedType_FailsToCompile()
        {
            const string source = """
                using CSharpCodeRecipes.Recipes.HideInheritedMember;

                public static class Worker
                {
                    public static void Finish(BufferPool pool, byte[] buffer) => pool.Return(buffer);
                }
                """;

            Assert.Equal(["CS0619"], ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// 型では防げない基底型を経由した呼出しの成功の検査
        /// </summary>
        [Fact]
        public void ReturnThroughBaseType_StillCompiles()
        {
            const string source = """
                using CSharpCodeRecipes.Recipes.HideInheritedMember;

                public static class Worker
                {
                    public static void Finish(BufferPool pool, byte[] buffer)
                    {
                        Pool<byte[]> basePool = pool;
                        basePool.Return(buffer);
                    }
                }
                """;

            Assert.Empty(ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// override に付けた Obsolete が呼出し側で報告されないことの検査
        /// </summary>
        [Fact]
        public void ObsoleteOnOverride_IsNotReported()
        {
            const string source = """
                using System;

                public class Connection
                {
                    public virtual void Close()
                    {
                    }
                }

                public sealed class SharedConnection : Connection
                {
                #pragma warning disable CS0809
                    [Obsolete("共有の接続は閉じない。", error: true)]
                    public override void Close() => throw new NotSupportedException();
                #pragma warning restore CS0809
                }

                public static class Worker
                {
                    public static void Finish(SharedConnection connection) => connection.Close();

                    public static int Hash() => new HashCode().GetHashCode();
                }
                """;

            Assert.Empty(ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// 正しい返却での中身の消去と再利用の検査
        /// </summary>
        [Fact]
        public void ClearAndReturn_ClearsBeforeReuse()
        {
            var pool = new BufferPool(4);
            var buffer = pool.Rent();
            buffer[0] = 42;

            pool.ClearAndReturn(buffer);
            var reused = pool.Rent();

            Assert.Same(buffer, reused);
            Assert.Equal(0, reused[0]);
        }

        /// <summary>
        /// 基底型を経由した返却で中身が次の貸出しに残ることの検査
        /// </summary>
        [Fact]
        public void ReturnThroughBaseType_LeavesContents()
        {
            var pool = new BufferPool(4);
            var buffer = pool.Rent();
            buffer[0] = 42;

            Pool<byte[]> basePool = pool;
            basePool.Return(buffer);

            Assert.Equal(42, pool.Rent()[0]);
        }
    }
}
