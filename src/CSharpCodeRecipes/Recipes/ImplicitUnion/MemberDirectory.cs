namespace CSharpCodeRecipes.Recipes.ImplicitUnion
{
    /// <summary>
    /// 会員番号による登録済みの会員の検索
    /// </summary>
    public sealed class MemberDirectory
    {
        /// <summary>
        /// 会員番号を鍵とする登録済みの会員
        /// </summary>
        private readonly Dictionary<int, Member> members;

        /// <summary>
        /// 検索対象の会員の登録
        /// </summary>
        /// <param name="members">会員番号が重複しない登録済みの会員</param>
        /// <exception cref="ArgumentException">会員番号が重複した会員</exception>
        public MemberDirectory(IEnumerable<Member> members)
        {
            this.members = members.ToDictionary(member => member.Id);
        }

        /// <summary>
        /// 会員番号による会員の検索
        /// </summary>
        /// <param name="id">検索する会員番号</param>
        /// <returns>該当する会員か該当がないことのどちらか一方</returns>
        public Union<Member, MemberNotFound> Find(int id)
        {
            return members.TryGetValue(id, out var member) ? member : new MemberNotFound(id);
        }
    }
}
