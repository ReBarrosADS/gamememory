using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jogo_memoria_com_Formas
{
    public partial class Form1 : Form
    {


        List<int> numbers = new List<int> { 1,1,2,2,3,3,4,4,5,5,6,6};
        string firstChoice;
        string secondChoice;
        int tries;
        List<PictureBox> pictures = new List<PictureBox>();
        PictureBox picA;
        PictureBox picB;
        int totaTime = 30;
        int countDownTime;
        bool fimJogo = false;
        public Form1()
        {
            InitializeComponent();
            LoadPictures();

        }
        private void TimerEvent(object sender, EventArgs e)
        {
            countDownTime--;

            lblTempoRestante.Text = "Tempo Restante: " + countDownTime;

            if (countDownTime < 1)
            {
                FimJogo("Tempo acabou, Você perdeu");

                foreach (PictureBox x in pictures)
                {
                    if (x.Tag != null)
                    {
                        x.Image = Image.FromFile("pics/" + (string)x.Tag + ".png");
                    }
                }
            }
        }

        private void LoadPictures()
        {
            int leftPos = 20;
            int topPos = 20;
            int rows = 0;

            for (int i = 0; i < 12; i++)
            {
                PictureBox newPic = new PictureBox();
                newPic.Height = 50;
                newPic.Width = 50;
                newPic.BackColor = Color.LightGray;
                newPic.SizeMode = PictureBoxSizeMode.StretchImage;
                newPic.Click += NewPic_click;
                pictures.Add(newPic);

                if (rows < 3)
                {
                    rows++;
                    newPic.Left = leftPos;
                    newPic.Top = topPos;
                    this.Controls.Add(newPic);
                    leftPos = leftPos + 60;
                }
                if (rows == 3)
                {
                    leftPos = 20;
                    topPos += 60;
                    rows = 0;
                }


            }
            RestartGame();
        }

        private void NewPic_click(object sender, EventArgs e)
        {
            if (fimJogo)
            {
                //não registar o click 
                return;
            }

            if(firstChoice == null)
            {
                picA = sender as PictureBox;
                if (picA.Tag != null && picA.Image == null)
                {
                    picA.Image = Image.FromFile("pics/" + (string)picA.Tag + ".png");
                    firstChoice = (string)picA.Tag;

                }
            }
            else if(secondChoice == null)
            {
                picB = sender as PictureBox;

                if(picB.Tag != null && picB.Image == null)
                {
                    picB.Image = Image.FromFile("pics/" + (string)picB.Tag + ".png");
                    secondChoice = (string)picB.Tag;
                }
            }
            else
            {
                CheckPictures(picA, picB);
            }
        } 
               

        private void RestartGameEvent(object sender, EventArgs e)
        {
            RestartGame();
        }

        private void RestartGame()
        {   
            //randon na lista
            var randomList = numbers.OrderBy(x => Guid.NewGuid()).ToList();

            //alterando a lista aletoria para original
            numbers = randomList;

            for (int i = 0; i < pictures.Count; i++)
            {
                pictures[i].Image = null;
                pictures[i].Tag = numbers[i].ToString();
            }

            tries = 0;
            lblStatus.Text = "Tentativas: " + tries;
            lblTempoRestante.Text = "Tempo Restate: " + totaTime;
            fimJogo = false;
            TempoJogo.Start();
            countDownTime = totaTime;                    

        }

        private void CheckPictures(PictureBox A, PictureBox B)
        {
            if(firstChoice == secondChoice)
            {
                A.Tag = null;
                B.Tag = null;
            }
            else
            {
                tries++;
                lblStatus.Text = "Tentativas erradas " + tries + " vezes.";
            }

            firstChoice = null;
            secondChoice = null;

            foreach (PictureBox pics in pictures.ToList())
            {
                if (pics.Tag != null)
                {
                    pics.Image = null;
                }
            }

            // verificar se todos os itens foram resolvidos

            if (pictures.All(o => o.Tag == pictures[0].Tag))
            {
                FimJogo("Bom trabalho, Você ganhou!!");
            }

        }

        private void FimJogo(string msg)
        {
            TempoJogo.Stop();
            fimJogo = true;
            MessageBox.Show(msg + "Click em Restar para jogar novamente");
        }

      

       
    }
}
