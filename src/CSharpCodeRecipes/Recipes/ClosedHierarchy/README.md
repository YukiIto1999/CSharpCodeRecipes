# 別アセンブリからの派生を禁じる閉じた型階層

支払方法のような場合分けを、宣言したバリアントだけに閉じるパターンである。
基底の record に `private protected` の抽象メンバーを置き、別アセンブリからの派生をコンパイルエラーにする。

## 使いどころ

基盤側が場合分けの種類を所有し、ドメイン側や別チームのアセンブリがその型を使う構成で使う。
支払方法や注文の状態のように、種類ごとの扱いを基盤側が全て把握していたい概念に向く。
別アセンブリからはバリアントを足せないので、基盤側の知らない種類が処理に紛れ込むことを防げる。

## private コンストラクタだけでは閉じない理由

基底のコンストラクタを private にしても、抽象 record にはコンパイラが合成する protected のコピーコンストラクタが残る。
そのため、別アセンブリでも `: base(original)` でコピーコンストラクタを呼べば派生できる。

```csharp
public sealed record Points : PaymentMethod
{
    public Points(PaymentMethod original)
        : base(original)
    {
    }
}
```

`private protected` の抽象メンバーは、同じアセンブリの派生型からしかオーバーライドできない。
基底がこのメンバーを持てば、別アセンブリの派生型は抽象メンバーを実装できず、上のコードは CS0534 のコンパイルエラーになる。

## 仕組み

| 宣言 | 書き方 | 効果 |
|---|---|---|
| 基底 | `public abstract record PaymentMethod` | 呼び出し側が扱う型 |
| 基底のコンストラクタ | `private PaymentMethod()` | ネストした型以外からの通常の構築の阻止 |
| 外部からの派生の阻止 | `private protected abstract string GetLabel()` | 別アセンブリでの実装の不可能 |
| バリアント | 基底にネストした `public sealed record` | 宣言済みのバリアントからの派生の阻止 |
| 公開する振る舞い | `public string Label => GetLabel();` | 呼び出し側からの表示名の取得 |

`GetLabel` は外部からの派生を塞ぐだけでなく、バリアントごとの振る舞いも受け持つ。
同じアセンブリでバリアントを足したときに `GetLabel` を実装し忘れると、そのバリアントも CS0534 になる。

## 型では塞げない範囲とテストによる検査

同じアセンブリの中では、ネストしていない型もコピーコンストラクタと `GetLabel` のオーバーライドで派生できる。
この範囲は [ClosedHierarchyTests](../../../../tests/CSharpCodeRecipes.Tests/Recipes/ClosedHierarchy/ClosedHierarchyTests.cs) が、`PaymentMethod` の派生型が全て基底にネストした sealed のバリアントであることで検査する。
同じテストは、別アセンブリからの派生が CS0534 になることと、基底が `private protected` の抽象メンバーを持つことも検査する。

## 実行

```bash
dotnet run --project src/CSharpCodeRecipes -- ClosedHierarchy
```
