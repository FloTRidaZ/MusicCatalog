namespace Client01.ru.kso.Database.Datatype
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
