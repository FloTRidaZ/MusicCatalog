using System;
using System.Data.SqlTypes;

namespace Client01.ru.kso.Database.Datatype
{
    public sealed class Track
    {
        public int Id { get; }
        public string Name { get; }
        public Album Album { get; }
        public int AlbumPosition {get;}
        public Artist Artist { get; }
        public Guid SrcId { get; set; }
        public SqlBinary Letters { get; set; }

        public Track(int id, string name, Album album, int albumPos)
        {
            Id = id;
            Name = name;
            Album = album;
            AlbumPosition = albumPos;
            Artist = album.Artist;
        }

        public string GetStringGenre()
        {
            return Album.GetStringGenre();
        }
        
    }
}
