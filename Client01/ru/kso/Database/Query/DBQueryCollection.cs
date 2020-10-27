namespace Client01.ru.kso.Database.Query
{
    public class DBQueryCollection
    {
        public const string QUERY_FROM_ARTIST = "EXEC select_from_artist;";

        public const string QUERY_FROM_ALBUM = "EXEC select_from_album;";

        public const string QUERY_FROM_TRACK = "EXEC select_from_track";

        public const string QUERY_FROM_REVIEW = "EXEC select_from_review {0};";

        public const string QUERY_FOR_MUSIC_SRC = "EXEC get_track_src '{0}';";

        public const string QUERY_FOR_REVIEW_INSERT = "EXEC insert_into_review '{0}', '{1}', {2};";

        public const string QUERY_FOR_ACC_INSERT = "EXEC insert_into_account '{0}', '{1}', '{2}';";

        public const string QUERY_FROM_ACCOUNT = "EXEC select_from_account '{0}', '{1}';";
    }
}
