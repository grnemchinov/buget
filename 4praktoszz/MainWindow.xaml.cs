using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _4praktoszz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static bool isSave = false;
        private List<string> types = new List<string>();
        List<dataclass> datas = new List<dataclass>();


        public MainWindow()
        {
            InitializeComponent();

            if (!File.Exists("D:\\budgetdata.json"))
                File.Create("D:\\budgetdata.json").Close();

            date.SelectedDate = DateTime.Now.Date;
            typespick.ItemsSource = types;

            datas = json.getInfo<List<dataclass>>("D:\\budgetdata.json");

            if (datas == null)
            {
                datas = new List<dataclass>();
            }
            Updatedata();
        }


        public void Updatedata()
        {
            data.ItemsSource = null;
            List<dataclass> sorted = new List<dataclass>();
            int summ = 0;
            foreach (var item in datas)
            {
                if (item.Date == date.SelectedDate.Value)
                {
                    if (item.isIncome) { summ += item.Cost; }
                    else { summ -= item.Cost; item.cost *= -1; }
                    sorted.Add(item);
                }
            }
            if (summ < 0)
            {
                summa.Content = "Итого -:" + -summ;
            }
            else
            {
                summa.Content = "Итого: " + summ;
            }
            data.ItemsSource = sorted;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch ((e.OriginalSource as Button).Name)
            {
                case "createtype":
                    CreateType();
                    break;
                case "create":
                    Create();
                    break;
                case "update":
                    Update();
                    break;
                case "delete":
                    Delete();
                    break;
                default:
                    break;
            }
        }

        private void CreateType()
        {
            typeCreation tc = new typeCreation();
            tc.ShowDialog();
            if(!String.IsNullOrEmpty(tc.text))
            {
                types.Add(tc.text);
                typespick.ItemsSource = null;
                typespick.ItemsSource = types;
            }
        }

        private void Create()
        {
            int parsed = 0;
            if(!String.IsNullOrEmpty(NameNode.Text) && !String.IsNullOrEmpty(typespick.Text) && int.TryParse(SummaNode.Text, out parsed))
            {
                datas.Add(new dataclass(NameNode.Text, parsed, typespick.Text, date.SelectedDate.Value));
                data.ItemsSource = null;
                data.ItemsSource = datas;
                json.saveInfo("D:\\budgetdata.json", datas);
            }
            Updatedata();
        }
        private void Update()
        {
            if(data.SelectedItem != null)
            {
                var _selected = data.SelectedItem as dataclass;
                foreach(var _data in datas)
                {
                    if (_data == _selected)
                    {
                        int parsed = 0;
                        if (!String.IsNullOrEmpty(NameNode.Text) && !String.IsNullOrEmpty(typespick.Text) && int.TryParse(SummaNode.Text, out parsed))
                        {
                            datas[datas.IndexOf(_data)] = new dataclass(NameNode.Text, parsed, typespick.Text, date.SelectedDate.Value);
                            json.saveInfo("D:\\budgetdata.json", datas);
                            data.ItemsSource = null;
                            data.ItemsSource = datas;
                        }
                    }
                }
            }
            Updatedata();
        }
        private void Delete()
        {
            if(data.SelectedItem != null)
            {
                var _selected = data.SelectedItem as dataclass;
                datas.RemoveAt(datas.IndexOf(_selected));
                json.saveInfo("D:\\budgetdata.json", datas);
                data.ItemsSource = null;
                data.ItemsSource = datas;
            }
            Updatedata();
        }

        private void date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Updatedata();
        }

        private void data_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(data.SelectedItem != null)
            {
                var _selected = data.SelectedItem as dataclass;
                NameNode.Text = _selected.Name;
                if (!types.Contains(_selected.Type))
                {
                    types.Add(_selected.Type);
                    typespick.ItemsSource = null;
                    typespick.ItemsSource = types;
                    typespick.SelectedItem = _selected.Type;
                }
                SummaNode.Text = _selected.cost.ToString();

            }
        }
    }
}
