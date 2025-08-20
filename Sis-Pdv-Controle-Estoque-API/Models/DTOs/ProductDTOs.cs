using System.ComponentModel.DataAnnotations;
using Sis_Pdv_Controle_Estoque_API.Models.Validation;

namespace Sis_Pdv_Controle_Estoque_API.Models.DTOs
{
    /// <summary>
    /// DTO for creating a new product
    /// </summary>
    public class CreateProductRequest
    {
        [Required(ErrorMessage = "O código de barras é obrigatório")]
        [Barcode]
        public string CodBarras { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nome do produto é obrigatório")]
        [ProductName]
        public string NomeProduto { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
        public string? DescricaoProduto { get; set; }

        public bool IsPerecivel { get; set; }

        [NotEmptyGuid]
        public Guid FornecedorId { get; set; }

        [NotEmptyGuid]
        public Guid CategoriaId { get; set; }

        [Range(0, 1, ErrorMessage = "Status deve ser 0 (inativo) ou 1 (ativo)")]
        public int StatusAtivo { get; set; } = 1;
    }

    /// <summary>
    /// DTO for updating an existing product
    /// </summary>
    public class UpdateProductRequest
    {
        [Required(ErrorMessage = "O ID do produto é obrigatório")]
        [NotEmptyGuid]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O código de barras é obrigatório")]
        [Barcode]
        public string CodBarras { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nome do produto é obrigatório")]
        [ProductName]
        public string NomeProduto { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
        public string? DescricaoProduto { get; set; }

        public bool IsPerecivel { get; set; }

        [NotEmptyGuid]
        public Guid FornecedorId { get; set; }

        [NotEmptyGuid]
        public Guid CategoriaId { get; set; }

        [Range(0, 1, ErrorMessage = "Status deve ser 0 (inativo) ou 1 (ativo)")]
        public int StatusAtivo { get; set; }
    }

    /// <summary>
    /// DTO for product response
    /// </summary>
    public class ProductResponse
    {
        public Guid Id { get; set; }
        public string CodBarras { get; set; } = string.Empty;
        public string NomeProduto { get; set; } = string.Empty;
        public string? DescricaoProduto { get; set; }
        public bool IsPerecivel { get; set; }
        public Guid FornecedorId { get; set; }
        public string? FornecedorNome { get; set; }
        public Guid CategoriaId { get; set; }
        public string? CategoriaNome { get; set; }
        public int StatusAtivo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// DTO for product search and filtering
    /// </summary>
    public class ProductSearchRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Página deve ser maior que 0")]
        public int Page { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "Tamanho da página deve estar entre 1 e 100")]
        public int PageSize { get; set; } = 20;

        [StringLength(100, ErrorMessage = "Termo de busca deve ter no máximo 100 caracteres")]
        public string? Search { get; set; }

        public Guid? CategoriaId { get; set; }

        public Guid? FornecedorId { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsPerecivel { get; set; }
    }

    /// <summary>
    /// DTO for paginated product results
    /// </summary>
    public class PagedProductResponse
    {
        public IEnumerable<ProductResponse> Items { get; set; } = new List<ProductResponse>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}