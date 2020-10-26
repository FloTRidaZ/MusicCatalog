namespace Client01.Scripts
{
    public sealed class Track
    {
        public int Id { get; }
        public string Name { get; }
        public Album Album { get; }
        public int AlbumPosition {get;}
        public Artist Artist { get; }
        public Asset Asset { get; }

        public Track(int id, string name, Album album, int albumPos, Asset asset)
        {
            Id = id;
            Name = name;
            Album = album;
            AlbumPosition = albumPos;
            Artist = album.Artist;
            Asset = asset;
        }

        public string GetGenre()
        {
            return Album.GetGenre();
        }
        
    }
}
