using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SavedataBackup
{
    internal class Viewmodel: INotifyPropertyChanged
    {
        public Viewmodel() 
        {
            GameEntries = new List<GameEntry>();
            
            if(File.Exists(dataFile)){
                try
                {
                    using (StreamReader sr = new StreamReader(dataFile))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            string save = sr.ReadLine();
                            string back = sr.ReadLine();
                            if (save == null || back == null)
                            {
                                throw new Exception("Invalid data");
                            }
                            else
                            {
                                GameEntries.Add(new GameEntry(line, save, back));
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    GameEntries = new List<GameEntry>();
                }
            }

            GameList = new ObservableCollection<GameEntry>(GameEntries);

            if (GameList.Count > 0)
            {
                CurrentGame = GameList[0];
            }
        }

        private string dataFile = "C:\\Users\\sijia\\Desktop\\New folder\\data";

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _savedataFile = "not set";
        private string _backupFile = "not set";

        public string SavedataFile {  
            get { 
                return _savedataFile; 
            } 
            set { 
                _savedataFile = value;
                OnPropertyChanged("SavedataFile");
            } 
        }
        public string BackupFile { 
            get { 
                return _backupFile; 
            } 
            set { 
                _backupFile = value;
                OnPropertyChanged("BackupFile");
            } 
        }

        private string _savedataTime = "";
        private string _backupTime = "";

        public string SavedataTime
        {
            get
            {
                return _savedataTime;
            }
            set
            {
                _savedataTime = value;
                OnPropertyChanged("SavedataTime");
            }
        }
        public string BackupTime
        {
            get
            {
                return _backupTime;
            }
            set
            {
                _backupTime = value;
                OnPropertyChanged("BackupTime");
            }
        }

        private string _searchString = "";
        public string SearchString
        {
            get
            {
                return _searchString;
            }
            set
            {
                _searchString = value;
                OnPropertyChanged("SearchString");
                if (string.IsNullOrEmpty(_searchString))
                {
                    GameList = new ObservableCollection<GameEntry>(GameEntries);
                }
                else
                {
                    GameList = new ObservableCollection<GameEntry>(GameEntries.Where(e => e.GameName.Contains(_searchString)));
                }
                OnPropertyChanged("GameList");
            }
        }

        private List<GameEntry> GameEntries { get; }

        public ObservableCollection<GameEntry> GameList { get; set;  }

        public void AddGame()
        {
            GameEntries.Add(new GameEntry("New Game", "not set", "not set"));
            GameList = new ObservableCollection<GameEntry>(GameEntries.Where(e => e.GameName.Contains(_searchString)));
            OnPropertyChanged("GameList");
        }

        private GameEntry _currentGame;
        public GameEntry CurrentGame {
            get { 
                return _currentGame; 
            }
            set { 
                _currentGame = value;
                onUpdateCurrentGame();
            }
        }

        private void onUpdateCurrentGame()
        {
            if (CurrentGame != null)
            {
                SavedataFile = CurrentGame.SavedataFile;
                SavedataTime = System.IO.File.GetLastWriteTime(CurrentGame.SavedataFile).ToString();

                BackupFile = CurrentGame.BackupFile;
                BackupTime = System.IO.File.GetLastWriteTime(CurrentGame.BackupFile).ToString();
            }
        }

        public void OnClosing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                using (StreamWriter wr = new StreamWriter(dataFile))
                {
                    foreach(GameEntry entry in GameEntries)
                    {
                        wr.WriteLine(entry.GameName);
                        wr.WriteLine(entry.SavedataFile);
                        wr.WriteLine(entry.BackupFile);
                    }
                }
            }
            catch (Exception ex)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
