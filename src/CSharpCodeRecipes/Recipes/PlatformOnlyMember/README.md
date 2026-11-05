# 基盤だけが呼べるメンバーをドメインが継承する型に置く

基盤側が提供してドメイン側が継承する基底クラスに、ドメイン側に使わせるメンバーと、基盤側だけが使うメンバーを並べるパターンである。
ドメイン側に使わせるメンバーは `protected`、基盤側だけが使うメンバーは `internal` にする。
ドメイン側のアセンブリからは `internal` のメンバーが見えないので、呼ぼうとするとコンパイルエラーになる。

## 使いどころ

基盤側が保存やメッセージ送信の仕組みを持ち、ドメイン側がその仕組みに乗る集約やエンティティを書く構成で使う。
このレシピの例では、ドメインイベントの記録を集約自身に許し、未発行のイベントの取り出しを基盤側の保存処理だけに許す。
ドメイン側が保存を経ずにイベントを取り出したり、集約の外から偽のイベントを記録したりすることを防げる。

## 仕組み

| メンバー | 公開範囲 | 呼べる場所 |
|---|---|---|
| `Raise(IDomainEvent)` | `protected` | `AggregateRoot` を継承した集約の中だけ |
| `DequeueEvents()` | `internal` | 基盤側のアセンブリの中だけ |

```csharp
public sealed class Invoice : AggregateRoot
{
    public void Issue(string invoiceNumber) => Raise(new InvoiceIssued(invoiceNumber));
}
```

ドメイン側のアセンブリで `invoice.DequeueEvents()` と書くと、そのメソッドが見えずに CS1061 になる。
集約の外で `invoice.Raise(...)` と書くと、`protected` のメンバーには触れられずに CS0122 になる。
基盤側の `AggregateStore.Save` は同じアセンブリにあるので、`DequeueEvents` を呼んでイベントを発行できる。

記録の部品を集約のフィールドに持たせる合成でも同じ機能は作れる。
ただし、部品の記録の操作を public にする必要があるので、部品を受け取った集約の外のコードからも記録できてしまう。
記録を集約自身に限るために、このレシピは継承と `protected` を使う。

## 型では防げない範囲

基盤側のアセンブリが `InternalsVisibleTo` でドメイン側のアセンブリを指定すると、`DequeueEvents` はドメイン側からも見える。
`InternalsVisibleTo` を基盤側のテストのアセンブリだけに向けることで、この境界を保つ。
集約の作者が `Raise` を包む public のメソッドを足せば、集約の外からも記録できるようになるので、そこはレビューで止める。

[PlatformOnlyMemberTests](../../../../tests/CSharpCodeRecipes.Tests/Recipes/PlatformOnlyMember/PlatformOnlyMemberTests.cs) は、ドメイン側を別アセンブリとしてコンパイルし、二つの誤りが CS1061 と CS0122 になることを検査する。

## 実行

```bash
dotnet run --project src/CSharpCodeRecipes -- PlatformOnlyMember
```
