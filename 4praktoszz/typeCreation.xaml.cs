using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _4praktoszz
{
    /// <summary>
    /// Логика взаимодействия для typeCreation.xaml
    /// </summary>
    public partial class typeCreation : Window
    {

        public string text = "";
        public typeCreation()
        {
            InitializeComponent();
        }

        private void create_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(name.Text))
            {
                text = name.Text;
                Close();
            }
        }
    }
}
