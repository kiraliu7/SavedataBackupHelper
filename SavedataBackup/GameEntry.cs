using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavedataBackup
{
    internal class GameEntry
    {
        private static int count = 0;
        public string GameName { get; set; }
        public string SavedataFile { get; set; }
        public string BackupFile { get; set; }
        public int index { get; set; }

        public GameEntry(string name, string savedata, string backup)
        {
            GameName = name;
            SavedataFile = savedata;
            BackupFile = backup;
            index = count++;
        }
    } 
}
