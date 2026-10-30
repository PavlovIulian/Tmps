namespace MusicPlayer.Abstractions;

public interface IPrototype<T>
{
    T Clone();
}
