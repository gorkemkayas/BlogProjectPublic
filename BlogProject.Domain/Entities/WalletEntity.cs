using BlogProject.Domain.Entities.Base;
using BlogProject.Domain.Enums;

namespace BlogProject.Domain.Entities
{
    public class WalletEntity : BaseEntity
    {
        public decimal Balance { get; set; }
        public Currency Currency { get; set; }
        public DateTime LastUpdated { get; set; }

        public byte[] RowVersion { get; set; }
        public Guid UserId { get; set; }
        public virtual AppUser User { get; set; }

        public virtual ICollection<TransactionEntity>? TransactionEntities { get; set; }

    }

}
