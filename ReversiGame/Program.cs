using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReversiGame
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            //Hier maak ik het scherm, de labels, combobox en graphics aan
            Form scherm = new Form();

            scherm.Text = "Reversi";
            scherm.BackColor = Color.White;

            scherm.ClientSize = new Size(600, 600);


            Label afbeelding = new Label();
            afbeelding.Location = new Point(150, 250);
            afbeelding.Size = new Size(301, 301);
            scherm.Controls.Add(afbeelding);

            Button nieuwSpelknop = new Button();

            nieuwSpelknop.Text = "Nieuw spel";
            nieuwSpelknop.AutoSize = true;
            nieuwSpelknop.Location = new Point(150, 200);
            nieuwSpelknop.BackColor = Color.LightGray;
            scherm.Controls.Add(nieuwSpelknop);

            Button hulpKnop = new Button();

            hulpKnop.Text = "Help";
            hulpKnop.AutoSize = true;
            hulpKnop.Location = new Point(375, 200);
            hulpKnop.BackColor = Color.LightGray;
            scherm.Controls.Add(hulpKnop);

            ComboBox keuzeBordafmeting = new ComboBox();
            keuzeBordafmeting.Location = new Point(100, 0);
            keuzeBordafmeting.Text = "6 x 6";
            keuzeBordafmeting.Items.Add("4 x 4");
            keuzeBordafmeting.Items.Add("6 x 6");
            keuzeBordafmeting.Items.Add("8 x 8");
            keuzeBordafmeting.Items.Add("10 x 10");
            scherm.Controls.Add(keuzeBordafmeting);

            Label bordafmetingText = new Label();
            bordafmetingText.Text = "Bordafmeting: ";

            bordafmetingText.Location = new Point(5, 2);
            bordafmetingText.AutoSize = true;
            scherm.Controls.Add(bordafmetingText);

            Label stand = new Label();
            stand.Text = "Blauw is aan zet";
            stand.AutoSize = true;

            stand.Location = new Point(300, 50);
            scherm.Controls.Add(stand);

            Label scoreBlauw = new Label();

            scoreBlauw.AutoSize = true;
            scoreBlauw.Location = new Point(270, 85);
            scoreBlauw.AutoSize = true;

            scoreBlauw.BorderStyle = BorderStyle.FixedSingle;
            scherm.Controls.Add(scoreBlauw);

            scoreBlauw.Text = "2 Blauwe stenen";

            Label scoreRood = new Label();

            scoreRood.AutoSize = true;
            scoreRood.Location = new Point(270, 155);
            scoreRood.AutoSize = true;

            scoreRood.BorderStyle = BorderStyle.FixedSingle;
            scoreRood.Text = "2 Rode stenen";
            scherm.Controls.Add(scoreRood);

            Bitmap plaatje = new Bitmap(301, 301);

            afbeelding.Image = plaatje;
            Graphics gr = Graphics.FromImage(plaatje);

            // hier kies ik de beginwaarde die de afmeting van het bord moet hebben
            int aantalrijen = 6;
            //teller houdt bij welke speler aan de beurt is
            int teller = 0;

            int aantalBlauwestenen = 2;
            int aantalRodestenen = 2;

            //dit is de array achter het bord met waarde 1 voor kleur blauw oftewel speler 1 en waarde 2 voor kleur rood oftewel speler 2
            int[,] bordIndeling = new int[aantalrijen, aantalrijen];
            // hier leg ik alle mogelijke insluitingen om mij een steen heen
            int[,] beweging =
            {
    {-1, -1}, {-1, 0},{-1, 1},{0, -1},{0, 1},{1, -1},{1, 0}, {1, 1}
};
            int blokje = 300 / aantalrijen;

            //deze cirkels horen bij de score
            void cirkelWeergaves(object o, PaintEventArgs e)
            {
                Graphics tekenaar = e.Graphics;
                tekenaar.FillEllipse(Brushes.Blue, 200, 70, 300 / 6, 300 / 6);
                tekenaar.FillEllipse(Brushes.Red, 200, 140, 300 / 6, 300 / 6);
            }
            tekenBord();
            //de eventhandlers
            scherm.Paint += cirkelWeergaves;
            afbeelding.MouseClick += kiesVakje;
            keuzeBordafmeting.SelectedIndexChanged += menuKeuze;
            nieuwSpelknop.Click += spelOpnieuw;

            hulpKnop.Click += hulpWeergave;

            void tekenBord()
            {
                gr.Clear(Color.White);

                for (int i = 0; i <= aantalrijen; i++)
                {
                    int locatieX = i * blokje;
                    int locatieY = i * blokje;
                    gr.DrawLine(Pens.Black, locatieX, 0, locatieX, 300);
                    gr.DrawLine(Pens.Black, 0, locatieY, 300, locatieY);
                }
                int helftVeld = aantalrijen / 2;

                int rij = helftVeld;
                int kolom = helftVeld;
                bordIndeling[rij, kolom] = 1;
                gr.FillEllipse(Brushes.Blue, rij * blokje, kolom * blokje, blokje, blokje);

                rij = helftVeld - 1;
                kolom = helftVeld;
                bordIndeling[rij, kolom] = 2;
                gr.FillEllipse(Brushes.Red, rij * blokje, kolom * blokje, blokje, blokje);

                rij = helftVeld;
                kolom = helftVeld - 1;
                bordIndeling[rij, kolom] = 2;
                gr.FillEllipse(Brushes.Red, rij * blokje, kolom * blokje, blokje, blokje);

                rij = helftVeld - 1;
                kolom = helftVeld - 1;
                bordIndeling[rij, kolom] = 1;
                gr.FillEllipse(Brushes.Blue, rij * blokje, kolom * blokje, blokje, blokje);

                afbeelding.Invalidate();
            }

            //keuze voor afmeting
            void menuKeuze(object o, EventArgs e)
            {
                if (keuzeBordafmeting.SelectedItem == "4 x 4") aantalrijen = 4;
                if (keuzeBordafmeting.SelectedItem == "6 x 6") aantalrijen = 6;
                if (keuzeBordafmeting.SelectedItem == "8 x 8") aantalrijen = 8;
                if (keuzeBordafmeting.SelectedItem == "10 x 10") aantalrijen = 10;

                blokje = 300 / aantalrijen;
                bordIndeling = new int[aantalrijen, aantalrijen];
                aantalRodestenen = 2;
                aantalBlauwestenen = 2;
                scoreRood.Text = "2 Rode stenen";
                scoreBlauw.Text = "2 Blauwe stenen";
                tekenBord();
            }


            //een eventhandler om mogelijke zetten te weeregeven na het beklikke van de hulp knop
            void hulpWeergave(object o, EventArgs e)
            {


                int speler;
                int tegenstander;

                if (teller % 2 == 0)
                {
                    speler = 1;
                    tegenstander = 2;
                }
                else
                {
                    speler = 2;
                    tegenstander = 1;
                }

                for (int rij = 0; rij < aantalrijen; rij++)
                {
                    for (int kolom = 0; kolom < aantalrijen; kolom++)
                    {
                        if (bordIndeling[rij, kolom] == 0)
                        {
                            for (int i = 0; i < 8; i++)
                            {
                                int checkRij = beweging[i, 0];
                                int checkKolom = beweging[i, 1];
                                int x = rij + checkRij;
                                int y = kolom + checkKolom;
                                bool tegenstanderGevonden = false;

                                while (x >= 0 && x < aantalrijen && y >= 0 && y < aantalrijen)
                                {
                                    if (bordIndeling[x, y] == tegenstander)
                                    {
                                        tegenstanderGevonden = true;
                                    }
                                    else if (bordIndeling[x, y] == speler && tegenstanderGevonden)
                                    {

                                        gr.DrawEllipse(Pens.Gray, rij * blokje, kolom * blokje, blokje, blokje);
                                        break;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                    x += checkRij;
                                    y += checkKolom;
                                }
                            }
                        }
                    }
                }

                afbeelding.Invalidate();
            }


            //kiesvakje maakt het mogelijk om een vak te klikken, de geklikte plek in de array een waarde te geven en die waarde te koppelen aan een kleur cirkel/steen en houdt zo ook de score bij.

            void kiesVakje(object o, MouseEventArgs e)
            {

                
                int rij = e.X / blokje;
                int kolom = e.Y / blokje;
                aantalBlauwestenen = 0;
                aantalRodestenen = 0;

                for (int i = 0; i < aantalrijen; i++)
                {
                    for (int j = 0; j < aantalrijen; j++)
                    {
                        if (bordIndeling[i, j] == 0)
                        {
                            gr.FillRectangle(Brushes.White, i * blokje, j * blokje, blokje, blokje);
                            gr.DrawLine(Pens.Black, i * blokje, 0, i * blokje, 300);
                            gr.DrawLine(Pens.Black, 0, j * blokje, 300, j * blokje);
                        }
                    }
                }


                if (bordIndeling[rij, kolom] == 0)
                {
                    int speler;
                    int tegenstander;

                    if (teller % 2 == 0)
                    {
                        speler = 1;
                        tegenstander = 2;
                    }
                    else
                    {
                        speler = 2;
                        tegenstander = 1;
                    }
                    bool steenGeplaatst = false;

                    for (int i = 0; i < 8; i++)
                    {
                        int checkRij = beweging[i, 0];
                        int checkKolom = beweging[i, 1];
                        int x = rij + checkRij;
                        int y = kolom + checkKolom;


                        bool tegenstanderGevonden = false;
                        while (x >= 0 && x < aantalrijen && y >= 0 && y < aantalrijen)
                        {
                            if (bordIndeling[x, y] == tegenstander)
                            {
                                tegenstanderGevonden = true;
                            }

                            else if (bordIndeling[x, y] == speler && tegenstanderGevonden)
                            {

                                while (x != rij || y != kolom)
                                {
                                    x -= checkRij;
                                    y -= checkKolom;
                                    bordIndeling[x, y] = speler;


                                    if (speler == 1)
                                    {
                                        gr.FillEllipse(Brushes.Blue, x * blokje, y * blokje, blokje, blokje);


                                    }
                                    if (speler == 2)
                                    {
                                        gr.FillEllipse(Brushes.Red, x * blokje, y * blokje, blokje, blokje);

                                    }

                                }
                                steenGeplaatst = true;
                                break;
                            }
                            else
                            {
                                break;
                            }
                            x += checkRij;
                            y += checkKolom;
                        }
                    }


                    if (steenGeplaatst)
                    {


                        if (speler == 1)
                        {
                            gr.FillEllipse(Brushes.Blue, rij * blokje, kolom * blokje, blokje, blokje);
                        }
                        else if (speler == 2)
                        {
                            gr.FillEllipse(Brushes.Red, rij * blokje, kolom * blokje, blokje, blokje);
                        }
                        teller++;



                        for (int i = 0; i < aantalrijen; i++)
                        {
                            for (int j = 0; j < aantalrijen; j++)
                            {
                                if (bordIndeling[i, j] == 1)
                                    aantalBlauwestenen++;
                                else if (bordIndeling[i, j] == 2)
                                    aantalRodestenen++;
                            }
                        }

                        if (aantalRodestenen + aantalBlauwestenen == aantalrijen * aantalrijen)
                        {
                            if (aantalBlauwestenen > aantalRodestenen)
                            {
                                stand.Text = "Blauw heeft gewonnen!";

                            }
                            else if (aantalBlauwestenen < aantalRodestenen)
                            {
                                stand.Text = "Rood heeft gewonnen!";

                            }
                            else
                            {

                                stand.Text = "Remise!";
                            }
                        }

                        else
                        {

                            if (speler == 1)
                            {

                                stand.Text = "Rood is aan zet";
                            }
                            else
                            {

                                stand.Text = "Blauw is aan zet";
                            }
                        }
                        scoreBlauw.Text = $"{(aantalBlauwestenen)} Blauwe stenen";
                        scoreRood.Text = $"{(aantalRodestenen)} Rode stenen";

                    }

                    afbeelding.Invalidate();

                }
            }


            // deze eventhandler zorgt ervoor dat er bij het klikken op de knop nieuw spel, de array wordt geleegd en het bord opneiwu wordt getekend
            void spelOpnieuw(object o, EventArgs e)
            {
                bordIndeling = new int[aantalrijen, aantalrijen];
                aantalRodestenen = 2;
                aantalBlauwestenen = 2;
                teller = 0;

                scoreRood.Text = "2 Rode stenen";
                scoreBlauw.Text = "2 Blauwe stenen";
                stand.Text = "Blauw is aan zet";



                tekenBord();
            }

            Application.Run(scherm);
        }
    }
}
