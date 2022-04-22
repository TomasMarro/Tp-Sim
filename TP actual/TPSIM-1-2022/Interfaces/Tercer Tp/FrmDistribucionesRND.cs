using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TPSIM_1_2022.Interfaces.Tercer_Tp
{
    public partial class FrmNrsRND : Form
    {
        public FrmNrsRND()
        {
            InitializeComponent();
        }

        // Creacion de una clase global para tratar la muestra inicial de los numeros random generados

        public static class GlobalData
        {
            public static double[] Muestra = new double[] { };
        }

        // Carga de los combo box de los intervalos y los tipo de distribuciones 

        private void CargarCantIntervalos()
        {
            CBCantIntervalos.Items.Clear();
            CBCantIntervalos.Items.Add("Seleccionar");
            CBCantIntervalos.SelectedItem = "Seleccionar";
            CBCantIntervalos.Items.Add("5");
            CBCantIntervalos.Items.Add("10");
            CBCantIntervalos.Items.Add("15");
            CBCantIntervalos.Items.Add("20");
        }

        private void CargarTipoDistribuciones()
        {
            CbDistribución.Items.Clear();
            CbDistribución.Items.Add("Seleccionar");
            CbDistribución.SelectedItem = "Seleccionar";
            CbDistribución.Items.Add("Uniforme");
            CbDistribución.Items.Add("Exponencial");
            CbDistribución.Items.Add("Normal");
            CbDistribución.Items.Add("Poisson");
        }

        // Al tocar el boton generar numeros, se carga la primera grilla de la izquierda con los numeros randoms y la cantidad ingresada por teclado junto con las validaciones en caso de ingresar letras o no ingresar nada y oprimir el botón 

        public void BtnGenerarNros_Click(object sender, EventArgs e)
        {
            DgvDatos.Rows.Clear();
            Random myObject = new Random();
            var random = new Random();
            LimpiarGrillas();
            BtnGenerarDistribucion.Enabled = true;
            CBCantIntervalos.Enabled = true;
            CbDistribución.Enabled = true;
            CBCantIntervalos.SelectedItem = "Seleccionar";
            CbDistribución.SelectedItem = "Seleccionar";
            PonerEnBlancoPanelDistribución();
            ChartHistograma.Series["Frecuencias"].Points.Clear();

            if (TxtTamaño.Text == "")
            {
                MessageBox.Show("¡Primero debe seleccionar un tamaño de muestra para poder generar los números!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                int N = Convert.ToInt32(TxtTamaño.Text);
                GlobalData.Muestra = new double[N];
                for (int i = 0; i < N; i++)
                {

                    GlobalData.Muestra[i] = Math.Round(random.NextDouble(), 4);
                }
                for (int i = 0; i < GlobalData.Muestra.Length; i++)
                {
                    var fila = new string[2];
                    fila[0] = (i + 1).ToString();
                    fila[1] = GlobalData.Muestra[i].ToString();


                    DgvDatos.Rows.Add(fila);

                }
            }
        }

        private void BtnGenerarDistribucion_Click(object sender, EventArgs e)
        {
            LimpiarGrillas();
            ResultadoPruebaBondad.Text = "";


            if (CbDistribución.SelectedItem.ToString() == "Seleccionar")
            {
                MessageBox.Show("¡Primero debe seleccionar  el tipo de distribución a utilizar!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else if (CbDistribución.SelectedItem.ToString() != "Poisson" && CBCantIntervalos.SelectedItem.ToString() == "Seleccionar")
            {


                MessageBox.Show("¡Primero debe seleccionar  cantidad de intervalos a utilizar!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            else
            {

                Random myObject = new Random();
                var random = new Random();
                double acumulada = 0;
                double varianza = 0;
                int N = Convert.ToInt32(TxtTamaño.Text);

                if (CbDistribución.SelectedItem.ToString() == "Uniforme")
                {
                    if (TxtA.Text == "" || TxtB.Text == "")
                    {
                        MessageBox.Show("¡Primero debe seleccionar un rango para la distribución uniforme ingresando valores para A y B!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    else
                    {
                        var Muestra2 = new double[GlobalData.Muestra.Length];
                        double A = Convert.ToDouble(TxtA.Text);
                        double B = Convert.ToDouble(TxtB.Text);
                        for (int i = 0; i < Muestra2.Length; i++)
                        {
                            double RndD = A + GlobalData.Muestra[i] * (B - A);
                            Muestra2[i] = RndD;

                        }

                        for (int i = 0; i < Muestra2.Length; i++)
                        {
                            var filas = new string[2];
                            filas[0] = (i + 1).ToString();
                            filas[1] = Muestra2[i].ToString();


                            DgvDistribucion.Rows.Add(filas);

                        }
                        var menorDeTodos = Muestra2.Min();
                        var mayorDeTodos = Muestra2.Max();
                        double increm = menorDeTodos;
                        var cantIntervalos = Convert.ToInt32(CBCantIntervalos.SelectedItem);
                        var incrementoIntervalo = Math.Round((mayorDeTodos - menorDeTodos) / cantIntervalos, 4);
                        var cont = new int[cantIntervalos];

                        for (int i = 0; i < cantIntervalos; i++)

                        {
                            var C = increm + incrementoIntervalo;

                            if (i == cantIntervalos - 1)
                            {
                                C = increm + incrementoIntervalo + 0.0004;
                            }
                            for (int f = 0; f < Muestra2.Length; f++)
                            {

                                if (increm <= Muestra2[f] && Muestra2[f] <= C)
                                {
                                    cont[i] += 1;
                                }
                            }
                            if (i == cantIntervalos - 1)
                            {
                                increm = mayorDeTodos;
                            }
                            increm += incrementoIntervalo;
                        }


                        var V1 = new double[cantIntervalos];
                        var V2 = new double[cantIntervalos];

                        for (int i = 0; i < cantIntervalos; i++)
                        {
                            if (i != 0 && i != cantIntervalos - 1)
                            {
                                V1[i] = Math.Round(V2[i - 1], 4);
                                V2[i] = Math.Round((V1[i] + incrementoIntervalo), 4);
                            }
                            else if (i == cantIntervalos - 1)
                            {
                                V1[i] = Math.Round(V2[i - 1], 4);
                                V2[i] = Math.Round(mayorDeTodos, 4);
                            }
                            else
                            {
                                V1[i] = Math.Round(menorDeTodos, 4);
                                V2[i] = Math.Round((V1[i] + incrementoIntervalo), 4);
                            }
                        }
                        double[] inter = V2;
                        double[] inter1 = V1;
                        int[] frec = cont;
                        ChartHistograma.Series["Frecuencias"].Points.Clear();
                        ChartHistograma.Titles.Clear();
                        ChartHistograma.Titles.Add("Histograma");


                        ChartHistograma.Series["Frecuencias"]["PointWidth"] = "0.5";

                        for (int i = 0; i < inter.Length; i++)
                        {
                            ChartHistograma.Series["Frecuencias"].Points.AddXY(inter1[i].ToString() + " - " + inter[i].ToString(), frec[i]);
                        }


                        for (int i = 0; i < cont.Length; i++)
                        {
                            var fila = new string[6];
                            fila[0] = V1[i].ToString();
                            fila[1] = V2[i].ToString();
                            fila[2] = Math.Round(((V2[i] + V1[i]) / 2), 4).ToString();
                            fila[3] = (cont[i]).ToString();

                            DgvHistograma.Rows.Add(fila);

                        }
                        for (int i = 0; i < cantIntervalos; i++)
                        {
                            var fila = new string[2];
                            var marcaclase = (V1[i] + V2[i]) / 2;
                            acumulada = (marcaclase - V1[i]) / (V2[i] - V1[i]);
                            double a = Convert.ToDouble(Muestra2.Length / cantIntervalos);
                            fila[0] = Math.Round((acumulada), 4).ToString();
                            fila[1] = a.ToString();
                            DgvSegundaGrilla.Rows.Add(fila);
                        }
                    }
                }

                if (CbDistribución.SelectedItem.ToString() == "Exponencial")
                {
                    if (TxtMedias.Text == "")
                    {
                        MessageBox.Show("¡Primero debe ingresar un valor de la media  ", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    else
                    {
                        double Media = Convert.ToDouble(TxtMedias.Text);
                        List<Double> MuestraE = new List<Double>();
                        foreach (DataGridViewRow row in DgvDatos.Rows)
                        {
                            double numero = Convert.ToDouble((row.Cells["RND1"].Value.ToString()));
                            MuestraE.Add(numero);
                        }

                        double lambda = 1 / Media;
                        var Muestra2 = new double[MuestraE.Count];

                        for (int i = 0; i < GlobalData.Muestra.Length; i++)
                        {
                            var ex = -(Math.Pow(lambda, -1));
                            double rnd = ex * Math.Log(1 - MuestraE[i]);
                            Muestra2[i] = Math.Round(rnd, 6);
                        }

                        for (int i = 0; i < Muestra2.Length; i++)
                        {
                            var fila = new string[2];
                            fila[0] = (i + 1).ToString();
                            fila[1] = Muestra2[i].ToString();

                            DgvDistribucion.Rows.Add(fila);
                        }


                        var menorDeTodos = Muestra2.Min();
                        var mayorDeTodos = Muestra2.Max();
                        var increm = menorDeTodos;
                        int cantIntervalos = Convert.ToInt32(CBCantIntervalos.SelectedItem);
                        double incrementoIntervalo = Math.Round((mayorDeTodos - menorDeTodos) / cantIntervalos, 4);
                        var cont = new int[cantIntervalos];

                        for (int i = 0; i < cantIntervalos; i++)

                        {
                            var C = increm + incrementoIntervalo;

                            if (i == cantIntervalos - 1)
                            {
                                C = increm + incrementoIntervalo + 0.0004;
                            }
                            for (int f = 0; f < Muestra2.Length; f++)
                            {

                                if (increm <= Muestra2[f] && Muestra2[f] <= C)
                                {
                                    cont[i] += 1;
                                }
                            }
                            if (i == cantIntervalos - 1)
                            {
                                increm = mayorDeTodos;
                            }
                            increm += incrementoIntervalo;
                        }


                        var V1 = new double[cantIntervalos];
                        var V2 = new double[cantIntervalos];

                        for (int i = 0; i < cantIntervalos; i++)
                        {
                            if (i != 0 && i != cantIntervalos - 1)
                            {
                                V1[i] = Math.Round(V2[i - 1], 4);
                                V2[i] = Math.Round((V1[i] + incrementoIntervalo), 4);
                            }
                            else if (i == cantIntervalos - 1)
                            {
                                V1[i] = Math.Round(V2[i - 1], 4);
                                V2[i] = Math.Round(mayorDeTodos, 4);
                            }
                            else
                            {
                                V1[i] = Math.Round(menorDeTodos, 4);
                                V2[i] = Math.Round((V1[i] + incrementoIntervalo), 4);
                            }
                        }

                        var inter = V2;
                        var inter1 = V1;
                        var frec = cont;
                        ChartHistograma.Series["Frecuencias"].Points.Clear();
                        ChartHistograma.Titles.Clear();
                        ChartHistograma.Titles.Add("Histograma");


                        ChartHistograma.Series["Frecuencias"]["PointWidth"] = "0.5";

                        for (int i = 0; i < inter.Length; i++)
                        {
                            ChartHistograma.Series["Frecuencias"].Points.AddXY(inter1[i].ToString() + " - " + inter[i].ToString(), frec[i]);
                        }

                        for (int i = 0; i < cont.Length; i++)
                        {
                            var fila = new string[6];
                            fila[0] = V1[i].ToString();
                            fila[1] = V2[i].ToString();
                            fila[2] = Math.Round(((V2[i] + V1[i]) / 2), 4).ToString();
                            fila[3] = (cont[i]).ToString();

                            DgvHistograma.Rows.Add(fila);

                        }
                        for (int i = 0; i < cantIntervalos; i++)
                        {
                            var fila = new string[2];

                            acumulada = (1 - Math.Exp(-lambda * V2[i])) - (1 - Math.Exp(-lambda * V1[i]));
                            double a = Math.Round(acumulada * N, 4);
                            fila[0] = Math.Round((acumulada), 4).ToString();
                            fila[1] = Math.Round(a, 4).ToString();
                            DgvSegundaGrilla.Rows.Add(fila);
                        }
                    }
                }

                if (CbDistribución.SelectedItem.ToString() == "Normal")
                {
                    if (TxtMedias.Text == "" || TxtR.Text == "")
                    {
                        MessageBox.Show("¡Primero debe ingresar un valor para la media y para la desviación estandar ", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    else
                    {
                        if ((N % 2) == 0)
                        {
                            double Media = Convert.ToDouble(TxtMedias.Text);
                            double R = Convert.ToDouble(TxtR.Text);
                            List<Double> MuestraE = new List<Double>();
                            foreach (DataGridViewRow row in DgvDatos.Rows)
                            {
                                double numero = Convert.ToDouble((row.Cells["RND1"].Value.ToString()));
                                MuestraE.Add(numero);
                            }
                            var lambda = Convert.ToDouble(TxtMedias.Text);



                            // Aca transforma los NrosRND en RND con distribucion normal
                            var Muestra2 = new double[MuestraE.Count];
                            for (int i = 0; i < GlobalData.Muestra.Length; i = i + 2)
                            {
                                double ex = MuestraE[i];
                                var ex2 = MuestraE[i + 1];

                                double rnd1 = (Math.Sqrt(-2 * Math.Log(ex)) * Math.Cos(2 * (2 * Math.PI * ex2))) * R + lambda;
                                double rnd2 = (Math.Sqrt(-2 * Math.Log(ex)) * Math.Sin(2 * (2 * Math.PI * ex2))) * R + lambda;

                                Muestra2[i] = rnd1;
                                Muestra2[i + 1] = rnd2;
                            }

                            for (int i = 0; i < Muestra2.Length; i++)
                            {
                                var fila = new string[2];
                                fila[0] = (i + 1).ToString();
                                fila[1] = Muestra2[i].ToString();

                                DgvDistribucion.Rows.Add(fila);
                            }
                            var menorDeTodos = Muestra2.Min();
                            var mayorDeTodos = Muestra2.Max();
                            var increm = menorDeTodos;
                            int cantIntervalos = Convert.ToInt32(CBCantIntervalos.SelectedItem);
                            double incrementoIntervalo = Math.Round((mayorDeTodos - menorDeTodos) / cantIntervalos, 4);
                            var cont = new int[cantIntervalos];

                            for (int i = 0; i < cantIntervalos; i++)

                            {
                                var C = increm + incrementoIntervalo;

                                if (i == cantIntervalos - 1)
                                {
                                    C = increm + incrementoIntervalo + 0.0004;
                                }
                                for (int f = 0; f < Muestra2.Length; f++)
                                {

                                    if (increm <= Muestra2[f] && Muestra2[f] <= C)
                                    {
                                        cont[i] += 1;
                                    }
                                }
                                if (i == cantIntervalos - 1)
                                {
                                    increm = mayorDeTodos;
                                }
                                increm += incrementoIntervalo;
                            }


                            var V1 = new double[cantIntervalos];
                            var V2 = new double[cantIntervalos];

                            for (int i = 0; i < cantIntervalos; i++)
                            {
                                if (i != 0 && i != cantIntervalos - 1)
                                {
                                    V1[i] = Math.Round(V2[i - 1], 4);
                                    V2[i] = Math.Round((V1[i] + incrementoIntervalo), 4);
                                }
                                else if (i == cantIntervalos - 1)
                                {
                                    V1[i] = Math.Round(V2[i - 1], 4);
                                    V2[i] = Math.Round(mayorDeTodos, 4);
                                }
                                else
                                {
                                    V1[i] = Math.Round(menorDeTodos, 4);
                                    V2[i] = Math.Round((V1[i] + incrementoIntervalo), 4);
                                }
                            }

                            var inter = V2;
                            var inter1 = V1;
                            var frec = cont;
                            ChartHistograma.Series["Frecuencias"].Points.Clear();
                            ChartHistograma.Titles.Clear();
                            ChartHistograma.Titles.Add("Histograma");


                            ChartHistograma.Series["Frecuencias"]["PointWidth"] = "0.5";

                            for (int i = 0; i < inter.Length; i++)
                            {
                                ChartHistograma.Series["Frecuencias"].Points.AddXY(inter1[i].ToString() + " - " + inter[i].ToString(), frec[i]);
                            }

                            for (int i = 0; i < cont.Length; i++)
                            {
                                var fila = new string[6];
                                fila[0] = V1[i].ToString();
                                fila[1] = V2[i].ToString();
                                fila[2] = Math.Round(((V2[i] + V1[i]) / 2), 4).ToString();
                                fila[3] = (cont[i]).ToString();

                                DgvHistograma.Rows.Add(fila);

                            }

                            double DesvStd = R;

                            for (int i = 0; i < cantIntervalos; i++)
                            {
                                var fila = new string[2];
                                double marcaclase = (V1[i] + V2[i]) / 2;
                                double x = Math.Exp(-0.5 * Math.Pow((marcaclase - lambda) / DesvStd, 2));
                                double y = (DesvStd * Math.Sqrt(2 * Math.PI));
                                double acumuladas = x / y * (V2[i] - V1[i]);
                                double a = Math.Round(acumuladas * N, 4);
                                fila[0] = Math.Round((acumuladas), 4).ToString();
                                fila[1] = Math.Round(a, 4).ToString();
                                DgvSegundaGrilla.Rows.Add(fila);
                            }

                        }
                        else
                        {
                            MessageBox.Show("¡Para poder usar esta distribución el tamaño de muestra debe ser par!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                            PonerEnBlancoTodo();
                        }
                    }
                }

                if (CbDistribución.SelectedItem.ToString() == "Poisson")
                {
                    if (TxtMedias.Text == "")
                    {
                        MessageBox.Show("¡Primero debe ingresar un valor para la media", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    else
                    {
                        double Media = Convert.ToDouble(TxtMedias.Text);
                        double P = 1;
                        int X = -1;
                        double A = Math.Exp(-Media);
                        List<int> Muestra2 = new List<int>();
                        var n = N;
                        double es = Math.E;
                        for (int i = 0; i < n; i++)
                        {
                            double p = 1;
                            int x = -1;
                            double a = Math.Pow(es, -Media);
                            do
                            {
                                //double u = generarRandom(random);
                                double u = Math.Round(random.NextDouble(), 4);
                                p *= u;
                                x += 1;
                            } while (p >= a);
                            Muestra2.Add(x);
                        }


                        for (int i = 0; i < Muestra2.Count; i++)
                        {
                            var fila = new string[2];
                            fila[0] = (i + 1).ToString();
                            fila[1] = Muestra2[i].ToString();

                            DgvDistribucion.Rows.Add(fila);
                        }
                        int menorDeTodos = Muestra2.Min();
                        int mayorDeTodos = Muestra2.Max();
                        int increm = menorDeTodos;
                        int cantIntervalos = (mayorDeTodos - menorDeTodos) + 1;
                        int incrementoIntervalo = 1;
                        var cont = new int[cantIntervalos];

                        for (int i = menorDeTodos; i < mayorDeTodos + 1; i++)
                        {
                            for (int f = 0; f < Muestra2.Count; f++)
                            {
                                if (Muestra2[f] == i)
                                {
                                    cont[i - menorDeTodos] += 1;

                                }



                            }


                        }


                        var V1 = new int[cantIntervalos];


                        for (int i = 0; i < cantIntervalos; i++)
                        {
                            V1[i] = menorDeTodos + i;
                        }


                        var inter1 = V1;
                        var frec = cont;
                        ChartHistograma.Series["Frecuencias"].Points.Clear();
                        ChartHistograma.Titles.Clear();
                        ChartHistograma.Titles.Add("Histograma");


                        ChartHistograma.Series["Frecuencias"]["PointWidth"] = "0.5";

                        for (int i = 0; i < inter1.Length; i++)
                        {
                            ChartHistograma.Series["Frecuencias"].Points.AddXY(inter1[i].ToString() + " - " + inter1[i].ToString(), frec[i]);
                        }

                        for (int i = 0; i < cont.Length; i++)
                        {
                            var fila = new string[6];
                            fila[0] = V1[i].ToString();

                            //fila[2] = Math.Round((V1[i] / 2), 4).ToString();
                            fila[1] = (cont[i]).ToString();

                            DgvPo.Rows.Add(fila);

                        }
                        for (int i = 0; i < cantIntervalos; i++)
                        {
                            var fila = new string[2];
                            double x = Math.Pow(Media, V1[i]);
                            double y = Math.Exp(-Media);
                            double factorial = V1[i];
                            for (int j = V1[i] - 1; j > 0; j--)
                            {
                                factorial = factorial * (j);
                            }

                            double acumuladas = (x * y) / factorial;
                            double a = Math.Round(acumuladas * N, 0);
                            fila[0] = Math.Round((acumuladas), 4).ToString();
                            fila[1] = a.ToString();
                            DgvSegundaGrilla.Rows.Add(fila);
                        }
                    }



                }
            }
        }


        private void BtnPruebaBondad_Click(object sender, EventArgs e)
        {
            int N = Convert.ToInt32(TxtTamaño.Text);
            var k = Math.Round(Math.Sqrt(N), 0);


            List<Double> frecEsperadas = new List<Double>();
            List<Double> PfrecEsperadas = new List<Double>();
            List<Double> ListaRandom = new List<Double>();
            List<Double> cont = new List<Double>();



            foreach (DataGridViewRow row in DgvDistribucion.Rows)
            {
                double numero = Convert.ToDouble((row.Cells["RND"].Value.ToString()));
                ListaRandom.Add(numero);
            }






            foreach (DataGridViewRow row in DgvSegundaGrilla.Rows)
            {
                double numero = Convert.ToDouble((row.Cells["Fe"].Value.ToString()));
                double numero2 = Convert.ToDouble((row.Cells["Pfe"].Value.ToString()));
                frecEsperadas.Add(numero);
                PfrecEsperadas.Add(numero2);
            }


            List<Double> V1 = new List<Double>();
            List<Double> V2 = new List<Double>();
            foreach (DataGridViewRow row in DgvHistograma.Rows)
            {
                double numero = Convert.ToDouble((row.Cells["Desde"].Value.ToString()));
                double numero2 = Convert.ToDouble((row.Cells["Hasta"].Value.ToString()));
                double numeros = Convert.ToDouble((row.Cells["Frec"].Value.ToString()));
                cont.Add(numeros);
                V1.Add(numero);
                V2.Add(numero2);
            }



            List<int> valor = new List<int>();
            foreach (DataGridViewRow row in DgvPo.Rows)
            {
                int numero = Convert.ToInt32((row.Cells["Valor"].Value.ToString()));
                int numero2 = Convert.ToInt32((row.Cells["Fo"].Value.ToString()));
                valor.Add(numero);
                cont.Add(numero2);
            }

            int mayorDeTodos = valor.Max();
            int menorDeTodos = valor.Min();
            int cantIntervalosPo = (mayorDeTodos - menorDeTodos) + 1;




            if (N > 30)
            {
                DgvChi.Rows.Clear();
                DgvChi.Visible = true;

                List<int> posInicial = new List<int>();
                List<int> pos2Ini = new List<int>();
                List<int> posFin = new List<int>();
                List<Double> V1chi = new List<Double>();
                List<Double> V2chi = new List<Double>();
                List<Double> cont2 = new List<Double>();
                List<Double> frecEsperadas2 = new List<Double>();
                double acumulador = 0;
                double acumulador2 = 0;
                double conta = 0;
                double contaa = 0;
                double acum = 0;
                if (CbDistribución.SelectedItem.ToString() == "Poisson")
                {

                    for (int i = 0; i < cantIntervalosPo; i++)
                    {
                        posInicial.Add(i);
                        if (frecEsperadas[i] < 5)
                        {

                            int pos = i;
                            acumulador += frecEsperadas[i];
                            conta += cont[i];
                            if (acumulador >= 5)
                            {
                                posFin.Add(i);
                                if (i != cantIntervalosPo - 1)
                                {
                                    for (int f = pos + 1; f < cantIntervalosPo; f++)
                                    {
                                        int pos2 = f - 1;
                                        pos2Ini.Add(pos2);
                                        if (f != cantIntervalosPo)
                                        {
                                            acumulador2 += frecEsperadas[f];
                                            contaa += cont[f];
                                            if (acumulador2 >= 5)
                                            {
                                                V1chi.Add(valor[posInicial[0]]);
                                                V2chi.Add(valor[pos2Ini[0]]);
                                                frecEsperadas2.Add(acumulador);
                                                cont2.Add(conta);
                                                acumulador = 0;
                                                acumulador2 = 0;
                                                conta = 0;
                                                contaa = 0;
                                                posInicial.Clear();
                                                pos2Ini.Clear();
                                                break;
                                            }
                                            else
                                            {
                                                if (f == cantIntervalosPo - 1)
                                                {
                                                    V1chi.Add(valor[posInicial[0]]);
                                                    V2chi.Add(valor[cantIntervalosPo - 1]);
                                                    frecEsperadas2.Add(acumulador + acumulador2);
                                                    cont2.Add(conta + contaa);
                                                    acumulador = 0;
                                                    acumulador2 = 0;
                                                    conta = 0;
                                                    contaa = 0;
                                                    posInicial.Clear();
                                                    pos2Ini.Clear();
                                                    posFin.Clear();
                                                    break;
                                                }
                                                else
                                                {
                                                    continue;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    V1chi.Add(valor[posInicial[0]]);
                                    V2chi.Add(valor[posInicial[0]]);
                                    frecEsperadas2.Add(acumulador);
                                    cont2.Add(conta);
                                    acumulador = 0;
                                    acumulador2 = 0;
                                    conta = 0;
                                    contaa = 0;
                                    posInicial.Clear();
                                    break;
                                }

                            }
                            else
                            {
                                continue;
                            }

                        }
                        else
                        {


                            int pos = i;
                            acumulador += frecEsperadas[i];
                            conta += cont[i];
                            if (i != cantIntervalosPo - 1)
                            {
                                for (int f = pos + 1; f < cantIntervalosPo; f++)
                                {
                                    int pos2 = f - 1;
                                    pos2Ini.Add(pos2);
                                    acumulador2 += frecEsperadas[f];
                                    contaa += cont[f];
                                    if (acumulador2 >= 5)
                                    {
                                        if (f == cantIntervalosPo - 1)
                                        {

                                            V1chi.Add(valor[i]);
                                            V2chi.Add(valor[i]);
                                            V1chi.Add(valor[posInicial[0]]);
                                            V2chi.Add(valor[pos2Ini[0]]);

                                            frecEsperadas2.Add(acumulador);
                                            cont2.Add(conta);
                                            acumulador = 0;
                                            acumulador2 = 0;
                                            conta = 0;
                                            contaa = 0;
                                            posInicial.Clear();
                                            pos2Ini.Clear();
                                            break;
                                        }
                                        else
                                        {
                                            continue;
                                        }

                                    }
                                    else
                                    {
                                        if (f == cantIntervalosPo - 1)
                                        {
                                            V1chi.Add(valor[posInicial[0]]);
                                            V2chi.Add(valor[cantIntervalosPo - 1]);
                                            frecEsperadas2.Add(acumulador + acumulador2);
                                            cont2.Add(conta + contaa);
                                            acumulador = 0;
                                            acumulador2 = 0;
                                            conta = 0;
                                            contaa = 0;
                                            posInicial.Clear();
                                            break;
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }

                                }
                            }
                            else
                            {
                                V1chi.Add(valor[posInicial[0]]);
                                V2chi.Add(valor[posInicial[0]]);
                                frecEsperadas2.Add(acumulador);
                                cont2.Add(conta);
                                acumulador = 0;
                                acumulador2 = 0;
                                conta = 0;
                                contaa = 0;
                                posInicial.Clear();
                                break;
                            }

                        }

                    }

                }
                else
                {
                    var cantIntervalos = Convert.ToInt32(CBCantIntervalos.SelectedItem);


                    for (int i = 0; i < cantIntervalos; i++)
                    {
                        posInicial.Add(i);
                        if (frecEsperadas[i] < 5)
                        {

                            int pos = i;
                            acumulador += frecEsperadas[i];
                            conta += cont[i];
                            if (acumulador >= 5)
                            {
                                posFin.Add(i);
                                if (i != cantIntervalos - 1)
                                {
                                    for (int f = pos + 1; f < cantIntervalos; f++)
                                    {
                                        int pos2 = f - 1;
                                        pos2Ini.Add(pos2);
                                        if (f != cantIntervalos)
                                        {
                                            acumulador2 += frecEsperadas[f];
                                            contaa += cont[f];
                                            if (acumulador2 >= 5)
                                            {
                                                V1chi.Add(V1[posInicial[0]]);
                                                V2chi.Add(V2[pos2Ini[0]]);
                                                frecEsperadas2.Add(acumulador);
                                                cont2.Add(conta);
                                                acumulador = 0;
                                                acumulador2 = 0;
                                                conta = 0;
                                                contaa = 0;
                                                posInicial.Clear();
                                                pos2Ini.Clear();
                                                break;
                                            }
                                            else
                                            {
                                                if (f == cantIntervalos - 1)
                                                {
                                                    V1chi.Add(V1[posInicial[0]]);
                                                    V2chi.Add(V2[cantIntervalos - 1]);
                                                    frecEsperadas2.Add(acumulador + acumulador2);
                                                    cont2.Add(conta + contaa);
                                                    acumulador = 0;
                                                    acumulador2 = 0;
                                                    conta = 0;
                                                    contaa = 0;
                                                    posInicial.Clear();
                                                    pos2Ini.Clear();
                                                    posFin.Clear();
                                                    break;
                                                }
                                                else
                                                {
                                                    continue;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    V1chi.Add(V1[posInicial[0]]);
                                    V2chi.Add(V2[posInicial[0]]);
                                    frecEsperadas2.Add(acumulador);
                                    cont2.Add(conta);
                                    acumulador = 0;
                                    acumulador2 = 0;
                                    conta = 0;
                                    contaa = 0;
                                    posInicial.Clear();
                                    break;
                                }

                            }
                            else
                            {
                                continue;
                            }

                        }
                        else
                        {
                            int pos = i;
                            acumulador += frecEsperadas[i];
                            conta += cont[i];
                            if (i != cantIntervalos - 1)
                            {
                                for (int f = pos + 1; f < cantIntervalos; f++)
                                {
                                    int pos2 = f - 1;
                                    pos2Ini.Add(pos2);
                                    acumulador2 += frecEsperadas[f];
                                    contaa += cont[f];
                                    if (acumulador2 >= 5)
                                    {
                                        if (f == cantIntervalos - 1)
                                        {

                                            V1chi.Add(V1[i]);
                                            V2chi.Add(V2[i]);
                                            V1chi.Add(V1[posInicial[0]]);
                                            V2chi.Add(V2[pos2Ini[0]]);

                                            frecEsperadas2.Add(acumulador);
                                            cont2.Add(conta);
                                            acumulador = 0;
                                            acumulador2 = 0;
                                            conta = 0;
                                            contaa = 0;
                                            posInicial.Clear();
                                            pos2Ini.Clear();
                                            break;
                                        }
                                        else
                                        {
                                            continue;
                                        }

                                    }
                                    else
                                    {
                                        if (f == cantIntervalos - 1)
                                        {
                                            V1chi.Add(V1[posInicial[0]]);
                                            V2chi.Add(V2[cantIntervalos - 1]);
                                            frecEsperadas2.Add(acumulador + acumulador2);
                                            cont2.Add(conta + contaa);
                                            acumulador = 0;
                                            acumulador2 = 0;
                                            conta = 0;
                                            contaa = 0;
                                            posInicial.Clear();
                                            break;
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }

                                }
                            }
                            else
                            {
                                V1chi.Add(V1[posInicial[0]]);
                                V2chi.Add(V2[posInicial[0]]);
                                frecEsperadas2.Add(acumulador);
                                cont2.Add(conta);
                                acumulador = 0;
                                acumulador2 = 0;
                                conta = 0;
                                contaa = 0;
                                posInicial.Clear();
                                break;
                            }

                        }

                    }
                }

                for (int i = 0; i < frecEsperadas2.Count; i++)
                {
                    var fila = new string[5];

                    fila[0] = V1chi[i].ToString() + " - " + V2chi[i].ToString();
                    fila[1] = (cont2[i]).ToString();
                    fila[2] = frecEsperadas2[i].ToString();
                    var a = Math.Round(((Math.Pow(frecEsperadas2[i] - (cont2[i]), 2)) / frecEsperadas2[i]), 4);
                    fila[3] = a.ToString();
                    acum += a;
                    fila[4] = acum.ToString();

                    DgvChi.Rows.Add(fila);
                }
                int v = Convert.ToInt32(k) - 1;
                int[] chi1 = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 40, 50, 60, 70, 80, 90, 100 };
                double[] chi2 = new double[] { 3.84, 5.99, 7.81, 9.49, 11.1, 12.6, 14.1, 15.5, 16.9, 18.3, 19.7, 21, 22.4, 23.7, 25, 26.3, 27.6, 28.9, 30.1, 31.4, 32.7, 33.9, 35.2, 36.4, 37.7, 38.9, 40.1, 41.3, 42.6, 43.8, 55.8, 67.5, 79.1, 90.5, 101.9, 113.1, 124.3 };
                int indicechi1 = Array.IndexOf(chi1, v);
                if (indicechi1 == -1)
                {
                    MessageBox.Show("No existe valor de X^2 tabulado para realizar prueba de bondad.", "Resulta Prueba Bondad", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (chi2[indicechi1] >= acum)
                {
                    ResultadoPruebaBondad.Text = "No se rechaza";
                }
                else
                {
                    ResultadoPruebaBondad.Text = "Se rechaza";
                }

            }
            else
            {
                DgvKs.Rows.Clear();
                DgvKs.Visible = true;
                double acum = 0;
                double acum2 = 0;
                double maximo = 0;
                double n = N;

                if (CbDistribución.SelectedItem.ToString() == "Poisson")
                {

                }
                else
                {
                    var cantIntervalos = Convert.ToInt32(CBCantIntervalos.SelectedItem);

                    for (int i = 0; i < cantIntervalos; i++)
                    {
                        var fila = new string[9];

                        double Pfo = cont[i] / n;
                        acum += Pfo;
                        double pfe = PfrecEsperadas[i] / n;
                        acum2 += PfrecEsperadas[i];

                        fila[0] = V1[i].ToString() + " - " + V2[i].ToString();
                        fila[1] = (cont[i]).ToString();
                        fila[2] = frecEsperadas[i].ToString();
                        fila[3] = Pfo.ToString();

                        fila[4] = PfrecEsperadas[i].ToString();
                        fila[5] = acum.ToString();
                        fila[6] = acum2.ToString();
                        double x = Math.Abs(acum - acum2);
                        fila[7] = x.ToString();
                        if (i == 0)
                        {
                            maximo = x;
                        }
                        else
                        {
                            if (maximo < x)
                            {
                                maximo = x;
                            }
                        }
                        fila[8] = maximo.ToString();

                        DgvKs.Rows.Add(fila);
                    }
                    int[] vector = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 };
                    double[] ks2 = new double[] { 0.97500, 0.84189, 0.70760, 0.62394, 0.56328, 0.51926, 0.48342, 0.45427, 0.43001, 0.40925, 0.39122, 0.37543, 0.36143, 0.34890, 0.33750, 0.32733, 0.31796, 0.30936, 0.30143, 0.29408, 0.28724, 0.28087, 0.27491, 0.26931, 0.26404, 0.25908, 0.25438, 0.24993, 0.24571, 0.24170 };

                    int indiceks1 = Array.IndexOf(vector, Convert.ToInt32(n));
                    if (ks2[indiceks1] >= maximo)
                    {
                        ResultadoPruebaBondad.Text = "No se rechaza";
                    }
                    else
                    {
                        ResultadoPruebaBondad.Text = "Se rechaza";
                    }
                }
            }
        }

        // Al abrir el formulario se llama a las funciones que cargan los combo box con sus datos 

        private void FrmNrsRND_Load(object sender, EventArgs e)
        {
            CargarCantIntervalos();
            CargarTipoDistribuciones();
        }

        // Al presionar una tecla que no sea un numero en los text box valida y tira error 

        private void TxtTamaño_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 47) || (e.KeyChar >= 58 && e.KeyChar <= 255))
            {
                MessageBox.Show("¡Solo se permite ingresar números enteros positivos!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void TxtA_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 43) || (e.KeyChar >= 58 && e.KeyChar <= 255) || (e.KeyChar == 45))
            {
                MessageBox.Show("¡Solo se permite ingresar números enteros positivos!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void TxtMedias_KeyPress(object sender, KeyPressEventArgs e)
        {



            if ((e.KeyChar >= 32 && e.KeyChar <= 43) || (e.KeyChar >= 58 && e.KeyChar <= 255) || (e.KeyChar == 45))
            {
                MessageBox.Show("¡Solo se permite ingresar números enteros positivos!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }

        }

        private void TxtR_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 43) || (e.KeyChar >= 58 && e.KeyChar <= 255) || (e.KeyChar == 45))
            {
                MessageBox.Show("¡Solo se permite ingresar números enteros positivos!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void TxtB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 43) || (e.KeyChar >= 58 && e.KeyChar <= 255) || (e.KeyChar == 45))
            {
                MessageBox.Show("¡Solo se permite ingresar números enteros positivos!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        // Al cambiar entre las diferentes distribuciones se habilitan los text box correspondientes a esa distribución 

        private void CbDistribución_SelectionChangeCommitted(object sender, EventArgs e)
        {
            PonerEnBlancoPanelDistribución();
            if (CbDistribución.SelectedItem.ToString() == "Uniforme")
            {
                TxtA.Enabled = true;
                TxtB.Enabled = true;
                TxtMedias.Enabled = false;
                TxtR.Enabled = false;
            }
            else if (CbDistribución.SelectedItem.ToString() == "Exponencial")
            {
                TxtA.Enabled = false;
                TxtB.Enabled = false;
                TxtMedias.Enabled = true;
                TxtR.Enabled = false;
            }
            else if (CbDistribución.SelectedItem.ToString() == "Normal")
            {
                TxtA.Enabled = false;
                TxtB.Enabled = false;
                TxtMedias.Enabled = true;
                TxtR.Enabled = true;
            }
            else if (CbDistribución.SelectedItem.ToString() == "Poisson")
            {
                CBCantIntervalos.Enabled = false;
                TxtA.Enabled = false;
                TxtB.Enabled = false;
                TxtMedias.Enabled = true;
                TxtR.Enabled = false;
            }
            else
            {
                TxtA.Enabled = false;
                TxtB.Enabled = false;
                TxtMedias.Enabled = false;
                TxtR.Enabled = false;
            }
        }

        private void PonerEnBlancoTodo()
        {
            DgvDatos.Rows.Clear();
            DgvDistribucion.Rows.Clear();
            DgvSegundaGrilla.Rows.Clear();
            DgvHistograma.Rows.Clear();
            BtnGenerarDistribucion.Enabled = false;
            TxtTamaño.Text = "";
            ResultadoPruebaBondad.Text = "";
            ChartHistograma.Series["Frecuencias"].Points.Clear();
            CBCantIntervalos.SelectedItem = "Seleccionar";
            CbDistribución.SelectedItem = "Seleccionar";
            CBCantIntervalos.Enabled = false;
            CbDistribución.Enabled = false;
            TxtA.Enabled = false;
            TxtB.Enabled = false;
            TxtMedias.Enabled = false;
            TxtR.Enabled = false;
            TxtA.Text = "";
            TxtB.Text = "";
            TxtMedias.Text = "";
            TxtR.Text = "";
            DgvChi.Rows.Clear();
            DgvKs.Rows.Clear();
        }

        private void LimpiarGrillas()
        {
            DgvDistribucion.Rows.Clear();

            DgvSegundaGrilla.Rows.Clear();
            DgvHistograma.Rows.Clear();
            DgvChi.Visible = false;
            DgvKs.Visible = false;
            DgvChi.Rows.Clear();
            DgvKs.Rows.Clear();
        }

        private void BtnLimpiarGrillas_Click(object sender, EventArgs e)
        {
            PonerEnBlancoTodo();
        }
        private void PonerEnBlancoPanelDistribución()
        {
            TxtA.Text = "";
            TxtB.Text = "";
            TxtMedias.Text = "";
            TxtR.Text = "";
        }
    }
}
