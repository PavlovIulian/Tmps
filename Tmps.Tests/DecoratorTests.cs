using MusicPlayer.Models;
using MusicPlayer.Structural.Decorator;
using Xunit;

namespace Tmps.Tests;

public class DecoratorTests
{
    private readonly Song _testSong = new("Midnight City", "M83", TimeSpan.FromMinutes(4.05));

    [Fact]
    public void SingleDecorator_EnrichesBasicStream_WithSpecificEffect()
    {
        // Arrange
        IAudioStream baseStream = new BasicAudioStream();
        IAudioStream bassStream = new BassBoostDecorator(baseStream, gainDb: 8);

        // Act
        var result = bassStream.Process(_testSong);

        // Assert
        Assert.Contains("[PCM Stereo 44.1kHz", result);
        Assert.Contains("[BassBoost: +8dB @ 80Hz]", result);
    }

    [Fact]
    public void Decorators_CanBeStackedInAnyOrder_ProducesExpectedEffects()
    {
        // Arrange
        IAudioStream baseStream = new BasicAudioStream();

        // Chain 1: Basic -> BassBoost -> Normalizer
        IAudioStream chain1 = new VolumeNormalizerDecorator(
            new BassBoostDecorator(baseStream, 6),
            -14
        );

        // Chain 2: Basic -> Normalizer -> BassBoost (reversed stacking order)
        IAudioStream chain2 = new BassBoostDecorator(
            new VolumeNormalizerDecorator(baseStream, -14),
            6
        );

        // Act
        var result1 = chain1.Process(_testSong);
        var result2 = chain2.Process(_testSong);

        // Assert
        // Both chains must contain both effects
        Assert.Contains("[BassBoost: +6dB @ 80Hz]", result1);
        Assert.Contains("[Normalized: -14 LUFS]", result1);

        Assert.Contains("[BassBoost: +6dB @ 80Hz]", result2);
        Assert.Contains("[Normalized: -14 LUFS]", result2);

        // Order in chain 1 has BassBoost before Normalizer
        Assert.True(result1.IndexOf("[BassBoost") < result1.IndexOf("[Normalized"));

        // Order in chain 2 has Normalizer before BassBoost
        Assert.True(result2.IndexOf("[Normalized") < result2.IndexOf("[BassBoost"));
    }

    [Fact]
    public void Decorators_CanStackThreeOrMoreLayersDynamically()
    {
        // Arrange
        IAudioStream stream = new BasicAudioStream();
        stream = new BassBoostDecorator(stream, 4);
        stream = new VolumeNormalizerDecorator(stream, -16);
        stream = new ReverbDecorator(stream, "Plate Reverb");

        // Act
        var output = stream.Process(_testSong);

        // Assert
        Assert.Contains("[PCM Stereo", output);
        Assert.Contains("[BassBoost: +4dB", output);
        Assert.Contains("[Normalized: -16 LUFS]", output);
        Assert.Contains("[Reverb: Plate Reverb]", output);
    }
}

