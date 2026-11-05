# Override以外のPublicメソッドの作成禁止

サービスの実装クラスに、インタフェースが定めるメソッド以外の Public メソッドを持たせないパターンである。
呼び出し側はインタフェースを通してしか実装を扱えないので、公開される操作はインタフェースの定義に限られる。

## 使いどころ

基盤側がサービスのインタフェースを定め、ドメイン側がその実装を書く構成で使う。
ドメイン側が実装クラスに便利なメソッドを足しても、呼び出し側からは見えず、テストで止まる。
サービスの入口を増やすにはインタフェースを変えるしかなくなるので、実装の都合で公開範囲が広がることを防げる。

## 仕組み

| 宣言 | 書き方 | 効果 |
|---|---|---|
| インタフェース | `public interface IService` | 呼び出し側が扱う唯一の型 |
| 実装クラス | `internal sealed class ServiceImpl` | アセンブリの外から見えない型と継承による Public メソッドの追加の阻止 |
| メソッド | `void IService.Execute()` | 明示的なインタフェース実装による `ServiceImpl` 型からの呼出しの阻止 |

`ServiceImpl` 型の変数から `Execute()` を呼ぶとコンパイルエラー CS1061 になり、`IService` 型の変数からは呼べる。

```csharp
IService service = new ServiceImpl();
service.Execute();
```

## テストによる検査

実装クラスに Public メソッドを足しても、コンパイルエラーにはならない。
また、`internal` はアセンブリの外に対してだけ働き、同じアセンブリの中からは実装クラスを直接扱える。
そこで [NoPermissionPublicMethodTests](../../../../tests/CSharpCodeRecipes.Tests/Recipes/NoPermissionPublicMethod/NoPermissionPublicMethodTests.cs) が、`IService` を実装する全てのクラスについて、`public` でないことと、Override 以外の Public メソッドを宣言していないことを検査する。
`ToString` のような基底クラスのメソッドのオーバーライドは、この検査で許容する。

## 実行

```bash
dotnet run --project src/CSharpCodeRecipes -- NoPermissionPublicMethod
```
