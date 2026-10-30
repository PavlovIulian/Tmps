using MusicPlayer.Models;

namespace MusicPlayer.Builders;

public class PlaylistBuilder
{
    private string _name = "Default Playlist";
    private readonly List<Song> _songs = new();

    public PlaylistBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public PlaylistBuilder AddSong(string title, string artist, TimeSpan duration)
    {
        _songs.Add(new Song(title, artist, duration));
        return this;
    }

    public PlaylistBuilder AddSong(Song song)
    {
        _songs.Add(song);
        return this;
    }

    public PlaylistBuilder AddSongs(IEnumerable<Song> songs)
    {
        _songs.AddRange(songs);
        return this;
    }

    public Playlist Build()
    {
        var playlist = new Playlist(_name);
        foreach (var song in _songs)
        {
            playlist.Add(song);
        }
        return playlist;
    }

    public PlaylistBuilder Reset()
    {
        _name = "Default Playlist";
        _songs.Clear();
        return this;
    }
}
