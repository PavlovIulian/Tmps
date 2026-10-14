using MusicPlayer.Abstractions;
using MusicPlayer.Models;

namespace MusicPlayer.Ordering;

public class ByArtistOrder : IPlaybackOrder
{
    public IEnumerable<Song> Order(IEnumerable<Song> songs)
        => songs
            .OrderBy(s => s.Artist, StringComparer.OrdinalIgnoreCase)
            .ThenBy(s => s.Title, StringComparer.OrdinalIgnoreCase);
}