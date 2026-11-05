using CSharpCodeRecipes.Recipes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CSharpCodeRecipes.Tests.Compiling
{
    /// <summary>
    /// レシピのアセンブリを参照する別アセンブリとしてのソースのコンパイル
    /// </summary>
    /// <remarks>
    /// InternalsVisibleTo の対象外のアセンブリ名と警告をエラーとする設定でのコンパイル
    /// </remarks>
    internal static class ExternalCompilation
    {
        /// <summary>
        /// レシピのアセンブリの内部を見られない別アセンブリの名前
        /// </summary>
        private const string AssemblyName = "ExternalAssembly";

        /// <summary>
        /// 最新の言語バージョンでの構文解析の設定
        /// </summary>
        private static readonly CSharpParseOptions ParseOptions = new(LanguageVersion.Latest);

        /// <summary>
        /// nullable を有効にして警告をエラーとするライブラリのコンパイルの設定
        /// </summary>
        /// <remarks>
        /// SDK の既定の NoWarn と同じ CS1701 と CS1702 の抑止
        /// </remarks>
        private static readonly CSharpCompilationOptions CompilationOptions = new CSharpCompilationOptions(
                OutputKind.DynamicallyLinkedLibrary,
                nullableContextOptions: NullableContextOptions.Enable,
                generalDiagnosticOption: ReportDiagnostic.Error)
            .WithSpecificDiagnosticOptions(
                new Dictionary<string, ReportDiagnostic>
                {
                    ["CS1701"] = ReportDiagnostic.Suppress,
                    ["CS1702"] = ReportDiagnostic.Suppress,
                });

        /// <summary>
        /// 実行中のランタイムの全アセンブリとレシピのアセンブリへの参照
        /// </summary>
        private static readonly MetadataReference[] References = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!)
            .Split(Path.PathSeparator)
            .Append(typeof(IRecipe).Assembly.Location)
            .Distinct(StringComparer.Ordinal)
            .Select(path => MetadataReference.CreateFromFile(path))
            .ToArray();

        /// <summary>
        /// 別アセンブリからの利用で報告されるコンパイルエラーの診断 ID
        /// </summary>
        /// <param name="source">暗黙の using を持たない一つのファイルの C# ソース</param>
        /// <returns>重複を除いて昇順に並べたエラーの診断 ID でコンパイルの成功時は空</returns>
        public static IReadOnlyList<string> ErrorIds(string source)
        {
            var compilation = CSharpCompilation.Create(
                AssemblyName,
                [CSharpSyntaxTree.ParseText(source, ParseOptions)],
                References,
                CompilationOptions);

            return compilation.GetDiagnostics()
                .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
                .Select(diagnostic => diagnostic.Id)
                .Distinct()
                .Order(StringComparer.Ordinal)
                .ToArray();
        }
    }
}
