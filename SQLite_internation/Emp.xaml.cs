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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SQLite_internation
{
    public partial class Emp : Window
    {
        public static DataGrid datagrid;
        public Emp()
        {
            InitializeComponent();
            Load();
            Switchlang("ru");
        }

        public void Load()
        {
            using (AppContext db = new AppContext())
            {
                Data.ItemsSource = db.Employees.ToList();
                datagrid = Data;
            }
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {
            using (AppContext db = new AppContext())
            {
                int Id = (Data.SelectedItem as Employee).id;
                var delete = db.Employees.Where(m => m.id == Id).Single();
                db.Employees.Remove(delete);
                db.SaveChanges();
                Data.ItemsSource = db.Employees.ToList();
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            using (AppContext db = new AppContext())
            {
                Employee employee = new Employee()
                {
                    name = Name_t.Text,
                    surname = Surname_t.Text,
                    post = Post_t.Text,
                    email = Email_t.Text
                };
                db.Employees.Add(employee);
                db.SaveChanges();
                Load();
            }
        }

        private void UpdBtn_Click(object sender, RoutedEventArgs e)
        {
            using (AppContext db = new AppContext())
            {
                int Id = (Data.SelectedItem as Employee).id;

                Employee upd = (from m in db.Employees where m.id == Id select m).Single();
                upd.name = Name_t.Text;
                upd.surname = Surname_t.Text;
                upd.post = Post_t.Text;
                upd.email = Email_t.Text;
                db.SaveChanges();
                Data.ItemsSource = db.Employees.ToList();
                Load();
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            string language = (cb.SelectedItem as ComboBoxItem).Tag.ToString();
            Switchlang(language);
        }

        public void Switchlang(string LangCode)
        {
            ResourceDictionary dictionary = new ResourceDictionary();
            switch (LangCode)
            {
                case "ru":
                    dictionary.Source = new Uri("..\\Resources/Lang.xaml", UriKind.Relative);
                    break;
                case "en":
                    dictionary.Source = new Uri("..\\Resources/Lang.en.xaml", UriKind.Relative);
                    break;
                case "de":
                    dictionary.Source = new Uri("..\\Resources/Lang.de.xaml", UriKind.Relative);
                    break;
            }
            this.Resources.MergedDictionaries.Add(dictionary);
        }

        private void Themes_Click(object sender, RoutedEventArgs e)
        {
            var theme = "LightTheme";

            if (Themes.IsChecked == true)
                theme = "DarkTheme";
            else
                theme = "LightTheme";

            var uri = new Uri($"Themes/{theme}.xaml", UriKind.Relative);

            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;

            Application.Current.Resources.Clear();

            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
        }
    }
}
