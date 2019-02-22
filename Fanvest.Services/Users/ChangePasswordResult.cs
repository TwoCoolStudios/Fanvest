﻿using System.Collections.Generic;
using System.Linq;
namespace Fanvest.Services.Users
{
    public partial class ChangePasswordResult
    {
        public ChangePasswordResult() => Errors = new List<string>();

        public bool Success => !Errors.Any();

        public void AddError(string error) => Errors.Add(error);

        public IList<string> Errors { get; set; }
    }
}