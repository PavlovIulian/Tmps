using MusicPlayer.Models;

namespace MusicPlayer.Structural.Decorator;

/// <summary>
/// Concrete Component in the Decorator pattern.
/// Represents raw PCM audio streaming without any digital signal processing.
/// </summary>
public class BasicAudioStream : IAudioStream
{
    public string Process(Song song)
    {
        ArgumentNullException.ThrowIfNull(song);
        return $"[PCM Stereo 44.1kHz | '{song.Title}' by {song.Artist}]";
    }
}
