﻿using System.Collections.Generic;
using System.Globalization;
using System.Text;
namespace Fanvest.Services.Seo
{
    public partial class SlugHelper : ISlugHelper
    {
        public virtual string CreateSlug(string title)
        {
            title = title.ToLowerInvariant().Replace(" ", "-");
            title = RemoveDiacritics(title);
            title = RemoveReservedUrlCharacters(title);
            return title.ToLowerInvariant();
        }

        private string RemoveReservedUrlCharacters(string text)
        {
            var reservedCharacters = new List<string>
            {
                "!", "#", "$", "&", "'", "(", ")", "*", ",", "/",
                ":", ";", "=", "?", "@", "[", "]", "\"", "%", ".",
                "<", ">", "\\", "^", "_", "'", "{", "}", "|", "~",
                "`", "+"
            };
            foreach (var chr in reservedCharacters)
            {
                text = text.Replace(chr, "");
            }
            return text;
        }

        private string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();
            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }
            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}