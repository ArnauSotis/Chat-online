using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Interfazv1
{
    public partial class PingPong : Form
    {
        public PingPong()
        {
            InitializeComponent();
        }


        int velocidad = 5;
        int cont = 0;
        int puntuaje = 0;

        bool Arriba, Izquierda;


        private void Juego_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox2.Top = e.Y;
        }

        private void PingPong_Load(object sender, EventArgs e)
        {
            this.Text = "Puntuaje: 0";
            Random aleatorio = new Random();
            pictureBox1.Location = new Point(0, aleatorio.Next(this.Height));
            Arriba = true;
            Izquierda = true;
            timer1.Enabled = true;
            puntuaje = 0;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (pictureBox1.Left > pictureBox2.Left) //perdemos
            {
                timer1.Enabled = false;
                MessageBox.Show("Puntuaje: " + puntuaje.ToString() + " puntos");
                puntuaje = 0;
                velocidad = 5;
                cont = 0;

            }

            if (pictureBox1.Left + pictureBox1.Width >= pictureBox2.Left && pictureBox1.Left + pictureBox1.Width <= pictureBox2.Left + pictureBox2.Width
                &&
                pictureBox1.Top + pictureBox1.Height >= pictureBox2.Top &&
                pictureBox1.Top + pictureBox1.Height <= pictureBox2.Top + pictureBox2.Height)
            {
                Izquierda = false;
                puntuaje += 1;
                this.Text = "Puntuacion: " + puntuaje.ToString() + " puntos";
                cont += 1;
                if (cont > 5)
                {
                    velocidad += 5;
                    cont = 0;
                }

            }



            #region Movimiento de la pelota
            if (Izquierda)
            { pictureBox1.Left += velocidad; }
            else
            { pictureBox1.Left -= velocidad; }
            if (Arriba)
            { pictureBox1.Top += velocidad; }
            else
            { pictureBox1.Top -= velocidad; }

            if (pictureBox1.Top >= this.Height - 50)//si pega en la pared de abajo
            {
                Arriba = false;

            }

            if (pictureBox1.Top <= 0)
            {
                Arriba = true;

            }
            if (pictureBox1.Left <= 0)
            {
                Izquierda = true;
            }
            if (pictureBox1.Left >= this.Width - 10)
            {
                Izquierda = false;
            }

            #endregion
        }
    }
}
