# Music Player & Playlist System - SOLID Principles Implementation

**Laboratory Work 1**

---

## Table of Contents

- [Introduction](#introduction)
- [Features](#features)
- [SOLID Principles Applied](#solid-principles-applied)
  - [Single Responsibility Principle (SRP)](#single-responsibility-principle-srp)
  - [Open/Closed Principle (OCP)](#openclosed-principle-ocp)
  - [Liskov Substitution Principle (LSP)](#liskov-substitution-principle-lsp)
  - [Interface Segregation Principle (ISP)](#interface-segregation-principle-isp)
  - [Dependency Inversion Principle (DIP)](#dependency-inversion-principle-dip)
- [Project Structure](#project-structure)
  - [Directory Breakdown](#directory-breakdown)
- [Usage](#usage)
  - [Running the Application](#running-the-application)
  - [Example Output](#example-output)
- [Conclusion](#conclusion)
- [Author](#author)

---

## Introduction

Welcome to the **Music Player & Playlist Management System** project, developed as part of **Laboratory Work 1** for the **TMPS** (Techniques and Mechanisms of Software Design) course.

This project demonstrates the practical application of **SOLID** design principles to construct a modular, decoupled, and easily extensible music playback and playlist management architecture. The system models songs and playlists, supports multiple playback ordering strategies (Normal order, Shuffle order, and By-Artist order), and coordinates playback through decoupled abstractions.

---

## Features

- **Song & Playlist Management**: Model songs with title, artist, and duration; manage playlists with dynamic addition and removal of tracks.
- **Pluggable Playback Orders**:
  - **Normal Order**: Preserves the original playlist insertion sequence.
  - **Shuffle Order**: Dynamically randomizes song playback using shared entropy.
  - **By-Artist Order**: Sorts tracks alphabetically by artist name, then by title.
- **Interface Segregation**: Clean separation between read-only playlist consumer contracts and mutable playlist contracts.
- **High Extensibility**: Easily introduce new playback algorithms (e.g., sort by duration, reverse order, genre filtering) without modifying any existing player or playlist logic.
- **Dependency Inversion**: High-level player services rely exclusively on abstractions (`IPlaybackOrder`, `IReadablePlaylist`), facilitating constructor dependency injection.

---

## SOLID Principles Applied

### Single Responsibility Principle (SRP)

> **Definition**: A class should have only one reason to change, meaning it should encapsulate a single job or responsibility.

#### Implementation in the Project:
- **`Song`**: Acts strictly as an immutable data container representing a music track (title, artist, duration).
- **`Playlist`**: Solely responsible for storing and managing the collection of songs (`Add`, `Remove`). It does not handle playback mechanics, console output, or sorting algorithms.
- **`PlaylistPlayer`**: Responsible exclusively for orchestrating the playback presentation flow.
- **Ordering Strategies (`NormalOrder`, `ShuffleOrder`, `ByArtistOrder`)**: Each class encapsulates exactly one ordering algorithm.

```csharp
// Single Responsibility: Playlist only manages track storage and collection integrity.
public class Playlist : IEditablePlaylist
{
    private readonly List<Song> _songs = new();

    public string Name { get; }
    public IReadOnlyList<Song> Songs => _songs;

    public Playlist(string name) => Name = name;

    public void Add(Song song) => _songs.Add(song);
    public void Remove(Song song) => _songs.Remove(song);
}
```

---

### Open/Closed Principle (OCP)

> **Definition**: Software entities (classes, modules, functions, etc.) should be open for extension, but closed for modification.

#### Implementation in the Project:
- **`IPlaybackOrder` Interface**: Defines a common contract (`Order(IEnumerable<Song> songs)`).
- **Extensible Ordering Strategies**: Adding new playback behaviors (e.g. `ShuffleOrder`, `ByArtistOrder`, or future algorithms such as `DurationOrder`) does not require modifying `PlaylistPlayer` or existing order classes.

```csharp
// Open for extension, closed for modification:
// Adding ByArtistOrder implements IPlaybackOrder without modifying PlaylistPlayer or NormalOrder.
public class ByArtistOrder : IPlaybackOrder
{
    public IEnumerable<Song> Order(IEnumerable<Song> songs)
        => songs
            .OrderBy(s => s.Artist, StringComparer.OrdinalIgnoreCase)
            .ThenBy(s => s.Title, StringComparer.OrdinalIgnoreCase);
}
```

---

### Liskov Substitution Principle (LSP)

> **Definition**: Objects of a superclass should be replaceable with objects of a subclass without affecting the correctness of the program.

#### Implementation in the Project:
- Any implementation of `IPlaybackOrder` (`NormalOrder`, `ShuffleOrder`, `ByArtistOrder`) can be substituted interchangeably into `PlaylistPlayer` without altering the correctness of playback execution.
- `Playlist` implements `IEditablePlaylist`, which inherits from `IReadablePlaylist`. Any consumer expecting `IReadablePlaylist` (such as `PlaylistPlayer.Play`) accepts an instance of `Playlist` transparently without violating assumptions or behavior contracts.

---

### Interface Segregation Principle (ISP)

> **Definition**: Clients should not be forced to depend on interfaces they do not use.

#### Implementation in the Project:
- Instead of a single "fat" interface containing both reading and mutating operations, the interfaces are segregated:
  - **`IReadablePlaylist`**: Exposes read-only properties (`Name`, `IReadOnlyList<Song> Songs`).
  - **`IEditablePlaylist`**: Extends `IReadablePlaylist` by adding mutation methods (`Add`, `Remove`).
- **`PlaylistPlayer`** only needs to read songs for playback, so its `Play` method accepts `IReadablePlaylist`. It is not granted mutation capabilities, protecting the playlist from accidental alterations during playback.

```csharp
// Segregated interfaces: read-only consumer vs mutable owner
public interface IReadablePlaylist
{
    string Name { get; }
    IReadOnlyList<Song> Songs { get; }
}

public interface IEditablePlaylist : IReadablePlaylist
{
    void Add(Song song);
    void Remove(Song song);
}
```

---

### Dependency Inversion Principle (DIP)

> **Definition**: High-level modules should not depend on low-level modules. Both should depend on abstractions. Abstractions should not depend on details; details should depend on abstractions.

#### Implementation in the Project:
- **`PlaylistPlayer` (High-Level Service)**: Does not depend directly on concrete classes (`NormalOrder`, `Playlist`). Instead, it depends on `IPlaybackOrder` and `IReadablePlaylist` abstractions.
- **Dependency Injection**: The ordering algorithm is injected via the constructor of `PlaylistPlayer`, allowing runtime algorithm substitution.

```csharp
// Dependency Injection: Injecting the IPlaybackOrder abstraction into PlaylistPlayer
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
```

---

## Project Structure

### Directory Breakdown

```text
Tmps/
├── Abstractions/
│   ├── IEditablePlaylist.cs     # Interface for editable playlist (ISP)
│   ├── IPlaybackOrder.cs        # Common contract for playback ordering strategies (OCP, DIP)
│   └── IReadablePlaylist.cs     # Segregated read-only playlist interface (ISP, DIP)
├── Models/
│   ├── Playlist.cs              # Concrete playlist managing song collection (SRP)
│   └── Song.cs                  # Immutable track entity (SRP)
├── Ordering/
│   ├── ByArtistOrder.cs         # Strategy: sorts songs by artist and title (SRP, OCP)
│   ├── NormalOrder.cs           # Strategy: preserves initial playlist order (SRP, OCP)
│   └── ShuffleOrder.cs          # Strategy: randomizes playback order (SRP, OCP)
├── Services/
│   └── PlaylistPlayer.cs        # High-level service orchestrating playback via DIP
├── program.cs                   # Application entry point demonstrating polymorphism and DI
├── Tmps.csproj                  # .NET project configuration
└── report.md                    # Laboratory work report
```

---

## Usage

### Running the Application

Ensure the .NET SDK (version 8.0 or later) is installed. From the project directory, run:

```bash
dotnet run
```

### Example Output

```text
=== Normal order ===
Playing: Song A - Artist 2
Playing: Song B - Artist 1
Playing: Song C - Artist 3
Playing: Song D - Artist 1

=== Shuffle ===
Playing: Song D - Artist 1
Playing: Song A - Artist 2
Playing: Song C - Artist 3
Playing: Song B - Artist 1

=== By artist ===
Playing: Song B - Artist 1
Playing: Song D - Artist 1
Playing: Song A - Artist 2
Playing: Song C - Artist 3
```

---

## Conclusion

The **Music Player & Playlist Management System** provides a clear, practical demonstration of how SOLID design principles transform application architecture:

1. **SRP** ensures classes have focused, distinct responsibilities—separating data representations, collection manipulation, ordering algorithms, and playback orchestration.
2. **OCP** enables seamless integration of new ordering strategies without modifying existing codebase entities.
3. **LSP** guarantees that any playback order strategy or playlist implementation can be substituted without unintended side effects.
4. **ISP** enforces interface separation so components like `PlaylistPlayer` only depend on read access (`IReadablePlaylist`) rather than full mutation access.
5. **DIP** decouples high-level services from concrete implementations by injecting abstractions, drastically increasing testability and runtime flexibility.

Applying these principles results in clean, extensible, and maintainable software that easily adapts to evolving requirements.

---

## Author

- **Grigoraș Dumitru FAF-221**
- **Laboratory Work 1** – *Techniques and Mechanisms of Software Design (TMPS)*
