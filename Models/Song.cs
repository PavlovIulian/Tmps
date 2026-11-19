using MusicPlayer.Abstractions;
using MusicPlayer.Structural.Composite;

namespace MusicPlayer.Models;

/// <summary>
/// Domain model representing an audio song.
/// Acts as Prototype (Lab 1) and Leaf in the Composite pattern (Lab 2).
/// </summary>
public class Song : IPrototype<Song>, IPlaylistItem
{
    public string Title { get; }
    public string Artist { get; }
    public TimeSpan Duration { get; }

    public Song(string title, string artist, TimeSpan duration)
        => (Title, Artist, Duration) = (title, artist, duration);

    public Song Clone() => new(Title, Artist, Duration);

    public void Display(int depth = 0)
    {
        Console.WriteLine($"{new string(' ', depth * 2)}└── [Track] {Title} - {Artist} ({Duration:m\\:ss})");
    }

    public IEnumerable<Song> GetAllSongs()
    {
        yield return this;
    }
}
