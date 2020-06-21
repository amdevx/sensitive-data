namespace sensitiveAPI.Helper
{
    public class KeyDerivation
    {
        public virtual string Function { get; set; }
        public virtual int WorkFactor { get; set; }
        public virtual byte[] Salt { get; set; }
    }
}