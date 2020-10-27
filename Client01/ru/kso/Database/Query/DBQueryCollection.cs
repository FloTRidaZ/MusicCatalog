using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client01.ru.kso.Database.Query
{
    public class DBQueryCollection
    {
        public const string QUERY_FROM_ARTIST =
            "SELECT at.id, at.name, it.file_stream AS cover, tdt.file_stream AS info FROM artist_table AS at " +
                        "JOIN image_table AS it ON CAST(it.path_locator AS NVARCHAR(MAX)) LIKE CAST(at.cover AS NVARCHAR(MAX)) " +
                        "JOIN text_data_table AS tdt ON CAST(tdt.path_locator AS NVARCHAR(MAX)) LIKE CAST(at.info AS NVARCHAR(MAX));";

        public const string QUERY_FROM_ALBUM =
            "SELECT at.id, at.name, art.name AS artist, gt.name AS genre, it.file_stream AS cover, tdt.file_stream AS info " +
                        "FROM album_table AS at " +
                        "JOIN artist_table AS art ON art.id LIKE at.artist " +
                        "JOIN genre_table AS gt ON gt.id LIKE at.genre " +
                        "JOIN image_table AS it ON CAST(it.path_locator AS NVARCHAR(MAX)) LIKE CAST(at.cover AS NVARCHAR(MAX)) " +
                        "JOIN text_data_table AS tdt ON CAST(tdt.path_locator AS NVARCHAR(MAX)) LIKE CAST(at.info AS NVARCHAR(MAX));";

        public const string QUERY_FROM_TRACK =
            "SELECT tb.id, tb.name, alt.name AS album, mt.stream_id as src, tdt.file_stream as letters, tb.album_pos FROM track_table AS tb " +
                        "JOIN album_table AS alt ON alt.id LIKE tb.album " +
                        "JOIN music_table AS mt ON CAST(mt.path_locator AS NVARCHAR(MAX)) LIKE CAST(tb.src AS NVARCHAR(MAX)) " +
                        "JOIN text_data_table AS tdt ON CAST(tdt.path_locator AS NVARCHAR(MAX)) LIKE CAST(tb.letters AS NVARCHAR(MAX));";

        public const string QUERY_FROM_REVIEW =
            "SELECT ac.name, rev.review_data FROM review_table AS rev JOIN account_table AS ac ON ac.login LIKE rev.acc_id WHERE rev.album LIKE {0}";
    }
}
