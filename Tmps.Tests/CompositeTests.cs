using MusicPlayer.Models;
using MusicPlayer.Structural.Composite;
using Xunit;

namespace Tmps.Tests;

public class CompositeTests
{
    [Fact]
    public void Composite_CalculatesTotalDuration_RecursivelyAcrossLeavesAndGroups()
    {
        // Arrange
        var song1 = new Song("Track A", "Artist 1", TimeSpan.FromMinutes(3));
        var song2 = new Song("Track B", "Artist 1", TimeSpan.FromMinutes(4));
        var song3 = new Song("Track C", "Artist 2", TimeSpan.FromMinutes(5));

        var subGroup = new PlaylistGroup("Sub Group");
        subGroup.Add(song1);
        subGroup.Add(song2); // 3 + 4 = 7 min

        var rootGroup = new PlaylistGroup("Root Group");
        rootGroup.Add(subGroup);
        rootGroup.Add(song3); // 7 + 5 = 12 min

        // Act
        var totalDuration = rootGroup.Duration;

        // Assert
        Assert.Equal(TimeSpan.FromMinutes(12), totalDuration);
        Assert.Equal(TimeSpan.FromMinutes(7), subGroup.Duration);
    }

    [Fact]
    public void CompositeLeafAndGroup_CanBeTreatedUniformly_ThroughSameInterface()
    {
        // Arrange
        IPlaylistItem leaf = new Song("Solo", "Artist", TimeSpan.FromMinutes(2.5));
        var group = new PlaylistGroup("Group");
        group.Add(new Song("Part 1", "Artist", TimeSpan.FromMinutes(1.5)));
        group.Add(new Song("Part 2", "Artist", TimeSpan.FromMinutes(2.0)));
        IPlaylistItem composite = group;

        // Act
        var items = new List<IPlaylistItem> { leaf, composite };
        var total = items.Aggregate(TimeSpan.Zero, (acc, item) => acc + item.Duration);

        // Assert
        Assert.Equal(TimeSpan.FromMinutes(6.0), total);
        Assert.Single(leaf.GetAllSongs());
        Assert.Equal(2, composite.GetAllSongs().Count());
    }

    [Fact]
    public void Composite_EmptyGroup_HasZeroDuration()
    {
        // Arrange
        var emptyGroup = new PlaylistGroup("Empty");

        // Act & Assert
        Assert.Equal(TimeSpan.Zero, emptyGroup.Duration);
        Assert.Empty(emptyGroup.GetAllSongs());
    }
}

