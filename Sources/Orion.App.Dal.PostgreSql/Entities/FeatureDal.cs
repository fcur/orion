using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Orion.App.Dal.PostgreSql.Entities;

[Table("Features")]
internal sealed class FeatureDal
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }
    
    public string ContentId { get; set; } = null!;
    
    public  string Title { get; set; }= null!;
    
    public  DateTimeOffset StartAt { get; set; }
    
    public  DateTimeOffset? EndAt { get; set; }
    
    public  DateTimeOffset ChangedAt { get; set; }
    
    [Column(TypeName = "jsonb")]
    public FeatureContentDal[] Contents { get; set; } = null!;
    
    public long Version { get; set; }
    
    [Timestamp]
    public uint ConcurrencyToken { get; set; }
    
    internal static void ConfigureModel(EntityTypeBuilder<FeatureDal> entityTypeBuilder)
    {
        entityTypeBuilder.HasIndex(v => v.ContentId);
    }
}