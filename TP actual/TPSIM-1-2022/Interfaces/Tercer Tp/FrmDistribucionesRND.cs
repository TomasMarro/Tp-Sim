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
            CargarCantIntervalos();
            CargarTipoDistribuciones();

        }





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

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }






        public void BtnGenerarNros_Click(object sender, EventArgs e)
        {
            Random myObject = new Random();
            var random = new Random();
            DgvDatos.Rows.Clear();
            DgvDistribucion.Rows.Clear();
            DgvSegundaGrilla.Rows.Clear();
            DgvHistograma.Rows.Clear();
            int N = Convert.ToInt32(TxtTamaño.Text);
            var Muestra = new double[N];
            double acumulada = 0;
            double varianza = 0;
            double eu = Convert.ToDouble(TxtMedias.Text);



            for (int i = 0; i < N; i++)
            {

                Muestra[i] = Math.Round(random.NextDouble(), 4);
            }
            for (int i = 0; i < Muestra.Length; i++)
            {
                var fila = new string[2];
                fila[0] = (i + 1).ToString();
                fila[1] = Muestra[i].ToString();


                DgvDatos.Rows.Add(fila);

            }


            if (CbDistribución.SelectedItem.ToString() == "Uniforme")
            {
                var Muestra2 = new double[Muestra.Length];
                double A = Convert.ToDouble(TxtA.Text);
                double B = Convert.ToDouble(TxtB.Text);
                for (int i = 0; i < Muestra2.Length; i++)
                {
                    double RndD = A + Muestra[i] * (B - A);
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
                    double fe = Convert.ToDouble(Muestra2.Length / cantIntervalos);
                    fila[0] = Math.Round((acumulada), 4).ToString();
                    fila[1] = fe.ToString();
                    DgvSegundaGrilla.Rows.Add(fila);
                }
            }

            if (CbDistribución.SelectedItem.ToString() == "Exponencial")
            {
                List<Double> MuestraE = new List<Double>();
                foreach (DataGridViewRow row in DgvDatos.Rows)
                {
                    double numero = Convert.ToDouble((row.Cells["RND1"].Value.ToString()));
                    MuestraE.Add(numero);
                }
                
                double lambda = 1 / eu;
                var Muestra2 = new double[MuestraE.Count];

                for (int i = 0; i < Muestra.Length; i++)
                {
                    var ex = - (Math.Pow(lambda,-1));
                    double rnd =  ex * Math.Log(1 - MuestraE[i]) ;
                    Muestra2[i] = Math.Round(rnd,6);
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
                double  incrementoIntervalo = Math.Round((mayorDeTodos - menorDeTodos) / cantIntervalos, 4);
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
                    
                    acumulada = (1 - Math.Exp(-lambda* V2[i])) - (1-Math.Exp(-lambda*V1[i]));
                    double fe = Math.Round(acumulada * N,4);
                    fila[0] = Math.Round((acumulada), 4).ToString();
                    fila[1] = Math.Round(fe, 4).ToString();
                    DgvSegundaGrilla.Rows.Add(fila);
                }


            }
            if (CbDistribución.SelectedItem.ToString() == "Normal")
            {
                double R = Convert.ToDouble(TxtR.Text);
                List<Double> MuestraE = new List<Double>();
                foreach (DataGridViewRow row in DgvDatos.Rows)
                {
                    double numero = Convert.ToDouble((row.Cells["RND1"].Value.ToString()));
                    MuestraE.Add(numero);
                }

                var lambdas = Convert.ToDouble(TxtMedias.Text);
                double lambda = 1 / lambdas;
                


                // Aca transforma los NrosRND en RND con distribucion normal
                var Muestra2 = new double[MuestraE.Count];
                for (int i = 0; i < Muestra.Length; i = i + 2)
                {
                    double ex = MuestraE[i];
                    var ex2 = MuestraE[i + 1];
                    
                    double rnd1 = (Math.Sqrt(-2 * Math.Log(ex)) * Math.Cos(2 * (2 * Math.PI * ex2))) * R + lambda;
                    double rnd2 = (Math.Sqrt(-2 * Math.Log(ex)) * Math.Sin(2 * (2 * Math.PI * ex2))) * R + lambda;

                    Muestra2[i] = Math.Round(rnd1,4);
                    Muestra2[i + 1] = Math.Round(rnd2,4);
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
                //double ex1 = 0;
                //for (int i = 0; i < Muestra2.Length; i++)
                //{
                //    ex1 += Math.Pow((Muestra2[i] - lambda),2);
                //}
                //var asa = ex1 / (N - 1);
                 double DesvStd = R;

                for (int i = 0; i < cantIntervalos; i++)
                {
                    var fila = new string[2];
                    double marcaclase = (V1[i] + V2[i]) / 2;
                    double x = Math.Exp(-0.5 * Math.Pow((marcaclase - lambda) / DesvStd, 2));
                    double y= (DesvStd* Math.Sqrt(2 * Math.PI));
                    double acumuladas = x/y * (V2[i] - V1[i]);
                    double fe = Math.Round(acumuladas * N, 4);
                    fila[0] = Math.Round((acumuladas), 4).ToString();
                    fila[1] = Math.Round(fe, 4).ToString();
                    DgvSegundaGrilla.Rows.Add(fila);
                }

            }
            if (CbDistribución.SelectedItem.ToString() == "Poisson")
            {
                
                double P = 1;
                int X = -1;
                double A = Math.Exp(-eu);
                List<int> Muestra2 = new List<int>();

                do
                {

                    double U = Math.Round(random.NextDouble(), 4);
                    P = P * U;
                    X = X + 1;
                    Muestra2.Add(X);

                } while (P >= A);

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
                int cantIntervalos = mayorDeTodos - menorDeTodos;
                int incrementoIntervalo = 1;
                var cont = new int[cantIntervalos];

                for (int i = 0; i < cantIntervalos; i++)

                {
                    var C = increm + incrementoIntervalo;

                    if (i == cantIntervalos - 1)
                    {
                        C = increm + incrementoIntervalo ;
                    }
                    for (int f = 0; f < Muestra2.Count; f++)
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
                        V2[i] = mayorDeTodos;
                    }
                    else
                    {
                        V1[i] = menorDeTodos;
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

            }
        }

        private void BtnGenerarDistribucion_Click(object sender, EventArgs e)
        {

        }

        private void BtnPruebaBondad_Click(object sender, EventArgs e)
        {
           int N = Convert.ToInt32(TxtTamaño.Text);
           var k = Math.Round(Math.Sqrt(N), 0);
             double eu = Convert.ToDouble(TxtMedias.Text);
             double lambda = 1 / eu;
             double R = Convert.ToDouble(TxtR.Text);
             double DesvStd = R;
            
            List<Double> frecEsperadas = new List<Double>();
            List<Double> PfrecEsperadas = new List<Double>();

           


            

            List<Double> ListaRandom = new List<Double>();

            foreach (DataGridViewRow row in DgvDistribucion.Rows)
            {
                double numero = Convert.ToDouble((row.Cells["RND"].Value.ToString()));
                ListaRandom.Add(numero);
            }
            var menorDeTodos = ListaRandom.Min();
            var mayorDeTodos = ListaRandom.Max();
            double increm = menorDeTodos;
            var cantIntervalos = Convert.ToInt32(k);
            var incrementoIntervalo = Math.Round((mayorDeTodos - menorDeTodos) / cantIntervalos, 4);
            var cont = new int[cantIntervalos];

            for (int i = 0; i < cantIntervalos; i++)

            {
                var C = increm + incrementoIntervalo;

                if (i == cantIntervalos - 1)
                {
                    C = increm + incrementoIntervalo + 0.0004;
                }
                for (int f = 0; f < ListaRandom.Count; f++)
                {

                    if (increm <= ListaRandom[f] && ListaRandom[f] <= C)
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

            V2[cantIntervalos - 1] = V2[cantIntervalos - 1] - 0.0001;

            double acum = 0;

            if (CbDistribución.SelectedItem.ToString() == "Uniforme")
            {
                for (int i = 0; i < cantIntervalos; i++)
                {
                    var frecEsperada = N / k;
                    double acumuladas = frecEsperada / N;
                    frecEsperadas.Add(frecEsperada);
                    PfrecEsperadas.Add(acumuladas);
                }
                
            }
            if (CbDistribución.SelectedItem.ToString() == "Normal")
            {
                for (int i = 0; i < cantIntervalos; i++)
                {
                    
                    double marcaclase = (V1[i] + V2[i]) / 2;
                    double x = Math.Exp(-0.5 * Math.Pow((marcaclase - lambda) / DesvStd, 2));
                    double y = (DesvStd * Math.Sqrt(2 * Math.PI));
                    double acumuladas = x / y * (V2[i] - V1[i]);
                    double fe = Math.Round(acumuladas * N, 4);
                    frecEsperadas.Add(fe);
                    PfrecEsperadas.Add(acumuladas);
                }

            }
            if (CbDistribución.SelectedItem.ToString() == "Exponencial")
            {
                for (int i = 0; i < cantIntervalos; i++)
                {
                    double acumulada = (1 - Math.Exp(-lambda * V2[i])) - (1 - Math.Exp(-lambda * V1[i]));
                    double fe = Math.Round(acumulada * N, 4);
                    double acumuladas = Math.Round((acumulada), 4);
                    frecEsperadas.Add(fe);
                    PfrecEsperadas.Add(acumuladas);
                }

            }
            if (CbDistribución.SelectedItem.ToString() == "Poisson")
            {

            }

            if (N > 30)
            {
                DgvChi.Rows.Clear();
                DgvChi.Visible = true;

                List<Double> V1chi = new List<Double>();
                List<Double> V2chi = new List<Double>();
                List<Double> cont2 = new List<Double>();
                List<Double> frecEsperadas2 = new List<Double>();

                for (int i = 0; i < cantIntervalos; i++)
                {

                    if (frecEsperadas[i] < 5)
                    {
                        int pos = i;
                        double suma= 0;
                        int suma2 = 0;
                        var bandera = false;

                        for (int t = pos; t < cantIntervalos; t++)
                        {
                            double fre = frecEsperadas[t] ;
                            suma += fre;
                            suma2 += cont[t];
                            if (suma >= 5)
                            {
                                int posi = t;
                                double acumulador = 0;
                                if (bandera)
                                {
                                    break;
                                }
                                for (int g = posi + 1  ; g < cantIntervalos; g++)
                                {
                                    acumulador += frecEsperadas[g];
                                    if (acumulador >= 5)
                                    {
                                        V1chi.Add(V1[g]);
                                        V2chi.Add(V2[g]);
                                        double x = suma;
                                        int conta = suma2;
                                        cont2.Add(conta);
                                        frecEsperadas2.Add(x);
                                        i = g;
                                        bandera = true;
                                        break;
                                        
                                    }
                                    if (g == cantIntervalos -1)
                                    {
                                        frecEsperadas2[frecEsperadas2.Count - 1] += acumulador;
                                        V1chi.Add(V1[t]);
                                        V2chi.Add(V2[cantIntervalos - 1]);
                                        break;
                                    }
                                }
                                
                                


                            }
                            

                        }

                       

                        
                       
                    }
                    if (frecEsperadas[i] >= 5)
                    {
                        double acumulador = 0;
                        for (int g = i + 1; g < cantIntervalos; g++)
                        {
                            acumulador += frecEsperadas[g];
                            if (acumulador >= 5)
                            {
                                V1chi.Add(V1[i]);
                                V2chi.Add(V2[i]);
                                double x = frecEsperadas[i];
                                int conta = cont[i];
                                cont2.Add(conta);
                                frecEsperadas2.Add(x);
                                break;
                            }
                            if (g == cantIntervalos - 1)
                            {
                                frecEsperadas2[frecEsperadas2.Count - 1] += acumulador;
                                V1chi.Add(V1[i]);
                                V2chi.Add(V2[cantIntervalos - 1]);
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
                    MessageBox.Show("No Rechaza.", "Resultado Prueba Bondad", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    MessageBox.Show("Rechaza.", "Resultado Prueba Bondad", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

            }
            else
            {
                DgvKs.Rows.Clear();
                DgvKs.Visible = true;
                acum = 0;
                double acum2 = 0;
                double maximo = 0;
                double n = N;

                for (int i = 0; i < cantIntervalos; i++)
                {
                    var fila = new string[9];

                    double Pfo = cont[i] / n;
                    acum += Pfo;
                    double pfe = 
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
                    MessageBox.Show("No Rechaza.", "Resultado Prueba Bondad", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    MessageBox.Show("Rechaza.", "Resultado Prueba Bondad", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

            }
        }

        private void DgvKs_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DgvDistribucion_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DgvSegundaGrilla_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
