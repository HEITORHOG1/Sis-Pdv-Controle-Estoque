using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sis_Pdv_Controle_Estoque.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sis_Pdv_Controle_Estoque_Infra.Repositories.Map
{
    public class MapFornecedor : IEntityTypeConfiguration<Fornecedor>
    {
        public void Configure(EntityTypeBuilder<Fornecedor> builder)
        {
            builder.ToTable("Fornecedor");
            ////Propriedades
            builder.HasKey(x => x.Id);
            builder.Property(x => x.inscricaoEstadual).HasMaxLength(150).IsRequired();
            builder.Property(x => x.nomeFantasia).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Uf).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Numero).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Complemento).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Bairro).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Cidade).HasMaxLength(150).IsRequired();
            builder.Property(x => x.cepFornecedor).HasMaxLength(150).IsRequired();
            builder.Property(x => x.statusAtivo).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Cnpj).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Rua).HasMaxLength(150).IsRequired();
        }
    }
}
