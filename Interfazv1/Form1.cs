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
using System.Diagnostics;
using System.Threading;

namespace Interfazv1
{
    public partial class Form1 : Form
    {
        Socket server;
        Thread atender;
        string nombreregistro, pasregistro;
        List<Form2> formularios = new List<Form2>();

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }


        private void atender_mensajes_servidor()
        {
            /* este thread atiende los mensajes del servidor. Los tipos de mensajes:

    
             * 1:   Lista de conectados
             * 2:   Registro
             * 3:   Login
            
             */

            while (true)
            {
                int op;
                byte[] msg = new byte[80];
                // recibo mensaje del servidor
                server.Receive(msg);
                string mensaje = Encoding.ASCII.GetString(msg);
                string[] trozos = mensaje.Split('/');
                op = Convert.ToInt32(trozos[0]);
                // Averiguo el tipo de mensaje

                switch (op)
                {

                    case 2:
                        string respuesta2 = trozos[1].Split(',')[0];
                        if (respuesta2 == "Guardado")
                        { MessageBox.Show("Registro realizado"); }
                        else
                        {
                            MessageBox.Show("Error en el registro");
                        }
                        break;

                    case 3:
                        string respuesta3 = trozos[1].Split(',')[0];

                        if (respuesta3 == "ok")
                        {
                            MessageBox.Show("Contraseña correcta");
                            label1.Hide();
                            label2.Hide();
                            textBox1.Hide();
                            textBox2.Hide();
                            button1.Hide();
                            button2.Hide();
                            BackColor = Color.Aqua;
                            this.Size = new Size(1000, 1000);
                            listBox1.Show();
                            listBox2.Show();
                            textBox3.Show();
                            textBox4.Show();
                            button4.Show();
                            button3.Show();
                            label3.Show();
                        }
                        else
                            MessageBox.Show("No es correcto");

                        break;

                    case 4:  // Me indican el número de conectados hasta el momento

                        string respuesta4 = trozos[1].Split(',')[0];
                        listBox1.Items.Clear();
                        listBox1.Items.Add(respuesta4);
                        break;
                    case 100:

                        string respuesta100=trozos[1].Split(',')[0];
                        listBox2.Items.Add(respuesta100);
                        break;
                    case 200:
                        string respuesta200 = trozos[1].Split(',')[0];
                        int sala200 = Convert.ToInt32(trozos[1].Split(',')[1]);
                        formularios[sala200-1].Respuesta(sala200, respuesta200);
                        break;
                    
                    case 201:
                        MessageBox.Show("Usuario no conectado");
                        
                        break;



                }
            }

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (server != null)
            {
                string mensaje = "999/0/0/0/0/0/0";
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                atender.Abort();
                server.Send(msg);
                server.Shutdown(SocketShutdown.Both);
                server.Close();
            }
            else
                Close();
        }

        private void nuevoUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Registro Ventanaregistro = new Registro();
            Ventanaregistro.pasado += new Registro.pasar(ejecutar);
            Ventanaregistro.ShowDialog();

            string mensaje = "1/" + nombreregistro + "/" + pasregistro + "/0/0/0/" + nombreregistro;
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
        }
        public void ejecutar(string dato, string pas)
        {
            nombreregistro = dato;
            pasregistro = pas;
        }

        private void ayudaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ayuda Ventanaayuda = new Ayuda();
            Ventanaayuda.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((textBox1.Text == "") || (textBox2.Text == ""))
            {
                MessageBox.Show("Debe rellenar los huecos");
            }
            else
            {
                string mensaje = "2/" + textBox1.Text + "/" + textBox2.Text + "/0/0/0/" + textBox1.Text;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                /*
                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[1024];
                int msg2size = server.Receive(msg2);
                string mensaje2 = Encoding.ASCII.GetString(msg2, 0, msg2size) ;
                if (mensaje2 == "ok")
                {
                    MessageBox.Show("Contraseña correcta");
                    label1.Hide();
                    label2.Hide();
                    textBox1.Hide();
                    textBox2.Hide();
                    button1.Hide();
                    button2.Hide();
                    listBox1.Show();
                    textBox3.Show();
                    button3.Show();
                    BackColor = Color.Aqua;
                    this.Size = new Size(700, 500);


                }
                else
                    MessageBox.Show("No es correcto");*/
            }
                

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listBox1.Hide();
            listBox2.Hide();
            button3.Hide();
            label3.Hide();
            textBox3.Hide();
            textBox4.Hide();
            button4.Hide();
            //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
            //al que deseamos conectarnos
            IPAddress direc = IPAddress.Parse("192.168.56.102");
            IPEndPoint ipep = new IPEndPoint(direc, 9050);



            //Creamos el socket 
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);//Intentamos conectar el socket

            }
            catch (SocketException ex)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("No he podido conectar con el servidor");
                Close();
            }
            ThreadStart ts = delegate { atender_mensajes_servidor(); };
            atender = new Thread(ts);
            atender.Start();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string mensaje = "3/0/0/0/0/0";
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);



            //Recibimos la respuesta del servidor
            byte[] msg2 = new byte[1024];
            int msg2size = server.Receive(msg2);
            string mensaje2 = Encoding.ASCII.GetString(msg2, 0, msg2size);
            if (mensaje2 == "ok,")
            {
                MessageBox.Show("El usuario existe");
            }
            else
                MessageBox.Show("El usuario no existe");
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            string mensaje = "100/" + textBox3.Text + "/0/0/0/0/0";
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            textBox3.Clear();
           
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click_1(object sender, EventArgs e)
        {

        }
       


        private void atender_conversacion(int sala)
        {
            // Este thread simplemente pone en marcha el formulacio para atender al conversación cuyo identificador es id
            // Creo ese formulario y le paso el socket y el id de la conversación
            Form2 f = new Form2(sala, server, textBox4.Text);

            // añado el formulario a la tabla de hash
            formularios.Add(f);
            f.ShowDialog();

        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            //Ponemos en marcha un nuevo formulario de solicitud de servicios al servidor
            ThreadStart ts;
            Thread t;

            int sala= Convert.ToInt32(textBox4.Text);
            sala= formularios.Count+1;
            // Creamos un thread que simplemente pondrá en marcha el nuevo formulario.
            // Enviamos al thread el numero de orden que servirá de identificador de conversación en los mensajes que se 
            // intercambien con el servidor
            ts = delegate { atender_conversacion(sala); };
            t = new Thread(ts);
            t.Start();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            PingPong Ventanajuego2 = new PingPong();
            Ventanajuego2.Show();
        }




    }
}

/*


namespace Interfazv1
{
    public partial class Form1 : Form
    {
        Socket server;
        Thread atender;
        string nombreregistro, pasregistro;
        
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }



        private void atender_mensajes_servidor()
        {
            /* este thread atiende los mensajes del servidor. Los tipos de mensajes:

    
             * 1:   Lista de conectados
             * 2:   Registro
             * 3:   Login
            
             

            while (true)
            {
                int op;
                byte[] msg = new byte[80];
                // recibo mensaje del servidor
                server.Receive(msg);
                string mensaje = Encoding.ASCII.GetString(msg);
                string[] trozos = mensaje.Split('/');
                op = Convert.ToInt32(trozos[0]);
                // Averiguo el tipo de mensaje

                switch (op)
                {

                    case 2:
                        string respuesta2 = trozos[1].Split(',')[0];
                        if (respuesta2 == "Guardado")
                        { MessageBox.Show("Registro realizado"); }
                        else {
                            MessageBox.Show("Error en el registro");
                        }
                        break;

                    case 3:
                        string respuesta3 = trozos[1].Split(',')[0];
                       
                              if (respuesta3 == "ok")
                                    {
                                     MessageBox.Show("Contraseña correcta");
                                     label1.Hide();
                                     label2.Hide();
                                     textBox1.Hide();
                                     textBox2.Hide();
                                     button1.Hide();
                                     button2.Hide();
                                     button4.Hide();
                                     BackColor = Color.Aqua;
                                     this.Size = new Size(1000, 1000);
                                     listBox1.Show();
                                     inv1.Show();
                                     inv2.Show();
                                     inv3.Show();
                                     inv4.Show();
                                     inv5.Show();
                                     button3.Show();
                                    }
                              else
                                    MessageBox.Show("No es correcto");

                  break;

                 case 4:  // Me indican el número de conectados hasta el momento

                  string respuesta4 = trozos[1].Split(',')[0];
                                    listBox1.Items.Clear();
                                    listBox1.Items.Add(respuesta4);
                    break;
                }
            }

        }







        private void label1_Click(object sender, EventArgs e)
        {

        }
       
        private void nuevoUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Registro Ventanaregistro = new Registro();
            Ventanaregistro.pasado += new Registro.pasar(ejecutar);
            Ventanaregistro.ShowDialog();

            string mensaje = "1/" + nombreregistro + "/" + pasregistro + "/0/0/0/" + nombreregistro;
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

        }
        public void ejecutar(string dato, string pas)
        {
            nombreregistro = dato;
            pasregistro = pas;
        }

        private void ayudaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ayuda Ventanaayuda = new Ayuda();
            Ventanaayuda.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((textBox1.Text == "") || (textBox2.Text == ""))
            {
                MessageBox.Show("Debe rellenar los huecos");
            }
            else
            {
                string mensaje = "2/" + textBox1.Text + "/" + textBox2.Text + "/0/0/0/" + textBox1.Text;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);



            }
        }




        private void Form1_Load(object sender, EventArgs e)
        {
            label5.Hide();
            listBox1.Hide();
            inv1.Hide();
            inv2.Hide();
            inv3.Hide();
            inv4.Hide();
            inv5.Hide();
            button3.Hide();
            //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
            //al que deseamos conectarnos
            IPAddress direc = IPAddress.Parse("147.83.117.22");
            IPEndPoint ipep = new IPEndPoint(direc, 50022);


            //Creamos el socket 
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);//Intentamos conectar el socket

            }
            catch (SocketException ex)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("No he podido conectar con el servidor");
                Close();
            }
            ThreadStart ts = delegate { atender_mensajes_servidor(); };
            atender = new Thread(ts);
            atender.Start();

        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        

        private void label4_Click(object sender, EventArgs e)
        {
            Registro Ventanaregistro = new Registro();
            Ventanaregistro.pasado += new Registro.pasar(ejecutar);
            Ventanaregistro.ShowDialog();

            string mensaje = "1/" + nombreregistro + "/" + pasregistro + "/0/0/0/" + nombreregistro;
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);


        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string mensaje = "4/0/0/0/0/0/0";
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);


        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            string mensaje = "10/" + inv1 + "/" + inv2 + "/"+inv3+"/"+inv4+"/"+inv5+"/"+inv1+"";
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Registro Ventanaregistro = new Registro();
            Ventanaregistro.pasado += new Registro.pasar(ejecutar);
            Ventanaregistro.ShowDialog();

            string mensaje = "1/" + nombreregistro + "/" + pasregistro + "/0/0/0/" + nombreregistro;
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

        }
    }
}*/

