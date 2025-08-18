using Sis_Pdv_Controle_Estoque_Form.Dto.Categoria;
using Sis_Pdv_Controle_Estoque_Form.Services.Categoria;
using System.ComponentModel;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Categoria
{
    public partial class CadCategoria : Form
    {
        CategoriaService categoriaService;
        private BindingList<CategoriaDto> categoriasList;

        public CadCategoria()
        {
            InitializeComponent();
            categoriasList = new BindingList<CategoriaDto>();
        }

        private async void btnCadastrar_Click_1(object sender, EventArgs e)
        {
            try
            {
                await CadastrarCategoria();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao cadastrar categoria: {ex.Message}");
            }
        }
        private async void CadCategoria_Load(object sender, EventArgs e)
        {
            await Consultar();
        }

        private async void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtNomeCategoria.Text))
                {
                    await ConsultarPorNomeCategoria(txtNomeCategoria.Text);
                }
                else
                {
                    await Consultar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro na consulta: {ex.Message}");
            }
        }
        private async void btnAlterar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNomeCategoria.Text))
            {
                AltCategoria form = new AltCategoria(txtNomeCategoria.Text, LblId.Text);
                form.ShowDialog();
                await Consultar(); // Atualiza após fechar o dialog
            }
        }
        private async void btnExcluir_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNomeCategoria.Text))
            {
                ExcluirCategoria form = new ExcluirCategoria(txtNomeCategoria.Text, LblId.Text);
                form.ShowDialog();
                await Consultar(); // Atualiza após fechar o dialog
            }
        }
        private async Task CadastrarCategoria()
        {
            var nome = (txtNomeCategoria.Text ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(nome))
            {
                MessageBox.Show("Informe um nome para a categoria.");
                txtNomeCategoria.Focus();
                return;
            }
            if (nome.Length < 2)
            {
                MessageBox.Show("Nome muito curto (mín. 2 caracteres).");
                txtNomeCategoria.Focus();
                return;
            }

            categoriaService = new CategoriaService();

            CategoriaDto dto = new CategoriaDto()
            {
                NomeCategoria = nome
            };
            txtNomeCategoria.Enabled = true;
            var response = await categoriaService.Adicionar(dto);

            if (response.success == true)
            {
                txtNomeCategoria.Text = "";
                LblId.Text = "";
                MessageBox.Show(string.Format($"Categoria {response.data.NomeCategoria} inserida com sucesso"));
                await Consultar();
            }
            else
            {
                var resp = response.notifications?.FirstOrDefault()?.ToString() ?? "Falha ao cadastrar";
                MessageBox.Show(resp);
            }
        }

        public async Task Consultar()
        {
            try
            {
                txtNomeCategoria.Enabled = true;
                txtNomeCategoria.Text = "";
                LblId.Text = "";
                
                categoriaService = new CategoriaService();
                
                // Preparar para carregar categorias
                
                var response = await categoriaService.ListarCategoria();
                
                if (response.success == true && response.data != null)
                {
                    // Atualizar a BindingList
                    categoriasList.Clear();
                    foreach (var cat in response.data)
                    {
                        categoriasList.Add(cat);
                    }
                    
                    // Configurar o grid
                    lstGridCategoria.DataSource = null;
                    lstGridCategoria.DataSource = categoriasList;
                    lstGridCategoria.AutoGenerateColumns = true;
                    
                    DefinirCabecalhos(new List<string>() { "NomeCategoria", "id" });
                    lstGridCategoria.Refresh();
                    Application.DoEvents();
                    
                    System.Diagnostics.Debug.WriteLine($"Grid atualizada com {response.data.Count} categorias");
                }
                else
                {
                    var resp = response.notifications?.FirstOrDefault()?.ToString() ?? "Falha ao consultar categorias";
                    MessageBox.Show(resp);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao consultar categorias: {ex.Message}");
            }
        }
        
        private async Task ConsultarPorNomeCategoria(string NomeCategoria)
        {
            txtNomeCategoria.Enabled = true;
            categoriaService = new CategoriaService();
            var response = await categoriaService.ListarCategoriaPorNomeCategoria(NomeCategoria);
            if (response.success == true)
            {
                var items = response.data?.ToList() ?? new List<CategoriaDto>();
                
                categoriasList.Clear();
                foreach (var cat in items)
                {
                    categoriasList.Add(cat);
                }
                
                lstGridCategoria.DataSource = null;
                lstGridCategoria.DataSource = categoriasList;
                DefinirCabecalhos(new List<string>() { "NomeCategoria", "id" });
                lstGridCategoria.Refresh();
            }
            else
            {
                txtNomeCategoria.Text = "";
                LblId.Text = "";
                var resp = response.notifications?.FirstOrDefault()?.ToString() ?? "Nenhuma categoria encontrada.";
                await Consultar();
                MessageBox.Show(resp);
            }
        }

        private void DefinirCabecalhos(List<String> ListaCabecalhos)
        {
            int _index = 0;

            foreach (DataGridViewColumn coluna in lstGridCategoria.Columns)
            {
                if (coluna.Visible)
                {
                    if (_index < ListaCabecalhos.Count)
                    {
                        coluna.HeaderText = ListaCabecalhos[_index];
                    }
                    _index++;
                }
            }
        }

        private void lstGridCategoria_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (lstGridCategoria?.CurrentRow == null || lstGridCategoria.CurrentRow.Index < 0) return;
            txtNomeCategoria.Enabled = false;
            txtNomeCategoria.Text = this.lstGridCategoria.CurrentRow?.Cells[0]?.Value?.ToString();
            LblId.Text = this.lstGridCategoria.CurrentRow?.Cells[1]?.Value?.ToString();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private async void btnAtualziar_Click(object sender, EventArgs e)
        {
            await Consultar();
        }
    }
}




