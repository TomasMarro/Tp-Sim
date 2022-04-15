using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace TP_SIM_1.Interfaces.Principal
{
    public partial class FrmGeneradorNrosRamdom : Form
    {

        public FrmGeneradorNrosRamdom()
        {
            InitializeComponent();
        }

        // Carga de los ComboBox cuando se inicia la ejecución de la aplicación //

        private void FrmGeneradorNrosRamdom_Load(object sender, EventArgs e)
        {
            CargarMetodos();
            CargarCantIntervalos();
            CargarTipoGenerador();
        }

        // Validación en los campos NroSemilla, Multiplicador, Módulo, Incremento e Iteraciones donde solo se ingresen números enteros //

        private void TxtNroSemilla_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 47) || (e.KeyChar >= 58 && e.KeyChar <= 255))
            {
                MessageBox.Show("Solo se permite ingresar números enteros positivos.", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void TxtMultiplicador_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 47) || (e.KeyChar >= 58 && e.KeyChar <= 255))
            {
                MessageBox.Show("Solo se permite ingresar números enteros positivos.", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void TxtModulo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 47) || (e.KeyChar >= 58 && e.KeyChar <= 255))
            {
                MessageBox.Show("Solo se permite ingresar números enteros positivos.", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void TxtIncremento_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 47) || (e.KeyChar >= 58 && e.KeyChar <= 255))
            {
                MessageBox.Show("Solo se permite ingresar números enteros positivos.", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void TxtIteraciones_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 47) || (e.KeyChar >= 58 && e.KeyChar <= 255))
            {
                MessageBox.Show("Solo se permite ingresar números enteros positivos.", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        // Comportamiento principal al clickear el boton generar números // 

        private void BtnGenerarNumeros_Click(object sender, EventArgs e)
        {
            Random myObject = new Random();
            var random = new Random();

            cc.Rows.Clear();
            dtg_hist.Rows.Clear();

            double c = 0;
            double Xo = 0;
            double a = 0;
            double M = 0;

            // Validaciones en los Textbox y ComboBox. //

            if (CBTipoGenerador.SelectedItem.ToString() == "Seleccionar")
            {
                MessageBox.Show("¡Debe seleccionar un tipo de generador para generar los numeros aleatorios!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                if (CBTipoGenerador.SelectedItem.ToString() == "Lineal congruencial")
                {
                    if (CBMetodos.SelectedItem.ToString() == "Seleccionar")
                    {
                        MessageBox.Show("¡Debe seleccionar un método para generar los numeros aleatorios!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    else
                    {
                        if (TxtIncremento.Enabled == false)
                        {
                            if (TxtNroSemilla.Text == "" || TxtMultiplicador.Text == "" || TxtModulo.Text == "" || TxtIteraciones.Text == "")
                            {
                                MessageBox.Show("¡Debe ingresar un valor numérico a todos los campos anteriores!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                            if (CBCantIntervalos.SelectedItem.ToString() == "Seleccionar")
                            {
                                MessageBox.Show("¡Debe seleccionar la cantidad de intérvalos!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                            Xo = Convert.ToDouble(TxtNroSemilla.Text);
                            a = Convert.ToDouble(TxtMultiplicador.Text);
                            M = Convert.ToDouble(TxtModulo.Text);
                            if ((Xo % 2) == 0)
                            {
                                MessageBox.Show("¡El número semilla obligatoriamente debe ser impar!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                        }
                        else
                        {
                            if (TxtNroSemilla.Text == "" || TxtMultiplicador.Text == "" || TxtIncremento.Text == "" || TxtModulo.Text == "" || TxtIteraciones.Text == "")
                            {
                                MessageBox.Show("¡Debe ingresar un valor numérico a todos los campos anteriores!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                            if (CBCantIntervalos.SelectedItem.ToString() == "Seleccionar")
                            {
                                MessageBox.Show("¡Debe seleccionar la cantidad de intérvalos!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                            c = Convert.ToDouble(TxtIncremento.Text);
                            Xo = Convert.ToDouble(TxtNroSemilla.Text);
                            a = Convert.ToDouble(TxtMultiplicador.Text);
                            M = Convert.ToDouble(TxtModulo.Text);
                        }
                    }
                }
                else
                {
                    if (TxtIteraciones.Text == "")
                    {
                        MessageBox.Show("¡Debe ingresar un valor numérico a la cantidad de iteraciones!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    if (CBCantIntervalos.SelectedItem.ToString() == "Seleccionar")
                    {
                        MessageBox.Show("¡Debe seleccionar la cantidad de intérvalos!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
            }

            var N = Convert.ToInt32(TxtIteraciones.Text);
            double iteraciones = Convert.ToDouble(N);
            var RAMDOM = new double[N];
            var X = new double[N];
            double X1 = Xo;
            var intervalos = Convert.ToInt32(CBCantIntervalos.SelectedItem);
            double increm = 0;

            var cont = new int[intervalos];

            // Carga de la primera grilla 

            for (var i = 0; i < N; i++)
            {
                if (CBTipoGenerador.SelectedItem.ToString() == "Del lenguaje")
                {
                    var rnd = Math.Round(random.NextDouble(), 4);
                    RAMDOM[i] = rnd;
                }
                else
                {
                    X1 = ((a * X1) + c) % M;
                    double rnd = Math.Round(X1 / M, 4);
                    RAMDOM[i] = rnd;
                    X[i] = X1;
                }
            }

            for (int i = 0; i < RAMDOM.Length; i++)
            {
                var fila = new string[3];
                fila[0] = (i + 1).ToString();
                fila[1] = (X[i]).ToString();
                fila[2] = (RAMDOM[i]).ToString();

                cc.Rows.Add(fila);

            }

            // A partir de aca carga segunda grilla

            for (int i = 0; i < intervalos; i++)
            {
                for (int f = 0; f < RAMDOM.Length; f++)
                {
                    double R = Convert.ToDouble(i);
                    var C = Math.Round(((R + 1) / intervalos), 4);
                    if (increm <= RAMDOM[f] && RAMDOM[f] < C)
                    {
                        cont[i] += 1;
                    }
                }
                increm += (1.0 / intervalos);

            }

            var V1 = new double[intervalos];
            var V2 = new double[intervalos];
            increm = 0;

            for (int i = 0; i < intervalos; i++)
            {
                if (i != 0)
                {
                    V1[i] = Math.Round(increm + 0.0001, 4);
                    V2[i] = Math.Round((V1[i] + 1.0 / intervalos), 4) - 0.0001;
                }
                else
                {
                    V1[i] = Math.Round(increm, 4);
                    V2[i] = Math.Round((V1[i] + 1.0 / intervalos), 4);
                }
                increm += (1.0 / intervalos);

            }
            V2[intervalos - 1] = V2[intervalos - 1] - 0.0001;

            double acumulador = 0;
            double acumulador2 = 0;

            for (int i = 0; i < cont.Length; i++)
            {
                var fila = new string[6];
                double frecAcum = (cont[i] / iteraciones);
                fila[0] = V1[i].ToString() + " - " + V2[i].ToString();
                fila[1] = Math.Round(((V2[i] + V1[i]) / 2), 4).ToString();
                fila[2] = (cont[i]).ToString();
                fila[3] = Math.Round(frecAcum, 4).ToString();
                acumulador = acumulador + cont[i];
                fila[4] = Math.Round(acumulador, 4).ToString();
                acumulador2 = acumulador2 + frecAcum;
                fila[5] = Math.Round(acumulador2, 4).ToString();

                dtg_hist.Rows.Add(fila);

            }

            // A partir de aca se genera el Grafico de histograma

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

            double sum = 0;
            double mediaA = 0;

            for (int i = 0; i < intervalos; i++)
            {
                var frecuencia = cont[i];
                double marca = Math.Round(((V2[i] + V1[i]) / 2), 4);
                double ma = frecuencia * marca;
                sum = sum + ma;

            }
            mediaA = sum / N;
            TxtMedia.Text = (Math.Round(mediaA, 4)).ToString();

            double var = 0;
            double mediaC = 0;


            for (int i = 0; i < intervalos; i++)
            {
                double frecuencia = cont[i];
                double marcaC = Math.Pow((Math.Round(((V2[i] + V1[i]) / 2), 4)), 2);
                mediaC = Math.Pow(mediaA, 2);
                double ma = (marcaC * frecuencia);
                var += ma;

            }
            var = var - (N * mediaC);
            double Varianza = var / (N - 1);

            TxtVarianza.Text = (Math.Round(Varianza, 4).ToString());
        }

        private static double Truncate(double value,int decimales)
        {
            double aux_value = Math.Pow(10, decimales);
            return (Math.Truncate(value * aux_value) / aux_value);
        }

        // Cambio de estado de los text box al interactuar con el combo box de seleccionar método //

        private void CBMetodos_SelectionChangeCommitted(object sender, EventArgs e)
        {
            TxtIncremento.Clear();
            if (CBMetodos.SelectedItem.ToString() == "Seleccionar")
            {
                DesabilitarCampos();
                PonerEn0();
            }
            else
            {
                DesabilitarIncremento();
                TxtNroSemilla.Enabled = true;
                TxtModulo.Enabled = true;
                TxtIteraciones.Enabled = true;
                TxtMultiplicador.Enabled = true;
                CBCantIntervalos.Enabled = true;
                if (CBMetodos.SelectedItem.ToString() == "Mixto")
                {
                    string sugerencia = "Para utilizar este método de una mejor manera se recomienda:" + Environment.NewLine +
                        "El módulo (m) debe ser base de 2^g (con g entero positivo)" + Environment.NewLine +
                        "El multiplicador (a) debe tener la forma de 1+4k (con k entero positivo" + Environment.NewLine +
                        "Y por último c debe ser relativamente primo a m";
                    SugerenciaMetodo(sugerencia);
                }
                else if (CBMetodos.SelectedItem.ToString() == "Multiplicativo")
                {
                    string sugerencia = "Para utilizar este método de una mejor manera se recomienda:" + Environment.NewLine +
                        "El módulo (m) debe ser base de 2^g (con g entero positivo)" + Environment.NewLine +
                        "El multiplicador (a) debe tener la forma de 3+8k o 5+8k (con k entero positivo)" + Environment.NewLine;
                    SugerenciaMetodo(sugerencia);
                }
            }
        }

        // Limpiar grillas tanto de generación de numeros random como histograma. 

        private void BtnLimpiarGrillas_Click(object sender, EventArgs e)
        {
            cc.Rows.Clear();
            dtg_hist.Rows.Clear();
            PonerEn0();
            TxtMedia.Clear();
            TxtVarianza.Clear();
            CBTipoGenerador.SelectedItem = "Seleccionar";
            ChartHistograma.Series["Frecuencias"].Points.Clear();
            ResultadoPruebaBondad.Clear();
            DgvChi.Rows.Clear();
            DgvKs.Rows.Clear();
            DgvChi.Visible = false;
            DgvKs.Visible = false;
        }

        // Carga de los Combo box (métodos, tipo de generador y cant de intervalos) // 

        private void CargarMetodos()
        {
            CBMetodos.Items.Clear();
            CBMetodos.Items.Add("Seleccionar");
            CBMetodos.SelectedItem = "Seleccionar";
            CBMetodos.Items.Add("Mixto");
            CBMetodos.Items.Add("Multiplicativo");
        }

        private void CargarCantIntervalos()
        {
            CBCantIntervalos.Items.Clear();
            CBCantIntervalos.Items.Add("Seleccionar");
            CBCantIntervalos.SelectedItem = "Seleccionar";
            CBCantIntervalos.Items.Add("5");
            CBCantIntervalos.Items.Add("8");
            CBCantIntervalos.Items.Add("10");
            CBCantIntervalos.Items.Add("12");
        }

        public void CargarTipoGenerador()
        {
            CBTipoGenerador.Items.Clear();
            CBTipoGenerador.Items.Add("Seleccionar");
            CBTipoGenerador.SelectedItem = "Seleccionar";
            CBTipoGenerador.Items.Add("Del lenguaje");
            CBTipoGenerador.Items.Add("Lineal congruencial");
        }

        // Desabilitar C en caso de seleccionar el método lineal congruencial multiplicativo //

        public void DesabilitarIncremento()
        {
            var selección = CBMetodos.SelectedItem.ToString();
            if (selección == "Multiplicativo")
            {
                TxtIncremento.Enabled = false;
            }
            else
            {
                TxtIncremento.Enabled = true;
            }
        }

        // Desabilitar todos los campos y ponerlos en 0 en caso de no seleccionar ningun método // 

        public void DesabilitarCampos()
        {
            TxtNroSemilla.Enabled = false;
            TxtModulo.Enabled = false;
            TxtIteraciones.Enabled = false;
            TxtMultiplicador.Enabled = false;
            CBCantIntervalos.Enabled = false;
            TxtIncremento.Enabled = false;
            CBMetodos.Enabled = false;
        }

        public void PonerEn0()
        {
            TxtNroSemilla.Clear();
            TxtModulo.Clear();
            TxtIteraciones.Clear();
            TxtMultiplicador.Clear();
            TxtIncremento.Clear();
            CBCantIntervalos.SelectedItem = "Seleccionar";
            CBMetodos.SelectedItem = "Seleccionar";
            if (CBMetodos.SelectedItem.ToString() == "Seleccionar")
            {
                DesabilitarCampos();
            }
        }

        // Sugerencia dependiendo del método seleccionado // 

        private void SugerenciaMetodo(string sugerencia)
        {
            MessageBox.Show(sugerencia, "Sugerencia", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        // Cambio de estado de los text box al interactuar con el combo box de seleccionar método //

        private void CBTipoGenerador_SelectionChangeCommitted(object sender, EventArgs e)
        {
            DesabilitarCampos();
            PonerEn0();
            cc.Rows.Clear();
            dtg_hist.Rows.Clear();
            TxtMedia.Clear();
            TxtVarianza.Clear();

            if (CBTipoGenerador.SelectedItem.ToString() != "Seleccionar")
            {
                CBCantIntervalos.Enabled = true;
                TxtIteraciones.Enabled = true;
                if (CBTipoGenerador.SelectedItem.ToString() == "Lineal congruencial")
                {
                    CBMetodos.Enabled = true;
                }
            }
        }

        // 

        private void BtnPruebaBondad_Click(object sender, EventArgs e)
        {
            List<Double> ListaRandom = new List<Double>();
            foreach (DataGridViewRow row in cc.Rows)
            {
                double numero = Convert.ToDouble((row.Cells["RND"].Value.ToString()));
                ListaRandom.Add(numero);
            }
            if (ListaRandom.Count == 0)
            {
                MessageBox.Show("¡Primero debe generar un conjunto de numeros aleatorios para poder realizar la prueba de bondad!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                int N = Convert.ToInt32(TxtIteraciones.Text);
                var k = Math.Round(Math.Sqrt(N), 0);
                var intervalos = Convert.ToInt32(k);
                var frecEsperada = N / k;
                double Pfe = frecEsperada / N;

                double increm = 0;
                var cont = new int[intervalos];
                for (int i = 0; i < intervalos; i++)
                {
                    for (int f = 0; f < ListaRandom.Count; f++)
                    {
                        double R = Convert.ToDouble(i);
                        var C = Math.Round(((R + 1) / intervalos), 4);
                        if (increm <= ListaRandom[f] && ListaRandom[f] < C)
                        {
                            cont[i] += 1;
                        }
                    }
                    increm += (1.0 / intervalos);

                }


                var V1 = new double[intervalos];
                var V2 = new double[intervalos];
                increm = 0;

                for (int i = 0; i < intervalos; i++)
                {
                    if (i != 0)
                    {
                        V1[i] = Math.Round(increm + 0.0001, 4);
                        V2[i] = Math.Round((V1[i] + 1.0 / intervalos), 4) - 0.0001;
                    }
                    else
                    {
                        V1[i] = Math.Round(increm, 4);
                        V2[i] = Math.Round((V1[i] + 1.0 / intervalos), 4);
                    }
                    increm += (1.0 / intervalos);

                }
                V2[intervalos - 1] = V2[intervalos - 1] - 0.0001;

                double acum = 0;

                if (N > 30)
                {
                    DgvChi.Rows.Clear();
                    DgvChi.Visible = true;


                    for (int i = 0; i < intervalos; i++)
                    {
                        var fila = new string[5];

                        fila[0] = V1[i].ToString() + " - " + V2[i].ToString();
                        fila[1] = (cont[i]).ToString();
                        fila[2] = Truncate(frecEsperada, 4).ToString();
                        var a = Math.Round(((Math.Pow((cont[i] - frecEsperada), 2)) / frecEsperada), 4);
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
                        DgvChi.Rows.Clear();
                        DgvChi.Visible = false;
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
                    acum = 0;
                    double acum2 = 0;
                    double maximo = 0;
                    double n = N;

                    for (int i = 0; i < intervalos; i++)
                    {
                        var fila = new string[9];

                        double Pfo = cont[i] / n;
                        acum += Pfo;
                        acum2 += Pfe;

                        fila[0] = V1[i].ToString() + " - " + V2[i].ToString();
                        fila[1] = (cont[i]).ToString();
                        fila[2] = Truncate(frecEsperada, 4).ToString();
                        fila[3] = Truncate(Pfo, 4).ToString();
                        fila[4] = Truncate(Pfe, 4).ToString();
                        fila[5] = Truncate(acum, 4).ToString();
                        fila[6] = Truncate(acum2,4).ToString();
                        double x = Math.Abs(acum - acum2);
                        fila[7] = Truncate(x, 4).ToString();
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
                        fila[8] = Truncate(maximo, 4).ToString();

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
    }
}
