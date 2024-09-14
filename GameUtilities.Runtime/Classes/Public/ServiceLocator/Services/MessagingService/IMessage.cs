namespace ADM87.GameUtilities.Messaging
{
    /// <summary>
    /// Represents a message.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// The name of the message.
        /// </summary>
        string Name { get; }
    }
}
