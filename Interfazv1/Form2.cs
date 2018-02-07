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
using System.Threading;

namespace Interfazv1
{
    public partial class Form2 : Form
    {
        int num;
        int num2;
        string mensaje;
        Socket server;
        public Form2(int num, Socket server, string canal)
        {
            InitializeComponent();
            this.num = num;
            this.server = server;
            label2.Text = "Canal " + canal;
        }
        private void Form2_Load(object sender, EventArgs e)
        {
        }
        public void Respuesta(int num, string respuesta)
        {
            this.num2 = num;
            this.mensaje = respuesta;

            if (num2 == num)
            {
                listBox1.Items.Add(mensaje);
            
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string mensaje = "200/" + textBox1.Text + "/"+num+"/0/0/0/0";
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            textBox1.Clear();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
