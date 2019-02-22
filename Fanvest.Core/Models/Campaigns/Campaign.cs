using System;
using System.Collections.Generic;
namespace Fanvest.Core.Models.Campaigns
{
    public partial class Campaign : BaseEntity
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public decimal Price { get; set; }
        public int TokenCount { get; set; }
        public bool AllowBuying { get; set; }
        public bool AllowSelling { get; set; }
        public int BuyerCount { get; set; }
        public bool ShowOnHomePage { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        private ICollection<Token> _tokens;
        public virtual ICollection<Token> Tokens
        {
            get => _tokens ?? (_tokens = new List<Token>());
            protected set => _tokens = value;
        }
    }
}