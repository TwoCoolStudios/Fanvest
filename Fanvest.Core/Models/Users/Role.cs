namespace Fanvest.Core.Models.Users
{
    public partial class Role : BaseEntity
    {
        public string Name { get; set; }
        public bool SystemRole { get; set; }
        public string SystemName { get; set; }
        public bool Active { get; set; }
    }
}