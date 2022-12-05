using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareersFralle.Models
{
    public class Click
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Performed { get; set; }
        public int ContentId { get; set; }

        public Click() { }

        public Click(int contentId)
        {
            Performed = DateTime.Now;
            ContentId = contentId;
        }
    }
}
