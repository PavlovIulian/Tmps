using MusicPlayer.Abstractions;
using MusicPlayer.Models;

namespace MusicPlayer.Ordering;

public class ShuffleOrder : IPlaybackOrder
{
    public IEnumerable<Song> Order(IEnumerable<Song> songs)
    => songs.OrderBy(_ => Random.Shared.Next());
}