using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace TodoTxtWin.Library
{
    public enum TaskListUpdateCommand
    {
        ToggleCompletion,
        SetPriority,
        IncreasePriority,
        DecreasePriority,
        RemovePriority,
        SetDueDate,
        IncrementDueDate,
        DecrementDueDate,
        RemoveDueDate,
        SetThresholdDate,
        IncrementThresholdDate,
        DecrementThresholdDate,
        RemoveThresholdDate,
        AppendText,
        PrependText
    }

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

            var appendedItems = new List<Task>(taskList.Count());

            foreach (Task task in taskList)
            {
                task.ID = this.Count + 1;
                this.Items.Add(task);
                appendedItems.Add(task);
            }

            this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, appendedItems));
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

            this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void ReplaceAll(IEnumerable<Task> range)
        {
            this.Items.Clear();

            Append(range);
        }

        #endregion

        #region Update Methods

        public void UpdateSelectedTasks(IList selectedItems, TaskListUpdateCommand command, dynamic parameter = null)
        {
            if (selectedItems == null) throw new ArgumentNullException("selectedItems");

            // capture indexes within the actual task list that should be modified
            var indexesToModify = new List<int>(selectedItems.Count);
            for (int i = 0; i < selectedItems.Count; i++)
            {
                int index = this.IndexOf((Task)selectedItems[i]);
                if (index != -1)
                {
                    indexesToModify.Add(index);
                }
            }

            // modify the actual task list via IList replacement by index
            for (int i = 0; i < indexesToModify.Count; i++)
            {
                var newTask = new Task(this[indexesToModify[i]]);
                switch (command)
                {
                    case TaskListUpdateCommand.ToggleCompletion:
                        newTask.ToggleCompletion();
                        break;
                    case TaskListUpdateCommand.IncreasePriority:
                        newTask.IncreasePriority();
                        break;
                    case TaskListUpdateCommand.DecreasePriority:
                        newTask.DecreasePriority();
                        break;
                    case TaskListUpdateCommand.RemovePriority:
                        newTask.RemovePriority();
                        break;
                    case TaskListUpdateCommand.SetPriority:
                        newTask.SetPriority((char)parameter);
                        break;
                    case TaskListUpdateCommand.IncrementDueDate:
                        newTask.IncrementDueDate(1);
                        break;
                    case TaskListUpdateCommand.DecrementDueDate:
                        newTask.DecrementDueDate(1);
                        break;
                    case TaskListUpdateCommand.RemoveDueDate:
                        newTask.RemoveDueDate();
                        break;
                    case TaskListUpdateCommand.SetDueDate:
                        newTask.SetDueDate(parameter);
                        break;
                    case TaskListUpdateCommand.IncrementThresholdDate:
                        newTask.IncrementThresholdDate(1);
                        break;
                    case TaskListUpdateCommand.DecrementThresholdDate:
                        newTask.DecrementThresholdDate(1);
                        break;
                    case TaskListUpdateCommand.RemoveThresholdDate:
                        newTask.RemoveThresholdDate();
                        break;
                    case TaskListUpdateCommand.SetThresholdDate:
                        newTask.SetThresholdDate(parameter);
                        break;
                    case TaskListUpdateCommand.PrependText:
                        newTask.PrependText(parameter);
                        break;
                    case TaskListUpdateCommand.AppendText:
                        newTask.AppendText(parameter);
                        break;
                    default:
                        break;
                }
                this[indexesToModify[i]] = newTask;
            }

            // modify the selectedItems list to update the tasks selected
            selectedItems.Clear();
            for (int i = 0; i < indexesToModify.Count; i++)
            {
                selectedItems.Add(this[indexesToModify[i]]);
            }
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

        #region Event Handlers

        protected override void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "RawText" || e.PropertyName == "ID")
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public static implicit operator TaskList(List<Task> v)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
