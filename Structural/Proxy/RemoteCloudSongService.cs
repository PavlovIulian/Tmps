using MusicPlayer.Models;

namespace MusicPlayer.Structural.Proxy;

/// <summary>
/// Real Subject in the Proxy pattern.
/// Simulates a remote cloud music catalog with network access overhead.
/// </summary>
public class RemoteCloudSongService : ISongService
{
    private readonly Dictionary<string, Song> _catalog = new(StringComparer.OrdinalIgnoreCase);

    public int RemoteFetchCount { get; private set; }

    public RemoteCloudSongService()
    {
        // Seed remote cloud catalog
        _catalog["track-101"] = new Song("Midnight City", "M83", TimeSpan.FromMinutes(4.05));
        _catalog["track-102"] = new Song("Resonance", "HOME", TimeSpan.FromMinutes(3.53));
        _catalog["track-103"] = new Song("Weightless", "Marconi Union", TimeSpan.FromMinutes(8.00));
        _catalog["track-104"] = new Song("Eye of the Tiger", "Survivor", TimeSpan.FromMinutes(4.08));
        _catalog["track-105"] = new Song("Blinding Lights", "The Weeknd", TimeSpan.FromMinutes(3.33));
    }

    public Song GetSong(string trackId)
    {
        RemoteFetchCount++;
        if (_catalog.TryGetValue(trackId, out var song))
        {
            Console.WriteLine($"[RemoteCloudSongService] [REMOTE FETCH #{RemoteFetchCount}] Retrieved '{song.Title}' ({trackId}) from cloud API.");
            return song;
        }

        throw new KeyNotFoundException($"Track '{trackId}' not found in remote catalog.");
    }
}

