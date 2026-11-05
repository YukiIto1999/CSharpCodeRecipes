# 補間した値を必ずパラメータにする SQL

SQL 文を補間文字列からしか作れないようにし、補間の穴に渡した値を必ずパラメータにするパターンである。
文字列の連結で組み立てた SQL は型が合わずに CS1503 のコンパイルエラーになるので、値を本文へ直接埋め込む SQL インジェクションの経路が型の上で消える。

## 仕組み

`Sql.Create` の引数は `string` ではなく、`[InterpolatedStringHandler]` を付けた `SqlInterpolatedStringHandler` である。
補間文字列をこの型の引数に渡すと、コンパイラは文字列を作らずに、穴でない部分を `AppendLiteral`、穴の値を `AppendFormatted` へ順に渡すコードを生成する。
`AppendFormatted` は値を本文に書かず、本文には `@p0` からの連番の名前だけを書いて、値をパラメータに加える。

```csharp
var statement = Sql.Create($"SELECT * FROM Members WHERE Name = {name} AND Age >= {minimumAge}");
// statement.Text は SELECT * FROM Members WHERE Name = @p0 AND Age >= @p1
```

`string` から補間文字列ハンドラへの変換はないので、連結した文字列や文字列の変数を `Sql.Create` に渡すと CS1503 になる。
穴のない補間文字列 `$"SELECT * FROM Members"` は渡せるので、固定の SQL も同じ入口から作れる。

ハンドラを struct にすると、`Sql.Create(default)` が nullable の検査も警告もなくコンパイルを通り、実行時に NullReferenceException になる。
このレシピはハンドラを sealed class にしているので、`default` は null として扱われ、nullable の検査で CS8625 になる。

## 型では防げない範囲

コンパイラが生成するコードから呼べるように、ハンドラのコンストラクタと `AppendLiteral` は public である。
そのため、ハンドラを手で作って `AppendLiteral` に連結した文字列を渡せば、値を本文に埋め込める。
この迂回は型では塞げないので、コードレビューか、ハンドラの手での構築を報告する analyzer で補う。

[InterpolatedSqlTests](../../../../tests/CSharpCodeRecipes.Tests/Recipes/InterpolatedSql/InterpolatedSqlTests.cs) は、穴がパラメータになることと、連結した文字列が CS1503 に、`default` が CS8625 になることを検査する。
同じテストは、手で作ったハンドラによる迂回がコンパイルを通ることも検査する。

## 実行

```bash
dotnet run --project src/CSharpCodeRecipes -- InterpolatedSql
```
