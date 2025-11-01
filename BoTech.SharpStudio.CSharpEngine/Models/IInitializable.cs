namespace BoTech.SharpStudio.CSharpEngine.Models;

public interface IInitializable
{
    /// <summary>
    /// Initialize all members that have to be initialized of the current instance.
    /// </summary>
    internal abstract void Initialize();
}