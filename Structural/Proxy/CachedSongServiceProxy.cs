using MusicPlayer.Models;

namespace MusicPlayer.Structural.Proxy;

/// <summary>
/// Proxy component in the Proxy pattern.
/// Implements transparent in-memory caching and access validation before delegating to the real service.
/// </summary>
public class CachedSongServiceProxy : ISongService
{
    private readonly ISongService _realService;
    private readonly Dictionary<string, Song> _cache = new(StringComparer.OrdinalIgnoreCase);

    public int RemoteFetchCount => _realService.RemoteFetchCount;
    public int CacheHits { get; private set; }

    public CachedSongServiceProxy(ISongService realService)
    {
        _realService = realService ?? throw new ArgumentNullException(nameof(realService));
    }

    public Song GetSong(string trackId)
    {
        if (string.IsNullOrWhiteSpace(trackId))
        {
            throw new ArgumentException("Track ID cannot be null or empty.", nameof(trackId));
        }

        // Access control: restricted tier validation
        if (trackId.StartsWith("restricted-", StringComparison.OrdinalIgnoreCase))
        {
            throw new UnauthorizedAccessException($"Access denied: Track '{trackId}' requires premium licensing tier.");
        }

        // Cache lookup
        if (_cache.TryGetValue(trackId, out var cachedSong))
        {
            CacheHits++;
            Console.WriteLine($"[CachedSongServiceProxy] [CACHE HIT #{CacheHits}] Served '{cachedSong.Title}' ({trackId}) directly from RAM.");
            return cachedSong;
        }

        // Cache miss: delegate to real subject
        Console.WriteLine($"[CachedSongServiceProxy] [CACHE MISS] Delegating request for '{trackId}' to remote service...");
        var song = _realService.GetSong(trackId);
        _cache[trackId] = song;
        return song;
    }

    public bool IsCached(string trackId) => _cache.ContainsKey(trackId);
    public void ClearCache() => _cache.Clear();
}
