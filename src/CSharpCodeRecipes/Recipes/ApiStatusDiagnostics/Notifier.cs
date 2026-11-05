using System.Diagnostics.CodeAnalysis;

namespace CSharpCodeRecipes.Recipes.ApiStatusDiagnostics
{
    /// <summary>
    /// 基盤側が提供する宛先への通知の送信
    /// </summary>
    public sealed class Notifier
    {
        /// <summary>
        /// 診断から開く説明の URL
        /// </summary>
        private const string HelpUrl = "https://github.com/YukiIto1999/CSharpCodeRecipes/blob/main/src/CSharpCodeRecipes/Recipes/ApiStatusDiagnostics/README.md";

        /// <summary>
        /// 送信した順の宛先と本文
        /// </summary>
        private readonly List<string> sent = [];

        /// <summary>
        /// これまでに送信した通知
        /// </summary>
        /// <value>送信した順の宛先と本文の組の文字列</value>
        public IReadOnlyList<string> Sent => sent;

        /// <summary>
        /// 一人の宛先への通知の送信
        /// </summary>
        /// <param name="recipient">通知を受け取る宛先</param>
        /// <param name="message">通知の本文</param>
        public void Notify(string recipient, string message)
        {
            sent.Add($"{recipient}: {message}");
        }

        /// <summary>
        /// 複数の宛先への同じ通知の一括の送信
        /// </summary>
        /// <param name="recipients">通知を受け取る宛先</param>
        /// <param name="message">全ての宛先に送る通知の本文</param>
        [Experimental(DiagnosticIds.BulkNotification, UrlFormat = HelpUrl)]
        public void NotifyAll(IEnumerable<string> recipients, string message)
        {
            foreach (var recipient in recipients)
            {
                Notify(recipient, message);
            }
        }

        /// <summary>
        /// <see cref="Notify"/> に名前を変える前の一人の宛先への通知の送信
        /// </summary>
        /// <param name="recipient">通知を受け取る宛先</param>
        /// <param name="message">通知の本文</param>
        [Obsolete("Notify に名前を変えた。", DiagnosticId = DiagnosticIds.RenamedSendNotification, UrlFormat = HelpUrl)]
        public void SendNotification(string recipient, string message)
        {
            Notify(recipient, message);
        }
    }
}
