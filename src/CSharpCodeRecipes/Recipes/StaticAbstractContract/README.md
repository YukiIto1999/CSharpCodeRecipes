# 静的メンバーの規約を static abstract で型に出す

「この型は文字列から自分自身を作る静的メソッドを持つ」のような静的メンバーの規約を、static abstract のインタフェースメンバーで宣言するパターンである。
規約を満たさない型はコンパイルエラーになり、ジェネリックな処理は型引数からその静的メソッドを直接呼べる。

## 使いどころ

基盤側が設定の読み込みやリクエストのバインドの仕組みを提供し、ドメイン側がそこで使う値の型を定義する構成で使う。
基盤側は、文字列からの作り方をドメイン側の型に持たせることを、インタフェースの契約として要求できる。
規約を満たさない型はコンパイルエラーになるので、起動時や実行時のリフレクションによるエラーを防げる。

## 名前による規約の問題

ASP.NET Core の Minimal API は、パラメータの型が `BindAsync` という名前の静的メソッドを持つかを、実行時にリフレクションで探している（[ParameterBindingMethodCache.cs L210-L221](https://github.com/dotnet/aspnetcore/blob/1c94b15aa54d3d490d0ee060b7d496db3cb291e7/src/Shared/ParameterBindingMethodCache.cs#L210-L221)）。
名前が合っていてシグネチャが違うメソッドを見つけた場合は、実行時に InvalidOperationException を投げる（[同 L273-L285](https://github.com/dotnet/aspnetcore/blob/1c94b15aa54d3d490d0ee060b7d496db3cb291e7/src/Shared/ParameterBindingMethodCache.cs#L273-L285)）。
同じ規約を static abstract で宣言した [IBindableFromHttpContext&lt;TSelf&gt;](https://github.com/dotnet/aspnetcore/blob/1c94b15aa54d3d490d0ee060b7d496db3cb291e7/src/Http/Http.Abstractions/src/IBindableFromHttpContextOfT.cs#L13-L21) を実装する経路では、シグネチャの誤りをコンパイラが検出する。

## 仕組み

```csharp
public interface ISettingValue<TSelf>
    where TSelf : ISettingValue<TSelf>
{
    public static abstract TSelf FromSetting(string text);
}

public T Get<T>(string key)
    where T : ISettingValue<T>
{
    return T.FromSetting(texts[key]);
}
```

| 誤り | 結果 |
|---|---|
| 契約を実装しない型を `Get<Uri>` のように読み出す | CS0311 |
| `FromSetting` を持たずに契約を実装する | CS0535 |
| `FromSetting(int)` のように引数の型を変えて実装する | CS0535 |

型引数 `TSelf` に実装する型自身を渡す制約によって、`FromSetting` の戻り値をその型自身に固定できる。
.NET の `IParsable<TSelf>` も同じ形の契約なので、書式の指定を受け取る変換が要るならそちらを使える。

## 型では防げない範囲

契約が固定するのはシグネチャだけで、変換の中身が正しいかは決めない。
`Port.FromSetting` が範囲の外の数を拒むことは、[StaticAbstractContractTests](../../../../tests/CSharpCodeRecipes.Tests/Recipes/StaticAbstractContract/StaticAbstractContractTests.cs) の実行時のテストで確かめる。
同じテストは、三つの誤りが上のコンパイルエラーになることも検査する。

## 実行

```bash
dotnet run --project src/CSharpCodeRecipes -- StaticAbstractContract
```
