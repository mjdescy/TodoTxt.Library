using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TodoTxt.Library;

namespace TodoTxt.Library.UnitTests
{
    [TestClass]
    public class ItemsChangedObservableCollection_UnitTests
    {
        [TestClass]
        public class ItemsChangedObservableCollection_UnitTests_Method_Constructor
        {
            [TestMethod]
            public void ItemsChangedObservableCollection_Constructor_WhenNoArguments_CountShouldBeZero()
            {
                var taskList = new ItemsChangedObservableCollection<Task>();
                Assert.AreEqual(0, taskList.Count);
            }
        }

        [TestClass]
        public class TaskList_UnitTests_Method_ReplaceItem
        {
            private ItemsChangedObservableCollection<Task> _taskList;
            private List<Task> _startingTaskList;
            private Task _itemToReplace;
            private Task _replacementItem;
            private List<Task> _expectedResult;

            [TestInitialize]
            public void Initialize()
            {
                _startingTaskList = new List<Task>() {
                    new Task("(A) Schedule annual checkup +Health"),
                    new Task("(B) Outline chapter 5 +Novel @Computer"),
                    new Task("(C) Grocery shopping +Chores @Errands")
                };

                _itemToReplace = new Task("(B) Outline chapter 5 +Novel @Computer");

                _replacementItem = new Task("x 2016-01-01 Outline chapter 5 +Novel @Computer");

                _expectedResult = new List<Task>() {
                    new Task("(A) Schedule annual checkup +Health"),
                    new Task("x 2016-01-01 Outline chapter 5 +Novel @Computer"),
                    new Task("(C) Grocery shopping +Chores @Errands")
                };

                _taskList = new ItemsChangedObservableCollection<Task>();
                foreach (var t in _startingTaskList)
                {
                    _taskList.Add(t);
                }
            }

            [TestMethod]
            public void TaskList_UnitTests_Method_ReplaceItem_ShouldReplaceItem()
            {
                _taskList.ReplaceItem(_itemToReplace, _replacementItem);
                CollectionAssert.AreEquivalent(_taskList, _expectedResult);
            }

            [TestMethod]
            public void TaskList_UnitTests_Method_ReplaceItem_ShouldNotifyCollectionChangedOnce()
            {
                int changeCount = 0;
                _taskList.CollectionChanged += (sender, e) =>
                {
                    changeCount++;
                };

                _taskList.ReplaceItem(_itemToReplace, _replacementItem);
                const int expectedChangeCountAfterRemove = 1;
                Assert.AreEqual(expectedChangeCountAfterRemove, changeCount);
            }
        }

        [TestClass]
        public class TaskList_UnitTests_Method_ReplaceItems
        {
            private ItemsChangedObservableCollection<Task> _taskList;
            private List<Task> _startingTaskList;
            private List<Task> _itemsToReplace;
            private List<Task> _replacementItems;
            private List<Task> _expectedResult;

            [TestInitialize]
            public void Initialize()
            {
                _startingTaskList = new List<Task>() {
                    new Task("(A) Schedule annual checkup +Health"), 
                    new Task("(B) Outline chapter 5 +Novel @Computer"),
                    new Task("(C) Grocery shopping +Chores @Errands")
                };

                _itemsToReplace = new List<Task>() {
                    new Task("(A) Schedule annual checkup +Health"), 
                    new Task("(B) Outline chapter 5 +Novel @Computer"),
                };

                _replacementItems = new List<Task>() {
                    new Task("x 2016-01-01 Schedule annual checkup +Health"), 
                    new Task("x 2016-01-01 Outline chapter 5 +Novel @Computer")
                };

                _expectedResult = new List<Task>() {
                    new Task("x 2016-01-01 Schedule annual checkup +Health"), 
                    new Task("x 2016-01-01 Outline chapter 5 +Novel @Computer"),
                    new Task("(C) Grocery shopping +Chores @Errands")
                };

                _taskList = new ItemsChangedObservableCollection<Task>();
                foreach (var t in _startingTaskList)
                {
                    _taskList.Add(t);
                }
            } 

            [TestMethod]
            public void TaskList_UnitTests_Method_ReplaceItems_ShouldReplaceItems()
            {
                _taskList.ReplaceItems(_itemsToReplace, _replacementItems);
                CollectionAssert.AreEquivalent(_expectedResult, _taskList);
            }

            [TestMethod]
            public void TaskList_UnitTests_Method_ReplaceItems_ShouldNotifyCollectionChangedOnce()
            {
                int changeCount = 0;
                _taskList.CollectionChanged += (sender, e) =>
                {
                    changeCount++;
                };

                _taskList.ReplaceItems(_itemsToReplace, _replacementItems);
                const int expectedChangeCountAfterRemove = 1;
                Assert.AreEqual(expectedChangeCountAfterRemove, changeCount);
            }
        }
    }
}
