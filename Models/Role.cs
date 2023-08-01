using System.ComponentModel.DataAnnotations.Schema;

namespace ContactList.Models
{
    [Table("ContactListRole")]
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
