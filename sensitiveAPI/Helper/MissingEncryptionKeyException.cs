using System;

namespace sensitiveAPI.Helper
{
    public class MissingEncryptionKeyException : Exception
    {
        public MissingEncryptionKeyException(string message) : base(message)
        {
        }
    }
}