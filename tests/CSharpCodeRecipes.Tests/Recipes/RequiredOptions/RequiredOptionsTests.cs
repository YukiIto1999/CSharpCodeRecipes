using System.Text.Json;
using CSharpCodeRecipes.Recipes.RequiredOptions;
using CSharpCodeRecipes.Tests.Compiling;

namespace CSharpCodeRecipes.Tests.Recipes.RequiredOptions
{
    /// <summary>
    /// required による設定項目の設定漏れの防止の検査
    /// </summary>
    public class RequiredOptionsTests
    {
        /// <summary>
        /// 必須の項目を設定しない構築のコンパイルエラーの検査
        /// </summary>
        [Fact]
        public void MissingRequiredSetting_FailsToCompile()
        {
            const string source = """
                using CSharpCodeRecipes.Recipes.RequiredOptions;

                public static class Startup
                {
                    public static SmtpOptions Smtp() => new SmtpOptions { Host = "smtp.example.com" };
                }
                """;

            Assert.Equal(["CS9035"], ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// 必須の項目を全て設定した構築の成功の検査
        /// </summary>
        [Fact]
        public void AllRequiredSettings_Compiles()
        {
            const string source = """
                using CSharpCodeRecipes.Recipes.RequiredOptions;

                public static class Startup
                {
                    public static MailSender Mail() =>
                        new MailSender(new SmtpOptions { Host = "smtp.example.com", Port = 587 });
                }
                """;

            Assert.Empty(ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// 構築後の設定の書き換えのコンパイルエラーの検査
        /// </summary>
        [Fact]
        public void ChangeAfterConstruction_FailsToCompile()
        {
            const string source = """
                using CSharpCodeRecipes.Recipes.RequiredOptions;

                public static class Startup
                {
                    public static void Override(SmtpOptions options) => options.Port = 25;
                }
                """;

            Assert.Equal(["CS8852"], ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// 型では防げない必須の項目への不正な値の設定の成功の検査
        /// </summary>
        [Fact]
        public void InvalidRequiredValues_StillCompile()
        {
            const string source = """
                using CSharpCodeRecipes.Recipes.RequiredOptions;

                public static class Startup
                {
                    public static SmtpOptions Smtp() => new SmtpOptions { Host = "", Port = 0 };
                }
                """;

            Assert.Empty(ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// 必須の項目のない JSON からの読み込みの拒否の検査
        /// </summary>
        [Fact]
        public void JsonWithoutRequiredProperty_Throws()
        {
            const string json = """{ "Host": "smtp.example.com" }""";

            Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<SmtpOptions>(json));
        }

        /// <summary>
        /// 設定の値と既定値の送信への引継ぎの検査
        /// </summary>
        [Fact]
        public void MailSender_UsesSettingsAndDefaults()
        {
            var sender = new MailSender(new SmtpOptions { Host = "smtp.example.com", Port = 587 });

            Assert.Equal("smtp://smtp.example.com:587", sender.Endpoint);
            Assert.Equal(TimeSpan.FromSeconds(30), sender.Timeout);
        }
    }
}
