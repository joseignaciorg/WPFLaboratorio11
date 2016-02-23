using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.WindowsAzure.ActiveDirectory.Authentication;

namespace WPFLaboratorio11
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private AuthenticationResult auth;

        private AuthenticationResult Autenticar()
        {
            var ctx=new AuthenticationContext("https://login.windows.net/Laboratorio1113.onmicrosoft.com");

            var res = ctx.AcquireToken("https://Laboratorio1113.onmicrosoft.com/ApiLaboratorio11", "b71dc982-9302-40f6-86d6-388b312e79fc", "http://midominio/ok");
            return res;
        }

        private async Task<String> Operar(AuthenticationResult auth, String op, Operacion operacion)
        {
            var res = String.Empty;
            HttpClient cl=new HttpClient();
            cl.DefaultRequestHeaders.Authorization=new AuthenticationHeaderValue("Bearer",auth.AccessToken);

            var response = await cl.PostAsync("https://apilaboratorio11.azurewebsites.net/api" + op,new StringContent(operacion.ToJson(),Encoding.UTF8,"aplication/json"));

            if (response.IsSuccessStatusCode)
                res = await response.Content.ReadAsStringAsync();

            return res;
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            var op=new Operacion()
            {
                Op2 = Convert.ToInt32(textBox1.Text),
                Op1 = Convert.ToInt32(textBox.Text)
            };
            if (auth==null)
            {
                auth = Autenticar();
            }
            label.Content = await Operar(auth, "suma", op);
        }
    }
}
