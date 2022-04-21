using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TPSIM_1_2022.Interfaces;
using TPSIM_1_2022.Interfaces.Tercer_Tp;

namespace TPSIM_1_2022.Interfaces
{
    public partial class MenuPrincipal : Form
    {
        public MenuPrincipal()
        {
            InitializeComponent();
        }

        private void BtnTomaDatos_Click(object sender, EventArgs e)
        {
                var frmdistribucion = new FrmNrsRND();
               frmdistribucion.Show();
               this.Hide();
        }

        private void PanelPrincipal_Paint(object sender, PaintEventArgs e)
        {

        }

        private void BtnGeneradorDeNumeros_Click(object sender, EventArgs e)
        {
            var frmGeneradorDeNumeros = new FrmGeneradorNumerosAleatorios(this);
            frmGeneradorDeNumeros.Show();
            this.Hide();
        }
    }
}
