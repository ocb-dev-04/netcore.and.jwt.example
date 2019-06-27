using System;
using System.ComponentModel.DataAnnotations;

namespace DomainCore.Data.Entities
{
    public class SameProps
    {
        #region Properties

        [Key]
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }

        #endregion

        #region Construct

        public SameProps()
            => CreateDate = DateTime.UtcNow;

        #endregion
    }
}
