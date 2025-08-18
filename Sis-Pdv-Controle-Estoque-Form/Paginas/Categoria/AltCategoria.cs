using Sis_Pdv_Controle_Estoque_Form.Dto.Categoria;
using Sis_Pdv_Controle_Estoque_Form.Services.Categoria;
using System.Data;

namespace Sis_Pdv_Controle_Estoque_Form.Paginas.Categoria
{
    public partial class AltCategoria : Form
    {
        CategoriaService categoriaService;
        public AltCategoria(string _NomeCategoria, string Id)
        {
            InitializeComponent();

            txtNomeCategoria.Text = _NomeCategoria;
            LblId.Text = Id;
        }

        private void AltCategoria_Load(object sender, EventArgs e)
        {

        }

        private async void btnAlterar_Click(object sender, EventArgs e)
        {
            await Alterar();
        }

        private async Task Alterar()
        {
            try
            {
                categoriaService = new CategoriaService();

                CategoriaDto dto = new CategoriaDto()
                {
                    id = Guid.Parse(LblId.Text),
                    NomeCategoria = txtNomeCategoria.Text
                };
                CategoriaResponse response = await categoriaService.AlterarCategoria(dto);

                if (response.success == true)
                {
                    txtNomeCategoria.Text = "";
                    LblId.Text = "";
                    MessageBox.Show("Alterado com sucesso");
                    this.Close();
                }
                else
                {
                    var resp = response.notifications?.FirstOrDefault()?.ToString() ?? "Falha ao alterar";
                    MessageBox.Show(resp);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao alterar categoria: {ex.Message}");
            }
        }

        private void AltCategoria_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Remove o código que cancela o fechamento e força atualização
            // O formulário pai atualizará via ShowDialog
        }

        private void AltCategoria_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
    }
}
