# 基盤の設定項目の設定漏れをコンパイルエラーにする

基盤側が提供する設定の型で、省けない項目に `required` を付けるパターンである。
必須の項目を設定せずに設定の型を作るコードは、CS9035 のコンパイルエラーになる。

## 使いどころ

基盤側がメール送信やデータベース接続のような部品を提供し、ドメイン側やアプリの起動処理がその設定を書く構成で使う。
設定項目が増えたときも、必須の項目を足せば、設定していない全ての箇所がビルドで見つかる。
接続先の書き忘れのような設定漏れを、起動後の実行時エラーではなくビルドの時点で防げる。

## 仕組み

```csharp
public sealed class SmtpOptions
{
    public required string Host { get; init; }

    public required int Port { get; init; }

    public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(30);
}
```

| 書き方 | 結果 |
|---|---|
| `new SmtpOptions { Host = "smtp.example.com" }` | `Port` の設定漏れで CS9035 |
| `new SmtpOptions { Host = "smtp.example.com", Port = 587 }` | 成功し `Timeout` は既定値の 30 秒 |
| 構築後の `options.Port = 25` | `init` のため CS8852 |

コンストラクタの引数で必須の項目を受け取る方法と違い、オブジェクト初期化子で項目を名前で設定でき、既定値のある項目は省ける。
`init` と組み合わせると、構築後に設定を書き換えられない。
System.Text.Json も `required` を解釈するので、必須の項目がない JSON を読み込むと JsonException になる。

## 型では防げない範囲

`required` が検査するのは項目を設定したかどうかだけで、値が正しいかは検査しない。
`new SmtpOptions { Host = "", Port = 0 }` はコンパイルを通るので、値の範囲や形式は設定を受け取る側で検証する。

[RequiredOptionsTests](../../../../tests/CSharpCodeRecipes.Tests/Recipes/RequiredOptions/RequiredOptionsTests.cs) は、設定漏れが CS9035 に、構築後の書き換えが CS8852 になることと、必須の項目がない JSON の拒否を検査する。
同じテストは、不正な値の設定がコンパイルを通ることも検査する。

## 実行

```bash
dotnet run --project src/CSharpCodeRecipes -- RequiredOptions
```
