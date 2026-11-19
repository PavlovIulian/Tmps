using MusicPlayer.Models;

namespace MusicPlayer.Structural.Proxy;

/// <summary>
/// Subject interface for the Proxy pattern.
/// Contract for retrieving audio tracks by ID.
/// </summary>
public interface ISongService
{
    Song GetSong(string trackId);
    int RemoteFetchCount { get; }
}
