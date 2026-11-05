# 返せる型を型引数に限る和型

メソッドが返してよい型を、戻り値の型 `Union<T1, T2>` の型引数で宣言するパターンである。
候補の型の値からの暗黙の変換でしか構築できないので、型引数にない型の値を返すコードは CS0029 のコンパイルエラーになる。
ASP.NET Core の [`Results<TResult1, TResult2>`](https://github.com/dotnet/aspnetcore/blob/1c94b15aa54d3d490d0ee060b7d496db3cb291e7/src/Http/Http.Results/src/ResultsOfT.Generated.cs#L24-L62) も、エンドポイントが返せる応答をこの形で限っている。

## 使いどころ

サービスや Web API の境界で、処理が返す結果の種類を宣言するときに使う。
基盤側とドメイン側のどちらでも、戻り値の型を見るだけで起こり得る結果が分かる。
宣言にない種類の結果を返すコードは書けないので、呼び出し側が想定していない結果の追加を防げる。

## 仕組み

| 宣言 | 書き方 | 効果 |
|---|---|---|
| 構築 | `private Union()` と候補の型ごとの `implicit operator` | 候補の型の値からの変換だけによる構築 |
| バリアント | ネストした `private sealed class First` と `Second` | 外部からの派生とバリアントの追加の阻止 |
| 取り出し | `abstract TOut Match<TOut>(onFirst, onSecond)` | 両方の場合の処理の強制 |
| null | `where T1 : notnull` と変換の中の `ThrowIfNull` | nullable の警告と実行時の拒否 |

```csharp
public Union<Member, MemberNotFound> Find(int id)
{
    return members.TryGetValue(id, out var member) ? member : new MemberNotFound(id);
}
```

条件演算子の二つの分岐は型が違うが、戻り値の型を目標にして、それぞれが暗黙の変換で `Union<Member, MemberNotFound>` になる。
呼び出し側は `Match` に二つの場合の処理を渡し、どちらかを省くと引数が足りずにコンパイルエラーになる。
そのため、switch 式の discard のアームに頼らずに両方の場合を扱える。

## 型では防げない範囲

暗黙の変換は null も受け取れる。
`where T1 : notnull` によって、null になり得る値の変換は nullable の警告 CS8604 になる。
警告を無視して渡された null は、変換の中の `ThrowIfNull` が実行時に拒否する。
`Union<Member, Member>` のように同じ型を二つ挙げると、どちらの変換を使うかが決まらず CS0457 になるので、型引数には互いに異なる型を挙げる。

[ImplicitUnionTests](../../../../tests/CSharpCodeRecipes.Tests/Recipes/ImplicitUnion/ImplicitUnionTests.cs) は、型引数にない型の値が CS0029 になることと、別アセンブリからの派生が CS1729 になることを検査する。
同じテストは、null と同じ型の重複についても上の振る舞いを検査する。

## 実行

```bash
dotnet run --project src/CSharpCodeRecipes -- ImplicitUnion
```
