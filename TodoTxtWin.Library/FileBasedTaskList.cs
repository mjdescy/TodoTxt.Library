using System;
using System.IO;
using System.Threading;

namespace TodoTxtWin.Library
{
    /// <summary>
    /// A FileBasedTaskList is a TaskList with methods for saving to a text file and monitoring the file for changes. 
    /// </summary>
    public class FileBasedTaskList : TaskListWithMetadata
    {
        #region Static Properties

        static readonly string WindowsNewline = "\r\n";
        static readonly string UnixNewline = "\n";

        #endregion

        #region Properties

        public string FilePath { get; private set; }
        private FileSystemWatcher FileChangeObserver;
        public bool AutoSave { get; set; }

        private bool reloadFileWhenFileModifiedExternallyValue;
        public bool ReloadFileWhenFileModifiedExternally
        {
            get
            {
                return reloadFileWhenFileModifiedExternallyValue;
            }
            set
            {
                this.reloadFileWhenFileModifiedExternallyValue = value;
                if (reloadFileWhenFileModifiedExternallyValue)
                {
                    EnableFileChangeObserver();
                }
                else
                {
                    DisableFileChangeObserver();
                }
            }
        }

        #endregion

        #region Constructors

        public FileBasedTaskList(string filePath) : base()
        {
            this.FilePath = filePath;
            this.PreferredLineEnding = GetPreferredFileLineEndingFromFile();
            AppendFile(FilePath);
            this.AutoSave = false;
        }

        #endregion

        #region File Reload Methods

        public void ReloadFile()
        {
            if (String.IsNullOrEmpty(this.FilePath))
            {
                return;
            }

            bool autoSaveSetting = this.AutoSave;
            this.AutoSave = false;
            DisableFileChangeObserver();
            this.Clear();
            if (this.ReloadFileWhenFileModifiedExternally)
            {
                EnableFileChangeObserver();
            }
            this.AutoSave = autoSaveSetting;

            this.AppendFile(this.FilePath);
        }

        #endregion
        
        #region File Save Methods

        public void Save()
        {
            try
            {
                DisableFileChangeObserver();
                SaveToFile(this.FilePath);
            }
            finally
            {
                if (this.ReloadFileWhenFileModifiedExternally)
                {
                    EnableFileChangeObserver();
                }
            }
        }

        #endregion

        #region File Change Observer Methods

        private void EnableFileChangeObserver()
        {
            if (this.FileChangeObserver == null)
            {
                this.FileChangeObserver = new FileSystemWatcher();
                this.FileChangeObserver.Path = System.IO.Path.GetDirectoryName(this.FilePath);
                this.FileChangeObserver.Filter = System.IO.Path.GetFileName(this.FilePath);
                this.FileChangeObserver.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size;

                this.FileChangeObserver.Changed += FileChangeEventHandler;
            }

            this.FileChangeObserver.EnableRaisingEvents = true;
        }

        private void DisableFileChangeObserver()
        {
            if (this.FileChangeObserver == null)
            {
                return;
            }

            this.FileChangeObserver.EnableRaisingEvents = false;
        }

        private void FileChangeEventHandler(object source, FileSystemEventArgs e)
        {
            if (this.ReloadFileWhenFileModifiedExternally)
            {
                Thread.Sleep(1000); // give the writing app time to release its lock
                ReloadFile();
            }
        }

        #endregion

        #region OnCollectionChanged Event Handler

        protected override void OnCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
            if (this.AutoSave)
            {
                Save();
            }            
        }

        #endregion

        #region Private Helper Methods

        private string GetPreferredFileLineEndingFromFile()
        {
            try
            {
                using (StreamReader fileStream = new StreamReader(this.FilePath))
                {
                    char previousChar = '\0';

                    // Read up to the first 5000 characters to try and find a newline
                    for (int i = 0; i < 5000; i++)
                    {
                        int b = fileStream.Read();
                        if (b == -1) break;

                        char currentChar = (char)b;
                        if (currentChar == '\n')
                        {
                            return (previousChar == '\r') ? WindowsNewline : UnixNewline;
                        }

                        previousChar = currentChar;
                    }

                    // if no newline found, use the default newline character for the environment
                    return Environment.NewLine;
                }
            }
            catch (IOException ex)
            {
                var msg = "An error occurred while trying to read the task list file";
                throw new Exception(msg, ex);
            }
        }

        #endregion
    }
}
