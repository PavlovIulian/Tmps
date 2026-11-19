# Music Player & Playlist System — Structural Design Patterns

**Laboratory Work No. 2 — Techniques and Mechanisms of Software Design (TMPS)**  
**Author:** Pavlov Iulian, Group FAF-241  
**Institution:** Technical University of Moldova (UTM), FCIM / FAF  

---

## 1. Domain Description

The **Music Player & Playlist Management System** is an extensible object-oriented audio player built in C# (.NET 10). It provides audio stream processing, remote cloud track retrieval with caching and access control, hierarchical nested playlist organization, and a unified client playback facade.

The system builds upon the creational patterns established in Laboratory Work 1 (`AudioEngine` Singleton, `PlaylistBuilder` Builder, `Song`/`Playlist` Prototype) and introduces four structural design patterns working in concert.

---

## 2. Implemented Structural Design Patterns

| Pattern | Category | Role in Project | Key Classes |
| :--- | :--- | :--- | :--- |
| **Decorator** | Structural | Dynamically wraps audio streams with real-time digital signal processing (DSP) effects in any stacking order. | `IAudioStream`, `BasicAudioStream`, `AudioStreamDecorator`, `BassBoostDecorator`, `VolumeNormalizerDecorator`, `ReverbDecorator` |
| **Proxy** | Structural | Controls access to remote audio catalogs, enforcing access-tier validation and transparent in-memory caching. | `ISongService`, `RemoteCloudSongService`, `CachedSongServiceProxy` |
| **Composite** | Structural | Treats individual tracks (`Song`) and nested groups of tracks (`PlaylistGroup` / albums / folders) uniformly through a single interface. | `IPlaylistItem`, `Song`, `PlaylistGroup` |
| **Facade** | Structural | Provides a high-level, unified playback API coordinating the proxy, composite trees, decorator DSP pipelines, and audio engine. | `MusicPlayerFacade`, `AudioPreset` |

---

## 3. Pattern Interaction

The application satisfies the mandatory requirement that execution paths cross through multiple interacting patterns:

```
[Client Application]
       │
       ▼
MusicPlayerFacade (FACADE)
  │
  ├── 1. Fetches track data via ISongService
  │       └── CachedSongServiceProxy (PROXY)
  │             ├── Cache Hit  --> Returns cached Song instantly
  │             └── Cache Miss --> RemoteCloudSongService (Real Subject)
  │
  ├── 2. Traverses hierarchical IPlaylistItem (COMPOSITE)
  │       ├── Song (Leaf)
  │       └── PlaylistGroup (Composite container)
  │
  ├── 3. Constructs runtime DSP pipeline via IAudioStream (DECORATOR)
  │       └── ReverbDecorator -> VolumeNormalizerDecorator -> BassBoostDecorator -> BasicAudioStream
  │
  └── 4. Transmits output to hardware audio sink
          └── AudioEngine.Instance (SINGLETON - Lab 1)
```

---

## 4. Project Structure

```
Tmps/
├── Abstractions/            # Domain interfaces (IReadablePlaylist, IEditablePlaylist, IPrototype, IPlaybackOrder)
├── Builders/                # Lab 1 Creational: PlaylistBuilder
├── Models/                  # Domain entities: Song (implements IPlaylistItem), Playlist
├── Ordering/                # Playback strategies: NormalOrder, ShuffleOrder, ByArtistOrder
├── Services/                # AudioEngine (Singleton), PlaylistPlayer
├── Structural/
│   ├── Composite/           # Composite Pattern: IPlaylistItem, PlaylistGroup
│   ├── Decorator/           # Decorator Pattern: IAudioStream, BasicAudioStream, AudioStreamDecorator, concrete decorators
│   ├── Facade/              # Facade Pattern: MusicPlayerFacade, AudioPreset
│   └── Proxy/               # Proxy Pattern: ISongService, RemoteCloudSongService, CachedSongServiceProxy
├── Tmps.Tests/              # 15 unit tests covering all four structural patterns (xUnit)
│   ├── CompositeTests.cs
│   ├── DecoratorTests.cs
│   ├── FacadeTests.cs
│   ├── ProxyTests.cs
│   └── Tmps.Tests.csproj
├── program.cs               # Console demonstration program
├── README.md                # Project documentation
├── reportlab2.md            # Markdown laboratory report
├── report.tex               # Formal LaTeX report source
├── report.pdf               # Compiled laboratory report document
├── Tmps.csproj              # Main executable project
├── Tmps.sln                 # Standard solution manifest
└── Tmps.slnx                # Modern .NET 10 solution manifest
```

---

## 5. Build, Run, and Test Instructions

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/)

### Run Console Demo
```bash
# Set environment if using local SDK installation
export DOTNET_ROOT=~/.dotnet
export PATH=~/.dotnet:$PATH

# Run main demonstration
cd /home/sunflower/TMPS/Tmps
dotnet run
```

### Run All Unit Tests (Single Command)
```bash
dotnet test
```

---

## 6. Unit Test Summary

15 unit tests across 4 test suites verify the critical properties of each pattern:
- **Decorator**: Stacking order independence, single-effect enrichment, multi-layer chaining.
- **Proxy**: Cache hit vs cache miss behavior, delegation verification, access-control exceptions for restricted tracks.
- **Composite**: Recursive total duration calculation across arbitrary nesting depths, uniform treatment of leaves and groups, empty group handling.
- **Facade**: Subsystem coordination, preset pipeline construction, integration with Lab 1 `PlaylistBuilder`.
