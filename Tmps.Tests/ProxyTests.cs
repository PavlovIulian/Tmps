using MusicPlayer.Models;
using MusicPlayer.Structural.Proxy;
using Xunit;

namespace Tmps.Tests;

public class ProxyTests
{
    [Fact]
    public void Proxy_DelegatesToRealService_OnInitialCacheMiss()
    {
        // Arrange
        var realService = new RemoteCloudSongService();
        var proxy = new CachedSongServiceProxy(realService);

        // Act
        var song = proxy.GetSong("track-101");

        // Assert
        Assert.NotNull(song);
        Assert.Equal("Midnight City", song.Title);
        Assert.Equal(1, proxy.RemoteFetchCount);
        Assert.True(proxy.IsCached("track-101"));
    }

    [Fact]
    public void Proxy_ReturnsCachedSongOnSubsequentCalls_WithoutHittingRemoteService()
    {
        // Arrange
        var realService = new RemoteCloudSongService();
        var proxy = new CachedSongServiceProxy(realService);

        // Act - Call 1 (Miss)
        var firstCall = proxy.GetSong("track-102");
        var fetchCountAfterFirst = proxy.RemoteFetchCount;

        // Act - Call 2 (Hit)
        var secondCall = proxy.GetSong("track-102");
        var fetchCountAfterSecond = proxy.RemoteFetchCount;

        // Assert
        Assert.Same(firstCall, secondCall);
        Assert.Equal(1, fetchCountAfterFirst);
        Assert.Equal(1, fetchCountAfterSecond); // Did not increment!
        Assert.Equal(1, proxy.CacheHits);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Proxy_BlocksInvalidTrackId_ThrowsArgumentException(string? invalidId)
    {
        // Arrange
        var proxy = new CachedSongServiceProxy(new RemoteCloudSongService());

        // Act & Assert
        Assert.Throws<ArgumentException>(() => proxy.GetSong(invalidId!));
    }

    [Fact]
    public void Proxy_BlocksRestrictedTrack_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var proxy = new CachedSongServiceProxy(new RemoteCloudSongService());

        // Act & Assert
        var ex = Assert.Throws<UnauthorizedAccessException>(() => proxy.GetSong("restricted-vip-content"));
        Assert.Contains("requires premium licensing", ex.Message);
    }
}

