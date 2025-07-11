using BlogProject.Domain.Entities.Base;
using BlogProject.Domain.Enums;

namespace BlogProject.Domain.Entities
{
    public class TransactionEntity : BaseEntity
    {
        public decimal Amount { get; set; }
        public Status? Status { get; set; }
        public TransactionType TransactionType { get; set; }

        public Guid WalletId { get; set; }
        public virtual WalletEntity Wallet { get; set; }
    }
    
}
