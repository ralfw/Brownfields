using System;
using System.Windows;

namespace Wecker
{
    public partial class Ui : Window
    {
        public Ui() {
            InitializeComponent();

            starten.Click += (s, e) => Start(Zeitholen());
        }

        private DateTime Zeitholen() {
            if (txtWann.Text == "") {
                txtWann.Text = DateTime.Now.ToLongTimeString();
            }
            return DateTime.Parse(txtWann.Text);
        }

        public event Action<DateTime> Start;

        public void Uhrzeit(DateTime weckenUm) {
            if (lblUhrzeit.Dispatcher.CheckAccess()) {
                lblUhrzeit.Content = DateTime.Now.ToLongTimeString();
                lblRest.Content = (weckenUm - DateTime.Now).ToString(@"hh\:mm\:ss");
            }
            else {
                lblUhrzeit.Dispatcher.Invoke(() => {
                    lblUhrzeit.Content = DateTime.Now.ToLongTimeString();
                    lblRest.Content = (weckenUm - DateTime.Now).ToString(@"hh\:mm\:ss");
                });
            }
        }
    }
}