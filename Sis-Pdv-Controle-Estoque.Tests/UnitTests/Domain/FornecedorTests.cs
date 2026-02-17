using FluentAssertions;
using Model;
using Model.Base;
using Xunit;

namespace Sis_Pdv_Controle_Estoque.Tests.UnitTests.Domain;

public class FornecedorTests
{
    [Fact]
    public void Fornecedor_Constructor_ShouldSetProperties()
    {
        // Arrange
        var inscricaoEstadual = "123456789";
        var nomeFantasia = "Fornecedor Teste LTDA";
        var uf = "SP";
        var numero = "123";
        var complemento = "Sala 1";
        var bairro = "Centro";
        var cidade = "São Paulo";
        var cep = 12345678;
        var statusAtivo = 1;
        var cnpj = "12345678000195";
        var rua = "Rua Teste";

        // Act
        var fornecedor = new Fornecedor(
            inscricaoEstadual,
            nomeFantasia,
            uf,
            numero,
            complemento,
            bairro,
            cidade,
            cep,
            statusAtivo,
            cnpj,
            rua
        );

        // Assert
        fornecedor.InscricaoEstadual.Should().Be(inscricaoEstadual);
        fornecedor.NomeFantasia.Should().Be(nomeFantasia);
        fornecedor.Uf.Should().Be(uf);
        fornecedor.Numero.Should().Be(numero);
        fornecedor.Complemento.Should().Be(complemento);
        fornecedor.Bairro.Should().Be(bairro);
        fornecedor.Cidade.Should().Be(cidade);
        fornecedor.CepFornecedor.Should().Be(cep);
        fornecedor.StatusAtivo.Should().Be(statusAtivo);
        fornecedor.Cnpj.Should().Be(cnpj);
        fornecedor.Rua.Should().Be(rua);
    }

    [Fact]
    public void Fornecedor_ShouldInheritFromEntityBase()
    {
        // Arrange
        var fornecedor = new Fornecedor();

        // Assert
        fornecedor.Should().BeAssignableTo<EntityBase>();
    }

    [Fact]
    public void Fornecedor_DefaultConstructor_ShouldCreateInstance()
    {
        // Arrange & Act
        var fornecedor = new Fornecedor();

        // Assert
        fornecedor.Should().NotBeNull();
    }

    [Fact]
    public void Fornecedor_Properties_ShouldBeSettable()
    {
        // Arrange
        var fornecedor = new Fornecedor();
        var inscricaoEstadual = "987654321";
        var nomeFantasia = "Novo Fornecedor";
        var uf = "RJ";
        var numero = "456";
        var complemento = "Sala 2";
        var bairro = "Copacabana";
        var cidade = "Rio de Janeiro";
        var cep = 87654321;
        var statusAtivo = 0;
        var cnpj = "98765432000196";
        var rua = "Nova Rua";

        // Act
        fornecedor.InscricaoEstadual = inscricaoEstadual;
        fornecedor.NomeFantasia = nomeFantasia;
        fornecedor.Uf = uf;
        fornecedor.Numero = numero;
        fornecedor.Complemento = complemento;
        fornecedor.Bairro = bairro;
        fornecedor.Cidade = cidade;
        fornecedor.CepFornecedor = cep;
        fornecedor.StatusAtivo = statusAtivo;
        fornecedor.Cnpj = cnpj;
        fornecedor.Rua = rua;

        // Assert
        fornecedor.InscricaoEstadual.Should().Be(inscricaoEstadual);
        fornecedor.NomeFantasia.Should().Be(nomeFantasia);
        fornecedor.Uf.Should().Be(uf);
        fornecedor.Numero.Should().Be(numero);
        fornecedor.Complemento.Should().Be(complemento);
        fornecedor.Bairro.Should().Be(bairro);
        fornecedor.Cidade.Should().Be(cidade);
        fornecedor.CepFornecedor.Should().Be(cep);
        fornecedor.StatusAtivo.Should().Be(statusAtivo);
        fornecedor.Cnpj.Should().Be(cnpj);
        fornecedor.Rua.Should().Be(rua);
    }
}
