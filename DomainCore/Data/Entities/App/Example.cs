using System.ComponentModel.DataAnnotations;

namespace DomainCore.Data.Entities.App
{
    public class Example : SameProps
    {
        #region Properties

        [Required]
        public string ProductoName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int Stock { get; set; }

        #endregion
    }
}
