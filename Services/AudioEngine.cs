using MusicPlayer.Models;

namespace MusicPlayer.Services;

public sealed class AudioEngine
{
    private static readonly Lazy<AudioEngine> _instance = new(() => new AudioEngine());

    public static AudioEngine Instance => _instance.Value;

    public int Volume { get; set; } = 70;
    public bool IsMuted { get; set; } = false;

    private AudioEngine()
    {
    }

    public void Play(Song song)
    {
        if (IsMuted)
        {
            Console.WriteLine($"[AudioEngine] (Muted) Skipping sound output for '{song.Title}'");
            return;
        }

        Console.WriteLine($"[AudioEngine] Outputting '{song.Title}' by {song.Artist} ({song.Duration:m\\:ss}) at Volume {Volume}%");
    }

    public void SetVolume(int volume)
    {
        Volume = Math.Clamp(volume, 0, 100);
        Console.WriteLine($"[AudioEngine] Master volume set to {Volume}%");
    }
}
