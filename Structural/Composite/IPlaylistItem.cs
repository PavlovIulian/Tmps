using MusicPlayer.Models;

namespace MusicPlayer.Structural.Composite;

/// <summary>
/// Component interface for the Composite pattern.
/// Treats individual audio tracks and composite playlists uniformly.
/// </summary>
public interface IPlaylistItem
{
    string Title { get; }
    TimeSpan Duration { get; }
    void Display(int depth = 0);
    IEnumerable<Song> GetAllSongs();
}

