using MusicPlayer.Abstractions;

namespace MusicPlayer.Models;

public class Song : IPrototype<Song>
{
    public string Title { get; }
    public string Artist { get; }
    public TimeSpan Duration { get; }

    public Song(string title, string artist, TimeSpan duration)
        => (Title, Artist, Duration) = (title, artist, duration);

    public Song Clone() => new(Title, Artist, Duration);
}