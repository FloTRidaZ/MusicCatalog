using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client01.Scripts
{
    public class Review
    {
        public string Reviewer { get; }
        public string Content { get; }
        public Review(string reviewer, string content)
        {
            Reviewer = reviewer;
            Content = content;
        }
    }
}
