using MusicPlayer.Models;
using MusicPlayer.Services;
using MusicPlayer.Structural.Composite;
using MusicPlayer.Structural.Facade;
using MusicPlayer.Structural.Proxy;
using Xunit;

namespace Tmps.Tests;

public class FacadeTests
{
    [Fact]
    public void Facade_CoordinatesSubsystems_ToRetrieveAndProcessTrack()
    {
        // Arrange
        var realService = new RemoteCloudSongService();
        var proxy = new CachedSongServiceProxy(realService);
        var facade = new MusicPlayerFacade(proxy);

        // Act
        var dspOutput = facade.PlayTrack("track-101", AudioPreset.ConcertHall);

        // Assert
        Assert.Contains("Midnight City", dspOutput);
        Assert.Contains("[Reverb: Symphony Hall]", dspOutput);
        Assert.Contains("[Normalized: -14 LUFS]", dspOutput);
        Assert.Contains("[BassBoost: +6dB", dspOutput);
        Assert.True(proxy.IsCached("track-101"));
    }

    [Fact]
    public void Facade_AppliesPresetsCorrectly_ThroughDecoratorPipeline()
    {
        // Arrange
        var facade = new MusicPlayerFacade();
        var song = new Song("Test", "Artist", TimeSpan.FromMinutes(3));

        // Act & Assert
        var standardStream = facade.CreateStreamPipeline(AudioPreset.Standard);
        var standardOutput = standardStream.Process(song);
        Assert.DoesNotContain("BassBoost", standardOutput);
        Assert.DoesNotContain("Reverb", standardOutput);

        var bassStream = facade.CreateStreamPipeline(AudioPreset.BassBoosted);
        var bassOutput = bassStream.Process(song);
        Assert.Contains("BassBoost: +8dB", bassOutput);

        var studioStream = facade.CreateStreamPipeline(AudioPreset.StudioMaster);
        var studioOutput = studioStream.Process(song);
        Assert.Contains("BassBoost: +4dB", studioOutput);
        Assert.Contains("Normalized: -14 LUFS", studioOutput);
    }

    [Fact]
    public void Facade_CreatesPlaylist_UsingUnderlyingBuilderAndProxy()
    {
        // Arrange
        var facade = new MusicPlayerFacade();

        // Act
        var playlist = facade.CreatePlaylist("My Mix", "track-101", "track-102");

        // Assert
        Assert.NotNull(playlist);
        Assert.Equal("My Mix", playlist.Name);
        Assert.Equal(2, playlist.Songs.Count);
        Assert.Equal("Midnight City", playlist.Songs[0].Title);
        Assert.Equal("Resonance", playlist.Songs[1].Title);
    }
}
