using System;
namespace Fanvest.Core.Models.Users
{
    public partial class Transaction : BaseEntity
    {
        public int UserId { get; set; }
        public string Description { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual User User { get; set; }
    }
}