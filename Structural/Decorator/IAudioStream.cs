using MusicPlayer.Models;

namespace MusicPlayer.Structural.Decorator;

/// <summary>
/// Component interface for the Decorator pattern.
/// Defines the contract for processing/streaming audio data.
/// </summary>
public interface IAudioStream
{
    string Process(Song song);
}
