namespace MusicPlayer.Models;

public class Song
{
    public string Title {get; }
    public string Artist {get; }
    public TimeSpan Duration { get; }

    public Song(string title, string artist, TimeSpan duration) => (Title, Artist, Duration) = (title, artist, duration);
}