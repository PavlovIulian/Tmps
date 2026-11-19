using MusicPlayer.Models;

namespace MusicPlayer.Structural.Decorator;

/// <summary>
/// Concrete Decorator adding loudness normalization (EBU R128 standard).
/// </summary>
public class VolumeNormalizerDecorator : AudioStreamDecorator
{
    private readonly int _targetLufs;

    public VolumeNormalizerDecorator(IAudioStream innerStream, int targetLufs = -14) : base(innerStream)
    {
        _targetLufs = targetLufs;
    }

    public override string Process(Song song)
    {
        return $"{base.Process(song)} + [Normalized: {_targetLufs} LUFS]";
    }
}
