using MusicPlayer.Abstractions;
using MusicPlayer.Models;
using MusicPlayer.Ordering;
using MusicPlayer.Services;

var playlist = new Playlist("Chill");
playlist.Add(new Song("Song A", "Artist 2", TimeSpan.FromMinutes(3)));
playlist.Add(new Song("Song B", "Artist 1", TimeSpan.FromMinutes(4)));
playlist.Add(new Song("Song C", "Artist 3", TimeSpan.FromMinutes(2)));
playlist.Add(new Song("Song D", "Artist 1", TimeSpan.FromMinutes(5)));

PlayWith("Normal order", new NormalOrder(), playlist);
PlayWith("Shuffle", new ShuffleOrder(), playlist);
PlayWith("By artist", new ByArtistOrder(), playlist);

static void PlayWith(string label, IPlaybackOrder order, IReadablePlaylist playlist)
{
    Console.WriteLine($"\n=== {label} ===");
    var player = new PlaylistPlayer(order);
    player.Play(playlist);
}