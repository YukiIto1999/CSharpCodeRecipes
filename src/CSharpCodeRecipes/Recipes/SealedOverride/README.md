# 基底クラスの振る舞いをドメイン側に上書きさせない

基盤側が提供してドメイン側が継承する基底クラスで、変えてはいけない仮想メンバーを `sealed override` にするパターンである。
派生したクラスでそのメンバーを上書きしようとすると、CS0239 のコンパイルエラーになる。

## 使いどころ

基盤側がエンティティや集約の基底クラスを提供し、保存や同一性の判定をその前提で作る構成で使う。
このレシピの例では、型と ID が同じなら同じエンティティとみなす等価性を、基盤側が固定する。
ドメイン側が等価性を独自に上書きし、変更の追跡やキャッシュが同じエンティティを別のものとして扱う事故を防げる。

## 仕組み

`Equals` と `GetHashCode` は `object` の仮想メソッドなので、基底クラスで override しただけでは、派生したクラスがさらに override できる。
基底クラスで `sealed override` にすると、それより下の型は override できなくなる。

```csharp
public sealed override bool Equals(object? obj)
{
    return obj is Entity<TId> other
        && other.GetType() == GetType()
        && EqualityComparer<TId>.Default.Equals(Id, other.Id);
}
```

`GetType()` を比べるので、ID が同じでも型が違うエンティティ（顧客と仕入先など）は別のものとみなす。
継承を自分で選べるなら、record のように等価性を型ごとに持つ方法もある。
このパターンは、基盤側が継承を必須にしている基底クラスで、変えてはいけない振る舞いを守るために使う。

## 型では防げない範囲

`sealed` が禁じるのは override だけで、`new` による同じ名前のメソッドの隠蔽は禁じない。
派生したクラスが `public new bool Equals(object? obj)` を宣言するとコンパイルを通り、その型の変数から呼んだときだけ隠蔽した側が使われる。
`HashSet<T>` のように `object` の `Equals` を通して比べる処理は基底の等価性を使い続けるので、等価性が二通りになる。
隠蔽はレビューで止めるか、基底の型にない `Equals` を宣言した派生型をリフレクションのテストで探して止める。

[SealedOverrideTests](../../../../tests/CSharpCodeRecipes.Tests/Recipes/SealedOverride/SealedOverrideTests.cs) は、等価性の上書きが CS0239 になることを検査する。
同じテストは、`new` による隠蔽がコンパイルを通り、変数の型によって等価性の結果が変わることも検査する。

## 実行

```bash
dotnet run --project src/CSharpCodeRecipes -- SealedOverride
```
