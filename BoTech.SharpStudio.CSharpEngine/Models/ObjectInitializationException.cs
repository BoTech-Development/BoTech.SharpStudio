namespace BoTech.SharpStudio.CSharpEngine.Models;
/// <summary>
/// This Execption will be normally thrown when any initialization of an object goes wrong
/// </summary>
public class ObjectInitializationException : Exception
{
    /// <summary>
    /// Default constructor
    /// </summary>
    public ObjectInitializationException() { }
    /// <summary>
    /// Constructor that creates a new Instance of the <see cref="ObjectInitializationException"/> with an error message.
    /// </summary>
    /// <param name="message"></param>
    public ObjectInitializationException(string message) : base(message) { }
    /// <summary>
    /// Constructor that creates a new Instance of the <see cref="ObjectInitializationException"/> with an error message and an inner exception.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public ObjectInitializationException(string message, Exception innerException)
        : base(message, innerException) { }

}