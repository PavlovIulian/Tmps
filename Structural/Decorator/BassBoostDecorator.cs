using MusicPlayer.Models;

namespace MusicPlayer.Structural.Decorator;

/// <summary>
/// Concrete Decorator adding bass equalization enhancement to the audio stream.
/// </summary>
public class BassBoostDecorator : AudioStreamDecorator
{
    private readonly int _gainDb;

    public BassBoostDecorator(IAudioStream innerStream, int gainDb = 6) : base(innerStream)
    {
        _gainDb = gainDb;
    }

    public override string Process(Song song)
    {
        return $"{base.Process(song)} + [BassBoost: +{_gainDb}dB @ 80Hz]";
    }
}

