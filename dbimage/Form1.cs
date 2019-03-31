using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace dbimage
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9CBGPDG\ASHIRAFZAL;Initial Catalog=PracticeDB;Integrated Security=True;Pooling=False");

        string imgLocation = "";
        SqlCommand cmd;


        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            //dialog.Filter = "png files(*.png)|*.png|jpg files(*.jpg)|*.jpg All files(*.*)|*.* ";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                imgLocation = dialog.FileName.ToString();
                pictureBox1.ImageLocation = imgLocation;
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            byte[] images = null;
            FileStream Stream = new FileStream(imgLocation, FileMode.Open,FileAccess.Read);
            BinaryReader brs = new BinaryReader(Stream);
            images = brs.ReadBytes((int)Stream.Length);

            connection.Open();
            int a = Convert.ToInt32(textID.Text);
            string sqlQuery = "insert into BioData(ID,Name,Image) VALUES ('"+ a + "' , '" + textName.Text + "' ,@images)";
            cmd = new SqlCommand(sqlQuery,connection);
            cmd.Parameters.Add(new SqlParameter("@images", images));
            int N = cmd.ExecuteNonQuery();
            connection.Close();
            MessageBox.Show(N.ToString()+"Data Saved Successfully");
            
        }

        private void View_Click(object sender, EventArgs e)
        {
         

            connection.Open();
            string sqlQuery = "select Name,Image from BioData where id = '" + textID.Text + "'";
            cmd = new SqlCommand(sqlQuery,connection);
            SqlDataReader DataRead = cmd.ExecuteReader();
            DataRead.Read();
            

            if (DataRead.HasRows)
            {
                textName.Text = DataRead[0].ToString();
                byte[] images = ((byte[])DataRead[1]);

                if (images == null)
                {
                    pictureBox1.Image = null;
                }
                else
                {
                    MemoryStream mstream = new MemoryStream(images);
                    pictureBox1.Image = Image.FromStream(mstream);

                }
                connection.Close();

            }
            else
            {
                MessageBox.Show("This Data is Not Available");
            }

            if (cmd.Connection.State == ConnectionState.Open)
            {
                cmd.Connection.Close();
            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
        }
    }
}
