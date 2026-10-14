using MusicPlayer.Models;

namespace MusicPlayer.Abstractions;

public interface IEditablePlaylist : IReadablePlaylist
{
    void Add(Song song);
    void Remove(Song song);
}