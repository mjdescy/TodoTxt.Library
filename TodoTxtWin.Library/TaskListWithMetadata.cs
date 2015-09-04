using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoTxtWin.Library
{
    public class TaskListWithMetadata : TaskList
    {
        #region Properties

        public List<string> Projects { get; private set; }
        public List<string> Contexts { get; private set; }
        public List<char> Priorities { get; private set; }

        #endregion

        #region Constructors

        public TaskListWithMetadata() : base() { }

        public TaskListWithMetadata(IEnumerable<Task> taskList) : base(taskList) { }

        public TaskListWithMetadata(IEnumerable<String> taskStringList) : base(taskStringList) { }

        public TaskListWithMetadata(string taskList) : base(taskList) { }

        #endregion

        #region Task List Metadata Methods

        public void UpdateTaskListMetaData()
        {
            var UniqueProjects = new SortedSet<string>();
            var UniqueContexts = new SortedSet<string>();
            var UniquePriorities = new SortedSet<char>();

            foreach (Task t in this.Items)
            {
                foreach (string p in t.Projects)
                {
                    UniqueProjects.Add(p);
                }
                foreach (string c in t.Contexts)
                {
                    UniqueContexts.Add(c);
                }
                UniquePriorities.Add(t.Priority);
            }

            this.Projects = UniqueProjects.ToList<string>();
            this.Contexts = UniqueContexts.ToList<string>();
            this.Priorities = UniquePriorities.ToList<char>();
        }

        #endregion

        #region OnCollectionChanged Event Handler

        protected override void OnCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
            UpdateTaskListMetaData();
        }

        #endregion
    }
}
