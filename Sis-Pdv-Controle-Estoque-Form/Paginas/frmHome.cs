namespace Sis_Pdv_Controle_Estoque_Form.Paginas
{
    public partial class frmHome : Form
    {
        public frmHome()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void frmHome_Load(object sender, EventArgs e)
        {
            timerData.Start();
        }
    }
}
