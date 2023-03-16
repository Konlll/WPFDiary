using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using Microsoft.Win32;

namespace WpfOsztalyzas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string fileName = "naplo.txt";
        //Így minden metódus fogja tudni használni.
        ObservableCollection<Osztalyzat> jegyek = new ObservableCollection<Osztalyzat>();
        List<double> grades = new List<double>();

        public MainWindow()
        {
            InitializeComponent();
            // todo Fájlok kitallózásával tegye lehetővé a naplófájl kiválasztását! - KÉSZ
            // Ha nem választ ki semmit, akkor "naplo.csv" legyen az állomány neve. A későbbiekben ebbe fog rögzíteni a program. - KÉSZ
            OpenFileDialog ofd = new OpenFileDialog();
            if(ofd.ShowDialog() == true)
            {
                fileName = ofd.FileName;
            }
            else
            {
                fileName = "naplo.csv";
            }

            StreamReader sr = new StreamReader(fileName);
            while (!sr.EndOfStream)
            {
                string[] data = sr.ReadLine().Split(";");
                jegyek.Add(new Osztalyzat(data[0], data[1], data[2], int.Parse(data[3])));
                grades.Add(double.Parse(data[3]));
            }

            lblFolder.Content = System.IO.Path.GetFullPath(fileName);


            // todo A kiválasztott naplót egyből töltse be és a tartalmát jelenítse meg a datagrid-ben! - KÉSZ
            lblNumberOfGrades.Content = Convert.ToString(jegyek.Count);
            lblAverage.Content = Math.Round(grades.Sum() / grades.Count, 2);
            dgJegyek.ItemsSource = jegyek;
        }

        private void btnRogzit_Click(object sender, RoutedEventArgs e)
        {
            //todo Ne lehessen rögzíteni, ha a következők valamelyike nem teljesül!
            // a) - A név legalább két szóból álljon és szavanként minimum 3 karakterből!
            //      Szó = A szöközökkel határolt karaktersorozat.
            // b) - A beírt dátum újabb, mint a mai dátum

            //todo A rögzítés mindig az aktuálisan megnyitott naplófájlba történjen!

            string[] splittedName = txtNev.Text.Split(" ");
            bool nameError = false;
            bool dateError = false;
            if(DateTime.Parse(datDatum.Text) > DateTime.Now)
            {
                dateError = true;
            }
            if(splittedName.Length >= 2)
            {
                for (int i = 0; i < splittedName.Length; i++)
                {
                    if (splittedName[i].Length < 3)
                    {
                        nameError = true;
                        break;
                    }
                }
            }
            else
            {
                nameError = true;
            }

            if(nameError && dateError)
            {
                MessageBox.Show("A név legalább két szóból álljon és szavanként minimum 3 karakterből és a dátum nem lehet jövőbeli!");
            }
            else if (nameError)
            {
                MessageBox.Show("A név legalább két szóból álljon és szavanként minimum 3 karakterből.");
            } else if (dateError)
            {
                MessageBox.Show("Nem lehet jövőbeli dátum!");
            }
            else
            {
                //A CSV szerkezetű fájlba kerülő sor előállítása
                string csvSor = $"{txtNev.Text};{datDatum.Text.Trim()};{cboTantargy.Text};{sliJegy.Value}";
                //Megnyitás hozzáfűzéses írása (APPEND)
                StreamWriter sw = new StreamWriter(fileName, append: true);
                sw.WriteLine(csvSor);
                sw.Close();
                //todo Az újonnan felvitt jegy is jelenjen meg a datagrid-ben!
                string[] data = csvSor.Split(";");
                jegyek.Add(new Osztalyzat(data[0], data[1], data[2], int.Parse(data[3])));
                grades.Add(int.Parse(data[3]));
                lblNumberOfGrades.Content = Convert.ToString(jegyek.Count);
                lblAverage.Content = Math.Round(grades.Sum() / grades.Count, 2);
            }
        }

        private void btnBetolt_Click(object sender, RoutedEventArgs e)
        {
            jegyek.Clear();  //A lista előző tartalmát töröljük
            StreamReader sr = new StreamReader(fileName); //olvasásra nyitja az állományt
            while (!sr.EndOfStream) //amíg nem ér a fájl végére
            {
                string[] mezok = sr.ReadLine().Split(";"); //A beolvasott sort feltördeli mezőkre
                //A mezők értékeit felhasználva létrehoz egy objektumot
                Osztalyzat ujJegy = new Osztalyzat(mezok[0], mezok[1], mezok[2], int.Parse(mezok[3]));
                grades.Add(int.Parse(mezok[3]));
                
                jegyek.Add(ujJegy); //Az objektumot a lista végére helyezi
            }
            sr.Close(); //állomány lezárása

            lblFolder.Content = System.IO.Path.GetFullPath(fileName);

            //A Datagrid adatforrása a jegyek nevű lista lesz.
            //A lista objektumokat tartalmaz. Az objektumok lesznek a rács sorai.
            //Az objektum nyilvános tulajdonságai kerülnek be az oszlopokba.
            lblNumberOfGrades.Content = Convert.ToString(jegyek.Count);
            dgJegyek.ItemsSource = jegyek;

            lblNumberOfGrades.Content = Convert.ToString(jegyek.Count);
            lblAverage.Content = Math.Round(grades.Sum() / grades.Count, 2);
        }

        private void sliJegy_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblJegy.Content = sliJegy.Value; //Több alternatíva van e helyett! Legjobb a Data Binding!
        }

        private void rdoFirstName_Checked(object sender, RoutedEventArgs e)
        {
            foreach(var item in jegyek)
            {
                item.Nev = item.ForditottNev();
            }
            dgJegyek.ItemsSource = jegyek;
        }

        private void rdoLastName_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var item in jegyek)
            {
                item.Nev = item.ForditottNev();
            }
            dgJegyek.ItemsSource = jegyek;
        }

        //todo Felület bővítése: Az XAML átszerkesztésével biztosítsa, hogy láthatóak legyenek a következők! - KÉSZ
        // - A naplófájl neve
        // - A naplóban lévő jegyek száma
        // - Az átlag

        //todo Új elemek frissítése: Figyeljen rá, ha új jegyet rögzít, akkor frissítse a jegyek számát és az átlagot is!

        //todo Helyezzen el alkalmas helyre 2 rádiónyomógombot!
        //Feliratok: [■] Vezetéknév->Keresztnév [O] Keresztnév->Vezetéknév
        //A táblázatban a név azserint szerepeljen, amit a rádiónyomógomb mutat!
        //A feladat megoldásához használja fel a ForditottNev metódust!
        //Módosíthatja az osztályban a Nev property hozzáférhetőségét!
        //Megjegyzés: Felételezzük, hogy csak 2 tagú nevek vannak
    }
}

