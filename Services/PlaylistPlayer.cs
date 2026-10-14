using MusicPlayer.Abstractions;

namespace MusicPlayer.Services;

public class PlaylistPlayer
{
    private readonly IPlaybackOrder _order;

    public PlaylistPlayer(IPlaybackOrder order) => _order = order;

    public void Play(IReadablePlaylist playlist)
    {
        foreach (var song in _order.Order(playlist.Songs))
            Console.WriteLine($"Playing: {song.Title} - {song.Artist}");
    }
}