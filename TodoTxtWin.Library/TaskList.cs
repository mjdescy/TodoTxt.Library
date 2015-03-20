using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace TodoTxtWin.Library
{
    /// <summary>
    /// A TaskList is an in-memory representation of a collection of Task objects, or lines from a todo.txt file.
    /// Typically a TaskList represents one todo.txt file.
    /// </summary>
    public class TaskList : ItemsChangedObservableCollection<Task>
    {
        #region Static Properties

        public static readonly string[] NewLine = new[] { "\r\n", "\r", "\n" };

        #endregion

        #region Properties

        public string PreferredLineEnding { get; set; }

        #endregion

        #region Constructors

        public TaskList() : base()
        {
            this.PreferredLineEnding = Environment.NewLine;
        }

        public TaskList(IEnumerable<Task> taskList) : this()
        {
            Append(taskList);
        }

        public TaskList(IEnumerable<String> taskStringList) : this()
        {
            Append(taskStringList);
        }

        public TaskList(string taskList) : this()
        {
            Append(taskList);
        }

        #endregion

        #region Append To List Methods

        public void AppendFile(string filePath)
        {
            this.Append(File.ReadAllLines(filePath));
        }

        public void Append(Stream stream)
		{
            string fileContentsString;
			try
			{
				using (var streamReader = new StreamReader(stream))
				{
                    fileContentsString = streamReader.ReadToEnd();
				}
			}
			catch (IOException ex)
			{
				throw new Exception("There was a problem trying to read from your file", ex);
			}

            this.Append(fileContentsString);
		}

		public void Append(String taskListString)
		{
            this.Append(taskListString.Split(NewLine, StringSplitOptions.None));
		}

        public void Append(IEnumerable<string> taskStringList)
        {
            if (taskStringList == null)
            {
                return;
            }

            var taskList = new List<Task>();
            foreach (string taskString in taskStringList)
            {
                taskList.Add(new Task(taskString));
            }

            Append(taskList);
        }

        public void Append(IEnumerable<Task> taskList)
        {
            if (taskList == null)
            {
                return;
            }

            foreach (Task task in taskList)
            {
                task.ID = this.Count + 1;
                this.Items.Add(task);
            }

            this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        #endregion

        #region Remove and Replace Methods

        public void Remove(IEnumerable<Task> range)
        {
            if (range == null)
            {
                return;
            }

            foreach (Task task in range)
            {
                this.Items.Remove(task);
            }

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, range.ToList()));
        }

        public void ReplaceAll(IEnumerable<Task> range)
        {
            this.Items.Clear();

            Append(range);
        }

        #endregion

        #region Output Methods

        public override string ToString()
        {
            return String.Join(this.PreferredLineEnding, Items.Select(x => x.RawText).ToArray());
        }

        public void SaveToFile(string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.NewLine = this.PreferredLineEnding;
                    foreach (Task task in this.Items)
                    {
                        writer.WriteLine(task.RawText);
                    }
                    writer.Close();
                }
            }
            catch (IOException ex)
            {
                var msg = "An error occurred while trying to write to the task list file.";
                throw new Exception(msg, ex);
            }
        }

        #endregion

        #region OnCollectionChanged and OnItemPropertyChanged Event Handlers

        protected override void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "RawText")
            {
                base.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        //protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        //{
        //    base.OnCollectionChanged(e);

        //    if (e.NewItems != null)
        //    {
        //        foreach (Task newItem in e.NewItems)
        //        {
        //            // Add listener for each item on PropertyChanged event.
        //            newItem.PropertyChanged += this.OnItemPropertyChanged;
        //        }
        //    }

        //    if (e.OldItems != null)
        //    {
        //        foreach (Task oldItem in e.OldItems)
        //        {
        //            oldItem.PropertyChanged -= this.OnItemPropertyChanged;
        //        }
        //    }
        //}

        //protected virtual void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    //this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace))
        //}

        //protected override void SetItem(int index, Task item)
        //{
        //    var oldItem = this[index];
        //    oldItem.PropertyChanged -= OnItemPropertyChanged;
        //    base.SetItem(index, item);
        //    item.PropertyChanged += OnItemPropertyChanged;
        //}

        #endregion
    }
}
