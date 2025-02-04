using BlogProject.src.Infra.Entitites.Base;
using BlogProject.src.Infra.Entitites.PartialEntities;

namespace BlogProject.src.Infra.Entitites
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
