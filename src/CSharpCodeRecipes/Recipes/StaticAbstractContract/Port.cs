using System.Globalization;

namespace CSharpCodeRecipes.Recipes.StaticAbstractContract
{
    /// <summary>
    /// 1 から 65535 の範囲に限る TCP のポート番号
    /// </summary>
    public sealed record Port : ISettingValue<Port>
    {
        /// <summary>
        /// 範囲を確かめた番号からのポート番号の構築
        /// </summary>
        /// <param name="number">1 から 65535 の範囲の番号</param>
        private Port(int number)
        {
            Number = number;
        }

        /// <summary>
        /// 接続に使うポートの番号
        /// </summary>
        /// <value>1 から 65535 の範囲の番号</value>
        public int Number { get; }

        /// <summary>
        /// 10 進数の文字列からのポート番号の作成
        /// </summary>
        /// <param name="text">符号のない 10 進数の文字列</param>
        /// <returns>文字列が表すポート番号</returns>
        /// <exception cref="FormatException">10 進数の整数として読めない文字列</exception>
        /// <exception cref="OverflowException"><see cref="int"/> の範囲を超える数</exception>
        /// <exception cref="ArgumentOutOfRangeException">1 から 65535 の範囲の外の数</exception>
        public static Port FromSetting(string text)
        {
            var number = int.Parse(text, NumberStyles.None, CultureInfo.InvariantCulture);
            ArgumentOutOfRangeException.ThrowIfLessThan(number, 1, nameof(text));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(number, 65535, nameof(text));
            return new Port(number);
        }
    }
}
