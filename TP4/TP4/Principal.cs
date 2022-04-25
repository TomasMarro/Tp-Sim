using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TP4
{
    public partial class Principal : Form
    {
        public Principal()
        {
            InitializeComponent();
        }

        // Carga del formulario y de los text boxs.

        private void Principal_Load(object sender, EventArgs e)
        {
            TxtQ.Text = "20";
            TxtR.Text = "12";
            TxtKm.Text = "600";
            TxtKo.Text = "570";
            TxtKs.Text = "4350";
        }

        // Validaciones de entrada en el txtMuestra y TxtDesde .

        private void TxtIteraciones_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 47) || (e.KeyChar >= 58 && e.KeyChar <= 255))
            {
                MessageBox.Show("¡Solo se permite ingresar números enteros!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void TxtDesde_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 47) || (e.KeyChar >= 58 && e.KeyChar <= 255))
            {
                MessageBox.Show("¡Solo se permite ingresar números enteros!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            TxtIteraciones.Clear();
            TxtDesde.Clear();
            TxtHasta.Clear();
            DgvTabla2Filas.Rows.Clear();
            DgvTabla400Filas.Rows.Clear();
        }

        private void BtnGenerar_Click(object sender, EventArgs e)
        {

        }
    }
}
