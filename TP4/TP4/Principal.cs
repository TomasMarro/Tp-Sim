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

        private void Calculador()
        {
            Random myObject = new Random();
            var random = new Random();
            var random2 = new Random();

            int Q = 20;
            int R = 12;
            int Km = 600;
            int Ko = 570;
            int Ks = 4350;

            int N = Convert.ToInt32(TxtIteraciones.Text);


            var V1Ventas = new double[] { 0, 0.0833, 0.1944, 0.3611, 0.6944, 0.9444, 0.9722 };
            var V2Ventas = new double[] { 0.0832, 0.1943, 0.3610, 0.6943, 0.9443, 0.9721, 0.9999 };
            var NrosVentas = new int[] { 6, 7, 8, 9, 10, 11, 12 };



            var V1Plazo = new double[] { 0, 0.4400, 0.7700, 0.9300 };
            var V2Plazo = new double[] { 0.4399, 0.7699, 0.9299, 0.9999 };
            var PlazoEntrega = new int[] { 1, 2, 3, 4 };


            var L1 = new double[] { 0, 0, 0, 0, 0, 0, 0, 25, 0, 0, 0, 0, 0 };
            var L2 = new double[13];

            List<double[]> listaDeVectores = new List<double[]>();

            listaDeVectores.Add(L1);
            listaDeVectores.Add(L2);

            bool BanderaStock = false;
            bool BanderaPedido = false;   
            double stock = Convert.ToDouble(L1[7]);

            int Demora = 0;
            double RND2 = 0;
            int LlegadaPedidos = 0;
            bool banderaDemora = false;
            long costoAC = 0;
            double stockNegativo = 0;
            int desde = 0;
            int contador = 1;
            double CostosTotales = 0;
            double CostoTotalAcum = 0;



            if (TxtDesde.Text == "")
            {
                desde = 0;
            }
            else
            {
                desde = Convert.ToInt32(TxtDesde.Text);
            }
            TxtHasta.Text = (desde + 400).ToString();




            for (int i = 0; i < N; i++)
            {

                int Iteracion = i + 1;



                if (contador == 0)
                {
                    contador = 1;
                    Iteracion = Iteracion;
                    double RND = Math.Round(random.NextDouble(), 4);
                    int Demandas = Demanda(V1Ventas, V2Ventas, NrosVentas, RND);
                    stock = listaDeVectores[contador-1][7];
                    //CostosTotales = listaDeVectores[contador][11];
                    //CostoTotalAcum = listaDeVectores[contador][12];



                    if (i == (LlegadaPedidos - 1) && i != 0)
                    {
                        stock += Q;
                        BanderaPedido = false;
                        banderaDemora = false;
                    }

                    if (stock - Demandas <= 0)
                    {
                        stockNegativo = (stock - Demandas) * -1;
                        stock = 0;

                    }
                    else
                    {
                        //stock -= Demandas;
                        stock = stock - Demandas;

                    }

                    bool ReposiciondeStock = ControlStock(stock);

                    var filas = new string[13];



                    if (ReposiciondeStock && BanderaPedido == false && banderaDemora == false)
                    {
                        RND2 = Math.Round(random2.NextDouble(), 4);
                        Demora = PlazoDeEntrega(V1Plazo, V2Plazo, PlazoEntrega, RND2);
                        LlegadaPedidos = Iteracion + Demora;
                        banderaDemora = true;
                        BanderaPedido = true;
                        BanderaStock = true;

                    }
                    if (i == (N - 1) || i == (N - 2))
                    {
                        var filass = new string[13];

                        listaDeVectores[contador][0] = Iteracion;
                        listaDeVectores[contador][1] = RND;
                        listaDeVectores[contador][2] = Demandas;


                        if (BanderaPedido == true)
                        {

                            listaDeVectores[contador][3] = RND2;
                            listaDeVectores[contador][4] = Demora;
                            listaDeVectores[contador][5] = Q;
                            BanderaPedido = false;
                        }
                        listaDeVectores[contador][6] = LlegadaPedidos;
                        listaDeVectores[contador][7] = stock;

                        if (BanderaStock)
                        {
                            listaDeVectores[contador][8] = Ko;
                        }
                        else
                        {
                            listaDeVectores[contador][8] = 0;

                        }

                        if (stock == 0)
                        {

                            listaDeVectores[contador][9] = 0;
                            listaDeVectores[contador][10] = (Ks * stockNegativo);

                        }

                        else
                        {


                            listaDeVectores[contador][9] = (Km * stock);
                            listaDeVectores[contador][10] = 0;
                        }
                        listaDeVectores[contador][11] = Convert.ToInt32(listaDeVectores[contador][8]) + Convert.ToInt32(listaDeVectores[contador][9]) + Convert.ToInt32(listaDeVectores[contador][10]);
                        costoAC += Convert.ToInt32(listaDeVectores[0][11]);
                        listaDeVectores[contador][12] = costoAC;
                        filass[0] = listaDeVectores[contador][0].ToString();
                        filass[1] = listaDeVectores[contador][1].ToString();
                        filass[2] = listaDeVectores[contador][2].ToString();
                        filass[3] = listaDeVectores[contador][3].ToString();
                        filass[4] = listaDeVectores[contador][4].ToString();
                        filass[5] = listaDeVectores[contador][5].ToString();
                        filass[6] = listaDeVectores[contador][6].ToString();
                        filass[7] = listaDeVectores[contador][7].ToString();
                        filass[8] = listaDeVectores[contador][8].ToString();
                        filass[9] = listaDeVectores[contador][9].ToString();
                        filass[10] = listaDeVectores[contador][10].ToString();
                        filass[11] = listaDeVectores[contador][11].ToString();
                        filass[12] = listaDeVectores[contador][12].ToString();

                        DgvTabla2Filas.Rows.Add(filass);
                    }
                    if (BanderaPedido == true)
                    {
                        listaDeVectores[contador][3] = RND2;
                        listaDeVectores[contador][4] = Demora;
                        listaDeVectores[contador][5] = Q;

                        BanderaPedido = false;
                    }
                    else
                    {
                        filas[3] = "-";
                        filas[4] = "-";
                        filas[5] = "-";
                    }
                    listaDeVectores[contador][0] = Iteracion;
                    listaDeVectores[contador][1] = RND;
                    listaDeVectores[contador][2] = Demandas;
                    listaDeVectores[contador][6] = LlegadaPedidos;
                    listaDeVectores[contador][7] = stock;

                    if (stock == 0)
                    {
                        listaDeVectores[contador][10] = (Ks * stockNegativo);
                        listaDeVectores[contador][9] = 0;

                    }
                    else
                    {
                        listaDeVectores[contador][10] = 0;
                        listaDeVectores[contador][9] = (Km * stock);


                    }


                    if (BanderaStock)
                    {
                        listaDeVectores[contador][8] = Ko;

                        BanderaStock = false;
                    }
                    else
                    {
                        listaDeVectores[contador][8] = 0;

                    }



                    listaDeVectores[contador][11] = Convert.ToInt32(listaDeVectores[contador][8]) + Convert.ToInt32(listaDeVectores[contador][9]) + Convert.ToInt32(listaDeVectores[contador][10]);
                    if (i == (N - 1) || i == (N - 2))
                    {
                        listaDeVectores[contador][12] = costoAC;
                    }
                    else
                    {
                        costoAC += Convert.ToInt32(listaDeVectores[0][11]);
                        listaDeVectores[contador][12] = costoAC;
                    }
                    


                    if (Iteracion >= desde && Iteracion <= desde + 400)
                    {
                        filas[0] = listaDeVectores[contador][0].ToString();
                        filas[1] = listaDeVectores[contador][1].ToString();
                        filas[2] = listaDeVectores[contador][2].ToString();
                        filas[3] = listaDeVectores[contador][3].ToString();
                        filas[4] = listaDeVectores[contador][4].ToString();
                        filas[5] = listaDeVectores[contador][5].ToString();
                        filas[6] = listaDeVectores[contador][6].ToString();
                        filas[7] = listaDeVectores[contador][7].ToString();
                        filas[8] = listaDeVectores[contador][8].ToString();
                        filas[9] = listaDeVectores[contador][9].ToString();
                        filas[10] = listaDeVectores[contador][10].ToString();
                        filas[11] = listaDeVectores[contador][11].ToString();
                        filas[12] = listaDeVectores[contador][12].ToString();
                        if (desde == 0 && Iteracion == 1)
                        {
                            string fil = "";
                            var filasas = new string[13];
                            for (int j = 0; j < listaDeVectores[contador].Length; j++)
                            {

                                if (listaDeVectores[contador][j] == 0)
                                {
                                    fil = "-";
                                }
                                else
                                {
                                    fil = listaDeVectores[contador].ToString();
                                }

                                filasas[j] = fil;
                            }
                            DgvTabla400Filas.Rows.Add(filasas);
                            DgvTabla400Filas.Rows.Add(filas);

                        }
                        else
                        {
                            DgvTabla400Filas.Rows.Add(filas);
                        }

                    }
                    continue;

                }



                if (contador == 1)
                {
                    contador = 0;
                    Iteracion = Iteracion;
                    double RND = Math.Round(random.NextDouble(), 4);
                    int Demandas = Demanda(V1Ventas, V2Ventas, NrosVentas, RND);
                    if (Iteracion - 1 == 0)
                    {
                        stock = listaDeVectores[contador][7];
                    }
                    else
                    {
                        stock = listaDeVectores[contador + 1][7];
                    }
                    
                    //CostosTotales = listaDeVectores[contador][11];
                    //CostoTotalAcum = listaDeVectores[contador][12];



                    if (i == (LlegadaPedidos - 1) && i != 0)
                    {
                        stock += Q;
                        BanderaPedido = false;
                        banderaDemora = false;
                    }

                    if (stock - Demandas <= 0)
                    {
                        stockNegativo = (stock - Demandas) * -1;
                        stock = 0;

                    }
                    else
                    {
                        stock = stock - Demandas;
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
                    if (i == (N - 1) || i == (N - 2))
                    {
                        var filass = new string[13];

                        listaDeVectores[contador][0] = Iteracion;
                        listaDeVectores[contador][1] = RND;
                        listaDeVectores[contador][2] = Demandas;


                        if (BanderaPedido == true)
                        {

                            listaDeVectores[contador][3] = RND2;
                            listaDeVectores[contador][4] = Demora;
                            listaDeVectores[contador][5] = Q;
                            BanderaPedido = false;
                        }
                        listaDeVectores[contador][6] = LlegadaPedidos;
                        listaDeVectores[contador][7] = stock;

                        if (BanderaStock)
                        {
                            listaDeVectores[contador][8] = Ko;

                        }
                        else
                        {
                            listaDeVectores[contador][8] = 0;

                        }

                        if (stock == 0)
                        {

                            listaDeVectores[contador][9] = 0;
                            listaDeVectores[contador][10] = (Ks * stockNegativo);

                        }
                        else
                        {


                            listaDeVectores[contador][9] = (Km * stock);
                            listaDeVectores[contador][10] = 0;
                        }
                        listaDeVectores[contador][11] = Convert.ToInt32(listaDeVectores[contador][8]) + Convert.ToInt32(listaDeVectores[contador][9]) + Convert.ToInt32(listaDeVectores[contador][10]);
                        costoAC += Convert.ToInt32(listaDeVectores[0][11]);
                        listaDeVectores[contador][12] = costoAC;
                        filass[0] = listaDeVectores[contador][0].ToString();
                        filass[1] = listaDeVectores[contador][1].ToString();
                        filass[2] = listaDeVectores[contador][2].ToString();
                        filass[3] = listaDeVectores[contador][3].ToString();
                        filass[4] = listaDeVectores[contador][4].ToString();
                        filass[5] = listaDeVectores[contador][5].ToString();
                        filass[6] = listaDeVectores[contador][6].ToString();
                        filass[7] = listaDeVectores[contador][7].ToString();
                        filass[8] = listaDeVectores[contador][8].ToString();
                        filass[9] = listaDeVectores[contador][9].ToString();
                        filass[10] = listaDeVectores[contador][10].ToString();
                        filass[11] = listaDeVectores[contador][11].ToString();
                        filass[12] = listaDeVectores[contador][12].ToString();

                        DgvTabla2Filas.Rows.Add(filass);
                    }
                    if (BanderaPedido == true)
                    {
                        listaDeVectores[contador][3] = RND2;
                        listaDeVectores[contador][4] = Demora;
                        listaDeVectores[contador][5] = Q;

                        BanderaPedido = false;
                    }
                    else
                    {
                        filas[3] = "-";
                        filas[4] = "-";
                        filas[5] = "-";
                    }
                    listaDeVectores[contador][0] = Iteracion;
                    listaDeVectores[contador][1] = RND;
                    listaDeVectores[contador][2] = Demandas;
                    listaDeVectores[contador][6] = LlegadaPedidos;
                    listaDeVectores[contador][7] = stock;

                    if (stock == 0)
                    {
                        listaDeVectores[contador][10] = (Ks * stockNegativo);
                        listaDeVectores[contador][9] = 0;

                    }
                    else
                    {
                        listaDeVectores[contador][10] = 0;
                        listaDeVectores[contador][9] = (Km * stock);

                    }


                    if (BanderaStock)
                    {
                        listaDeVectores[contador][8] = Ko;

                        BanderaStock = false;
                    }
                    else
                    {
                        listaDeVectores[contador][8] = 0;

                    }

                    listaDeVectores[contador][11] = Convert.ToInt32(listaDeVectores[contador][8]) + Convert.ToInt32(listaDeVectores[contador][9]) + Convert.ToInt32(listaDeVectores[contador][10]);
                    if (i == (N - 1) || i == (N - 2))
                    {
                        listaDeVectores[contador][12] = costoAC;
                    }
                    else
                    {
                        costoAC += Convert.ToInt32(listaDeVectores[0][11]);
                        listaDeVectores[contador][12] = costoAC;
                    }


                    if (Iteracion >= desde && Iteracion <= desde + 400)
                    {
                        filas[0] = listaDeVectores[contador][0].ToString();
                        filas[1] = listaDeVectores[contador][1].ToString();
                        filas[2] = listaDeVectores[contador][2].ToString();
                        filas[3] = listaDeVectores[contador][3].ToString();
                        filas[4] = listaDeVectores[contador][4].ToString();
                        filas[5] = listaDeVectores[contador][5].ToString();
                        filas[6] = listaDeVectores[contador][6].ToString();
                        filas[7] = listaDeVectores[contador][7].ToString();
                        filas[8] = listaDeVectores[contador][8].ToString();
                        filas[9] = listaDeVectores[contador][9].ToString();
                        filas[10] = listaDeVectores[contador][10].ToString();
                        filas[11] = listaDeVectores[contador][11].ToString();
                        filas[12] = listaDeVectores[contador][12].ToString();
                        if (desde == 0 && Iteracion == 1)
                        {

                            string fil = "";
                            var filasas = new string[13];

                            for (int j = 0; j < listaDeVectores[contador].Length; j++)
                            {

                                if (listaDeVectores[contador][j] == 0)
                                {
                                    fil = "-";
                                }
                                else
                                {
                                    fil = listaDeVectores[contador][j].ToString();
                                }

                                filasas[j] = fil;
                            }
                            DgvTabla400Filas.Rows.Add(filasas);
                            DgvTabla400Filas.Rows.Add(filas);

                        }
                        else
                        {
                            DgvTabla400Filas.Rows.Add(filas);
                        }

                    }




                }









            }
            int Demanda(double[] V1, double[] V2, int[] Ventas, double Ramdoms)
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

            bool ControlStock(double stocks)
            {
                if (stocks <= 12)
                {
                    return true;
                }
                return false;
            }
        }
        private void BtnGenerar_Click(object sender, EventArgs e)
        {

            Calculador();


        }

        private void TxtIteraciones_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
