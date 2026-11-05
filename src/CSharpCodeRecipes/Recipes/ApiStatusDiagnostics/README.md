# 基盤 API の試験的と非推奨を診断 ID で伝える

基盤側の API の状態を、`[Experimental]` と `[Obsolete]` に独自の診断 ID を付けて、使う側のコンパイラの診断として伝えるパターンである。
試験的な API は ID を指定して明示的に同意しないとコンパイルエラーになり、非推奨の API は ID ごとに抑止できる警告になる。

## 使いどころ

基盤側が API を段階的に入れ替え、ドメイン側の複数のチームがその API を使う構成で使う。
新しい API を試験的に出すときは、使うチームに互換性の保証がないことへの同意を求められる。
古い API を廃止するときは、残っている呼出しを ID で検索でき、診断から説明の URL へ案内できる。

## 仕組み

| 状態 | 書き方 | 既定の重大度 | 使う側の対応 |
|---|---|---|---|
| 試験的 | `[Experimental("RECIPE0001", UrlFormat = ...)]` | エラー | `#pragma warning disable RECIPE0001` か `<NoWarn>` での同意 |
| 非推奨 | `[Obsolete("...", DiagnosticId = "RECIPE0002", UrlFormat = ...)]` | 警告 | 代わりの API への移行か ID を指定した抑止 |

試験的な API の診断は、警告をエラーにする設定がなくてもエラーになり、同じアセンブリの中の呼出しでも報告される。
そのため、このレシピの実演のコードも `#pragma warning disable RECIPE0001` で同意してから `NotifyAll` を呼んでいる。

`DiagnosticId` を付けない `[Obsolete]` は、どの API でも同じ CS0618 で報告される。
使う側が CS0618 をまとめて抑止すると、それ以降に非推奨になった API の警告も全て隠れる。
独自の ID を付けた非推奨は CS0618 をまとめて抑止しても報告されるので、API ごとに移行の状況を追える。

`UrlFormat` を指定すると、診断に説明の URL が付く。
ID は `DiagnosticIds` に定数としてまとめ、一度使った ID を別の意味で再利用しないようにする。

## 型では防げない範囲

同意は ID の抑止なので、プロジェクトの `<NoWarn>` に ID を書けば、そのプロジェクトのどこからでも試験的な API を使える。
非推奨の診断は警告なので、使う側が警告をエラーにしていなければビルドは通る。
どちらも API の状態を伝える仕組みであり、利用そのものを禁じるには `internal` のような公開範囲を使う。

[ApiStatusDiagnosticsTests](../../../../tests/CSharpCodeRecipes.Tests/Recipes/ApiStatusDiagnostics/ApiStatusDiagnosticsTests.cs) は、同意のない試験的な API の利用が RECIPE0001 になり、同意すれば通ることを検査する。
同じテストは、非推奨の利用が RECIPE0002 で報告され、CS0618 をまとめて抑止しても隠れないことと、ID が重複しないことも検査する。

## 実行

```bash
dotnet run --project src/CSharpCodeRecipes -- ApiStatusDiagnostics
```
