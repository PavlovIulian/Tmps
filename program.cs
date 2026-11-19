using MusicPlayer.Builders;
using MusicPlayer.Models;
using MusicPlayer.Services;
using MusicPlayer.Structural.Composite;
using MusicPlayer.Structural.Decorator;
using MusicPlayer.Structural.Facade;
using MusicPlayer.Structural.Proxy;

Console.WriteLine("===================================================================");
Console.WriteLine("  TMPS LAB 2: STRUCTURAL DESIGN PATTERNS - MUSIC PLAYER SUBSYSTEM  ");
Console.WriteLine("  Author: Pavlov Iulian, FAF-241 | Technical University of Moldova  ");
Console.WriteLine("===================================================================\n");

// =========================================================================
// 0. INTEGRATION WITH LAB 1 CREATIONAL PATTERNS
// =========================================================================
Console.WriteLine(">>> 0. LAB 1 CREATIONAL FOUNDATION (Singleton, Builder, Prototype)");
var engine = AudioEngine.Instance;
engine.SetVolume(80);

var sampleTrack = new Song("Intro", "The XX", TimeSpan.FromMinutes(2.12));
var clonedTrack = sampleTrack.Clone();
Console.WriteLine($"[Prototype] Cloned track '{clonedTrack.Title}' identical data, distinct ref: {!ReferenceEquals(sampleTrack, clonedTrack)}");

var builtPlaylist = new PlaylistBuilder()
    .WithName("Morning Motivation")
    .AddSong(sampleTrack)
    .AddSong("Wake Up", "Arcade Fire", TimeSpan.FromMinutes(5.35))
    .Build();
Console.WriteLine($"[Builder] Built playlist '{builtPlaylist.Name}' with {builtPlaylist.Songs.Count} tracks.\n");

// =========================================================================
// 1. STRUCTURAL PATTERN 1: DECORATOR
// =========================================================================
Console.WriteLine("===================================================================");
Console.WriteLine(">>> 1. PATTERN: DECORATOR (Dynamic DSP Audio Stream Effects)");
Console.WriteLine("===================================================================");
var testSong = new Song("Resonance", "HOME", TimeSpan.FromMinutes(3.53));

// Base concrete component
IAudioStream baseStream = new BasicAudioStream();
Console.WriteLine($"[Base Stream]      : {baseStream.Process(testSong)}");

// Wrap with BassBoostDecorator
IAudioStream bassStream = new BassBoostDecorator(baseStream, 10);
Console.WriteLine($"[Single Decorator] : {bassStream.Process(testSong)}");

// Wrap with multiple decorators (Stacked runtime chain)
IAudioStream studioMaster = new VolumeNormalizerDecorator(
    new BassBoostDecorator(baseStream, 6),
    -14
);
Console.WriteLine($"[Stacked Chain A]  : {studioMaster.Process(testSong)}");

// Alternative stacking order with Reverb added
IAudioStream concertChain = new ReverbDecorator(
    new VolumeNormalizerDecorator(
        new BassBoostDecorator(baseStream, 8),
        -12
    ),
    "Cathedral Chamber"
);
Console.WriteLine($"[Stacked Chain B]  : {concertChain.Process(testSong)}\n");

// =========================================================================
// 2. STRUCTURAL PATTERN 2: PROXY
// =========================================================================
Console.WriteLine("===================================================================");
Console.WriteLine(">>> 2. PATTERN: PROXY (Cached & Protected Remote Song Service)");
Console.WriteLine("===================================================================");
var realCloudService = new RemoteCloudSongService();
var songProxy = new CachedSongServiceProxy(realCloudService);

Console.WriteLine("--- Request 1: First time fetching 'track-101' ---");
var track1 = songProxy.GetSong("track-101");
Console.WriteLine($"Fetched: '{track1.Title}' by {track1.Artist} | Remote Fetch Count: {songProxy.RemoteFetchCount}");

Console.WriteLine("\n--- Request 2: Second time fetching 'track-101' (Should HIT cache) ---");
var track1Cached = songProxy.GetSong("track-101");
Console.WriteLine($"Fetched: '{track1Cached.Title}' | Remote Fetch Count: {songProxy.RemoteFetchCount} (No network fetch!)");
Console.WriteLine($"Are song instances identical in memory? {ReferenceEquals(track1, track1Cached)}");

Console.WriteLine("\n--- Request 3: Access Control Validation ---");
try
{
    songProxy.GetSong("restricted-exclusive-vip-track");
}
catch (UnauthorizedAccessException ex)
{
    Console.WriteLine($"[Access Control] Correctly intercepted: {ex.Message}\n");
}

// =========================================================================
// 3. STRUCTURAL PATTERN 3: COMPOSITE
// =========================================================================
Console.WriteLine("===================================================================");
Console.WriteLine(">>> 3. PATTERN: COMPOSITE (Hierarchical Playlists and Tracks)");
Console.WriteLine("===================================================================");
// Individual leaf songs
IPlaylistItem s1 = new Song("Strobe", "deadmau5", TimeSpan.FromMinutes(10.37));
IPlaylistItem s2 = new Song("Ghosts 'n' Stuff", "deadmau5", TimeSpan.FromMinutes(3.15));
IPlaylistItem s3 = new Song("Language", "Porter Robinson", TimeSpan.FromMinutes(6.08));
IPlaylistItem s4 = new Song("Shelter", "Porter Robinson", TimeSpan.FromMinutes(3.39));

// Sub-playlists (Composites)
var deadmau5Album = new PlaylistGroup("Album: For Lack of a Better Name");
deadmau5Album.Add(s1);
deadmau5Album.Add(s2);

var porterEP = new PlaylistGroup("EP: Worlds Collide");
porterEP.Add(s3);
porterEP.Add(s4);

// Root Mega-Playlist containing sub-playlists and an individual leaf track
var mainLibrary = new PlaylistGroup("Master Electronic Music Library");
mainLibrary.Add(deadmau5Album);
mainLibrary.Add(porterEP);
mainLibrary.Add(new Song("Bonus Outro", "DJ Ambient", TimeSpan.FromMinutes(1.45)));

// Uniform display and recursive duration computation
mainLibrary.Display(0);
Console.WriteLine($"[Composite] Total recursive duration of '{mainLibrary.Title}': {mainLibrary.Duration:m\\:ss}\n");

// =========================================================================
// 4. STRUCTURAL PATTERN 4: FACADE & FULL PATTERN INTERACTION
// =========================================================================
Console.WriteLine("===================================================================");
Console.WriteLine(">>> 4. PATTERN: FACADE (Unified Subsystem API & Pattern Interaction)");
Console.WriteLine("    Execution path: Client -> Facade -> Proxy -> Composite -> Decorator -> Singleton");
Console.WriteLine("===================================================================");

var playerFacade = new MusicPlayerFacade(songProxy, engine);
playerFacade.SetMasterVolume(85);

// Interaction 1: Facade -> Proxy -> Decorator -> Singleton
Console.WriteLine("\n--- Scenario A: Play Remote Track via Facade with ConcertHall DSP Preset ---");
playerFacade.PlayTrack("track-102", AudioPreset.ConcertHall);

// Interaction 2: Facade -> Composite -> Decorator -> Singleton
Console.WriteLine("\n--- Scenario B: Play Composite Tree via Facade with StudioMaster Preset ---");
var chillSession = new PlaylistGroup("Mini Chill Session");
chillSession.Add(songProxy.GetSong("track-101"));
chillSession.Add(songProxy.GetSong("track-103"));
playerFacade.PlayComponent(chillSession, AudioPreset.StudioMaster);

Console.WriteLine("\n===================================================================");
Console.WriteLine("  DEMONSTRATION COMPLETED SUCCESSFULLY! ALL PATTERNS VERIFIED.    ");
Console.WriteLine("===================================================================");
