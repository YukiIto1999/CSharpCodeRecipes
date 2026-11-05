# 状態ごとの型で誤った操作を禁じる

注文の状態ごとに別の型を用意し、その状態で許す操作だけをメソッドとして持たせるパターンである。
誤った順序の操作はメソッドごと存在しないので、実行時の状態の検査ではなくコンパイルエラーで止まる。

## 仕組み

| 型 | 持つ操作 | 構築できる場所 |
|---|---|---|
| `UnpaidOrder` | `Pay` で `PaidOrder` を返す | どこからでも |
| `PaidOrder` | `Ship` で `ShippedOrder` を返す | `UnpaidOrder.Pay` だけ |
| `ShippedOrder` | なし | `PaidOrder.Ship` だけ |

未払いの注文に `Ship` を呼ぶと、そのメソッドがないので CS1061 のコンパイルエラーになる。
`PaidOrder` と `ShippedOrder` のコンストラクタは internal なので、別アセンブリからは見えず、支払を経ずに作ろうとすると CS1729 になる。

```csharp
var shipped = new UnpaidOrder("A-001", 3200m)
    .Pay(DateTimeOffset.Now)
    .Ship("TRK-123");
```

一つのクラスに状態のフラグを持たせる設計では、`Ship` を呼べるかは実行時の検査と例外でしか表せない。
状態ごとに型を分ければ、呼び出し側は型を見るだけで今できる操作を判断でき、補完の候補にも出てこない。

## 型では防げない範囲

遷移は新しい値を返すだけで、遷移前の値は使える状態のまま残る。
そのため、同じ `UnpaidOrder` に `Pay` を二回呼ぶことはコンパイルを通る。
C# には値を一度だけ使わせる線形型がないので、二重の支払を防ぐには永続化の一意制約のような別の仕組みが要る。

[TypestateTests](../../../../tests/CSharpCodeRecipes.Tests/Recipes/Typestate/TypestateTests.cs) は、誤った操作が CS1061 と CS1729 になることに加え、二重の支払がコンパイルを通ることも検査する。

## 実行

```bash
dotnet run --project src/CSharpCodeRecipes -- Typestate
```
