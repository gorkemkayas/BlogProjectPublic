using System.ComponentModel.DataAnnotations;
using System.Data;
using BlogProject.src.Infra.Entitites.Base;

namespace BlogProject.src.Infra.Entitites
{
    public class WalletEntity : BaseEntity
    {
        public decimal Balance { get; set; }
        public Currency Currency { get; set; }
        public DateTime LastUpdated { get; set; }

        public byte[] RowVersion { get; set; }
        public Guid UserId { get; set; }
        public UserEntity User { get; set; }

    }
    public enum Currency
    {
        TurkishLira = 0,
        Euro = 1,
        Dollar = 2,
        Dinar = 3,
        BTC = 4,
        DogeCoin = 5
    }

}
