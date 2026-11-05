using System.Reflection;
using CSharpCodeRecipes.Recipes.ApiStatusDiagnostics;
using CSharpCodeRecipes.Tests.Compiling;

namespace CSharpCodeRecipes.Tests.Recipes.ApiStatusDiagnostics
{
    /// <summary>
    /// 診断 ID による基盤 API の状態の伝え方の検査
    /// </summary>
    public class ApiStatusDiagnosticsTests
    {
        /// <summary>
        /// 同意のない試験的な API の利用の独自 ID でのコンパイルエラーの検査
        /// </summary>
        [Fact]
        public void ExperimentalApiWithoutOptIn_FailsToCompile()
        {
            const string source = """
                using CSharpCodeRecipes.Recipes.ApiStatusDiagnostics;

                public static class Campaign
                {
                    public static void Announce(Notifier notifier) => notifier.NotifyAll(["佐藤"], "お知らせ");
                }
                """;

            Assert.Equal(["RECIPE0001"], ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// ID を指定した同意での試験的な API の利用の成功の検査
        /// </summary>
        [Fact]
        public void ExperimentalApiWithOptIn_Compiles()
        {
            const string source = """
                using CSharpCodeRecipes.Recipes.ApiStatusDiagnostics;

                public static class Campaign
                {
                    public static void Announce(Notifier notifier)
                    {
                #pragma warning disable RECIPE0001
                        notifier.NotifyAll(["佐藤"], "お知らせ");
                #pragma warning restore RECIPE0001
                    }
                }
                """;

            Assert.Empty(ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// 非推奨の API の利用の独自 ID での報告の検査
        /// </summary>
        [Fact]
        public void ObsoleteApi_ReportsOwnDiagnosticId()
        {
            const string source = """
                using CSharpCodeRecipes.Recipes.ApiStatusDiagnostics;

                public static class Ordering
                {
                    public static void Confirm(Notifier notifier) => notifier.SendNotification("佐藤", "受注しました");
                }
                """;

            Assert.Equal(["RECIPE0002"], ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// CS0618 の一括の抑止で独自 ID の非推奨が隠れないことの検査
        /// </summary>
        [Fact]
        public void SuppressingObsoleteWarningsInBulk_DoesNotHideOwnId()
        {
            const string source = """
                using CSharpCodeRecipes.Recipes.ApiStatusDiagnostics;

                #pragma warning disable CS0618

                public static class Ordering
                {
                    public static void Confirm(Notifier notifier) => notifier.SendNotification("佐藤", "受注しました");
                }
                """;

            Assert.Equal(["RECIPE0002"], ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// 一覧の診断 ID の重複の不在の検査
        /// </summary>
        [Fact]
        public void DiagnosticIds_AreUnique()
        {
            var ids = typeof(DiagnosticIds)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(field => field.IsLiteral)
                .Select(field => (string)field.GetRawConstantValue()!)
                .ToArray();

            Assert.Equal(ids.Length, ids.Distinct(StringComparer.Ordinal).Count());
        }

        /// <summary>
        /// 同意した試験的な API による全ての宛先への送信の検査
        /// </summary>
        [Fact]
        public void NotifyAll_SendsToEveryRecipient()
        {
            var notifier = new Notifier();

#pragma warning disable RECIPE0001
            notifier.NotifyAll(["鈴木", "高橋"], "お知らせ");
#pragma warning restore RECIPE0001

            Assert.Equal(["鈴木: お知らせ", "高橋: お知らせ"], notifier.Sent);
        }
    }
}
