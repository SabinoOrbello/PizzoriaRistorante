namespace PizzoriaRistorante.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    [Table("Prodotti")]
    public partial class Prodotti
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Prodotti()
        {
            DettaglioOrdini = new HashSet<DettaglioOrdini>();
        }

        [Key]
        public int ProductId { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public string Image { get; set; } // percorso dell'immagine

        public decimal? Price { get; set; }

        public int? DeliveyTime { get; set; }

        public string Ingredients { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DettaglioOrdini> DettaglioOrdini { get; set; }

    }
}
