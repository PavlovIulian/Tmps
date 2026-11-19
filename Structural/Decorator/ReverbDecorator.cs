using MusicPlayer.Models;

namespace MusicPlayer.Structural.Decorator;

/// <summary>
/// Concrete Decorator adding acoustic room impulse response (reverberation).
/// </summary>
public class ReverbDecorator : AudioStreamDecorator
{
    private readonly string _roomType;

    public ReverbDecorator(IAudioStream innerStream, string roomType = "Concert Hall") : base(innerStream)
    {
        _roomType = roomType ?? "Studio";
    }

    public override string Process(Song song)
    {
        return $"{base.Process(song)} + [Reverb: {_roomType}]";
    }
}

