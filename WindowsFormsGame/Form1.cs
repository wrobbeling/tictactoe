using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsGame
{
    public partial class Form1 : Form
    {
       private void Form1_Load(object sender, EventArgs e)
        {
        }

        private SoundPlayer soundPlayer; // SoundPlayer-Instanz für das Abspielen des Sounds
        public bool gegenComputer { get; set; } = false;
        public int spielerXGewinne { get; set; }
        public int spielerOGewinne { get; set; }
        public bool xTurn { get; set; } = true;  // Variable, um den Spieler zu verfolgen


        public Form1()
        {
            InitializeComponent();
            InitializeGrid();
            UpdateGewinnLabels();
            soundPlayer = new SoundPlayer();

            // Zuweisung der SoundPlayer-Instanz zur Soundressource 
            soundPlayer.Stream = Properties.Resources.fanfare;
        }

        private void UpdateGewinnLabels()
        {
            labelSpielerXGewinne.Text = "Spieler X: " + spielerXGewinne.ToString();
            labelSpielerOGewinne.Text = "Spieler O: " + spielerOGewinne.ToString();
        }

        private void radioButtonGegenComputer_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonGegenComputer.Checked)
            {
                gegenComputer = true;
            }
        }

        private void radioButtonGegenSpielerO_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonGegenSpielerO.Checked)
            {
                gegenComputer = false;
            }
        }

        private void ComputerMove()
        {
            // Überprüfe, ob noch freie Felder auf dem Spielfeld sind
            if (!AllButtonsDisabled())
            {
                // Suche nach einem freien Feld und setze dort das Zeichen des Computers
                foreach (Control control in Controls)
                {
                    if (control is Button)
                    {
                        Button button = (Button)control;
                        if (button.Enabled)
                        {
                            button.Text = "O"; // Setze das Zeichen des Computers (O)
                            button.Enabled = false;
                            xTurn = true; // Wechsle zum nächsten Spieler (Spieler X)
                            CheckWinner(); // Überprüfe, ob jemand gewonnen hat
                            break; // Beende die Schleife nach dem Setzen des Zuges
                        }
                    }
                }
            }
        }

        private void InitializeGrid()
        {
            // Initialisierung des Gitters
            foreach (Control control in Controls)
            {
                if (control is Button)
                {
                    Button button = (Button)control;
                    button.Enabled = true;
                    button.Text = "";
                    button.Click += new EventHandler(Button_Click);
                }
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            
                        Button button = (Button)sender;
            if (xTurn)
            {
                button.Text = "X"; // Setze das Zeichen des Spielers (X)
            }
            else
            {
                button.Text = "O"; // Setze das Zeichen des Spielers (O)
            }
            button.Enabled = false;
            xTurn = !xTurn; // Wechsle zum nächsten Spieler
            CheckWinner(); // Überprüfe, ob jemand gewonnen hat
            if (gegenComputer && !AllButtonsDisabled() && !xTurn)
            {
                ComputerMove(); // Lasse den Computer seinen Zug machen, wenn gegen Computer gespielt wird und Spieler O an der Reihe ist
            }

        }

        private void CheckWinner()
        {
            // Überprüfung auf Gewinner oder Unentschieden
            if (CheckForWin("X"))
            {
                PlayWinSound(); // abspielen des Sounds, wenn Spieler X gewinnt
                MessageBox.Show("X hat gewonnen!","INFO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                spielerXGewinne++;
                DisableButtons();
            }
            else if (CheckForWin("O"))
            {
                MessageBox.Show("O hat gewonnen!", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                spielerOGewinne++;
                DisableButtons();
            }
            else if (AllButtonsDisabled())
            {
                MessageBox.Show("Unentschieden!", "INFO", MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }

            UpdateGewinnLabels(); // Aktualisiert die Anzeige der Gewinne
        }

        private void PlayWinSound()
        {
            try
            {
                soundPlayer.Play(); // Spielen Sie den Sound ab
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Abspielen des Sounds: " + ex.Message, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool CheckForWin(string player)
        {
            // Überprüfung der Gewinnkombinationen
            if ((btn1.Text == player && btn2.Text == player && btn3.Text == player) ||
                (btn4.Text == player && btn5.Text == player && btn6.Text == player) ||
                (btn7.Text == player && btn8.Text == player && btn9.Text == player) ||
                (btn1.Text == player && btn4.Text == player && btn7.Text == player) ||
                (btn2.Text == player && btn5.Text == player && btn8.Text == player) ||
                (btn3.Text == player && btn6.Text == player && btn9.Text == player) ||
                (btn1.Text == player && btn5.Text == player && btn9.Text == player) ||
                (btn3.Text == player && btn5.Text == player && btn7.Text == player))
            {
                return true;
            }
            return false;
        }

        private bool AllButtonsDisabled()
        {
            // Überprüfung, ob alle Buttons deaktiviert sind (Unentschieden)
            foreach (Control control in Controls)
            {
                if (control is Button && control.Enabled)
                {
                    return false;
                }
            }
            return true;
        }

        private void DisableButtons()
        {
            // Deaktivierung aller Buttons nach Spielende
            foreach (Control control in Controls)
            {
                if (control is Button)
                {
                    control.Enabled = false;
                }
            }
        }    



        private void beendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void neuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Alle Buttons zurücksetzen
            foreach (Control control in Controls)
            {
                if (control is Button)
                {
                    Button button = (Button)control;
                    button.Enabled = true;
                    button.Text = "";
                }
            }

            // Das xTurn zurücksetzen, um das nächste Spiel mit "X" zu starten
            xTurn = true;
        }

     
        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Simon Wroblewski\nFachinformatiker Anwendungsentwicklung", "Über mich...",MessageBoxButtons.OK);
        }

        private void radioButtonGegenComputer_CheckedChanged_1(object sender, EventArgs e)
        {
            gegenComputer = radioButtonGegenComputer.Checked;
        }

        private void radioButtonGegenSpielerO_CheckedChanged_1(object sender, EventArgs e)
        {
            gegenComputer = !radioButtonGegenSpielerO.Checked;

        }

        private void hilfeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hier ist eine grundlegende Spielanleitung:\n" +
                "1. Das Spielfeld besteht aus einem 3x3-Raster.\n" +
                "2. Ein Spieler beginnt das Spiel, indem er ein X oder O in ein leeres Feld setzt.\n" +
                "3. Die Spieler wechseln sich ab, bis das Spielfeld voll ist oder einer der Spieler eine Reihe von drei gleichen Symbolen erreicht.\n" +
                "4. Ein Spieler gewinnt, wenn er zuerst eine horizontale, vertikale oder diagonale Reihe von drei gleichen Symbolen bildet.\n" +
                "5. Wenn das Spielfeld voll ist und keiner der Spieler eine Reihe von drei Symbolen hat, endet das Spiel unentschieden.\n" +
                "6. Die Spieler können versuchen, die Spielzüge ihres Gegners zu blockieren und gleichzeitig eigene Gewinnchancen zu erhöhen.\n" + 
                "7. Über Radiobuttons erfolgt die Auswahl des Gegners, entweder der Computer(KI) oder ein Mensch.", "Über TicTacToe", MessageBoxButtons.OK);
        }
    }


}

