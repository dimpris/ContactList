namespace ContactList.Models
{
    public class Phone
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public PhoneType Type { get; set; }
        public int ContactId { get; set; }
        public virtual Contact? Contact { get; set; }
    }
}
