using System;
using System.Collections.Generic;
using Fanvest.Web.Framework.Models;
namespace Fanvest.Web.Models.Users
{
    public partial class UserTransactionListModel : BaseModel
    {
        public UserTransactionListModel() => Transactions = new List<TransactionDetailsModel>();

        public IList<TransactionDetailsModel> Transactions { get; set; }

        public partial class TransactionDetailsModel : BaseModel
        {
            public string Description { get; set; }
            public string Credit { get; set; }
            public string Debit { get; set; }
            public string Balance { get; set; }
            public DateTime CreatedOn { get; set; }
        }
    }
}