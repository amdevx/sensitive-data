namespace sensitiveAPI.Configuration
{
    public interface IEncryptionKeysConfig
    {
        /// <summary>
        /// the name of current key which use for encryption
        /// </summary>
        string CurrentKeyName { get; }

        /// <summary>
        /// Get the value of key by name
        /// </summary>
        string GetKeyByName(string keyName);

        /// <summary>
        /// Get the value of current key
        /// </summary>
        string GetCurrentKey();
    }
}