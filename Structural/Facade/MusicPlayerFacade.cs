using MusicPlayer.Builders;
using MusicPlayer.Models;
using MusicPlayer.Services;
using MusicPlayer.Structural.Composite;
using MusicPlayer.Structural.Decorator;
using MusicPlayer.Structural.Proxy;

namespace MusicPlayer.Structural.Facade;

/// <summary>
/// Facade pattern implementation.
/// Provides a simple, unified high-level interface over the entire playback, streaming,
/// caching, and playlist subsystems.
/// </summary>
public class MusicPlayerFacade
{
    private readonly ISongService _songService;
    private readonly AudioEngine _audioEngine;

    public ISongService SongService => _songService;
    public AudioEngine AudioEngine => _audioEngine;

    public MusicPlayerFacade(ISongService? songService = null, AudioEngine? audioEngine = null)
    {
        _songService = songService ?? new CachedSongServiceProxy(new RemoteCloudSongService());
        _audioEngine = audioEngine ?? AudioEngine.Instance;
    }

    /// <summary>
    /// Configures master output volume on the AudioEngine singleton.
    /// </summary>
    public void SetMasterVolume(int volume)
    {
        _audioEngine.SetVolume(volume);
    }

    /// <summary>
    /// Assembles an IAudioStream processing pipeline according to the requested DSP preset.
    /// Demonstrates runtime stacking of Decorators.
    /// </summary>
    public IAudioStream CreateStreamPipeline(AudioPreset preset)
    {
        IAudioStream stream = new BasicAudioStream();
        return preset switch
        {
            AudioPreset.BassBoosted => new BassBoostDecorator(stream, 8),
            AudioPreset.StudioMaster => new VolumeNormalizerDecorator(new BassBoostDecorator(stream, 4), -14),
            AudioPreset.ConcertHall => new ReverbDecorator(
                new VolumeNormalizerDecorator(new BassBoostDecorator(stream, 6), -14),
                "Symphony Hall"
            ),
            _ => stream
        };
    }

    /// <summary>
    /// Plays an individual track by ID.
    /// Coordinates: Facade -> Proxy (ISongService) -> Decorator (IAudioStream) -> AudioEngine (Singleton).
    /// </summary>
    public string PlayTrack(string trackId, AudioPreset preset = AudioPreset.Standard)
    {
        Console.WriteLine($"\n[MusicPlayerFacade] Requesting playback for track '{trackId}' with preset [{preset}]...");
        var song = _songService.GetSong(trackId);
        var pipeline = CreateStreamPipeline(preset);
        var dspOutput = pipeline.Process(song);

        Console.WriteLine($"  DSP Chain : {dspOutput}");
        _audioEngine.Play(song);
        return dspOutput;
    }

    /// <summary>
    /// Plays any Composite component (individual Song or hierarchical PlaylistGroup).
    /// Coordinates: Facade -> Composite -> Decorator -> AudioEngine.
    /// </summary>
    public void PlayComponent(IPlaylistItem component, AudioPreset preset = AudioPreset.Standard)
    {
        ArgumentNullException.ThrowIfNull(component);

        Console.WriteLine($"\n[MusicPlayerFacade] Initiating composite playback: '{component.Title}' (Total Duration: {component.Duration:m\\:ss})");
        component.Display(1);

        var pipeline = CreateStreamPipeline(preset);
        foreach (var song in component.GetAllSongs())
        {
            var dspOutput = pipeline.Process(song);
            Console.WriteLine($"  DSP Stream: {dspOutput}");
            _audioEngine.Play(song);
        }
    }

    /// <summary>
    /// Assembles a new Playlist using the Lab 1 PlaylistBuilder and tracks retrieved via Proxy.
    /// </summary>
    public Playlist CreatePlaylist(string playlistName, params string[] trackIds)
    {
        var builder = new PlaylistBuilder().WithName(playlistName);
        foreach (var id in trackIds)
        {
            var song = _songService.GetSong(id);
            builder.AddSong(song);
        }
        return builder.Build();
    }
}

