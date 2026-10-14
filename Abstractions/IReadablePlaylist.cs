using MusicPlayer.Models;

namespace MusicPlayer.Abstractions;

public interface IReadablePlaylist
{
    string Name {get; }
    IReadOnlyList<Song> Songs {get; }
}