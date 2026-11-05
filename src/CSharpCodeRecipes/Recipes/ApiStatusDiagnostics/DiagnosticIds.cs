namespace CSharpCodeRecipes.Recipes.ApiStatusDiagnostics
{
    /// <summary>
    /// 基盤 API の状態を伝える診断 ID の一覧
    /// </summary>
    /// <remarks>
    /// 一度使った ID を別の意味に再利用しないための一箇所での採番
    /// </remarks>
    internal static class DiagnosticIds
    {
        /// <summary>
        /// 試験的な一括通知の API の利用
        /// </summary>
        public const string BulkNotification = "RECIPE0001";

        /// <summary>
        /// 名前を変えた通知の API の古い名前での利用
        /// </summary>
        public const string RenamedSendNotification = "RECIPE0002";
    }
}
