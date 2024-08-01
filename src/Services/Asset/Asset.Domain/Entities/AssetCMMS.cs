using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Contracts.Domains;

namespace Asset.Domain.Entities
{
    public class AssetCMMS : EntityAuditBase<long>
    {
        [Required]
        [Column(TypeName = "varchar(150)")]
        public string Code { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(250)")]
        public string Name { get; set; }
    }
}
