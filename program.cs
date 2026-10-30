using MusicPlayer.Abstractions;
using MusicPlayer.Builders;
using MusicPlayer.Models;
using MusicPlayer.Ordering;
using MusicPlayer.Services;

Console.WriteLine("  CREATIONAL DESIGN PATTERNS DEMONSTRATION");


// 1. SINGLETON PATTERN
Console.WriteLine(">>> 1. PATTERN: SINGLETON (AudioEngine)");
var engine1 = AudioEngine.Instance;
var engine2 = AudioEngine.Instance;

Console.WriteLine($"engine1 HashCode: {engine1.GetHashCode()}");
Console.WriteLine($"engine2 HashCode: {engine2.GetHashCode()}");
Console.WriteLine($"Are both references identical? {ReferenceEquals(engine1, engine2)}");

// Configure the singleton state
AudioEngine.Instance.SetVolume(85);
Console.WriteLine();

// 2. BUILDER PATTERN
Console.WriteLine(">>> 2. PATTERN: BUILDER (PlaylistBuilder)");
var chillPlaylist = new PlaylistBuilder()
    .WithName("Chill Beats")
    .AddSong("Midnight City", "M83", TimeSpan.FromMinutes(4.05))
    .AddSong("Resonance", "HOME", TimeSpan.FromMinutes(3.53))
    .AddSong("Weightless", "Marconi Union", TimeSpan.FromMinutes(8.00))
    .Build();

Console.WriteLine($"Created playlist '{chillPlaylist.Name}' with {chillPlaylist.Songs.Count} tracks via PlaylistBuilder.");
Console.WriteLine();

// 3. PROTOTYPE PATTERN
Console.WriteLine(">>> 3. PATTERN: PROTOTYPE (Playlist.Clone)");
// Clone existing playlist into a new workout mix
var workoutPlaylist = chillPlaylist.Clone("Workout Mix");

// Add a high-tempo song only to the cloned workout playlist
workoutPlaylist.Add(new Song("Eye of the Tiger", "Survivor", TimeSpan.FromMinutes(4.08)));

Console.WriteLine($"Original Playlist: '{chillPlaylist.Name}' ({chillPlaylist.Songs.Count} tracks)");
Console.WriteLine($"Cloned Playlist:   '{workoutPlaylist.Name}' ({workoutPlaylist.Songs.Count} tracks)");
Console.WriteLine($"Are playlist references different? {!ReferenceEquals(chillPlaylist, workoutPlaylist)}");
Console.WriteLine();

// PLAYBACK DEMONSTRATION WITH ORDERING STRATEGIES
Console.WriteLine(">>> PLAYBACK DEMONSTRATION");
PlayWith("Normal Order", new NormalOrder(), chillPlaylist);
PlayWith("Shuffle Order", new ShuffleOrder(), workoutPlaylist);
PlayWith("By Artist Order", new ByArtistOrder(), workoutPlaylist);

static void PlayWith(string label, IPlaybackOrder order, IReadablePlaylist playlist)
{
    Console.WriteLine($"\n=== {label} ===");
    var player = new PlaylistPlayer(order);
    player.Play(playlist);
}