using System.ComponentModel.DataAnnotations;

namespace ReservationSystem.Models
{
    public class Item
    {
        public long Id { get; set; }
        public String Name { get; set; }
        public String? Description { get; set; }
        public virtual User Owner { get; set; }
        public List<Image>? Images { get; set; }
        public long accesCount { get; set; } //sisäinen kenttä - ei lähetetä verkon yli
    }

    public class ItemDTO
    {
        public long Id { get; set; }
        [Required]
        [MinLength(4)]
        [MaxLength(50)]

        public String Name { get; set; }
        public String? Description { get; set; }
        [Required]
        public String Owner { get; set; }
        public virtual List<ImageDTO>? Images { get; set; }
    }
}
