# 必要な依存をジェネリック制約で型に出す

処理が使う時計や採番のような依存を、実行環境の型引数に対するジェネリック制約として宣言するパターンである。
要求する能力を宣言しない呼び出し側や、能力を持たない実行環境を渡すコードは、コンパイルエラーになる。

## 仕組み

能力ごとにインタフェースを分け、処理は実行環境の型引数に必要な能力だけを制約として付ける。

```csharp
public static Receipt Issue<TEnvironment>(TEnvironment environment, decimal amount)
    where TEnvironment : IHasClock, IHasReceiptNumbers
```

| 誤り | 結果 |
|---|---|
| 呼び出し側が `where TEnvironment : IHasClock` だけを宣言して `Issue` を呼ぶ | CS0314 |
| `IHasClock` だけを実装した環境を `Issue` に渡す | CS0311 |

呼び出し側は、呼び出し先が要求する能力を自分の制約にも書かなければならない。
そのため、ある処理が最終的に何に依存するかが、呼び出しの経路をたどらずにメソッドの宣言から読める。

引数の型を `IHasClock` のようなインタフェースにする方法では、二つの能力を要求するために両方を継承した合成のインタフェースが必要になる。
ジェネリック制約なら `IHasClock, IHasReceiptNumbers` のように能力を並べるだけで要求を表せ、合成のインタフェースを能力の組み合わせごとに作らずに済む。
テストでは、固定の時刻を返す時計を持つ環境を渡すだけで、現在時刻に依存する処理を決まった値で検査できる。

## 型では防げない範囲

制約は、処理が使ってよい能力を宣言するだけで、それ以外の手段の利用は止めない。
`IHasReceiptNumbers` だけを要求する処理の中で `DateTimeOffset.UtcNow` を直接読んでも、コンパイルを通る。
この経路は、Microsoft.CodeAnalysis.BannedApiAnalyzers のような禁止 API の検査で補う。

[CapabilityConstraintTests](../../../../tests/CSharpCodeRecipes.Tests/Recipes/CapabilityConstraint/CapabilityConstraintTests.cs) は、二つの誤りが CS0314 と CS0311 になることと、時計の直接の利用がコンパイルを通ることを検査する。

## 実行

```bash
dotnet run --project src/CSharpCodeRecipes -- CapabilityConstraint
```
