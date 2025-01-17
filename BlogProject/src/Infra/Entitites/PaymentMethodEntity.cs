using BlogProject.src.Infra.Entitites.PartialEntities;

namespace BlogProject.src.Infra.Entitites
{
    public class PaymentMethodEntity
    {
        public Guid Id { get; set; }
        public string Details { get; set; }
        public bool IsDefault { get; set; }
        public PaymentMethodType PaymentMethodType { get; set; }

        public Guid DonationId { get; set; }
        public virtual DonationEntity Donation { get; set; }
        
    }
}
