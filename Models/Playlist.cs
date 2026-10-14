using MusicPlayer.Abstractions;

namespace MusicPlayer.Models;

public class Playlist : IEditablePlaylist
{
    private readonly List<Song> _songs = new();

    public string Name { get; }
    public IReadOnlyList<Song> Songs => _songs;

    public Playlist(string name) => Name = name;

    public void Add(Song song) => _songs.Add(song);
    public void Remove(Song song) => _songs.Remove(song);
}