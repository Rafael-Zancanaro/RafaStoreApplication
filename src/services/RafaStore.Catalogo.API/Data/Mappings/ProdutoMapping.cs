using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RafaStore.Catalogo.API.Models;

namespace RafaStore.Catalogo.API.Data.Mappings;

public class ProdutoMapping : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.ToTable("Produtos");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nome)
            .HasColumnType("varchar(250)")
            .IsRequired();

        builder.Property(x => x.Descricao)
            .HasColumnType("varchar(250)")
            .IsRequired();

        builder.Property(x => x.Imagem)
            .HasColumnType("varchar(250)")
            .IsRequired();
    }
}