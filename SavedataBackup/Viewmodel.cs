using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavedataBackup
{
    internal class Viewmodel: INotifyPropertyChanged
    {
        public Viewmodel() 
        {
            GameEntries = new List<GameEntry>();
            GameEntries.Add(new GameEntry("1","1","1"));
            GameEntries.Add(new GameEntry("2", "2", "2"));
            GameEntries.Add(new GameEntry("3", "3", "3"));

            GameList = new ObservableCollection<GameEntry>(GameEntries);
        }

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

        public GameEntry CurrentGame { get; set; }
    }
}
