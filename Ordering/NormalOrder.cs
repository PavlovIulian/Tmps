using MusicPlayer.Abstractions;
using MusicPlayer.Models;

namespace MusicPlayer.Ordering;

public class NormalOrder : IPlaybackOrder
{
    public IEnumerable<Song> Order(IEnumerable<Song> songs) => songs;
}