﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Map
{
    public class MapFornecedor : IEntityTypeConfiguration<Fornecedor>
    {
        public void Configure(EntityTypeBuilder<Fornecedor> builder)
        {
            builder.ToTable("Fornecedor");
            ////Propriedades
            builder.HasKey(x => x.Id);
            builder.Property(x => x.InscricaoEstadual).HasMaxLength(150).IsRequired();
            builder.Property(x => x.NomeFantasia).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Uf).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Numero).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Complemento).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Bairro).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Cidade).HasMaxLength(150).IsRequired();
            builder.Property(x => x.CepFornecedor).HasMaxLength(150).IsRequired();
            builder.Property(x => x.StatusAtivo).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Cnpj).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Rua).HasMaxLength(150).IsRequired();
        }
    }
}
