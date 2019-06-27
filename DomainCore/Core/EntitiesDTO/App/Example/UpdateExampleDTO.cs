using System;
using System.ComponentModel.DataAnnotations;

namespace DomainCore.Core.EntitiesDTO.App.Example
{
    public class UpdateExampleDTO : SamePropsDTO
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
        public DateTime CreateDate { get; set; }

        #endregion
    }
}
