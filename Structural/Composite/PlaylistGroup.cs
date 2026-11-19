using MusicPlayer.Models;

namespace MusicPlayer.Structural.Composite;

/// <summary>
/// Composite component in the Composite pattern.
/// Represents a hierarchical group/folder of playlist items (can contain Songs and child PlaylistGroups).
/// </summary>
public class PlaylistGroup : IPlaylistItem
{
    private readonly List<IPlaylistItem> _items = new();

    public string Title { get; }

    public TimeSpan Duration => _items.Aggregate(TimeSpan.Zero, (acc, item) => acc + item.Duration);

    public IReadOnlyList<IPlaylistItem> Items => _items.AsReadOnly();

    public PlaylistGroup(string title)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
    }

    public void Add(IPlaylistItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        _items.Add(item);
    }

    public void Remove(IPlaylistItem item)
    {
        _items.Remove(item);
    }

    public void Display(int depth = 0)
    {
        Console.WriteLine($"{new string(' ', depth * 2)}📁 [Playlist Group] '{Title}' (Duration: {Duration:m\\:ss}, Contains: {_items.Count} items)");
        foreach (var item in _items)
        {
            item.Display(depth + 1);
        }
    }

    public IEnumerable<Song> GetAllSongs()
    {
        foreach (var item in _items)
        {
            foreach (var song in item.GetAllSongs())
            {
                yield return song;
            }
        }
    }
}

