namespace Client01.Scripts
{
    public sealed class Track
    {
        public int Id { get; }
        public string Name { get; }
        public Album Album { get; }
        public Artist Artist { get; }
        public Asset Asset { get; }

        public Track(int id, string name, Album album, Asset asset)
        {
            Id = id;
            Name = name;
            Album = album;
            Artist = album.Artist;
            Asset = asset;
        }

        public string GetGenre()
        {
            return Album.GetGenre();
        }
        
    }
}
