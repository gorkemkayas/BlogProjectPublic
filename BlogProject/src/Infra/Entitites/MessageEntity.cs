namespace BlogProject.src.Infra.Entitites
{
    public class MessageEntity
    {
        public Guid Id { get; set; }    
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime SentDate { get; set; }

        public Guid SenderId { get; set; }
        public virtual UserEntity Sender { get; set; }

        public Guid ReceiverId { get; set; }
        public virtual UserEntity Receiver { get; set; }
    }
}
