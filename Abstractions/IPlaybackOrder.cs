using MusicPlayer.Models;

namespace MusicPlayer.Abstractions;

public interface IPlaybackOrder
{
    IEnumerable<Song> Order(IEnumerable<Song> songs);
}