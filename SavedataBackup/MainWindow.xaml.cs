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
            Closing += vm.OnClosing;
        }

        private void onOpenSavedataFile(object sender, RoutedEventArgs e)
        {
            if (vm.CurrentGame==null)
            {
                MessageBox.Show("You need to add a game first");
                return;
            }
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = vm.SavedataFile;
            if (ofd.ShowDialog() == true)
            {
                vm.CurrentGame.SavedataFile = ofd.FileName;
                vm.SavedataFile = ofd.FileName;
                vm.SavedataTime = System.IO.File.GetLastWriteTime(ofd.FileName).ToString();
            }
        }

        private void onSaveBackupFile(object sender, RoutedEventArgs e)
        {
            if (vm.CurrentGame == null)
            {
                MessageBox.Show("You need to add a game first");
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = new DirectoryInfo(vm.SavedataFile).Name;
            if (sfd.ShowDialog() == true)
            {
                vm.CurrentGame.BackupFile = sfd.FileName;
                vm.BackupFile = sfd.FileName;

                File.Copy(vm.SavedataFile, vm.BackupFile, true);
                vm.BackupTime = System.IO.File.GetLastWriteTime(sfd.FileName).ToString();
            }
        }

        private void OnOpenSavedataLocation(object sender, RoutedEventArgs e)
        {
            if (vm.CurrentGame == null)
            {
                MessageBox.Show("You need to add a game first");
                return;
            }
            string directory = new DirectoryInfo(vm.SavedataFile).Parent.FullName;
            if (Directory.Exists(directory))
            {
                Process.Start(Environment.GetEnvironmentVariable("WINDIR") + @"\explorer.exe", directory);
            }
        }

        private void OnOpenBackupLocation(object sender, RoutedEventArgs e)
        {
            if (vm.CurrentGame == null)
            {
                MessageBox.Show("You need to add a game first");
                return;
            }
            string directory = new DirectoryInfo(vm.BackupFile).Parent.FullName;
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
            }
        }

        private void OnBackupCurrent(object sender, RoutedEventArgs e)
        {
            if (vm.CurrentGame != null)
            {
                File.Copy(vm.SavedataFile, vm.BackupFile, true);
            }
        }

        private void OnBackupAll(object sender, RoutedEventArgs e)
        {
            foreach (GameEntry game in vm.GameList)
            {
                if (File.Exists(game.SavedataFile) && File.Exists(game.BackupFile))
                {
                    File.Copy(game.SavedataFile, game.BackupFile, true);
                }
            }
        }
    }    
}