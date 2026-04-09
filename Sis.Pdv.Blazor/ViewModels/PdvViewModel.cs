using Microsoft.Extensions.Options;
using Sis.Pdv.Blazor.Configuration;
using Sis.Pdv.Blazor.Models.Pdv;
using Sis.Pdv.Blazor.Models.Produto;
using Sis.Pdv.Blazor.Repositories;
using Sis.Pdv.Blazor.Services;
using Sis.Pdv.Blazor.ViewModels.Base;

namespace Sis.Pdv.Blazor.ViewModels;

/// <summary>
/// ViewModel principal do PDV — gerencia todo o estado da venda.
/// Centraliza lógica de negócio: busca, carrinho, pagamento, finalização.
/// </summary>
public sealed class PdvViewModel : ViewModelBase
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IVendaRepository _vendaRepository;
    private readonly IVendaRascunhoRepository _rascunhoRepository;
    private readonly IVendaApiService _vendaService;
    private readonly IItemVendaApiService _itemVendaService;
    private readonly PdvSettings _settings;
    private readonly ILogger<PdvViewModel> _logger;

    private EstadoPdv _estadoAtual = EstadoPdv.CaixaLivre;
    private VendaDto? _vendaAtual;
    private string _codigoBarras = string.Empty;
    private int _quantidade = 1;
    private bool _isLoading;
    private string _mensagemStatus = "CAIXA LIVRE";
    private string? _ultimoItemDescricao;
    private string? _mensagemErro;
    private string? _mensagemSucesso;
    private Guid _operadorId = Guid.Empty;
    private string _operadorNome = "Desconhecido";
    private bool _vendaRecuperada;


    public PdvViewModel(
        IProdutoRepository produtoRepository,
        IVendaRepository vendaRepository,
        IVendaRascunhoRepository rascunhoRepository,
        IVendaApiService vendaService,
        IItemVendaApiService itemVendaService,
        IOptions<PdvSettings> settings,
        ILogger<PdvViewModel> logger)
    {
        _produtoRepository = produtoRepository;
        _vendaRepository = vendaRepository;
        _rascunhoRepository = rascunhoRepository;
        _vendaService = vendaService;
        _itemVendaService = itemVendaService;
        _settings = settings.Value;
        _logger = logger;
    }

    #region Propriedades Bindáveis

    public EstadoPdv EstadoAtual
    {
        get => _estadoAtual;
        private set => SetProperty(ref _estadoAtual, value);
    }

    public VendaDto? VendaAtual
    {
        get => _vendaAtual;
        private set => SetProperty(ref _vendaAtual, value);
    }

    public string CodigoBarras
    {
        get => _codigoBarras;
        set => SetProperty(ref _codigoBarras, value);
    }

    public int Quantidade
    {
        get => _quantidade;
        set => SetProperty(ref _quantidade, value > 0 ? value : 1);
    }

    public bool IsLoading
    {
        get => _isLoading;
        private set => SetProperty(ref _isLoading, value);
    }

    public string MensagemStatus
    {
        get => _mensagemStatus;
        private set => SetProperty(ref _mensagemStatus, value);
    }

    public string? UltimoItemDescricao
    {
        get => _ultimoItemDescricao;
        private set => SetProperty(ref _ultimoItemDescricao, value);
    }

    public string? MensagemErro
    {
        get => _mensagemErro;
        private set => SetProperty(ref _mensagemErro, value);
    }

    public string? MensagemSucesso
    {
        get => _mensagemSucesso;
        private set => SetProperty(ref _mensagemSucesso, value);
    }

    private string? _mensagemInfo;
    public string? MensagemInfo
    {
        get => _mensagemInfo;
        private set => SetProperty(ref _mensagemInfo, value);
    }

    #endregion

    #region Computed Properties

    public decimal TotalVenda => VendaAtual?.ValorTotal ?? 0m;
    public int TotalItens => VendaAtual?.QuantidadeItensAtivos ?? 0;
    public bool TemItens => TotalItens > 0;
    public bool PodeFinalizarVenda => TemItens && VendaAtual?.PagamentoDefinido == true;
    public PdvSettings Settings => _settings;
    public bool VendaRecuperada => _vendaRecuperada;

    #endregion

    #region Recovery (Recuperacao de venda)

    /// <summary>
    /// Verifica se existe venda pendente (rascunho) no banco local.
    /// Chamado no startup do PDV.
    /// </summary>
    public async Task<bool> VerificarRascunhoPendenteAsync(CancellationToken ct = default)
    {
        try
        {
            var rascunho = await _rascunhoRepository.BuscarRascunhoPendenteAsync(
                _settings.NumeroCaixa, ct);

            if (rascunho != null && rascunho.Itens.Count > 0)
            {
                _logger.LogInformation("Rascunho de venda encontrado com {Count} itens", rascunho.Itens.Count);
                return true;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao verificar rascunho pendente");
        }
        return false;
    }

    /// <summary>
    /// Recupera venda do rascunho e coloca em andamento.
    /// </summary>
    public async Task RecuperarVendaAsync(CancellationToken ct = default)
    {
        try
        {
            var rascunho = await _rascunhoRepository.BuscarRascunhoPendenteAsync(
                _settings.NumeroCaixa, ct);

            if (rascunho == null || rascunho.Itens.Count == 0)
            {
                _logger.LogWarning("Nenhum rascunho para recuperar");
                return;
            }

            VendaAtual = rascunho;
            VendaAtual.ColaboradorId = _operadorId;
            VendaAtual.NomeOperador = _operadorNome;
            EstadoAtual = EstadoPdv.VendaEmAndamento;
            _vendaRecuperada = true;

            var ultimo = VendaAtual.Itens.LastOrDefault(i => !i.Cancelado);
            UltimoItemDescricao = ultimo != null
                ? $"{ultimo.Quantidade:N3} CDA X {ultimo.Descricao}"
                : null;
            MensagemStatus = "VENDA RECUPERADA";
            MensagemInfo = $"Venda recuperada com {rascunho.QuantidadeItensAtivos} item(ns)";

            _logger.LogInformation("Venda recuperada com {Count} itens", rascunho.QuantidadeItensAtivos);
            NotifyStateChanged();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao recuperar venda do rascunho");
            MensagemErro = "Erro ao recuperar venda anterior.";
        }
    }

    /// <summary>
    /// Descarta o rascunho e inicia venda nova.
    /// </summary>
    public async Task DescartarRascunhoAsync(CancellationToken ct = default)
    {
        try
        {
            var rascunho = await _rascunhoRepository.BuscarRascunhoPendenteAsync(
                _settings.NumeroCaixa, ct);
            if (rascunho != null)
            {
                await _rascunhoRepository.RemoverRascunhoAsync(rascunho.Id, ct);
            }
            IniciarNovaVenda();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao descartar rascunho");
        }
    }

    /// <summary>
    /// Salva estado atual da venda como rascunho (chamado a cada mudanca no carrinho).
    /// </summary>
    private async Task AutoSalvarRascunhoAsync()
    {
        if (VendaAtual == null || VendaAtual.QuantidadeItensAtivos == 0) return;

        try
        {
            await _rascunhoRepository.SalvarRascunhoAsync(VendaAtual, _settings.NumeroCaixa);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro no auto-save do rascunho");
        }
    }

    /// <summary>
    /// Remove rascunho apos venda finalizada ou cancelada.
    /// </summary>
    private async Task RemoverRascunhoAsync()
    {
        try
        {
            await _rascunhoRepository.RemoverRascunhoPorCaixaAsync(_settings.NumeroCaixa);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover rascunho");
        }
    }

    #endregion

    #region Inicialização

    public void IniciarNovaVenda()
    {
        VendaAtual = new VendaDto
        {
            ColaboradorId = _operadorId,
            NomeOperador = _operadorNome
        };
        EstadoAtual = EstadoPdv.CaixaLivre;
        CodigoBarras = string.Empty;
        Quantidade = 1;
        MensagemStatus = "CAIXA LIVRE";
        UltimoItemDescricao = null;
        MensagemErro = null;

        _logger.LogInformation("Nova venda iniciada — ID: {VendaId}", VendaAtual.Id);
        NotifyStateChanged();
    }

    public void AbrirVenda()
    {
        if (EstadoAtual != EstadoPdv.CaixaLivre) return;

        if (VendaAtual is null)
        {
            VendaAtual = new VendaDto
            {
                ColaboradorId = _operadorId,
                NomeOperador = _operadorNome
            };
        }

        EstadoAtual = EstadoPdv.VendaEmAndamento;
        MensagemStatus = "EM ATENDIMENTO";
        MensagemErro = null;

        _logger.LogInformation("Venda aberta — ID: {VendaId}", VendaAtual.Id);
        NotifyStateChanged();
    }

    public void DefinirOperador(Guid colaboradorId, string nomeOperador)
    {
        _operadorId = colaboradorId;
        _operadorNome = nomeOperador;
        
        if (VendaAtual != null)
        {
            VendaAtual.ColaboradorId = colaboradorId;
            VendaAtual.NomeOperador = nomeOperador;
        }
        NotifyStateChanged();
    }

    #endregion

    #region Busca e Adição de Produtos

    public async Task BuscarProdutoAsync(CancellationToken cancellationToken = default)
    {
        var codigo = CodigoBarras.Trim();
        if (string.IsNullOrWhiteSpace(codigo) || IsLoading) return;

        try
        {
            IsLoading = true;
            MensagemErro = null;
            MensagemStatus = "Buscando produto...";

            var produto = await _produtoRepository.BuscarPorCodigoBarrasAsync(
                codigo, cancellationToken);

            if (produto is null)
            {
                MensagemErro = $"Produto não encontrado: {codigo}";
                MensagemStatus = "PRODUTO NÃO ENCONTRADO";
                _logger.LogWarning("Produto não encontrado: {Codigo}", codigo);
                return;
            }

            AdicionarItem(produto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar produto: {Codigo}", codigo);
            MensagemErro = "Erro ao buscar produto. Tente novamente.";
            MensagemStatus = "ERRO NA BUSCA";
        }
        finally
        {
            IsLoading = false;
            CodigoBarras = string.Empty;
            Quantidade = 1;
        }
    }

    public void AdicionarItem(ProdutoDto produto)
    {
        if (VendaAtual is null)
            IniciarNovaVenda();

        var item = new ItemCarrinhoDto
        {
            Sequencial = VendaAtual!.ProximoSequencial,
            CodigoBarras = produto.CodBarras,
            Descricao = produto.NomeProduto,
            PrecoUnitario = produto.PrecoVenda,
            Quantidade = Quantidade,
            ProdutoId = Guid.TryParse(produto.Id, out var id) ? id : Guid.Empty,
            EstoqueDisponivel = produto.QuantidadeEstoqueProduto,
            DataVencimento = produto.DataVencimento
        };

        VendaAtual.Itens.Add(item);

        EstadoAtual = EstadoPdv.VendaEmAndamento;
        UltimoItemDescricao = $"{Quantidade:N3} CDA X {produto.NomeProduto}";
        MensagemStatus = UltimoItemDescricao;
        MensagemErro = null;

        _logger.LogInformation(
            "Item adicionado: {Descricao} x{Qtd} — R$ {Total:N2}",
            produto.NomeProduto, Quantidade, item.Total);

        // Auto-save rascunho a cada item adicionado
        _ = AutoSalvarRascunhoAsync();

        NotifyStateChanged();
    }

    #endregion

    #region Cancelamentos

    public bool CancelarItem(int sequencial, string motivo)
    {
        var item = VendaAtual?.Itens.FirstOrDefault(i => i.Sequencial == sequencial && !i.Cancelado);
        if (item is null) return false;

        item.Cancelado = true;

        _logger.LogInformation(
            "Item cancelado: {Seq} — {Descricao} — Motivo: {Motivo}",
            sequencial, item.Descricao, motivo);

        if (VendaAtual?.QuantidadeItensAtivos == 0)
        {
            EstadoAtual = EstadoPdv.CaixaLivre;
            MensagemStatus = "CAIXA LIVRE";
        }

        MensagemErro = null;

        // Auto-save rascunho apos cancelar item
        _ = AutoSalvarRascunhoAsync();

        NotifyStateChanged();
        return true;
    }

    public void CancelarVenda(string motivo)
    {
        if (VendaAtual is null) return;

        _logger.LogWarning(
            "Venda cancelada — ID: {VendaId}, Itens: {Itens}, Motivo: {Motivo}",
            VendaAtual.Id, VendaAtual.QuantidadeItensAtivos, motivo);

        // Remove rascunho ao cancelar venda
        _ = RemoverRascunhoAsync();

        IniciarNovaVenda();
    }

    #endregion

    #region Pagamento

    private decimal _valorPagoInput;
    public decimal ValorPagoInput
    {
        get => _valorPagoInput;
        set
        {
            if (SetProperty(ref _valorPagoInput, value))
            {
                OnPropertyChanged(nameof(TrocoPreview));
            }
        }
    }

    private string _formaPagamentoSelecionada = "DINHEIRO";
    public string FormaPagamentoSelecionada
    {
        get => _formaPagamentoSelecionada;
        set => SetProperty(ref _formaPagamentoSelecionada, value);
    }

    public decimal TrocoPreview => (VendaAtual != null && _valorPagoInput >= VendaAtual.ValorFinal)
        ? _valorPagoInput - VendaAtual.ValorFinal
        : 0m;

    public void IrParaPagamento()
    {
        if (VendaAtual is null || !TemItens) return;

        EstadoAtual = EstadoPdv.Pagamento;
        ValorPagoInput = 0; // Operador DEVE digitar o valor recebido do cliente
        FormaPagamentoSelecionada = "DINHEIRO";
        MensagemStatus = "INFORME O VALOR RECEBIDO";
        
        NotifyStateChanged();
    }

    public void CancelarPagamento()
    {
        if (VendaAtual != null)
        {
            VendaAtual.FormaPagamento = null;
            VendaAtual.ValorRecebido = 0;
        }
        EstadoAtual = EstadoPdv.VendaEmAndamento;
        MensagemStatus = UltimoItemDescricao ?? "EM ATENDIMENTO";
        NotifyStateChanged();
    }

    public async Task ConfirmarPagamento()
    {
        if (VendaAtual is null) return;

        if (ValorPagoInput < VendaAtual.ValorFinal)
        {
            MensagemErro = "Valor pago insuficiente.";
            return;
        }

        DefinirPagamento(FormaPagamentoSelecionada, ValorPagoInput);
        
        // Finalizar automaticamente após definir pagamento
        await FinalizarVendaAsync();
    }

    public void DefinirPagamento(string formaPagamento, decimal valorRecebido)
    {
        if (VendaAtual is null || !TemItens) return;

        VendaAtual.FormaPagamento = formaPagamento;
        VendaAtual.ValorRecebido = valorRecebido;

        // Mantém no estado Pagamento, mas agora com pagamento definido (PodeFinalizarVenda será true)
        MensagemStatus = $"PAGAMENTO CONFIRMADO: {formaPagamento}";
        MensagemErro = null;

        _logger.LogInformation(
            "Pagamento definido: {Forma} — Recebido: {Valor:C2}, Troco: {Troco:C2}",
            formaPagamento, valorRecebido, VendaAtual.Troco);

        NotifyStateChanged();
    }

    #endregion

    #region Finalização

    public async Task<bool> FinalizarVendaAsync(CancellationToken cancellationToken = default)
    {
        if (VendaAtual is null || !PodeFinalizarVenda) return false;

        try
        {
            IsLoading = true;
            EstadoAtual = EstadoPdv.Pagamento;
            MensagemStatus = "FINALIZANDO VENDA...";
            MensagemErro = null;
            MensagemSucesso = null;

            // 1. Preparar DTO da venda
            VendaAtual.NomeOperador = _operadorNome;
            VendaAtual.ColaboradorId = _operadorId;


            // Salvar ID para log posterior (pois VendaAtual será resetado)
            var vendaIdFinalizada = VendaAtual.Id;
            var valorFinal = VendaAtual.ValorFinal;
            var qtdItens = VendaAtual.QuantidadeItensAtivos;

            // 2. Salvar localmente (Offline First)
            await _vendaRepository.SalvarVendaAsync(VendaAtual, cancellationToken);

            // 2.1 Remover rascunho (venda concluida com sucesso)
            await RemoverRascunhoAsync();

            // 3. Mostrar tela de obrigado
            MensagemSucesso = "Obrigado por sua compra!";
            MensagemStatus = "VENDA FINALIZADA COM SUCESSO";
            EstadoAtual = EstadoPdv.Finalizada;
            NotifyStateChanged();

            _logger.LogInformation(
                "Venda finalizada — ID: {VendaId}, Total: {Total:C2}, Itens: {Itens}",
                vendaIdFinalizada, valorFinal, qtdItens);

            // Exibe tela de obrigado pelo tempo configurado
            await Task.Delay(_settings.TempoTelaObrigadoSegundos * 1000, cancellationToken);

            IniciarNovaVenda();

            return true;
        }
        catch (OperationCanceledException)
        {
            MensagemInfo = "Operação cancelada.";
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao finalizar venda.");
            MensagemErro = "Erro ao finalizar venda. Tente novamente.";
            EstadoAtual = EstadoPdv.Pagamento;
            return false;
        }
        finally
        {
            IsLoading = false;
        }
    }

    #endregion
}
