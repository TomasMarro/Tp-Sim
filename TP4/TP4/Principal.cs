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
            

            Random myObject = new Random();
            var random = new Random();

            int Q = 20;
            int R = 12;
            int Km = 600;
            int Ko = 570;
            int Ks = 4350;

            int N = Convert.ToInt32(TxtIteraciones.Text);


            var V1Ventas = new double[] { 0, 0.0833, 0.1944, 0.3611, 0.6944, 0.9444, 0.9722 };
            var V2Ventas = new double[] { 0.0832, 0.1943, 0.3610, 0.6943, 0.9443, 0.9721, 0.9999 };
            var NrosVentas = new int[]  { 6, 7, 8, 9, 10, 11, 12 };



            var V1Plazo = new double[] { 0, 0.4400, 0.7700, 0.9300 };
            var V2Plazo = new double[] { 0.4399, 0.7699, 0.9299, 0.9999 };
            var PlazoEntrega = new int[] { 1, 2, 3, 4 };


            var L1 = new double[] { 0, 0, 0, 0, 0, 0, 0, 25, 0, 0, 0, 0, 0 };
            var L2 = new double[13];

            bool BanderaStock = false;
            bool BanderaPedido = false;
            int stock = Convert.ToInt32(L1[7]);

            int Demora = 0;
            double RND2 = 0;
            int LlegadaPedidos = 0;
            bool banderaDemora = false;





            for (int i = 0; i < N; i++)
            {
                double RND = Math.Round(random.NextDouble(), 4);
                int Iteracion = i+1;
                int Demandas = Demanda(V1Ventas,V2Ventas,NrosVentas,RND);

                
                



                if (i == 0)
                {
                    string fil = "";
                    var filasas = new string[13];
                    for (int j = 0; j < L1.Length; j++)
                    {

                        if (L1[j] == 0)
                        {
                            fil = "-";
                        }
                        else
                        {
                            fil = L1[j].ToString();
                        }
                        
                        filasas[j] = fil;
                    }
                    DgvTabla400Filas.Rows.Add(filasas);

                }



                if (i == (N - 1) || i == (N - 2 ))
                {
                     var filass = new string[13];
                    filass[0] = Iteracion.ToString();
                    filass[1] = RND.ToString();
                    filass[2] = Demandas.ToString();

                    DgvTabla2Filas.Rows.Add(filass);
                }

                if (i == (LlegadaPedidos - 1) && i != 0)
                {
                    stock += Q;
                    BanderaPedido = false;
                    banderaDemora = false;
                }

                if (stock - Demandas <= 0)
                {
                    stock = 0;
                }
                else
                {
                    stock -= Demandas;
                }
                
                bool ReposiciondeStock = ControlStock(stock);





                var filas = new string[13];
                if (ReposiciondeStock && BanderaPedido == false && banderaDemora == false)
                {
                    RND2 = Math.Round(random.NextDouble(), 4);
                    Demora = PlazoDeEntrega(V1Plazo, V2Plazo, PlazoEntrega, RND2);
                    LlegadaPedidos = Iteracion + Demora;
                    banderaDemora = true;
                    BanderaPedido = true;
                    BanderaStock = true;


                    
                }
                if (BanderaPedido == true)
                {
                    filas[3] = RND2.ToString();
                    filas[4] = Demora.ToString();
                    BanderaPedido = false;
                }
                else
                {
                    filas[3] = "-";
                    filas[4] = "-";
                }
                filas[0] = Iteracion.ToString();
                filas[1] = RND.ToString();
                filas[2] = Demandas.ToString();
                filas[6] = LlegadaPedidos.ToString();
                filas[7] = stock.ToString();

                if (BanderaStock)
                  {
                         
                   filas[8] = Ko.ToString();
                   BanderaStock = false;

                }

                    DgvTabla400Filas.Rows.Add(filas);

                


            }




            int Demanda(double[] V1, double[] V2, int[] Ventas, double Ramdoms )
             {
                int NroVentas = 0;
                for (int i = 0; i < Ventas.Length; i++)
                {
                    if (V1[i] <= Ramdoms && Ramdoms <= V2[i])
                    {
                        NroVentas = Ventas[i];
                    }
                }
                return NroVentas;
             }

            int PlazoDeEntrega(double[] V1, double[] V2, int[] Plazos, double Ramdoms)
            {
                int Plazo = 0;
                for (int i = 0; i < Plazos.Length; i++)
                {
                    if (V1[i] <= Ramdoms && Ramdoms <= V2[i])
                    {
                        Plazo = Plazos[i];
                    }
                }
                return Plazo;
            }

            bool ControlStock(int stocks)
            {
                if (stocks <= 12)
                {
                    return true;
                }
                return false;
            }







        }

        private void TxtIteraciones_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
