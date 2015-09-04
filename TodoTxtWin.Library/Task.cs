using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TodoTxtWin.Library
{
    /// <summary>
    /// A Task is an in-memory representation of a single task, which is one line from a todo.txt file.
    /// </summary>
    public class Task : INotifyPropertyChanged, IComparable<Task>, IComparable, IEquatable<Task>
    {
        #region Enums

        public enum TaskDueState
        {
            Overdue,
            DueToday,
            NotDue
        }

        public enum TaskThresholdState
        {
            BeforeThresholdDate,
            OnThresholdDate,
            AfterThresholdDate
        }
        
        #endregion

        #region Static Properties

        static readonly Regex LineBreakPattern = new Regex("(\\r|\\n)", RegexOptions.Compiled);
        static readonly Regex CompletedPattern = new Regex("^x ((\\d{4})-(\\d{2})-(\\d{2})) ", RegexOptions.Compiled);
        static readonly Regex CompletionDatePattern = new Regex("(?<=^x )((\\d{4})-(\\d{2})-(\\d{2}))(?= )", RegexOptions.Compiled);
        static readonly Regex FullPriorityTextPattern = new Regex("^(\\([A-Z]\\) )", RegexOptions.Compiled);
        static readonly Regex PriorityLetterPattern = new Regex("[A-Z]", RegexOptions.Compiled);
        static readonly Regex PriorityTextPattern = new Regex("^(\\([A-Z]\\)(?= ))", RegexOptions.Compiled);
        static readonly Regex CreationDatePatternIncomplete = new Regex("(?<=^|\\([A-Z]\\) )((\\d{4})-(\\d{2})-(\\d{2})(?= ))", RegexOptions.Compiled);
        static readonly Regex CreationDatePatternCompleted = new Regex("(?<=^x ((\\d{4})-(\\d{2})-(\\d{2})) )((\\d{4})-(\\d{2})-(\\d{2})(?= ))", RegexOptions.Compiled);
        static readonly Regex DueDatePattern = new Regex("(?<=(^| )due:)((\\d{4})-(\\d{2})-(\\d{2}))(?= |$)", RegexOptions.Compiled);
        static readonly Regex FullDueDatePatternMiddleOrEnd = new Regex("(( )due:)((\\d{4})-(\\d{2})-(\\d{2}))(?= |$)", RegexOptions.Compiled);
        static readonly Regex FullDueDatePatternBeginning = new Regex("^due:((\\d{4})-(\\d{2})-(\\d{2})) ?|$", RegexOptions.Compiled);
        static readonly Regex ThresholdDatePattern = new Regex("(?<=(^| )t:)((\\d{4})-(\\d{2})-(\\d{2}))(?= |$)", RegexOptions.Compiled);
        static readonly Regex FullThresholdDatePatternMiddleOrEnd = new Regex("(( )t:)((\\d{4})-(\\d{2})-(\\d{2}))(?= |$)", RegexOptions.Compiled);
        static readonly Regex FullThresholdDatePatternBeginning = new Regex("^t:((\\d{4})-(\\d{2})-(\\d{2})) ?|$", RegexOptions.Compiled);
        static readonly Regex ProjectPattern = new Regex("(?<=^| )(\\+[^ ]+)", RegexOptions.Compiled);
        static readonly Regex ContextPattern = new Regex("(?<=^| )(\\@[^ ]+)", RegexOptions.Compiled);
        static readonly Regex TagPattern = new Regex("(?<=^| )([:graph:]+:[:graph:]+)", RegexOptions.Compiled);

        static readonly DateTime HighDate = new DateTime(9999, 12, 31);
        static readonly string HighDateText = "9999-12-31";

        #endregion

        #region Properties

        private string rawTextValue;
        public string RawText
        {
            get
            {
                return this.rawTextValue;
            }

            set
            {
                this.SetField(ref this.rawTextValue, value);
                SetDependentProperties();
            }
        }

        private int? idValue;
        public int? ID
        {
            get
            {
                return this.idValue;
            }

            set
            {
                this.SetField(ref this.idValue, value);
            }
        }

        private bool isBlankValue;
        public bool IsBlank
        {
            get
            {
                return this.isBlankValue;
            }
            private set
            {
                this.SetField(ref this.isBlankValue, value);
            }
        }

        private bool isCompletedValue;
        public bool IsCompleted
        {
            get
            {
                return this.isCompletedValue;
            }
            private set
            {
                this.SetField(ref this.isCompletedValue, value);
            }
        }

        private bool isPrioritizedValue;
        public bool IsPrioritized
        {
            get
            {
                return this.isPrioritizedValue;
            }
            private set
            {
                this.SetField(ref this.isPrioritizedValue, value);
            }
        }

        private string priorityTextValue;
        public string PriorityText
        {
            get
            {
                return this.priorityTextValue;
            }
            private set
            {
                this.SetField(ref this.priorityTextValue, value);
            }
        }

        private char priorityValue;
        public char Priority
        {
            get
            {
                return this.priorityValue;
            }
            private set
            {
                this.SetField(ref this.priorityValue, value);
            }
        }

        private string dueDateTextValue;
        public string DueDateText
        {
            get
            {
                return this.dueDateTextValue;
            }
            private set
            {
                this.SetField(ref this.dueDateTextValue, value);
            }
        }

        private DateTime dueDateValue;
        public DateTime DueDate
        {
            get
            {
                return this.dueDateValue;
            }
            private set
            {
                this.SetField(ref this.dueDateValue, value);
            }
        }

        private TaskDueState dueStateValue;
        public TaskDueState DueState
        {
            get
            {
                return this.dueStateValue;
            }
            private set
            {
                this.SetField(ref this.dueStateValue, value);
            }
        }

        private string creationDateTextValue;
        public string CreationDateText
        {
            get
            {
                return this.creationDateTextValue;
            }
            private set
            {
                this.SetField(ref this.creationDateTextValue, value);
            }
        }

        private DateTime creationDateValue;
        public DateTime CreationDate
        {
            get
            {
                return this.creationDateValue;
            }
            private set
            {
                this.SetField(ref this.creationDateValue, value);
            }
        }

        private string completionDateTextValue;
        public string CompletionDateText
        {
            get
            {
                return this.completionDateTextValue;
            }
            private set
            {
                this.SetField(ref this.completionDateTextValue, value);
            }
        }

        private DateTime completionDateValue;
        public DateTime CompletionDate
        {
            get
            {
                return this.completionDateValue;
            }
            private set
            {
                this.SetField(ref this.completionDateValue, value);
            }
        }

        private string thresholdDateTextValue;
        public string ThresholdDateText
        {
            get
            {
                return this.thresholdDateTextValue;
            }
            private set
            {
                this.SetField(ref this.thresholdDateTextValue, value);
            }
        }


        private DateTime thresholdDateValue;
        public DateTime ThresholdDate
        {
            get
            {
                return this.thresholdDateValue;
            }
            private set
            {
                this.SetField(ref this.thresholdDateValue, value);
            }
        }

        private TaskThresholdState thresholdStateValue;
        public TaskThresholdState ThresholdState
        {
            get
            {
                return this.thresholdStateValue;
            }
            private set
            {
                this.SetField(ref this.thresholdStateValue, value);
            }
        }

        private bool hasContextsValue;
        public bool HasContexts
        {
            get
            {
                return this.hasContextsValue;
            }
            private set
            {
                this.SetField(ref this.hasContextsValue, value);
            }
        }

        private List<string> contextsValue;
        public List<string> Contexts
        {
            get
            {
                return this.contextsValue;
            }
            private set
            {
                this.SetField(ref this.contextsValue, value);
            }
        }

        private bool hasProjectsValue;
        public bool HasProjects
        {
            get
            {
                return this.hasProjectsValue;
            }
            private set
            {
                this.SetField(ref this.hasProjectsValue, value);
            }
        }

        private List<string> projectsValue;
        public List<string> Projects
        {
            get
            {
                return this.projectsValue;
            }
            private set
            {
                this.SetField(ref this.projectsValue, value);
            }
        }

        #endregion

        #region Constructors

        public Task(string rawText, int? taskID, string prependedDateText)
        {
            this.ID = taskID;
            this.RawText = PrependDateTextToRawText(LineBreakPattern.Replace(rawText, String.Empty), prependedDateText);
        }

        public Task(string rawText, int? taskID) : this(rawText, taskID, null) { }

        public Task(string rawText) : this(rawText, null, null) { }

        public Task() : this(String.Empty, null, null) { }

        public Task(Task task) : this(task.RawText, task.ID) { }

        #endregion

        #region INotifyPropertyChanged Handlers

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion

        #region IComparable<Task> Handlers

        public int CompareTo(Task otherTask)
        {
            if (otherTask == null)
            {
                return 1;
            }

            int result = string.Compare(this.RawText, otherTask.RawText, true);
            if (result == 0)
            {
                result = this.ID.Value.CompareTo(otherTask.ID);
            }
            return result;
        }

        #endregion

        #region IComparable Handlers

        public int CompareTo(object obj)
        {
            if (obj != null && !(obj is Task))
            {
                throw new ArgumentException("Object must be of type Task");
            }

            return CompareTo(obj as Task);            
        }

        #endregion

        #region IEquatable<Task> Handlers

        public bool Equals(Task other)
        {
            if (other == null)
            {
                return false;
            }
            return this.ToString().Equals(other.ToString());
        }

        public override bool Equals(object obj)
        {
            Task otherTask = obj as Task;
            if (otherTask != null)
            {
                return Equals(otherTask as Task);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public static bool operator ==(Task task1, Task task2)
        {
            if ((object)task1 == null || ((object)task2) == null)
            {
                return Object.Equals(task1, task2);
            }

            return task1.Equals(task2);
        }

        public static bool operator !=(Task task1, Task task2)
        {
            if ((object)task1 == null || ((object)task2) == null)
            {
                return !Object.Equals(task1, task2);
            }

            return !task1.Equals(task2);
        }

        #endregion
        
        #region ToString Method

        public override String ToString()
        {
            return "[" + this.ID.ToString() + "] " + this.RawText;
        }

        #endregion

        #region Append, Prepend, and Replace Mehods

        public void AppendText(string textToAppend)
        {
            if (this.IsBlank)
            {
                this.RawText = textToAppend;
                return;
            }
            this.RawText = this.RawText + " " + textToAppend;
        }

        public void PrependText(string textToPrepend)
        {
            if (this.IsBlank)
            {
                this.RawText = textToPrepend;
                return;
            }

            int insertionIndex;

            // For completed tasks with creation date, prepend text after the completion date and creation date
            if (this.IsCompleted && !string.IsNullOrEmpty(this.CreationDateText))
            {
                insertionIndex = 24;
            }
            // For completed tasks with no creation date, prepend text after the completion date
            else if (this.IsCompleted && string.IsNullOrEmpty(this.CreationDateText))
            {
                insertionIndex = 13;
            }
            // For incomplete tasks with a creation date, prepend text after priority and creation date.
            else if (this.IsPrioritized && !string.IsNullOrEmpty(this.CreationDateText))
            {
                insertionIndex = 15;
            }
            // For incomplete tasks with a priority and no creation date, prepend text after priority.
            else if (this.IsPrioritized && string.IsNullOrEmpty(this.CreationDateText))
            {
                insertionIndex = 4;
            }
            // For incomplete tasks with a creation date, prepend text after creation date.
            else if (!String.IsNullOrEmpty(this.CreationDateText))
            {
                insertionIndex = 11;
            }
            // For all other types of tasks, prepend text to the beginning of the task.
            else
            {
                insertionIndex = 0;
            }

            if (insertionIndex == 0)
            {
                this.RawText = textToPrepend + " " + this.RawText;
            }
            else
            {
                string rawTextPrefix = this.RawText.Substring(0, insertionIndex - 1);
                string rawTextRemainder = this.RawText.Substring(insertionIndex);
                string[] rawTextComponents = { rawTextPrefix, textToPrepend, rawTextRemainder };
                this.RawText = String.Join(" ", rawTextComponents);
            }
        }

        public void ReplaceText(string textToReplace, string replacementText)
        {
            this.RawText = this.RawText.Replace(textToReplace, replacementText);
        }

        #endregion

        #region Completion Methods

        public void MarkComplete()
        {
            // Blank tasks can't be completed.
            // Completed tasks don't need to be completed again.
            if (this.IsBlank || this.IsCompleted) {
                return;
            }
    
            // Build new task rawText by removing priority and prepending "x" and today's date.
            string rawTextWithoutPriority = (this.IsPrioritized) ? FullPriorityTextPattern.Replace(this.RawText, string.Empty) : this.RawText;
            this.RawText = "x " + DateToString(DateTime.Today) + " " + rawTextWithoutPriority;
        }

        public void MarkIncomplete()
        {
            // Blank tasks can't be completed.
            // Incompleted tasks don't need to be completed again.
            if (this.IsBlank || !this.IsCompleted)
            {
                return;
            }

            // Remove the completed task prepended substring.
            this.RawText = CompletedPattern.Replace(this.RawText, String.Empty);
        }

        public void ToggleCompletion()
        {
            if (this.IsCompleted)
            {
                MarkIncomplete();
            }
            else
            {
                MarkComplete();
            }
        }

        #endregion

        #region Priority Methods

        public void SetPriority(char priority)
        {
            SetPriority(priority.ToString());
        }

        public void SetPriority(string priorityText)
        {
            // Abort if the priority parameter is not a valid priority ([A-Z]).
            if (!PriorityLetterPattern.IsMatch(priorityText))
            {
                return;
            }

            string newFullPriorityText = "(" + priorityText + ") ";
            this.RawText = (this.IsPrioritized) ?
                FullPriorityTextPattern.Replace(this.RawText, newFullPriorityText) :
                this.RawText = newFullPriorityText + this.RawText;
        }

        public void RemovePriority()
        {
            // Blank, completed, and non-prioritized tasks don't have priorities.
            if (this.IsBlank || this.IsCompleted || !this.IsPrioritized)
            {
                return;
            }

            this.RawText = FullPriorityTextPattern.Replace(this.RawText, string.Empty);
        }

        public void IncreasePriority()
        {
            // Blank and completed tasks don't get priorities.
            if (this.IsBlank || this.IsCompleted) {
                return;
            }
    
            // Non-prioritized tasks automatically get the top priority.
            if (!this.IsPrioritized) {
                SetPriority("A");
                return;
            }
    
            // There is no priority greater than 'A'.
            if (this.Priority == 'A') {
                return;
            }
    
            // Increase priority of task (e.g. 'B' - 1 = 'A')
            SetPriority((char)(this.Priority - 1));
        }

        public void DecreasePriority()
        {
            // Blank and completed tasks don't get priorities.
            if (this.IsBlank || this.IsCompleted)
            {
                return;
            }

            // Non-prioritized tasks automatically get the top priority.
            if (!this.IsPrioritized)
            {
                SetPriority("A");
                return;
            }

            // There is no priority lower than 'Z'.
            if (this.Priority == 'Z')
            {
                return;
            }

            // Decrease priority of task (e.g. 'B' + 1 = 'C')
            SetPriority((char)(this.Priority + 1));
        }

        #endregion

        #region Due Date Methods

        public void SetDueDate(string dueDateText)
        {
            if (!DateStringInProperFormat(dueDateText))
            {
                return;
            }
            
            if (!String.IsNullOrEmpty(this.DueDateText))
            {
                this.RawText = DueDatePattern.Replace(this.RawText, dueDateText);
            } 
            else
            {
                AppendText("due:" + dueDateText);
            }
        }

        public void SetDueDate(DateTime dueDate)
        {
            SetDueDate(DateToString(dueDate));
        }

        public void RemoveDueDate()
        {
            string newRawText = FullDueDatePatternBeginning.Replace(
                this.RawText, String.Empty);
            this.RawText = FullDueDatePatternMiddleOrEnd.Replace(
                newRawText, String.Empty);
        }

        public void IncrementDueDate(int days)
        {
            if (String.IsNullOrEmpty(this.DueDateText))
            {
                SetDueDate(DateTime.Today.AddDays(days));
            }
            else
            {
                SetDueDate(this.DueDate.AddDays(days));
            }
        }

        public void DecrementDueDate(int days)
        {
            IncrementDueDate(-1 * days);
        }

        #endregion

        #region Threshold Date Methods

        public void SetThresholdDate(string thresholdDateText)
        {
            if (!DateStringInProperFormat(thresholdDateText))
            {
                return;
            }

            if (!String.IsNullOrEmpty(this.ThresholdDateText))
            {
                this.RawText = ThresholdDatePattern.Replace(this.RawText, thresholdDateText);
            }
            else
            {
                AppendText("t:" + thresholdDateText);
            }
        }

        public void SetThresholdDate(DateTime thresholdDate)
        {
            SetThresholdDate(DateToString(thresholdDate));
        }

        public void RemoveThresholdDate()
        {
            string newRawText = FullThresholdDatePatternBeginning.Replace(
                this.RawText, String.Empty);
            this.RawText = FullThresholdDatePatternMiddleOrEnd.Replace(
                newRawText, String.Empty);
        }

        public void IncrementThresholdDate(int days)
        {
            if (String.IsNullOrEmpty(this.ThresholdDateText))
            {
                SetThresholdDate(DateTime.Today.AddDays(days));
            }
            else
            {
                SetThresholdDate(this.ThresholdDate.AddDays(days));
            }
        }

        public void DecrementThresholdDate(int days)
        {
            IncrementThresholdDate(-1 * days);
        }

        #endregion

        #region Private Helper Methods

        private string PrependDateTextToRawText(string rawText, string prependedDateText)
        {
            // if no prepended date is passed, or if there is already a creation date, prepend nothing
            if (string.IsNullOrEmpty(prependedDateText) ||
                CreationDatePatternIncomplete.IsMatch(rawText) ||
                CreationDatePatternCompleted.IsMatch(rawText))
            {
                return rawText;
            }

            // if the rawText has a priority, prepend the date after the priority
            if (PriorityTextPattern.IsMatch(rawText))
            {
                return rawText.Substring(0, 4) + prependedDateText + " " + rawText.Substring(4);
            }

            // if the rawText has no priority, prepend the date at the beginning of the string
            return prependedDateText + " " + rawText;
        }

        private void SetDependentProperties()
        {
            this.IsBlank = string.IsNullOrEmpty(this.RawText);
            this.IsCompleted = CompletedPattern.IsMatch(this.RawText);

            this.IsPrioritized = PriorityTextPattern.IsMatch(this.RawText);
            this.PriorityText = PriorityTextPattern.Match(this.RawText).ToString();
            this.Priority = IsPrioritized ? Convert.ToChar(this.PriorityText.Substring(1, 1)) : '~';

            this.Projects = MatchCollectionToList(ProjectPattern.Matches(this.RawText));
            this.HasProjects = (this.Projects.Count > 0);

            this.Contexts = MatchCollectionToList(ContextPattern.Matches(this.RawText));
            this.HasContexts = (this.Contexts.Count > 0);

            this.CreationDateText = (this.IsCompleted) ?
                CreationDatePatternCompleted.Match(this.RawText).ToString() :
                CreationDatePatternIncomplete.Match(this.RawText).ToString();
            this.CreationDate = StringToDate(this.CreationDateText);
            if (this.CreationDate == HighDate && this.CreationDateText != HighDateText)
            {
                this.CreationDateText = String.Empty;
            }

            this.CompletionDateText = CompletionDatePattern.Match(this.RawText).ToString();
            this.CompletionDate = StringToDate(this.CompletionDateText);
            if (this.CompletionDate == HighDate && this.CompletionDateText != HighDateText)
            {
                this.CompletionDateText = String.Empty;
            }

            this.DueDateText = DueDatePattern.Match(this.RawText).ToString();
            this.DueDate = StringToDate(this.DueDateText);
            if (this.DueDate == HighDate && this.DueDateText != HighDateText)
            {
                this.DueDateText = String.Empty;
            }
            this.DueState = GetDueState();

            this.ThresholdDateText = ThresholdDatePattern.Match(this.RawText).ToString();
            this.ThresholdDate = StringToDate(this.ThresholdDateText);
            if (this.ThresholdDate == HighDate && this.ThresholdDateText != HighDateText)
            {
                this.ThresholdDateText = String.Empty;
            }
            this.ThresholdState = GetThresholdState();
        }

        private bool DateStringInProperFormat(string dateText)
        {
            DateTime outputDate;
            return DateTime.TryParseExact(dateText, "yyyy-MM-dd",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out outputDate);
        }

        private DateTime StringToDate(string dateText)
        {
            DateTime outputDate;
            if (!DateTime.TryParseExact(dateText, "yyyy-MM-dd",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out outputDate))
            {
                return HighDate;
            }
            return outputDate.Date;
        }

        private string DateToString(DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

        private TaskDueState GetDueState()
        {
            var todaysDate = DateTime.Today;
            if (string.IsNullOrEmpty(this.DueDateText))
            {
                return TaskDueState.NotDue;
            }
            else if (this.DueDate > todaysDate)
            {
                return TaskDueState.NotDue;
            }
            else if (this.DueDate == todaysDate)
            {
                return TaskDueState.DueToday;
            }
            else // if (this.DueDate < todaysDate)
            {
                return TaskDueState.Overdue;
            }
        }

        private TaskThresholdState GetThresholdState()
        {
            var todaysDate = DateTime.Today;
            if (string.IsNullOrEmpty(ThresholdDateText))
            {
                return TaskThresholdState.AfterThresholdDate;
            }
            else if (this.ThresholdDate > todaysDate)
            {
                return TaskThresholdState.BeforeThresholdDate;
            }
            else if (this.ThresholdDate == todaysDate)
            {
                return TaskThresholdState.OnThresholdDate;
            }
            else // if (this.ThresholdDate < todaysDate)
            {
                return TaskThresholdState.AfterThresholdDate;
            }
        }

        private List<string> MatchCollectionToList(MatchCollection matches)
        {
            return matches.OfType<Match>().Select(m => m.Groups[0].Value).ToList<string>();
        }

        #endregion
    }
}
