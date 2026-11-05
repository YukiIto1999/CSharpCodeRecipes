# CSharpCodeRecipes

言語の仕組みで設計上の制約を守らせる C# の実装パターンを、実行できるサンプルとそれを検査するテストの組で集めたリポジトリである。
各レシピには、基盤側とドメイン側のどちらがどの場面で使うかを添えている。

## レシピ一覧

| レシピ | 概要 | 使う側 | 場面 |
|---|---|---|---|
| [ApiStatusDiagnostics](src/CSharpCodeRecipes/Recipes/ApiStatusDiagnostics/README.md) | 基盤 API の試験的と非推奨を診断 ID で伝える | 基盤 | API の入れ替えをドメインの各チームに伝える |
| [CapabilityConstraint](src/CSharpCodeRecipes/Recipes/CapabilityConstraint/README.md) | 必要な依存をジェネリック制約で型に出す | 両方 | ドメインの処理の依存を宣言し基盤が実体を渡す |
| [ClosedHierarchy](src/CSharpCodeRecipes/Recipes/ClosedHierarchy/README.md) | 別アセンブリからの派生を禁じる閉じた型階層 | 基盤 | 基盤が所有する場合分けの種類を増やさせない |
| [HideInheritedMember](src/CSharpCodeRecipes/Recipes/HideInheritedMember/README.md) | 継承したメソッドの呼出しをコンパイルエラーにする | 基盤 | ライブラリの基底クラスの危険な操作を塞ぐ |
| [ImplicitUnion](src/CSharpCodeRecipes/Recipes/ImplicitUnion/README.md) | 返せる型を型引数に限る和型 | 両方 | 処理が返す結果の種類を宣言する |
| [InterpolatedSql](src/CSharpCodeRecipes/Recipes/InterpolatedSql/README.md) | 補間した値を必ずパラメータにする SQL | 基盤 | ドメインに連結した SQL を書かせない |
| [NoPermissionPublicMethod](src/CSharpCodeRecipes/Recipes/NoPermissionPublicMethod/README.md) | Override以外のPublicメソッドの作成禁止 | 基盤 | サービスの実装に入口を増やさせない |
| [PlatformOnlyMember](src/CSharpCodeRecipes/Recipes/PlatformOnlyMember/README.md) | 基盤だけが呼べるメンバーをドメインが継承する型に置く | 基盤 | ドメインが継承する基底に基盤専用の操作を置く |
| [StaticAbstractContract](src/CSharpCodeRecipes/Recipes/StaticAbstractContract/README.md) | 静的メンバーの規約を static abstract で型に出す | 基盤 | ドメインの型に変換方法を要求する |
| [Typestate](src/CSharpCodeRecipes/Recipes/Typestate/README.md) | 状態ごとの型で誤った操作を禁じる | ドメイン | 業務の状態遷移で誤った操作を禁じる |

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

レシピの解説は同じフォルダの `README.md` に書き、誰がどの場面で何を防ぐために使うかを「使いどころ」の節に書く。
このファイルのレシピ一覧には、使う側と場面を添えて一行加える。
レシピ固有のテストは `tests/CSharpCodeRecipes.Tests/Recipes/<レシピ名>/` に置く。
別アセンブリからの誤った使い方がコンパイルエラーになることは、そのソースを `ExternalCompilation.ErrorIds` に渡し、返るエラーの診断 ID で確かめる。
