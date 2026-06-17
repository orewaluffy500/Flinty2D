namespace Flinty.ModSystem;

public interface IModule
{
    APIBuilder Builder { get; }
    ModEngine Engine { get; }
    void Build();
}