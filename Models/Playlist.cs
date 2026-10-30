using MusicPlayer.Abstractions;

namespace MusicPlayer.Models;

public class Playlist : IEditablePlaylist, IPrototype<Playlist>
{
    private readonly List<Song> _songs = new();

    public string Name { get; set; }
    public IReadOnlyList<Song> Songs => _songs;

    public Playlist(string name) => Name = name;

    public void Add(Song song) => _songs.Add(song);
    public void Remove(Song song) => _songs.Remove(song);

    public Playlist Clone() => Clone($"{Name} (Copy)");

    public Playlist Clone(string newName)
    {
        var copy = new Playlist(newName);
        foreach (var song in _songs)
        {
            copy.Add(song.Clone());
        }
        return copy;
    }
}