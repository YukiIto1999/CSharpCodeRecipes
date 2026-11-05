# 継承したメソッドの呼出しをコンパイルエラーにする

派生型にとって誤った使い方になる継承したメソッドを、`new` で隠して `[Obsolete(error: true)]` を付けるパターンである。
派生型から隠したメソッドを呼ぶと CS0619 のコンパイルエラーになり、`Obsolete` のメッセージで正しい代わりの呼び方を示せる。

継承を自分で選べるなら、基底クラスを継承せずに内部に持って必要な操作だけを公開する合成のほうが、誤った操作を確実に消せる。
このパターンは、ライブラリが派生を前提に基底クラスを提供していて継承を避けられない場合に使う。

## 仕組み

`BufferPool` は、`Pool<byte[]>` から継承した `Return` を、中身を消してから返す `ClearAndReturn` に置き換えたい。
中身を残したまま返すと、次にバッファを借りた処理に前の内容が見えてしまうためである。

```csharp
[Obsolete("中身を残したまま返すと次に借りた処理に前の内容が見えるので ClearAndReturn を使う。", error: true)]
public new void Return(byte[] buffer)
{
    throw new NotSupportedException("ClearAndReturn を使う。");
}
```

`BufferPool` 型の変数から `Return` を呼ぶと、隠した側のメソッドに束縛されて CS0619 になる。
本体の例外は、リフレクションのようにコンパイラの検査を経ない呼出しへの備えである。
Roslyn も、継承した `Free` を [同じ形](https://github.com/dotnet/roslyn/blob/38bc4bcb0612b786aa8bded52768c7133fde0ab5/src/Compilers/Core/Portable/DocumentationCommentId.cs#L22-L38) で塞ぎ、`ClearAndFree` を使わせている。

## 型では防げない範囲

隠したメソッドは基底のメソッドを置き換えないので、基底型の変数を経由すれば基底の `Return` がそのまま呼ばれる。
`Pool<byte[]>` 型の変数から `Return` を呼ぶとコンパイルを通り、中身が次の貸出しに残る。

`new` で隠す代わりに override に `[Obsolete(error: true)]` を付けても、呼出し側には何も報告されない。
C# の呼出しは override でなく基底の宣言に束縛されるためで、宣言側に CS0809 の警告が出るだけである。
.NET の `HashCode.GetHashCode` も [override に `[Obsolete(error: true)]` を付けている](https://github.com/dotnet/runtime/blob/7496f5e3859912052e96c65b80aa55aa100ec197/src/libraries/System.Private.CoreLib/src/System/HashCode.cs#L485-L491)が、`new HashCode().GetHashCode()` はコンパイルを通り、実行時の例外だけが誤用を止める。

[HideInheritedMemberTests](../../../../tests/CSharpCodeRecipes.Tests/Recipes/HideInheritedMember/HideInheritedMemberTests.cs) は、派生型からの呼出しが CS0619 になることを検査する。
同じテストは、基底型の経由で中身が残ることと、override の `Obsolete` が報告されないことも検査する。

## 実行

```bash
dotnet run --project src/CSharpCodeRecipes -- HideInheritedMember
```
