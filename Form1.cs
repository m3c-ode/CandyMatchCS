using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CandyMatchCS
{
    public partial class Form1 : Form
    {
        //Initializing our arrays. Could use for loops, which is said to be faster, or enumerable.repeat
        PictureBox[] candies = new PictureBox[12];
        //For improving the random attribution of the pictureboxes, Try to initialize list. From this list of numbers 1-12(0-11), select a random number from the numbers in this list, corresponding to the available indexes. When found, remove the number, so that at next iterations, we will have less numbers to choose from, and we avoid the program getting stuck on already found numbers. 
        List<int> listPick = Enumerable.Range(0, 12).ToList();
        PictureBox firstGuess;
        //string[] filled = Enumerable.Repeat("e", 12).ToArray();
        bool[] isFilled = Enumerable.Repeat(false, 12).ToArray();
        string[] iconNames = { "beans", "bear", "choco", "egg", "pop", "shmallow" };
        bool timerStart = false;
        Stopwatch stopWatch = new Stopwatch();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //On form load, load pictures to pictureboxes and put them in the candies picturebox array. Turn them to black. Then suffles.
            int i = 0;
            foreach (PictureBox icon in Controls.OfType<PictureBox>())
            {
                icon.BackColor = Color.Black;

                candies[i] = icon;
                candies[i].Tag = i;
                i++;

            }
            shuffleIcons(candies);
            foreach (var (icon, index) in candies.Select((Name, index) => (Name, index)))
            {
                Console.WriteLine($"index {index} name is {icon.Name}");
                ///icon.Click += new EventHandler(clickPicture);
                icon.Click += Icon_Click;
            }



        }

        private void Icon_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            var selectedPbox = sender as PictureBox;
            ///var pic = (PictureBox) sender;
            // To get index of the selected picture
            int index = Array.IndexOf(candies, sender);
            Trace.WriteLine(selectedPbox.Name + index);
            //We make sure we don't click on a already discovered pictuure
            if (timerStart == false)
            {
                timerStart = true;
                timer1.Start();
            }
            if (selectedPbox.Image != null)
            {
                MessageBox.Show("Please click on another picture");
                return;
            }
            if (firstGuess == null)
            {
                firstGuess = selectedPbox;
                Debug.WriteLine(firstGuess == selectedPbox);
                AssignImage(firstGuess);
                //int index = candies.IndexOf(candies, selectedPbox);
                return;
            }

            AssignImage(selectedPbox);
            if (firstGuess.Name == selectedPbox.Name)
            {

                MessageBox.Show("Got it!");
            }
            else
            {
                firstGuess.Image = null;
                selectedPbox.Image = null;
            }
            MatchCheck();
            firstGuess = null;

        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AssignImage(PictureBox icon)
        {
            if (icon.Name == "beans")
            {
                icon.Image = Properties.Resources.beans;
            }
            if (icon.Name == "bear")
            {
                icon.Image = Properties.Resources.bear;
            }
            if (icon.Name == "egg")
            {
                icon.Image = Properties.Resources.egg;
            }
            if (icon.Name == "pop")
            {
                icon.Image = Properties.Resources.pop;
            }
            if (icon.Name == "choco")
            {
                icon.Image = Properties.Resources.choco;
            }
            if (icon.Name == "shmallow")
            {
                icon.Image = Properties.Resources.shmallow;
            }
        }
        private void shuffleIcons(PictureBox[] icons)
        {
            Random random = new Random();
            int i = 0;
            while (i < candies.Length)
            {
                //every2 occurences, we add a new name. Everytime we check if we haven't already filled the index we are at. Not every efficient way as we may encounter the same fille dindex multiple times.
                int j = random.Next(0, listPick.Count);
                /*if (!isFilled[j])
                {*/
                //we use j as index and getting it's value from the list. By removing it at every run, we are sure to always get a new value. We should not have need for the if condtion statement anymore.
                icons[listPick[j]].Name = iconNames[i / 2];
                //isFilled[j] = true;
                Trace.WriteLine($"index {listPick[j]} name is {icons[listPick[j]].Name}");
                listPick.RemoveAt(j);
                i++;
                //}

            }

        }

        private void clickPicture(object sender, EventArgs e)
        {
            ///PictureBox selectedPbox = ctype(sender, PictureBox);

        }

        public void MatchCheck()
        {
            int counter = 0;
            foreach (var candy in candies)
            {
                if (candy.Image != null)
                {
                    counter++;
                }
            }
            if (counter == 12)
            {
                timer1.Stop();
                MessageBox.Show($"Game complete! It took you {label2.Text}");
                DialogResult dialogueResult = MessageBox.Show("Would you live to start over?", "Congratulations", MessageBoxButtons.YesNo);
                if (dialogueResult == DialogResult.Yes)
                {
                    Application.Restart();
                }
                else
                {
                    Close();
                }
            }
        }
        int _count = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            _count++;
            label2.Text = "00:" + _count.ToString();
        }
    }
}
