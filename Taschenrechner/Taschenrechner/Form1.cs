﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Taschenrechner
{
    public partial class Form1 : Form
    {
        private int _zwischenergebnis = 0;
        private string _vorherigeOp = "=";



        public Form1()
        {
            InitializeComponent();
        }



        private void btnZiffer_Click(object sender, EventArgs e)
        {
            var ziffer = ((Button) sender).Text;

            Ziffer_zu_Zahl_hinzufügen(ziffer);

            lblZahl.Text = zahl.ToString();   
        }


        private int zahl;
        private bool _neueZahlBeginnen = true;

        private void Ziffer_zu_Zahl_hinzufügen(string ziffer)
        {
            if (_neueZahlBeginnen)
                zahl = int.Parse(ziffer);
            else
            {
                zahl = 10*zahl + int.Parse(ziffer);
            }
            _neueZahlBeginnen = false;
        }


        private void btnOp_Click(object sender, EventArgs e)
        {
            var aktuelleZahl = int.Parse(lblZahl.Text);
            var aktuelleOp = ((Button)sender).Text;

            if (lstProtokoll.Items.Count == 0)
                lstProtokoll.Items.Add(aktuelleZahl);
            else
            {
                if (_vorherigeOp != "=")
                {
                    var _ = lstProtokoll.Items[lstProtokoll.Items.Count - 1];
                    lstProtokoll.Items.RemoveAt(lstProtokoll.Items.Count - 1);
                    lstProtokoll.Items.Add(_ + " " + aktuelleZahl);
                }
            }
            if (aktuelleOp != "=")
                lstProtokoll.Items.Add(aktuelleOp);

            switch (_vorherigeOp)
            {
                case "=":
                    _zwischenergebnis = aktuelleZahl;
                    break;
                case "+":
                    _zwischenergebnis = _zwischenergebnis + aktuelleZahl;
                    break;
                case "-":
                    _zwischenergebnis = _zwischenergebnis - aktuelleZahl;
                    break;
                case "*":
                    _zwischenergebnis = _zwischenergebnis * aktuelleZahl;
                    break;
                case "/":
                    _zwischenergebnis = _zwischenergebnis / aktuelleZahl;
                    break;
            }

            if (aktuelleOp == "=")
            {
                lstProtokoll.Items.Add("-------");
                lstProtokoll.Items.Add(_zwischenergebnis);
            }
            lstProtokoll.SelectedIndex = lstProtokoll.Items.Count - 1;

            lblZahl.Text = _zwischenergebnis.ToString();
            _vorherigeOp = aktuelleOp;
            _neueZahlBeginnen = true;
        }
    }
}
