using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sensitiveAPI.Entities
{
    public class SensitiveDataEntity
    {
        /// <summary>
        /// UID created in code
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual long Id { get; set; }

        /// <summary>
        /// This is AesModel in Json Format 
        /// </summary>
        public virtual string Data { get; set; }

        /// <summary>
        /// The name of the encryption key used
        /// when encrypting this data
        /// </summary>
        public virtual string EncryptionKeyName { get; set; }
    }
}
