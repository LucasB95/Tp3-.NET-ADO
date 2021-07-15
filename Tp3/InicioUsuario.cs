using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Tp3
{  

    public partial class InicioUsuario : Form
    {

        AgenciaManager ag = new AgenciaManager();
        int propiedad;
        int precioC;
        int precioH;
        int Total;
        string TipoAlojamiento = "";
        Reserva reservaEliminar;
        public InicioUsuario()
        {
            InitializeComponent();
        }


        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

      
        //boton para cerrar sesion
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 login = new Form1();
            login.Show();
        }
    
        //Limpiar grilla de alojamiento
        private void button6_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        //boton para refrescar la lista de reservas
        private void button4_Click(object sender, EventArgs e)
        {
            int dni = int.Parse(label10.Text);
            //MessageBox.Show("dni " + dni);
            dataGridView2.Rows.Clear();
            foreach (List<string> aloj in ag.obtenerReservas(dni))
                dataGridView2.Rows.Add(aloj.ToArray());
        }

        //boton de busqueda
        private void button1_Click(object sender, EventArgs e)
        {
            //bton de busqueda
            dataGridView1.Rows.Clear();

            String Ciudad = "";
            DateTime Pdesde = DateTime.Now;
            DateTime Phasta = DateTime.Now;
            int cantPersonas = 0;
            TipoAlojamiento = "";
            int estrellas = 0;


            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(dateTimePicker1.Text) || string.IsNullOrEmpty(dateTimePicker2.Text) ||
                string.IsNullOrEmpty(checkedListBox1.Text) || string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("No se completaron los datos");
            }
            else if (textBox1.Text != null && textBox2.Text != null && dateTimePicker1.Text != null && dateTimePicker2.Text != null && checkedListBox1.Text != null && textBox3.Text != null)
            {


                Ciudad = textBox2.Text;
                Pdesde = DateTime.Parse(dateTimePicker1.Text);
                Phasta = DateTime.Parse(dateTimePicker2.Text);
                cantPersonas = int.Parse(textBox1.Text);
                TipoAlojamiento = checkedListBox1.Text;
                estrellas = int.Parse(textBox3.Text);

                dataGridView1.Rows.Clear();
                foreach (List<string> aloj in ag.buscarAlojamiento(Ciudad, Pdesde, Phasta, cantPersonas, TipoAlojamiento, estrellas))
                {
                    dataGridView1.Rows.Add(aloj.ToArray());
                }

                //ag.buscarAlojamiento(Ciudad, Pdesde, Phasta, cantPersonas, Tipo,estrellas);

                int dni = int.Parse(label10.Text);

                MessageBox.Show("Parametros a Buscar : "+"\nCiudad :" + Ciudad + "\nDesde :" + Pdesde + "\nHasta :" + Phasta + "\nCantPersonas :" + cantPersonas + "\nTipo :" + TipoAlojamiento + "\nEstrellas :" + estrellas + "\nDNI :" + dni);
                //refreshVista();

            }



        }

        //Boton de reserva
        private void button3_Click(object sender, EventArgs e)
        {
            DateTime Pdesde = DateTime.Parse(dateTimePicker1.Text);
            DateTime Phasta = DateTime.Parse(dateTimePicker2.Text);
            int cantPersonas = int.Parse(textBox1.Text);
            TimeSpan ts = dateTimePicker2.Value - dateTimePicker1.Value;
            int diferencia = (int)ts.TotalDays;
            precioC = precioC * diferencia;
            precioH = precioH * cantPersonas;
            Total = precioC + precioH;
            int dni = int.Parse(label10.Text);

            int id = ag.contReservas + 1;

            Reserva reserva = new Reserva(id, Pdesde, Phasta, Total, propiedad, dni);

            if (ag.reservar(reserva))
            {
                MessageBox.Show("Se ha generado una nueva reserva para el Usuario :" + dni);
            }
            else
            {
                MessageBox.Show("No se pudo generar la reserva contacte con un administrador");
            }
        }

        //data de alojamientos
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (TipoAlojamiento == "Hotel")
            {
                propiedad = int.Parse(dataGridView1[1, e.RowIndex].Value.ToString());

                precioH = int.Parse(dataGridView1[9, e.RowIndex].Value.ToString());

                MessageBox.Show("Codigo del Alojamiento Seleccionado : " + propiedad + "\nPrecio Hotel sin calculo de cantidad de personas : " + precioH);
            }
            else if (TipoAlojamiento == "Cabaña")
            {
                propiedad = int.Parse(dataGridView1[1, e.RowIndex].Value.ToString());
                precioC = int.Parse(dataGridView1[8, e.RowIndex].Value.ToString());


                MessageBox.Show("Codigo del Alojamiento Seleccionado : " + propiedad + "\nPrecio Cabaña sin calculo de cantidad de dias : " + precioC);
            }
        }

        //data de reservas
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int numero_reserva = int.Parse(dataGridView2[0, e.RowIndex].Value.ToString());
            DateTime pdesde = DateTime.Parse(dataGridView2[1, e.RowIndex].Value.ToString());
            DateTime pHasta = DateTime.Parse(dataGridView2[2, e.RowIndex].Value.ToString());
            int precio = int.Parse(dataGridView2[3, e.RowIndex].Value.ToString());
            int aloj = int.Parse(dataGridView2[4, e.RowIndex].Value.ToString());
            int dni = int.Parse(dataGridView2[5, e.RowIndex].Value.ToString());

            //int id = ag.contadorReservas + 1;
            MessageBox.Show("La Reserva a Eliminar se compone de :" + "\nNumero de Reserva : " + numero_reserva + "\nFecha Desde :" + pdesde + "\nFecha Hasta :" + pHasta + "\nPrecio :" + precio + "\nCodigo de Alojamiento :" + aloj + "\nDNI :" + dni + "\nSi es correcto pulse Eliminar Reserva");
            reservaEliminar = new Reserva(numero_reserva, pdesde, pHasta, precio, aloj, dni);
        }

        //boton de eliminar reserva
        private void button5_Click(object sender, EventArgs e)
        {


            if (ag.eliminarReserva(reservaEliminar))
            {
                MessageBox.Show("Eliminado con éxito");
               
            }
            else
                MessageBox.Show("No se pudo eliminar la reserva");
        }
    }
}
