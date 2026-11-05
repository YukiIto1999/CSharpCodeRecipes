using CSharpCodeRecipes.Recipes.StaticAbstractContract;
using CSharpCodeRecipes.Tests.Compiling;

namespace CSharpCodeRecipes.Tests.Recipes.StaticAbstractContract
{
    /// <summary>
    /// static abstract による静的メンバーの要求の検査
    /// </summary>
    public class StaticAbstractContractTests
    {
        /// <summary>
        /// 契約を実装しない型の読み出しのコンパイルエラーの検査
        /// </summary>
        [Fact]
        public void TypeWithoutContract_FailsToCompile()
        {
            const string source = """
                using System;
                using CSharpCodeRecipes.Recipes.StaticAbstractContract;

                public static class Startup
                {
                    public static Uri Endpoint(SettingsReader settings) => settings.Get<Uri>("Endpoint");
                }
                """;

            Assert.Equal(["CS0311"], ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// 静的メンバーを持たない契約の実装のコンパイルエラーの検査
        /// </summary>
        [Fact]
        public void MissingStaticMember_FailsToCompile()
        {
            const string source = """
                using CSharpCodeRecipes.Recipes.StaticAbstractContract;

                public sealed class RetryCount : ISettingValue<RetryCount>
                {
                }
                """;

            Assert.Equal(["CS0535"], ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// 引数の型が契約と異なる静的メンバーのコンパイルエラーの検査
        /// </summary>
        [Fact]
        public void WrongSignature_FailsToCompile()
        {
            const string source = """
                using CSharpCodeRecipes.Recipes.StaticAbstractContract;

                public sealed class RetryCount : ISettingValue<RetryCount>
                {
                    public static RetryCount FromSetting(int count) => new();
                }
                """;

            Assert.Equal(["CS0535"], ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// 契約を実装した別アセンブリの型の読み出しの成功の検査
        /// </summary>
        [Fact]
        public void TypeImplementingContract_Compiles()
        {
            const string source = """
                using System.Globalization;
                using CSharpCodeRecipes.Recipes.StaticAbstractContract;

                public sealed class RetryCount : ISettingValue<RetryCount>
                {
                    private RetryCount(int count) => Count = count;

                    public int Count { get; }

                    public static RetryCount FromSetting(string text) =>
                        new(int.Parse(text, CultureInfo.InvariantCulture));
                }

                public static class Startup
                {
                    public static RetryCount Retries(SettingsReader settings) => settings.Get<RetryCount>("Retries");
                }
                """;

            Assert.Empty(ExternalCompilation.ErrorIds(source));
        }

        /// <summary>
        /// 型ごとの作り方による設定の文字列の変換の検査
        /// </summary>
        [Fact]
        public void Get_ConvertsWithEachTypeFactory()
        {
            var settings = new SettingsReader(new Dictionary<string, string>
            {
                ["Host"] = " db.example.com ",
                ["Port"] = "5432",
            });

            Assert.Equal("db.example.com", settings.Get<HostName>("Host").Value);
            Assert.Equal(5432, settings.Get<Port>("Port").Number);
        }

        /// <summary>
        /// 範囲の外のポート番号の拒否の検査
        /// </summary>
        /// <param name="text">1 から 65535 の範囲の外の数の文字列</param>
        [Theory]
        [InlineData("0")]
        [InlineData("65536")]
        public void PortOutOfRange_Throws(string text)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Port.FromSetting(text));
        }
    }
}
