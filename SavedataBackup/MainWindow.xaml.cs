using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SavedataBackup
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static Viewmodel vm = new Viewmodel();
        public MainWindow()
        {
            this.DataContext = vm;
            InitializeComponent();
        }

        private void onOpenSavedataFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = vm.SavedataFile;
            if (ofd.ShowDialog() == true)
            {
                vm.CurrentGame.SaveDataFile = ofd.FileName;
                vm.SavedataFile = ofd.FileName;
                vm.SavedataTime = System.IO.File.GetLastWriteTime(ofd.FileName).ToString();
            }
        }

        private void onSaveBackupFile(object sender, RoutedEventArgs e)
        {
            /*
            //NEED savefiledialog
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = vm.BackupFile;
            if (ofd.ShowDialog() == true)
            {
                vm.CurrentGame.BackupFile = ofd.FileName;
                vm.BackupFile = ofd.FileName;
                vm.BackupTime = System.IO.File.GetLastWriteTime(ofd.FileName).ToString();
            }
            */

        }

        private void OnOpenSavedataLocation(object sender, RoutedEventArgs e)
        {
            string directory = new DirectoryInfo(vm.SavedataFile).Parent.Name;
            if (Directory.Exists(directory))
            {
                Process.Start(Environment.GetEnvironmentVariable("WINDIR") + @"\explorer.exe", directory);
            }
        }

        private void OnOpenBackupLocation(object sender, RoutedEventArgs e)
        {
            string directory = new DirectoryInfo(vm.BackupFile).Parent.Name;
            if (Directory.Exists(directory))
            {
                Process.Start(Environment.GetEnvironmentVariable("WINDIR") + @"\explorer.exe", directory);
            }
        }

        private void OnSearchStringChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox box)
            {
                if (string.IsNullOrEmpty(box.Text))
                {
                    vm.SearchString = "";
                }
                else
                {
                    vm.SearchString = box.Text;
                }
            }
        }

        private void OnAddGame(object sender, RoutedEventArgs e)
        {
            vm.AddGame();
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox box)
            {
                vm.CurrentGame = (GameEntry)box.SelectedItem;

                vm.SavedataFile = vm.CurrentGame.SaveDataFile;
                vm.SavedataTime = System.IO.File.GetLastWriteTime(vm.CurrentGame.SaveDataFile).ToString();

                vm.BackupFile = vm.CurrentGame.BackupFile;
                vm.BackupTime = System.IO.File.GetLastWriteTime(vm.CurrentGame.BackupFile).ToString();
            }
        }
    }    
}