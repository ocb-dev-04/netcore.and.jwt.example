using System;

namespace DomainCore.Core.EntitiesDTO.App.Example
{
    public class ExampleDTO : SamePropsDTO
    {
        #region Properties

        public string ProductoName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }

        public DateTime CreateDate { get; set; }

        #endregion
    }
}
