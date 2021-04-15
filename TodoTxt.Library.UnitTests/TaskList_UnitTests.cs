using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TodoTxt.Library.UnitTests
{
    [TestClass]
    public class TaskList_UnitTests
    {
        [TestClass]
        public class TaskList_UnitTests_Method_Constructor
        {
            [TestMethod]
            public void TaskList_Constructor_WhenNoArguments_CountShouldBeZero()
            {
                var taskList = new TaskList();
                Assert.AreEqual(0, taskList.Count);
            }

            [TestMethod]
            public void TaskList_Constructor_WhenStringArrayArgument_CountShouldBeEqualStringArrayCount()
            {
                string[] taskList01 = {
                            "(A) Call Mom @Phone +Family",
                            "(A) Schedule annual checkup +Health", 
                            "(B) Outline chapter 5 +Novel @Computer" 
                        };
                const int count = 3;
                var taskList = new TaskList(taskList01);
                Assert.AreEqual(count, taskList.Count);
            }

            [TestMethod]
            public void TaskList_Constructor_WhenTaskListArgument_CountShouldBeEqualTaskListCount()
            {
                var taskList01 = new List<Task>() {
                            new Task("(A) Call Mom @Phone +Family"),
                            new Task("(A) Schedule annual checkup +Health"), 
                            new Task("(B) Outline chapter 5 +Novel @Computer")
                        };
                const int count = 3;
                var taskList = new TaskList(taskList01);
                Assert.AreEqual(count, taskList.Count);
            }

            [TestMethod]
            public void TaskList_Constructor_WhenStringArgument_CountShouldBeEqualTaskListCount()
            {
                var taskString = 
                    "(A) Call Mom @Phone +Family" + Environment.NewLine +
                    "(A) Schedule annual checkup +Health" + Environment.NewLine +
                    "(B) Outline chapter 5 +Novel @Computer";
                const int count = 3;
                var taskList = new TaskList(taskString);
                Assert.AreEqual(count, taskList.Count);
            }
        }

        [TestClass]
        public class TaskList_UnitTests_Method_Append
        {
            private List<Task> _taskList01;
            private List<Task> _taskList02;

            [TestInitialize]
            public void Initialize()
            {
                _taskList01 = new List<Task>() {
                    new Task("(A) Call Mom @Phone +Family", 0)
                };
                _taskList02 = new List<Task>() {
                    new Task("(A) Schedule annual checkup +Health"), 
                    new Task("(B) Outline chapter 5 +Novel @Computer")
                };
            }

            [TestMethod]
            public void TaskList_Append_WhenTaskListNotEmpty_ShouldAddTaskList()
            {
                var taskList = new TaskList(_taskList01);
                taskList.Append(_taskList02);
                const int expectedCountAfterAppend = 3;
                Assert.AreEqual(expectedCountAfterAppend, taskList.Count);
            }

            [TestMethod]
            public void TaskList_Append_WhenTaskListEmpty_ShouldAddTaskList()
            {
                var taskList = new TaskList();
                taskList.Append(_taskList02);
                const int expectedCountAfterAppend = 2;
                Assert.AreEqual(expectedCountAfterAppend, taskList.Count);
            }

            [TestMethod]
            public void TaskList_Append_WhenTaskListEmpty_ShouldNotifyCollectionIsChanged()
            {
                var taskList = new TaskList();
                int changeCount = 0;
                taskList.CollectionChanged += (sender, e) =>
                {
                    changeCount++;
                };
                taskList.Append(_taskList02);
                const int expectedChangeCountAfterAppend = 1;
                Assert.AreEqual(expectedChangeCountAfterAppend, changeCount);
            }
        }

        [TestClass]
        public class TaskList_UnitTests_Method_AppendFile
        {
            private MemoryStream GenerateStreamFromString(string value)
            {
                return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
            }

            [TestMethod]
            public void TaskList_AppendStream_ShouldAppendStream()
            {
                var taskList = new TaskList();
                var fileStreamContents = 
                    "(A) Call Mom @Phone +Family" + Environment.NewLine +
                    "(A) Schedule annual checkup +Health" + Environment.NewLine +
                    "(B) Outline chapter 5 +Novel @Computer";
                using (MemoryStream memory = GenerateStreamFromString(fileStreamContents))
                {
                    taskList.Append(memory);
                }
                Assert.AreEqual(fileStreamContents, taskList.ToString());
            }
        }

        //[TestClass]
        //public class TaskList_UnitTests_Method_Append
        //{
        //}

        [TestClass]
        public class TaskList_UnitTests_Method_Reset
        {
        }

        [TestClass]
        public class TaskList_UnitTests_Method_Remove
        {
            private List<Task> _taskList01;
            private List<Task> _taskList02;

            [TestInitialize]
            public void Initialize()
            {
                _taskList01 = new List<Task>() {
                    new Task("(A) Call Mom @Phone +Family", 0)
                };
                _taskList02 = new List<Task>() {
                    new Task("(A) Schedule annual checkup +Health"), 
                    new Task("(B) Outline chapter 5 +Novel @Computer")
                };
           } 

            [TestMethod]
            public void TaskListRemove_WhenRangeRemoved_ShouldRemove()
            {
                var taskList = new TaskList(_taskList01);
                taskList.Append(_taskList02);
                taskList.Remove(_taskList01);
                var taskList2 = new TaskList(_taskList02);
                CollectionAssert.AreEqual(taskList2, taskList);
            }

            [TestMethod]
            public void TaskList_Remove_WhenTaskListNotEmpty_ShouldNotifyCollectionIsChanged()
            {
                var taskList = new TaskList(_taskList01);
                taskList.Append(_taskList02);
                int changeCount = 0;
                taskList.CollectionChanged += (sender, e) =>
                {
                    changeCount++;
                };
                taskList.Remove(_taskList01);
                const int expectedChangeCountAfterRemove = 1;
                Assert.AreEqual(expectedChangeCountAfterRemove, changeCount);
            }
        }

        [TestClass]
        public class TaskList_UnitTests_Event_CollectionChanged
        {
            private List<Task> _taskList01;
            private List<Task> _taskList02;

            [TestInitialize]
            public void Initialize()
            {
                _taskList01 = new List<Task>() {
                    new Task("(A) Call Mom @Phone +Family", 0)
                };
                _taskList02 = new List<Task>() {
                    new Task("(A) Schedule annual checkup +Health"), 
                    new Task("(B) Outline chapter 5 +Novel @Computer")
                };
            }

            [TestMethod]
            public void TaskList_CollectionChanged_WhenOneTaskIsReplaced_ShouldNotifyCollectionIsChangedOneTime()
            {
                var taskList = new TaskList(_taskList02);
                int changeCount = 0;
                taskList.CollectionChanged += (sender, e) =>
                {
                    changeCount++;
                };
                taskList[0] = new Task("completely new task");
                const int expectedChangeCountAfterChanges = 1;
                Assert.AreEqual(expectedChangeCountAfterChanges, changeCount);
            }

            [TestMethod]
            public void TaskList_CollectionChanged_WhenOneTaskIsModified_ShouldNotifyCollectionIsChangedOneTime()
            {
                var taskList = new TaskList(_taskList02);
                int changeCount = 0;
                taskList.CollectionChanged += (sender, e) =>
                {
                    changeCount++;
                };
                taskList[0].RawText = "completely new task";
                const int expectedChangeCountAfterChanges = 1;
                Assert.AreEqual(expectedChangeCountAfterChanges, changeCount);
            }

            [TestMethod]
            public void TaskList_CollectionChanged_WhenTwoTasksAreModified_ShouldNotifyCollectionIsChangedTwoTimes()
            {
                var taskList = new TaskList(_taskList02);
                int changeCount = 0;
                taskList.CollectionChanged += (sender, e) =>
                {
                    changeCount++;
                };
                taskList[0].RawText = "completely new task";
                taskList[1].RawText = "completely new task";
                const int expectedChangeCountAfterChanges = 2;
                Assert.AreEqual(expectedChangeCountAfterChanges, changeCount);
            }
        }

        [TestClass]
        public class TaskList_UnitTests_Method_ToString
        {
            [TestMethod]
            public void TaskList_Constructor_WhenStringArgument_CountShouldBeEqualTaskListCount()
            {
                var taskList = new TaskList();
                taskList.Add(new Task("(A) Call Mom @Phone +Family", 0));
                taskList.Add(new Task("(A) Schedule annual checkup +Health", 1));
                taskList.Add(new Task("(B) Outline chapter 5 +Novel @Computer", 2));
                var taskString =
                    "(A) Call Mom @Phone +Family" + Environment.NewLine +
                    "(A) Schedule annual checkup +Health" + Environment.NewLine +
                    "(B) Outline chapter 5 +Novel @Computer";
                Assert.AreEqual(taskString, taskList.ToString());
            }
        }

        [TestClass]
        public class TaskList_UnitTests_Method_UpdateSelectedTasks
        {
            private TaskList _taskList;
            private List<Task> _selectedTasks;
            private List<Task> _startingTaskList;

            [TestInitialize]
            public void Initialize()
            {
                _startingTaskList = new List<Task>() {
                    new Task("(A) Schedule annual checkup +Health", 0),
                    new Task("(B) Outline chapter 5 +Novel @Computer t:2020-01-01 due:2020-12-31", 1),
                    new Task("(C) Grocery shopping +Chores @Errands t:2021-01-01 due:2021-12-31", 2)
                };

                _selectedTasks = new List<Task>() {
                    new Task("(B) Outline chapter 5 +Novel @Computer t:2020-01-01 due:2020-12-31", 1),
                    new Task("(C) Grocery shopping +Chores @Errands t:2021-01-01 due:2021-12-31", 2)
                };

                _taskList = new TaskList(_startingTaskList);
            }

            [TestMethod]
            public void TaskList_UpdateSelectedTasks_ToggleCompletion_ShouldToggleCompletion()
            {
                var expectedResult = new TaskList(_startingTaskList);
                expectedResult[1].ToggleCompletion();
                expectedResult[2].ToggleCompletion();

                _taskList.UpdateSelectedTasks(_selectedTasks, TaskListUpdateCommand.ToggleCompletion);
                CollectionAssert.AreEquivalent(expectedResult, _taskList);
            }

            [TestMethod]
            public void TaskList_UpdateSelectedTasks_IncreasePriority_ShouldIncreasePriority()
            {
                var expectedResult = new TaskList(_startingTaskList);
                expectedResult[1].IncreasePriority();
                expectedResult[2].IncreasePriority();

                _taskList.UpdateSelectedTasks(_selectedTasks, TaskListUpdateCommand.IncreasePriority);
                CollectionAssert.AreEquivalent(expectedResult, _taskList);
            }

            [TestMethod]
            public void TaskList_UpdateSelectedTasks_DecreasePriority_ShouldDecreasePriority()
            {
                var expectedResult = new TaskList(_startingTaskList);
                expectedResult[1].DecreasePriority();
                expectedResult[2].DecreasePriority();

                _taskList.UpdateSelectedTasks(_selectedTasks, TaskListUpdateCommand.DecreasePriority);
                CollectionAssert.AreEquivalent(expectedResult, _taskList);
            }

            [TestMethod]
            public void TaskList_UpdateSelectedTasks_RemovePriority_ShouldRemovePriority()
            {
                var expectedResult = new TaskList(_startingTaskList);
                expectedResult[1].RemovePriority();
                expectedResult[2].RemovePriority();

                _taskList.UpdateSelectedTasks(_selectedTasks, TaskListUpdateCommand.RemovePriority);
                CollectionAssert.AreEquivalent(expectedResult, _taskList);
            }

            [TestMethod]
            public void TaskList_UpdateSelectedTasks_SetPriority_ShouldSetPriority()
            {
                var expectedResult = new TaskList(_startingTaskList);
                expectedResult[1].SetPriority('L');
                expectedResult[2].SetPriority('L');

                _taskList.UpdateSelectedTasks(_selectedTasks, TaskListUpdateCommand.SetPriority, 'L');
                CollectionAssert.AreEquivalent(expectedResult, _taskList);
            }

            [TestMethod]
            public void TaskList_UpdateSelectedTasks_IncrementDueDate_ShouldIncrementDueDate()
            {
                var expectedResult = new TaskList(_startingTaskList);
                expectedResult[1].IncrementDueDate(3);
                expectedResult[2].IncrementDueDate(3);

                _taskList.UpdateSelectedTasks(_selectedTasks, TaskListUpdateCommand.IncrementDueDate, 3);
                CollectionAssert.AreEquivalent(expectedResult, _taskList);
            }

            [TestMethod]
            public void TaskList_UpdateSelectedTasks_DecrementDueDate_ShouldDecrementDueDate()
            {
                var expectedResult = new TaskList(_startingTaskList);
                expectedResult[1].DecrementDueDate(3);
                expectedResult[2].DecrementDueDate(3);

                _taskList.UpdateSelectedTasks(_selectedTasks, TaskListUpdateCommand.DecrementDueDate, 3);
                CollectionAssert.AreEquivalent(expectedResult, _taskList);
            }

            [TestMethod]
            public void TaskList_UpdateSelectedTasks_SetDueDate_ShouldSetDueDate()
            {
                var expectedResult = new TaskList(_startingTaskList);
                expectedResult[1].SetDueDate(DateTime.Today);
                expectedResult[2].SetDueDate(DateTime.Today);

                _taskList.UpdateSelectedTasks(_selectedTasks, TaskListUpdateCommand.SetDueDate, DateTime.Today);
                CollectionAssert.AreEquivalent(expectedResult, _taskList);
            }

            [TestMethod]
            public void TaskList_UpdateSelectedTasks_RemoveDueDate_ShouldRemoveDueDate()
            {
                var expectedResult = new TaskList(_startingTaskList);
                expectedResult[1].RemoveDueDate();
                expectedResult[2].RemoveDueDate();

                _taskList.UpdateSelectedTasks(_selectedTasks, TaskListUpdateCommand.RemoveDueDate);
                CollectionAssert.AreEquivalent(expectedResult, _taskList);
            }

            [TestMethod]
            public void TaskList_UpdateSelectedTasks_IncrementThresholdDate_ShouldIncrementThresholdDate()
            {
                var expectedResult = new TaskList(_startingTaskList);
                expectedResult[1].IncrementThresholdDate(3);
                expectedResult[2].IncrementThresholdDate(3);

                _taskList.UpdateSelectedTasks(_selectedTasks, TaskListUpdateCommand.IncrementThresholdDate, 3);
                CollectionAssert.AreEquivalent(expectedResult, _taskList);
            }

            [TestMethod]
            public void TaskList_UpdateSelectedTasks_DecrementThresholdDate_ShouldDecrementThresholdDate()
            {
                var expectedResult = new TaskList(_startingTaskList);
                expectedResult[1].DecrementThresholdDate(3);
                expectedResult[2].DecrementThresholdDate(3);

                _taskList.UpdateSelectedTasks(_selectedTasks, TaskListUpdateCommand.DecrementThresholdDate, 3);
                CollectionAssert.AreEquivalent(expectedResult, _taskList);
            }

            [TestMethod]
            public void TaskList_UpdateSelectedTasks_SetThresholdDate_ShouldSetThresholdDate()
            {
                var expectedResult = new TaskList(_startingTaskList);
                expectedResult[1].SetThresholdDate(DateTime.Today);
                expectedResult[2].SetThresholdDate(DateTime.Today);

                _taskList.UpdateSelectedTasks(_selectedTasks, TaskListUpdateCommand.SetThresholdDate, DateTime.Today);
                CollectionAssert.AreEquivalent(expectedResult, _taskList);
            }

            [TestMethod]
            public void TaskList_UpdateSelectedTasks_RemoveThresholdDate_ShouldRemoveThresholdDate()
            {
                var expectedResult = new TaskList(_startingTaskList);
                expectedResult[1].RemoveThresholdDate();
                expectedResult[2].RemoveThresholdDate();

                _taskList.UpdateSelectedTasks(_selectedTasks, TaskListUpdateCommand.RemoveThresholdDate);
                CollectionAssert.AreEquivalent(expectedResult, _taskList);
            }

            [TestMethod]
            public void TaskList_UpdateSelectedTasks_AppendText_ShouldAppendText()
            {
                var expectedResult = new TaskList(_startingTaskList);
                var textToAppend = "additional text";
                expectedResult[1].AppendText(textToAppend);
                expectedResult[2].AppendText(textToAppend);

                _taskList.UpdateSelectedTasks(_selectedTasks, TaskListUpdateCommand.AppendText, textToAppend);
                CollectionAssert.AreEquivalent(expectedResult, _taskList);
            }

            [TestMethod]
            public void TaskList_UpdateSelectedTasks_PrependText_ShouldPrependText()
            {
                var expectedResult = new TaskList(_startingTaskList);
                var textToPrepend = "additional text";
                expectedResult[1].PrependText(textToPrepend);
                expectedResult[2].PrependText(textToPrepend);

                _taskList.UpdateSelectedTasks(_selectedTasks, TaskListUpdateCommand.PrependText, textToPrepend);
                CollectionAssert.AreEquivalent(expectedResult, _taskList);
            }
        }
    }
}
