using MusicPlayer.Models;

namespace MusicPlayer.Structural.Decorator;

/// <summary>
/// Abstract Base Decorator in the Decorator pattern.
/// Maintains a reference to the wrapped IAudioStream and forwards requests to it.
/// </summary>
public abstract class AudioStreamDecorator : IAudioStream
{
    protected readonly IAudioStream _innerStream;

    protected AudioStreamDecorator(IAudioStream innerStream)
    {
        _innerStream = innerStream ?? throw new ArgumentNullException(nameof(innerStream));
    }

    public virtual string Process(Song song)
    {
        return _innerStream.Process(song);
    }
}

