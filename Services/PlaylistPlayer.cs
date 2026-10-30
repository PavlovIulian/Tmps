using MusicPlayer.Abstractions;

namespace MusicPlayer.Services;

public class PlaylistPlayer
{
    private readonly IPlaybackOrder _order;

    public PlaylistPlayer(IPlaybackOrder order) => _order = order;

    public void Play(IReadablePlaylist playlist)
    {
        Console.WriteLine($"\n--- Playing Playlist: '{playlist.Name}' ---");
        foreach (var song in _order.Order(playlist.Songs))
        {
            AudioEngine.Instance.Play(song);
        }
    }
}