# CSharpCodeRecipes

C# の実装パターンを、実行できるサンプルとそれを検査するテストの組で集めたリポジトリである。

## レシピ一覧

| レシピ | 概要 |
|---|---|
| [CapabilityConstraint](src/CSharpCodeRecipes/Recipes/CapabilityConstraint/README.md) | 必要な依存をジェネリック制約で型に出す |
| [ClosedHierarchy](src/CSharpCodeRecipes/Recipes/ClosedHierarchy/README.md) | 別アセンブリからの派生を禁じる閉じた型階層 |
| [ImplicitUnion](src/CSharpCodeRecipes/Recipes/ImplicitUnion/README.md) | 返せる型を型引数に限る和型 |
| [InterpolatedSql](src/CSharpCodeRecipes/Recipes/InterpolatedSql/README.md) | 補間した値を必ずパラメータにする SQL |
| [NoPermissionPublicMethod](src/CSharpCodeRecipes/Recipes/NoPermissionPublicMethod/README.md) | Override以外のPublicメソッドの作成禁止 |
| [Typestate](src/CSharpCodeRecipes/Recipes/Typestate/README.md) | 状態ごとの型で誤った操作を禁じる |

## 実行

.NET 10 SDK が必要である。
Visual Studio で開く場合は、.NET 10 SDK が要求する MSBuild 18 を含む Visual Studio 2026 を使う。

次のコマンドはレシピの一覧を表示し、入力された番号か名前のレシピを実行する。

```bash
dotnet run --project src/CSharpCodeRecipes
```

`<レシピ名>` の位置に一覧のレシピ名か番号を渡すと、一覧を表示せずにそのレシピを実行する。

```bash
dotnet run --project src/CSharpCodeRecipes -- <レシピ名>
```

テストは次のコマンドで実行する。

```bash
dotnet test
```

## レシピの追加

レシピは `src/CSharpCodeRecipes/Recipes/<レシピ名>/` に一つずつ置き、名前空間を `CSharpCodeRecipes.Recipes.<レシピ名>` にする。
同じフォルダに `IRecipe` を実装した `<レシピ名>Recipe` クラスを置くと、そのレシピは一覧に自動で加わる。
`IRecipe.Name` はフォルダ名と同じ値にし、その一致は `RecipeCatalogTests` が検査する。

```csharp
namespace CSharpCodeRecipes.Recipes.Xxx
{
    /// <summary>
    /// ○○を実演するレシピ
    /// </summary>
    internal sealed class XxxRecipe : IRecipe
    {
        /// <inheritdoc />
        string IRecipe.Name => "Xxx";

        /// <inheritdoc />
        string IRecipe.Description => "○○";

        /// <inheritdoc />
        void IRecipe.Run()
        {
        }
    }
}
```

レシピの解説は同じフォルダの `README.md` に書き、このファイルのレシピ一覧へ一行加える。
レシピ固有のテストは `tests/CSharpCodeRecipes.Tests/Recipes/<レシピ名>/` に置く。
別アセンブリからの誤った使い方がコンパイルエラーになることは、そのソースを `ExternalCompilation.ErrorIds` に渡し、返るエラーの診断 ID で確かめる。
