namespace sensitiveAPI.Helper
{
    public class AesModel
    {
        public virtual string Version { get; set; }
        public virtual byte[] InitializationVector { get; set; }
        public virtual string Algorithm { get; set; }
        public virtual KeyDerivation KeyDerivation { get; set; }
        public virtual string Payload { get; set; }
    }
}