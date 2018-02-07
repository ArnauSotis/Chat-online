using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace Interfazv1
{
    public partial class Registro : Form
    {
        public delegate void pasar(string dato, string pas);
        public event pasar pasado;
        public Registro()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pasado(textBox1.Text,textBox2.Text);
            this.Hide();
        }

        private void Registro_Load(object sender, EventArgs e)
        {
        }
    }
}
