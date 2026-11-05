using System.Reflection;
using CSharpCodeRecipes.Recipes.NoPermissionPublicMethod;

namespace CSharpCodeRecipes.Tests.Recipes.NoPermissionPublicMethod
{
    /// <summary>
    /// コンパイラが検出しない <see cref="IService"/> の実装クラスへの Public メソッドの追加の検出
    /// </summary>
    public class NoPermissionPublicMethodTests
    {
        /// <summary>
        /// 検査対象の <see cref="IService"/> の全実装クラス
        /// </summary>
        /// <value>このアプリのアセンブリにある具象の実装クラス</value>
        public static TheoryData<Type> ServiceImplementations { get; } = new(
            typeof(IService).Assembly.GetTypes()
                .Where(type => type is { IsClass: true, IsAbstract: false } && typeof(IService).IsAssignableFrom(type)));

        /// <summary>
        /// アセンブリの外から見えない実装クラスの検査
        /// </summary>
        /// <param name="implementation">検査する <see cref="IService"/> の実装クラス</param>
        [Theory]
        [MemberData(nameof(ServiceImplementations))]
        public void Implementation_IsNotPublic(Type implementation)
        {
            Assert.False(implementation.IsVisible, $"{implementation.Name} は public にせず internal にする。");
        }

        /// <summary>
        /// 実装クラスでの Override 以外の Public メソッドの不在の検査
        /// </summary>
        /// <param name="implementation">検査する <see cref="IService"/> の実装クラス</param>
        [Theory]
        [MemberData(nameof(ServiceImplementations))]
        public void Implementation_DeclaresNoPublicMethodsExceptOverrides(Type implementation)
        {
            var violations = implementation
                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Where(method => !IsOverride(method))
                .Select(method => method.Name)
                .ToArray();

            Assert.True(
                violations.Length == 0,
                $"{implementation.Name} に Override 以外の Public メソッドがある: {string.Join(", ", violations)}");
        }

        /// <summary>
        /// 基底クラスのメソッドのオーバーライドの判定
        /// </summary>
        /// <param name="method">判定する Public メソッド</param>
        /// <returns>基底クラスのメソッドをオーバーライドしている場合の true</returns>
        private static bool IsOverride(MethodInfo method)
        {
            return method.GetBaseDefinition().DeclaringType != method.DeclaringType;
        }
    }
}
