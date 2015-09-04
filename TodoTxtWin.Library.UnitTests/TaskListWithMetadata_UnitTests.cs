using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TodoTxtWin.Library;

namespace TodoTxtWin.Library.UnitTests
{
    [TestClass]
    class TaskListWithMetadata_UnitTests
    {

        [TestClass]
        public class TaskListWithMetadata_UnitTests_Event_CollectionChanged
        {
            private List<Task> _taskList01;
            private List<string> _projects01;
            private List<string> _contexts01;
            private List<char> _priorities01;
            private List<Task> _taskList02;
            private Task _replacementTask;

            [TestInitialize]
            public void Initialize()
            {
                _taskList01 = new List<Task>() {
                    new Task("(A) Call Mom @Phone +Family", 0),
                    new Task("(B) Call Boss @Mobile +Work", 1),
                    new Task("(C) Do work @Desk +Work", 2)
                };
                _projects01 = new List<string>() {
                    "+Family",
                    "+Work"
                };
                _contexts01 = new List<string>() {
                    "@Phone",
                    "@Mobile",
                    "@Desk"
                };
                _priorities01 = new List<char>() {
                    'A',
                    'B',
                    'C'
                };

                _taskList02 = new List<Task>() {
                    new Task("(A) Schedule annual checkup +Health", 0), 
                    new Task("(B) Outline chapter 5 +Novel @Computer", 1)
                };

                _replacementTask = new Task("(D) replacement task +ReplacementProject @ReplacementContext");
            }

            [TestMethod]
            public void TaskListWithMetadata_CollectionChanged_WhenAddTask_ProjectsShouldUpdate01()
            {
                var taskList = new TaskListWithMetadata(_taskList01);
                CollectionAssert.AreEquivalent(_projects01, taskList.Projects);
            }

            [TestMethod]
            public void TaskListWithMetadata_CollectionChanged_WhenAddTask_ContextsShouldUpdate01()
            {
                var taskList = new TaskListWithMetadata(_taskList01);
                CollectionAssert.AreEquivalent(_contexts01, taskList.Contexts);
            }

            [TestMethod]
            public void TaskListWithMetadata_CollectionChanged_WhenAddTask_PrioritiesShouldUpdate01()
            {
                var taskList = new TaskListWithMetadata(_taskList01);
                CollectionAssert.AreEquivalent(_priorities01, taskList.Priorities);
            }

            [TestMethod]
            public void TaskListWithMetadata_CollectionChanged_WhenAddTask_ProjectsShouldUpdate02()
            {
                var taskList = new TaskListWithMetadata(_taskList01);
                taskList.Add(new Task("(D) another task +AdditionalProject @AdditionalContext"));
                _projects01.Add("+AdditionalProject");
                CollectionAssert.AreEquivalent(_projects01, taskList.Projects);
            }

            [TestMethod]
            public void TaskListWithMetadata_CollectionChanged_WhenAddTask_ContextsShouldUpdate02()
            {
                var taskList = new TaskListWithMetadata(_taskList01);
                taskList.Add(new Task("(D) another task +AdditionalProject @AdditionalContext"));
                _contexts01.Add("@AdditionalContext");
                CollectionAssert.AreEquivalent(_contexts01, taskList.Contexts);
            }

            [TestMethod]
            public void TaskListWithMetadata_CollectionChanged_WhenAddTask_PrioritiesShouldUpdate02()
            {
                var taskList = new TaskListWithMetadata(_taskList01);
                taskList.Add(new Task("(D) another task +AdditionalProject @AdditionalContext"));
                _priorities01.Add('D');
                CollectionAssert.AreEquivalent(_priorities01, taskList.Priorities);
            }

            [TestMethod]
            public void TaskListWithMetadata_CollectionChanged_WhenRemoveTask_ProjectsShouldUpdate01()
            {
                var taskList = new TaskListWithMetadata(_taskList01);
                taskList.RemoveAt(0);
                _projects01.Remove("+Family");
                CollectionAssert.AreEquivalent(_projects01, taskList.Projects);
            }

            [TestMethod]
            public void TaskListWithMetadata_CollectionChanged_WhenRemoveTask_ContextsShouldUpdate01()
            {
                var taskList = new TaskListWithMetadata(_taskList01);
                taskList.RemoveAt(2);
                _contexts01.Remove("@Desk");
                CollectionAssert.AreEquivalent(_contexts01, taskList.Contexts);
            }

            [TestMethod]
            public void TaskListWithMetadata_CollectionChanged_WhenRemoveTask_PrioritiesShouldUpdate01()
            {
                var taskList = new TaskListWithMetadata(_taskList01);
                taskList.RemoveAt(0);
                _priorities01.Remove('A');
                CollectionAssert.AreEquivalent(_priorities01, taskList.Priorities);
            }

            [TestMethod]
            public void TaskListWithMetadata_CollectionChanged_WhenReplaceTask_ProjectsShouldUpdate01()
            {
                var taskList = new TaskListWithMetadata(_taskList01);
                taskList[0] = _replacementTask;
                _projects01[0] = "+ReplacementProject";
                CollectionAssert.AreEquivalent(_projects01, taskList.Projects);
            }

            [TestMethod]
            public void TaskListWithMetadata_CollectionChanged_WhenReplaceTask_ContextsShouldUpdate01()
            {
                var taskList = new TaskListWithMetadata(_taskList01);
                taskList[0] = _replacementTask;
                _contexts01[0] = "@ReplacementContext";
                CollectionAssert.AreEquivalent(_contexts01, taskList.Contexts);
            }

            [TestMethod]
            public void TaskListWithMetadata_CollectionChanged_WhenReplaceTask_PrioritiesShouldUpdate01()
            {
                var taskList = new TaskListWithMetadata(_taskList01);
                taskList[0] = _replacementTask;
                _priorities01[0] = 'D';
                CollectionAssert.AreEquivalent(_priorities01, taskList.Priorities);
            }
        }
    }
}
