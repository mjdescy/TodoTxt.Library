using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TodoTxt.Library;

namespace TodoTxt.Library.UnitTests
{
    [TestClass]
    public class Task_UnitTests
    {
        [TestClass]
        public class Task_UnitTests_Method_Constructor
        {
            [TestMethod]
            public void Task_Constructor_WhenNoArguments_RawTextShouldBeBlank()
            {
                var task = new Task();
                Assert.AreEqual(String.Empty, task.RawText);
            }

            [TestMethod]
            public void Task_Constructor_WhenNoArguments_IDShouldBeNull()
            {
                var task = new Task();
                Assert.IsNull(task.ID);
            }

            [TestMethod]
            public void Task_Constructor_WhenRawTextArgumentOnly_RawTextShouldBeRawText()
            {
                var task = new Task("(A) This is a test task");
                Assert.AreEqual("(A) This is a test task", task.RawText);
            }

            [TestMethod]
            public void Task_Constructor_WhenRawTextArgumentOnly_IDShouldBeNull()
            {
                var task = new Task("(A) This is a test task");
                Assert.IsNull(task.ID);
            }

            [TestMethod]
            public void Task_Constructor_WhenhRawTextAndIDArgumentsOnly_RawTextShouldBeRawText()
            {
                var task = new Task("(A) This is a test task", 50);
                Assert.AreEqual("(A) This is a test task", task.RawText);
            }

            [TestMethod]
            public void Task_Constructor_WhenhRawTextAndIDArgumentsOnly_IDShouldBeID()
            {
                var task = new Task("(A) This is a test task", 50);
                Assert.AreEqual(50, task.ID);
            }

            [TestMethod]
            public void Task_Constructor_WithRawTextAndIDAndPrependedDate_WithPriority()
            {
                var task = new Task("(A) This is a test task", 50, "2013-12-31");
                Assert.AreEqual("(A) 2013-12-31 This is a test task", task.RawText);
                Assert.AreEqual(50, task.ID);
            }

        }

        [TestClass]
        public class Task_UnitTests_Method_CompareTo
        {
            [TestMethod]
            public void Task_CompareTo_TaskWithGreaterRawTextAndNoID()
            {
                var task1 = new Task("(A) this is a test task");
                var task2 = new Task("(B) this is a test task");
                Assert.IsTrue(task1.CompareTo(task2) < 0);
            }

            [TestMethod]
            public void Task_CompareTo_TaskWithGreaterRawTextAndSameID()
            {
                var task1 = new Task("(A) this is a test task", 50);
                var task2 = new Task("(B) this is a test task", 50);
                Assert.IsTrue(task1.CompareTo(task2) < 0);
            }

            [TestMethod]
            public void Task_CompareTo_TaskWithSameRawTextAndSameID()
            {
                var task1 = new Task("(A) this is a test task", 50);
                var task2 = new Task("(A) this is a test task", 50);
                Assert.IsTrue(task1.CompareTo(task2) == 0);
            }

            [TestMethod]
            public void Task_CompareTo_Task_CompareTo_TaskWithLesserRawTextAndSameID()
            {
                var task1 = new Task("(B) this is a test task", 50);
                var task2 = new Task("(A) this is a test task", 50);
                Assert.IsTrue(task1.CompareTo(task2) > 0);
            }

            [TestMethod]
            public void Task_CompareTo_Task_CompareTo_TaskWithUppercaseRawTextSameID()
            {
                var task1 = new Task("(A) this is a test task", 50);
                var task2 = new Task("(A) THIS IS A TEST TASK", 50);
                Assert.IsTrue(task1.CompareTo(task2) == 0);
            }

            [TestMethod]
            public void Task_CompareTo_TaskWithSameRawTextAndGreaterID()
            {
                var task1 = new Task("(A) this is a test task", 50);
                var task2 = new Task("(A) this is a test task", 100);
                Assert.IsTrue(task1.CompareTo(task2) < 0);
            }

            [TestMethod]
            public void Task_CompareTo_TaskWithSameRawTextAndLesserID()
            {
                var task1 = new Task("(A) this is a test task", 50);
                var task2 = new Task("(A) THIS IS A TEST TASK", 0);
                Assert.IsTrue(task1.CompareTo(task2) > 0);
            }

            [TestMethod]
            public void Task_CompareTo_Task_CompareTo_TaskWithUppercaseRawTextAndGreaterID()
            {
                var task1 = new Task("(A) this is a test task", 50);
                var task2 = new Task("(A) THIS IS A TEST TASK", 100);
                Assert.IsTrue(task1.CompareTo(task2) < 0);
            }

            [TestMethod]
            public void Task_CompareTo_Task_CompareTo_TaskWithUppercaseRawTextAndLesserID()
            {
                var task1 = new Task("(A) this is a test task", 50);
                var task2 = new Task("(A) THIS IS A TEST TASK", 0);
                Assert.IsTrue(task1.CompareTo(task2) > 0);
            }
        }

        [TestClass]
        public class Task_UnitTests_Method_Equals
        {
            [TestMethod]
            public void Task_Equals_TaskWithSameRawTextAndSameID_ShouldBeEqual()
            {
                var task1 = new Task("(A) this is a test task", 50);
                var task2 = new Task("(A) this is a test task", 50);
                Assert.IsTrue(task1 == task2);
            }

            [TestMethod]
            public void Task_Equals_TaskWithSameRawTextAndDifferentID_ShouldBeEqual()
            {
                var task1 = new Task("(A) this is a test task");
                var task2 = new Task("(A) this is a test task", 50);
                Assert.IsTrue(task1 != task2);
            }

            [TestMethod]
            public void Task_Equals_TaskWithCapitalizedRawTextAndSameID_ShouldBeNotEqual()
            {
                var task1 = new Task("(A) this is a test task", 50);
                var task2 = new Task("(A) THIS IS A TEST TASK", 50);
                Assert.IsTrue(task1 != task2);
            }
        }

        [TestClass]
        public class Task_UnitTests_Property_RawText
        {
            [TestMethod]
            public void Task_RawText_WhenRawTextIsNotBlank_ShouldEqualRawText()
            {
                var task = new Task();
                task.RawText = "(A) This is a test task";
                Assert.AreEqual("(A) This is a test task", task.RawText);
            }

            [TestMethod]
            public void Task_RawText_WhenRawTextIsBlank_ShouldBeBlank()
            {
                var task = new Task("(A) This is a test task");
                task.RawText = String.Empty;
                Assert.AreEqual(String.Empty, task.RawText);
            }

            [TestMethod]
            public void Task_RawText_WhenPropertyIsChanged_ShouldNotifyPropertyChanged()
            {
                var task = new Task("(A) This is a test task");
                List<String> changedProperties = new List<string>();
                task.PropertyChanged += (sender, e) =>
                {
                    changedProperties.Add(e.PropertyName);
                };
                task.RawText = "(B) This is a test task";
                CollectionAssert.Contains(changedProperties, "RawText");
            }
        }

        [TestClass]
        public class Task_UnitTests_Property_ID
        {
            [TestMethod]
            public void Task_ID_WhenAssignedInConstructor_ShouldEqual50()
            {
                var task = new Task("(A) This is a test task", 50);
                Assert.AreEqual(50, task.ID);
            }

            [TestMethod]
            public void Task_ID_WhenAssignedSubsequentToConstructor_ShouldEqual50()
            {
                var task = new Task("(A) This is a test task");
                task.ID = 50;
                Assert.AreEqual(50, task.ID);
            }

            [TestMethod]
            public void Task_ID_WhenNotAssigned_ShouldBeNull()
            {
                var task = new Task("(A) This is a test task");
                Assert.IsNull(task.ID);
            }

            [TestMethod]
            public void Task_ID_WhenPropertyIsChanged_ShouldNotifyPropertyChanged()
            {
                var task = new Task("(A) This is a test task");
                List<String> changedProperties = new List<string>();
                task.PropertyChanged += (sender, e) =>
                {
                    changedProperties.Add(e.PropertyName);
                };
                task.ID = 100;
                CollectionAssert.Contains(changedProperties, "ID");
            }
        }

        [TestClass]
        public class Task_UnitTests_Property_IsBlank
        {
            [TestMethod]
            public void Task_IsBlank_WhenRawTextIsBlank_ShouldBeTrue()
            {
                var task = new Task(String.Empty);
                Assert.IsTrue(task.IsBlank);
            }

            [TestMethod]
            public void Task_IsBlank_WhenRawTextIsNotBlank_ShouldBeFalse()
            {
                var task = new Task("(A) This is a test task");
                Assert.AreEqual(task.IsBlank, false);
            }

            [TestMethod]
            public void Task_IsBlank_WhenPropertyIsChanged_ShouldNotifyPropertyChanged()
            {
                var task = new Task("(A) This is a test task");
                List<String> changedProperties = new List<string>();
                task.PropertyChanged += (sender, e) =>
                {
                    changedProperties.Add(e.PropertyName);
                };
                task.RawText = String.Empty;
                CollectionAssert.Contains(changedProperties, "IsBlank");
            }
        }

        [TestClass]
        public class Task_UnitTests_Property_IsCompleted
        {
            [TestMethod]
            public void Task_IsCompleted_WhenRawTextHasNoLeadingX_ShouldBeFalse()
            {
                var task = new Task("(A) This is a test task");
                Assert.IsFalse(task.IsCompleted);
            }

            [TestMethod]
            public void Task_IsCompleted_WhenRawTextHasUppercaseLeadingX_ShouldBeFalse()
            {
                var task = new Task("X 2015-12-31 This is a test task");
                Assert.IsFalse(task.IsCompleted);
            }

            [TestMethod]
            public void Task_IsCompleted_WhenRawTextHasLeadingLowercaseXAndNoCompletionDate_ShouldBeFalse()
            {
                var task = new Task("x This is a test task");
                Assert.IsFalse(task.IsCompleted);
            }

            [TestMethod]
            public void Task_IsCompleted_WhenRawTextHasLeadingXAndCompletionDate_ShouldBeTrue()
            {
                var task = new Task("x 2015-12-31 This is a test task");
                Assert.IsTrue(task.IsCompleted);
            }

            [TestMethod]
            public void Task_IsCompleted_WhenRawTextHasLeadingXAndCompletionDateAndPriority_ShouldBeTrue()
            {
                var task = new Task("x 2015-12-31 (A) This is a test task");
                Assert.IsTrue(task.IsCompleted);
            }

            [TestMethod]
            public void Task_IsCompleted_WhenRawTextIsBlank_ShouldBeFalse()
            {
                var task = new Task(String.Empty);
                Assert.IsFalse(task.IsCompleted);
            }

            [TestMethod]
            public void Task_IsCompleted_WhenPropertyIsChanged_ShouldNotifyPropertyChanged()
            {
                var task = new Task("(A) This is a test task");
                List<String> changedProperties = new List<string>();
                task.PropertyChanged += (sender, e) =>
                {
                    changedProperties.Add(e.PropertyName);
                };
                task.RawText = "x 2015-12-31 This is a test task";
                CollectionAssert.Contains(changedProperties, "IsCompleted");
            }
        }

        [TestClass]
        public class Task_UnitTests_Property_IsPrioritized
        {
            [TestMethod]
            public void Task_IsPrioritized_WhenPriorityIsUppercaseA_ShouldBeTrue()
            {
                var task = new Task("(A) This is a test task");
                Assert.IsTrue(task.IsPrioritized);
            }

            [TestMethod]
            public void Task_IsPrioritized_WhenPriorityIsLowercaseA_ShouldBeFalse()
            {
                var task = new Task("(a) This is a test task");
                Assert.IsFalse(task.IsPrioritized);
            }

            [TestMethod]
            public void Task_IsPrioritized_WhenPriorityIsNotAnUppercaseLetter_ShouldBeFalse()
            {
                var task = new Task("(1) This is a test task");
                Assert.IsFalse(task.IsPrioritized);
            }

            [TestMethod]
            public void Task_IsPrioritized_WhenNoPriority_ShouldBeFalse()
            {
                var task = new Task("This is a test task");
                Assert.IsFalse(task.IsPrioritized);
            }

            [TestMethod]
            public void Task_IsPrioritized_WhenRawTextIsBlank_ShouldBeFalse()
            {
                var task = new Task(String.Empty);
                Assert.IsFalse(task.IsPrioritized);
            }

            [TestMethod]
            public void Task_IsPrioritized_WhenPropertyIsChanged_ShouldNotifyPropertyChanged()
            {
                var task = new Task("(A) This is a test task");
                List<String> changedProperties = new List<string>();
                task.PropertyChanged += (sender, e) =>
                {
                    changedProperties.Add(e.PropertyName);
                };
                task.RawText = "This is a test task";
                CollectionAssert.Contains(changedProperties, "IsPrioritized");
            }
        }

        [TestClass]
        public class Task_UnitTests_Property_PriorityText
        {
            [TestMethod]
            public void Task_PriorityText_WhenPriorityIsUppercaseA_ShouldBeA()
            {
                var task = new Task("(A) This is a test task");
                Assert.AreEqual("(A)", task.PriorityText);
            }

            [TestMethod]
            public void Task_PriorityText_WhenPriorityIsLowercaseA_ShouldBeBlank()
            {
                var task = new Task("(a) This is a test task");
                Assert.AreEqual(String.Empty, task.PriorityText);
            }

            [TestMethod]
            public void Task_PriorityText_WhenPriorityIsNotAnUppercaseLetter_ShouldBeBlank()
            {
                var task = new Task("(1) This is a test task");
                Assert.AreEqual(String.Empty, task.PriorityText);
            }

            [TestMethod]
            public void Task_PriorityText_WhenPriorityIsNotAtBeginningOfRawText_ShouldBeBlank()
            {
                var task = new Task(" (A) This is a test task");
                Assert.AreEqual(String.Empty, task.PriorityText);
            }

            [TestMethod]
            public void Task_PriorityText_WhenPriorityHasNoSpaceAfter_ShouldBeBlank()
            {
                var task = new Task("(A)This is a test task");
                Assert.AreEqual(String.Empty, task.PriorityText);
            }

            [TestMethod]
            public void Task_PriorityText_WhenNoPriority_ShouldBeBlank()
            {
                var task = new Task("This is a test task");
                Assert.AreEqual(String.Empty, task.PriorityText);
            }

            [TestMethod]
            public void Task_PriorityText_WhenRawTextIsBlank_ShouldBeBlank()
            {
                var task = new Task(String.Empty);
                Assert.AreEqual(String.Empty, task.PriorityText);
            }

            [TestMethod]
            public void Task_PriorityText_WhenPropertyIsChanged_ShouldNotifyPropertyChanged()
            {
                var task = new Task("(A) This is a test task");
                List<String> changedProperties = new List<string>();
                task.PropertyChanged += (sender, e) =>
                {
                    changedProperties.Add(e.PropertyName);
                };
                task.RawText = "(B) This is a test task";
                CollectionAssert.Contains(changedProperties, "PriorityText");
            }
        }

        [TestClass]
        public class Task_UnitTests_Property_Priority
        {
            [TestMethod]
            public void Task_Priority_WhenPriorityIsUppercaseA_ShouldBeA()
            {
                var task = new Task("(A) This is a test task");
                Assert.AreEqual('A', task.Priority);
            }

            [TestMethod]
            public void Task_Priority_WhenPriorityIsLowercaseA_ShouldBeTilde()
            {
                var task = new Task("(a) This is a test task");
                Assert.AreEqual('~', task.Priority);
            }

            [TestMethod]
            public void Task_Priority_WhenPriorityIsNotAnUppercaseLetter_ShouldBeTilde()
            {
                var task = new Task("(1) This is a test task");
                Assert.AreEqual('~', task.Priority);
            }

            [TestMethod]
            public void Task_Priority_WhenPriorityIsNotAtBeginningOfRawText_ShouldBeTilde()
            {
                var task = new Task(" (A) This is a test task");
                Assert.AreEqual('~', task.Priority);
            }

            [TestMethod]
            public void Task_Priority_WhenPriorityHasNoSpaceAfter_ShouldBeTilde()
            {
                var task = new Task("(A)This is a test task");
                Assert.AreEqual('~', task.Priority);
            }

            [TestMethod]
            public void Task_Priority_WhenNoPriority_ShouldBeTilde()
            {
                var task = new Task("This is a test task");
                Assert.AreEqual('~', task.Priority);
            }

            [TestMethod]
            public void Task_Priority_WhenRawTextIsBlank_ShouldBeTilde()
            {
                var task = new Task(String.Empty);
                Assert.AreEqual('~', task.Priority);
            }

            [TestMethod]
            public void Task_Priority_WhenPropertyIsChanged_ShouldNotifyPropertyChanged()
            {
                var task = new Task("(A) This is a test task");
                List<String> changedProperties = new List<string>();
                task.PropertyChanged += (sender, e) =>
                {
                    changedProperties.Add(e.PropertyName);
                };
                task.RawText = "(B) This is a test task";
                CollectionAssert.Contains(changedProperties, "Priority");
            }
        }

        [TestClass]
        public class Task_UnitTests_Property_HasProjects
        {
            [TestMethod]
            public void Task_HasProjects_WhenSingleProject_ShouldBeTrue()
            {
                var task = new Task("(A) This is a test task +Project1");
                Assert.IsTrue(task.HasProjects);
            }

            [TestMethod]
            public void Task_HasProjects_WhenMultipleProjects_ShouldBeTrue()
            {
                var task = new Task("(A) This +Project1 is a test task +Project2");
                Assert.IsTrue(task.HasProjects);
            }

            [TestMethod]
            public void Task_HasProjects_WhenNoProject_ShouldBeFalse()
            {
                var task = new Task("(A) This is a test task");
                Assert.IsFalse(task.HasProjects);
            }

            [TestMethod]
            public void Task_HasProjects_WhenPropertyIsChanged_ShouldNotifyPropertyChanged()
            {
                var task = new Task("(A) This is a test task");
                List<String> changedProperties = new List<string>();
                task.PropertyChanged += (sender, e) =>
                {
                    changedProperties.Add(e.PropertyName);
                };
                task.RawText = "(A) This is a test task +Project1";
                CollectionAssert.Contains(changedProperties, "HasProjects");
            }
        }

        [TestClass]
        public class Task_UnitTests_Property_Projects
        {
            [TestMethod]
            public void Task_Projects_WhenSingleProject_ShouldContainSingleProject()
            {
                var task = new Task("(A) This is a test task +Project1");
                CollectionAssert.AreEqual(new List<string>() { "+Project1" }, task.Projects);
            }

            [TestMethod]
            public void Task_Projects_WhenMultipleProjects_ShouldContainAllProjectsInOrderInRawText()
            {
                var task = new Task("(A) This +Project2 is a test task +Project1");
                CollectionAssert.AreEqual(new List<string>() { "+Project2", "+Project1" }, task.Projects);
            }

            [TestMethod]
            public void Task_Projects_WhenNoProject_ShouldBeEmptyList()
            {
                var task = new Task("(A) This is a test task");
                CollectionAssert.AreEqual(new List<string>(), task.Projects);
            }

            [TestMethod]
            public void Task_Projects_WhenPropertyIsChanged_ShouldNotifyPropertyChanged()
            {
                var task = new Task("(A) This is a test task +Project1");
                List<String> changedProperties = new List<string>();
                task.PropertyChanged += (sender, e) =>
                {
                    changedProperties.Add(e.PropertyName);
                };
                task.RawText = "(A) This is a test task +Project1 +Project2";
                CollectionAssert.Contains(changedProperties, "Projects");
            }
        }

        [TestClass]
        public class Task_UnitTests_Property_HasContexts
        {
            [TestMethod]
            public void Task_HasContexts_WhenSingleContext_ShouldBeTrue()
            {
                var task = new Task("(A) This is a test task @Context1");
                Assert.IsTrue(task.HasContexts);
            }

            [TestMethod]
            public void Task_HasContexts_WhenMultipleContexts_ShouldBeTrue()
            {
                var task = new Task("(A) This @Context1 is a test task @Context2");
                Assert.IsTrue(task.HasContexts);
            }

            [TestMethod]
            public void Task_HasContexts_WhenNoContext_ShouldBeFalse()
            {
                var task = new Task("(A) This is a test task");
                Assert.IsFalse(task.HasContexts);
            }

            [TestMethod]
            public void Task_HasContexts_WhenPropertyIsChanged_ShouldNotifyPropertyChanged()
            {
                var task = new Task("(A) This is a test task");
                List<String> changedProperties = new List<string>();
                task.PropertyChanged += (sender, e) =>
                {
                    changedProperties.Add(e.PropertyName);
                };
                task.RawText = "(A) This is a test task @Context1";
                CollectionAssert.Contains(changedProperties, "HasContexts");
            }
        }

        [TestClass]
        public class Task_UnitTests_Property_Contexts
        {
            [TestMethod]
            public void Task_Contexts_WhenSingleContext_ShouldContainSingleContext()
            {
                var task = new Task("(A) This is a test task @Context1");
                CollectionAssert.AreEqual(new List<string>() { "@Context1" }, task.Contexts);
            }

            [TestMethod]
            public void Task_Contexts_WhenMultipleContexts_ShouldContainAllContextsInOrderInRawText()
            {
                var task = new Task("(A) This @Context2 is a test task @Context1");
                CollectionAssert.AreEqual(new List<string>() { "@Context2", "@Context1" }, task.Contexts);
            }

            [TestMethod]
            public void Task_Contexts_WhenNoContext_ShouldBeEmptyList()
            {
                var task = new Task("(A) This is a test task");
                CollectionAssert.AreEqual(new List<string>(), task.Projects);
            }

            [TestMethod]
            public void Task_Contexts_WhenPropertyIsChanged_ShouldNotifyPropertyChanged()
            {
                var task = new Task("(A) This is a test task @Context1");
                List<String> changedProperties = new List<string>();
                task.PropertyChanged += (sender, e) =>
                {
                    changedProperties.Add(e.PropertyName);
                };
                task.RawText = "(A) This is a test task @Context1 @Context2";
                CollectionAssert.Contains(changedProperties, "Contexts");
            }
        }

        [TestClass]
        public class Task_UnitTests_Property_DueDateText
        {
            [TestMethod]
            public void Task_DueDateText_WhenTaskContainsDueDate_ShouldBeDueDate()
            {
                var task = new Task("(A) test task due:2015-12-31");
                Assert.AreEqual("2015-12-31", task.DueDateText);
            }

            [TestMethod]
            public void Task_DueDateText_WhenTaskContainsMulitpleDueDates_ShouldBeFirstDueDate()
            {
                var task = new Task("(A) test task due:2015-12-31 +Project1 due:2016-06-30");
                Assert.AreEqual("2015-12-31", task.DueDateText);
            }

            [TestMethod]
            public void Task_DueDateText_WhenTaskContainsNoDueDate_ShouldBeBlank()
            {
                var task = new Task("(A) test task +Project1");
                Assert.AreEqual(String.Empty, task.DueDateText);
            }

            [TestMethod]
            public void Task_DueDateText_WhenDueDateMissingLeadingSpace_ShouldBeBlank()
            {
                var task = new Task("(A) test taskdue:2015-12-31 +Project1");
                Assert.AreEqual(String.Empty, task.DueDateText);
            }

            [TestMethod]
            public void Task_DueDateText_WhenDueDateMissingTrailingSpace_ShouldBeBlank()
            {
                var task = new Task("(A) test task due:2015-12-31+Project1");
                Assert.AreEqual(String.Empty, task.DueDateText);
            }

            [TestMethod]
            public void Task_DueDateText_WhenImproperlyFormattedDueDate_ShouldBeBlank()
            {
                var task = new Task("(A) test task due:12/31/2015 +Project1");
                Assert.AreEqual(String.Empty, task.DueDateText);
            }

            [TestMethod]
            public void Task_DueDateText_WhenDueDateMonthInvalid_ShouldBeBlank()
            {
                var task = new Task("(A) test task due:9999-13-31 +Project1");
                Assert.AreEqual(String.Empty, task.DueDateText);
            }

            [TestMethod]
            public void Task_DueDateText_WhenDueDateDayInvalid_ShouldBeBlank()
            {
                var task = new Task("(A) test task due:9999-12-32 +Project1");
                Assert.AreEqual(String.Empty, task.DueDateText);
            }

            [TestMethod]
            public void Task_DueDateText_WhenThresholdDateMonthInvalid_ShouldBeBlank()
            {
                var task = new Task("(A) test task due:9999-13-31 +Project1");
                Assert.AreEqual(String.Empty, task.DueDateText);
            }

            [TestMethod]
            public void Task_DueDateText_WhenThresholdDateDayInvalid_ShouldBeBlank()
            {
                var task = new Task("(A) test task due:9999-12-32 +Project1");
                Assert.AreEqual(String.Empty, task.DueDateText);
            }

            [TestMethod]
            public void Task_DueDateText_WhenDueTagIsCaplitalized_ShouldBeBlank()
            {
                var task = new Task("(A) test task DUE:9999-12-31 +Project1");
                Assert.AreEqual(String.Empty, task.DueDateText);
            }

            [TestMethod]
            public void Task_DueDateText_WhenRelativeDueDateText_ShouldBeBlank()
            {
                var task = new Task("(A) test task due:Monday +Project1");
                Assert.AreEqual(String.Empty, task.DueDateText);
            }

            [TestMethod]
            public void Task_DueDateText_WhenTaskIsOnlyDueDate_ShouldBeDueDate()
            {
                var task = new Task("due:2015-12-31");
                Assert.AreEqual("2015-12-31", task.DueDateText);
            }

            [TestMethod]
            public void Task_DueDateText_WhenPropertyIsChanged_ShouldNotifyPropertyChanged()
            {
                var task = new Task("(A) This is a test task due:2015-12-31");
                List<String> changedProperties = new List<string>();
                task.PropertyChanged += (sender, e) =>
                {
                    changedProperties.Add(e.PropertyName);
                };
                task.RawText = "(A) This is a test task due:2016-01-01";
                CollectionAssert.Contains(changedProperties, "DueDateText");
            }
        }

        [TestClass]
        public class Task_UnitTests_Property_CreationDateText
        {
            [TestMethod]
            public void Task_CreationDateText_WhenIncompleteWithCreationDateAndNoPriority_ShouldBeCreationDate()
            {
                var task = new Task("2015-12-01 test task due:2015-12-31");
                Assert.AreEqual("2015-12-01", task.CreationDateText);
            }

            [TestMethod]
            public void Task_CreationDateText_WhenIncompleteWithCreationDateAndPriority_ShouldBeCreationDate()
            {
                var task = new Task("(A) 2015-12-01 test task due:2015-12-31");
                Assert.AreEqual("2015-12-01", task.CreationDateText);
            }

            [TestMethod]
            public void Task_CreationDateText_WhenIncompleteWithNoCreationDate_ShouldBeBlank()
            {
                var task = new Task("(A) test task due:2015-12-31");
                Assert.AreEqual(String.Empty, task.CreationDateText);
            }

            [TestMethod]
            public void Task_CreationDateText_WhenCompleteWithCreationDateWithNoLeadingSpace_ShouldBeBlank()
            {
                var task = new Task("x 2015-12-312015-12-01 test task due:2015-12-31");
                Assert.AreEqual(String.Empty, task.CreationDateText);
            }

            [TestMethod]
            public void Task_CreationDateText_WhenCompleteWithCreationDateWithNoTrailingSpace_ShouldBeBlank()
            {
                var task = new Task("x 2015-12-31 2015-12-01test task due:2015-12-31");
                Assert.AreEqual(String.Empty, task.CreationDateText);
            }

            [TestMethod]
            public void Task_CreationDateText_WhenIncompleteWithCreationDateWithNoLeadingSpace_ShouldBeBlank()
            {
                var task = new Task("(A)2015-12-01 test task due:2015-12-31");
                Assert.AreEqual(String.Empty, task.CreationDateText);
            }

            [TestMethod]
            public void Task_CreationDateText_WhenIncompleteWithCreationDateWithNoTrailingSpace_ShouldBeBlank()
            {
                var task = new Task("(A) 2015-12-01test task due:2015-12-31");
                Assert.AreEqual(String.Empty, task.CreationDateText);
            }

            [TestMethod]
            public void Task_CreationDateText_WhenCompleteWithCreationDate_ShouldBeCreationDate()
            {
                var task = new Task("x 2013-12-31 2015-12-01 test task due:2015-12-31");
                Assert.AreEqual("2015-12-01", task.CreationDateText);
            }

            [TestMethod]
            public void Task_CreationDateText_WhenCompleteWithNoCreationDate_ShouldBeBlank()
            {
                var task = new Task("x 2015-12-31 test task due:2015-12-31");
                Assert.AreEqual(String.Empty, task.CreationDateText);
            }
        }

        [TestClass]
        public class Task_UnitTests_Property_CreationDate
        {
            [TestMethod]
            public void Task_CreationDate_WhenIncompleteWithCreationDateAndNoPriority_ShouldBeCreationDate()
            {
                var task = new Task("2015-12-01 test task due:2015-12-31");
                Assert.AreEqual(new DateTime(2015, 12, 01), task.CreationDate);
            }

            [TestMethod]
            public void Task_CreationDate_WhenIncompleteWithCreationDateAndPriority_ShouldBeCreationDate()
            {
                var task = new Task("(A) 2015-12-01 test task due:2015-12-31");
                Assert.AreEqual(new DateTime(2015, 12, 01), task.CreationDate);
            }

            [TestMethod]
            public void Task_CreationDate_WhenIncompleteWithNoCreationDate_ShouldBeHighDate()
            {
                var task = new Task("(A) test task due:2015-12-31");
                Assert.AreEqual(new DateTime(9999, 12, 31), task.CreationDate);
            }

            [TestMethod]
            public void Task_CreationDate_WhenCompleteWithCreationDateWithNoLeadingSpace_ShouldBeHighDate()
            {
                var task = new Task("x 2015-12-312015-12-01 test task due:2015-12-31");
                Assert.AreEqual(new DateTime(9999, 12, 31), task.CreationDate);                
            }

            [TestMethod]
            public void Task_CreationDate_WhenCompleteWithCreationDateWithNoTrailingSpace_ShouldBeHighDate()
            {
                var task = new Task("x 2015-12-31 2015-12-01test task due:2015-12-31");
                Assert.AreEqual(new DateTime(9999, 12, 31), task.CreationDate);
            }

            [TestMethod]
            public void Task_CreationDate_WhenIncompleteWithCreationDateWithNoLeadingSpace_ShouldBeHighDate()
            {
                var task = new Task("(A)2015-12-01 test task due:2015-12-31");
                Assert.AreEqual(new DateTime(9999, 12, 31), task.CreationDate);
            }

            [TestMethod]
            public void Task_CreationDate_WhenIncompleteWithCreationDateWithNoTrailingSpace_ShouldBeHighDate()
            {
                var task = new Task("(A) 2015-12-01test task due:2015-12-31");
                Assert.AreEqual(new DateTime(9999, 12, 31), task.CreationDate);
            }

            [TestMethod]
            public void Task_CreationDate_WhenCompleteWithCreationDate_ShouldBeBlank()
            {
                var task = new Task("x 2013-12-31 2015-12-01 test task due:2015-12-31");
                Assert.AreEqual(new DateTime(2015, 12, 01), task.CreationDate);
            }

            [TestMethod]
            public void Task_CreationDate_WhenCompleteWithNoCreationDate_ShouldBeHighDate()
            {
                var task = new Task("x 2015-12-31 test task due:2015-12-31");
                Assert.AreEqual(new DateTime(9999, 12, 31), task.CreationDate);
            }

            [TestMethod]
            public void Task_DueDate_WhenCreationDateMonthInvalid_ShouldBeHighDate()
            {
                var task = new Task("(A) 9999-13-31 test task +Project1");
                Assert.AreEqual(new DateTime(9999, 12, 31), task.CreationDate);
            }

            [TestMethod]
            public void Task_DueDate_WhenCreationDateDayInvalid_ShouldBeHighDate()
            {
                var task = new Task("(A) 9999-12-32 test task due:9999-12-32 +Project1");
                Assert.AreEqual(new DateTime(9999, 12, 31), task.CreationDate);
            }
        }

        [TestClass]
        public class Task_UnitTests_Property_DueDate
        {
            [TestMethod]
            public void Task_DueDate_WhenTaskContainsDueDate_ShouldBeDueDate()
            {
                var task = new Task("(A) test task due:2015-12-31");
                Assert.AreEqual(new DateTime(2015, 12, 31), task.DueDate);
            }

            [TestMethod]
            public void Task_DueDate_WhenTaskContainsMulitpleDueDates_ShouldBeFirstDueDate()
            {
                var task = new Task("(A) test task due:2015-12-31 +Project1 due:2016-06-30");
                Assert.AreEqual(new DateTime(2015, 12, 31), task.DueDate);
            }

            [TestMethod]
            public void Task_DueDate_WhenTaskContainsNoDueDate_ShouldBeHighDate()
            {
                var task = new Task("(A) test task +Project1");
                Assert.AreEqual(new DateTime(9999, 12, 31), task.DueDate);
            }

            [TestMethod]
            public void Task_DueDate_WhenDueDateMissingLeadingSpace_ShouldBeHighDate()
            {
                var task = new Task("(A) test taskdue:2015-12-31 +Project1");
                Assert.AreEqual(new DateTime(9999, 12, 31), task.DueDate);
            }

            [TestMethod]
            public void Task_DueDate_WhenDueDateMissingTrailingSpace_ShouldBeHighDate()
            {
                var task = new Task("(A) test task due:2015-12-31+Project1");
                Assert.AreEqual(new DateTime(9999, 12, 31), task.DueDate);
            }

            [TestMethod]
            public void Task_DueDate_WhenImproperlyFormattedDueDate_ShouldBeHighDate()
            {
                var task = new Task("(A) test task due:12/31/2015 +Project1");
                Assert.AreEqual(new DateTime(9999, 12, 31), task.DueDate);
            }

            [TestMethod]
            public void Task_DueDate_WhenDueDateMonthInvalid_ShouldBeHighDate()
            {
                var task = new Task("(A) test task due:9999-13-31 +Project1");
                Assert.AreEqual(new DateTime(9999, 12, 31), task.DueDate);
            }

            [TestMethod]
            public void Task_DueDate_WhenDueDateDayInvalid_ShouldBeHighDate()
            {
                var task = new Task("(A) test task due:9999-12-32 +Project1");
                Assert.AreEqual(new DateTime(9999, 12, 31), task.DueDate);
            }

            [TestMethod]
            public void Task_DueDate_WhenRelativeDueDate_ShouldBeHighDate()
            {
                var task = new Task("(A) test task due:Monday +Project1");
                Assert.AreEqual(new DateTime(9999, 12, 31), task.DueDate);
            }

            [TestMethod]
            public void Task_DueDateT_WhenTaskIsOnlyDueDate_ShouldBeDueDate()
            {
                var task = new Task("due:2015-12-31");
                Assert.AreEqual(new DateTime(2015, 12, 31), task.DueDate);
            }

            [TestMethod]
            public void Task_DueDate_WhenPropertyIsChanged_ShouldNotifyPropertyChanged()
            {
                var task = new Task("(A) This is a test task due:2015-12-31");
                List<String> changedProperties = new List<string>();
                task.PropertyChanged += (sender, e) =>
                {
                    changedProperties.Add(e.PropertyName);
                };
                task.RawText = "(A) This is a test task due:2016-01-01";
                CollectionAssert.Contains(changedProperties, "DueDate");
            }
        }

        [TestClass]
        public class Task_UnitTests_Property_DueState
        {
            [TestMethod]
            public void Task_DueState_WhenTaskContainsNoDueDate_ShouldBeNotDue()
            {
                var task = new Task("(A) test task");
                Assert.AreEqual(Task.TaskDueState.NotDue, task.DueState);
            }

            [TestMethod]
            public void Task_DueState_WhenTaskContainsFutureDueDate_ShouldBeNotDue()
            {
                DateTime dueDate = DateTime.Today.AddDays(1);
                string dueDateText = dueDate.ToString("yyyy-MM-dd");
                var task = new Task("(A) test task due:" + dueDateText);
                Assert.AreEqual(Task.TaskDueState.NotDue, task.DueState);
            }

            [TestMethod]
            public void Task_DueState_WhenTaskContainsTodayDueDate_ShouldBeOverdue()
            {
                DateTime dueDate = DateTime.Today;
                string dueDateText = dueDate.ToString("yyyy-MM-dd");
                var task = new Task("(A) test task due:" + dueDateText);
                Assert.AreEqual(Task.TaskDueState.DueToday, task.DueState);
            }

            [TestMethod]
            public void Task_DueState_WhenTaskContainsPastDueDate_ShouldBeOverdue()
            {
                DateTime dueDate = DateTime.Today.AddDays(-1);
                string dueDateText = dueDate.ToString("yyyy-MM-dd");
                var task = new Task("(A) test task due:" + dueDateText);
                Assert.AreEqual(Task.TaskDueState.Overdue, task.DueState);
            }

            [TestMethod]
            public void Task_DueState_WhenPropertyIsChanged_ShouldNotifyPropertyChanged()
            {
                var task = new Task("(A) This is a test task");
                List<String> changedProperties = new List<string>();
                task.PropertyChanged += (sender, e) =>
                {
                    changedProperties.Add(e.PropertyName);
                };
                task.RawText = "(A) This is a test task due:2000-01-01";
                CollectionAssert.Contains(changedProperties, "DueState");
            }
        }

        [TestClass]
        public class Task_UnitTests_Property_ThresholdDateText
        {
            [TestMethod]
            public void Task_ThresholdDateText_WhenTaskContainsThresholdDate_ShouldBeThresholdDate()
            {
                var task = new Task("(A) test task t:2015-12-31");
                Assert.AreEqual("2015-12-31", task.ThresholdDateText);
            }

            [TestMethod]
            public void Task_ThresholdDateText_WhenTaskContainsMulitpleThresholdDates_ShouldBeFirstThresholdDate()
            {
                var task = new Task("(A) test task t:2015-12-31 +Project1 t:2016-06-30");
                Assert.AreEqual("2015-12-31", task.ThresholdDateText);
            }

            [TestMethod]
            public void Task_ThresholdDateText_WhenTaskContainsNoThresholdDate_ShouldBeBlank()
            {
                var task = new Task("(A) test task +Project1");
                Assert.AreEqual(String.Empty, task.ThresholdDateText);
            }

            [TestMethod]
            public void Task_ThresholdDateText_WhenTaskContainsThresholdDateMissingLeadingSpace_ShouldBeBlank()
            {
                var task = new Task("(A) test taskt:2015-12-31 +Project1");
                Assert.AreEqual(String.Empty, task.ThresholdDateText);
            }

            [TestMethod]
            public void Task_ThresholdDateText_WhenTaskContainsThresholdDateMissingTrailingSpace_ShouldBeBlank()
            {
                var task = new Task("(A) test task t:2015-12-31+Project1");
                Assert.AreEqual(String.Empty, task.ThresholdDateText);
            }

            [TestMethod]
            public void Task_ThresholdDateText_WhenTaskContainsImproperlyFormattedThresholdDate_ShouldBeBlank()
            {
                var task = new Task("(A) test task t:12/31/2015 +Project1");
                Assert.AreEqual(String.Empty, task.ThresholdDateText);
            }

            [TestMethod]
            public void Task_ThresholdDateText_WhenThresholdDateMonthInvalid_ShouldBeBlank()
            {
                var task = new Task("(A) test task t:9999-13-31 +Project1");
                Assert.AreEqual(String.Empty, task.ThresholdDateText);
            }

            [TestMethod]
            public void Task_ThresholdDateText_WhenThresholdDateDayInvalid_ShouldBeBlank()
            {
                var task = new Task("(A) test task t:9999-12-32 +Project1");
                Assert.AreEqual(String.Empty, task.ThresholdDateText);
            }

            [TestMethod]
            public void Task_ThresholdDateText_WhenThresholdTagIsCaplitalized_ShouldBeBlank()
            {
                var task = new Task("(A) test task T:9999-12-31 +Project1");
                Assert.AreEqual(String.Empty, task.ThresholdDateText);
            }

            [TestMethod]
            public void Task_ThresholdDateText_WhenTaskContainsRelativeThresholdDateText_ShouldBeBlank()
            {
                var task = new Task("(A) test task t:Monday +Project1");
                Assert.AreEqual(String.Empty, task.ThresholdDateText);
            }

            [TestMethod]
            public void Task_ThresholdDateText_WhenPropertyIsChanged_ShouldNotifyPropertyChanged()
            {
                var task = new Task("(A) This is a test task t:2015-12-31");
                List<String> changedProperties = new List<string>();
                task.PropertyChanged += (sender, e) =>
                {
                    changedProperties.Add(e.PropertyName);
                };
                task.RawText = "(A) This is a test task t:2016-01-01";
                CollectionAssert.Contains(changedProperties, "ThresholdDateText");
            }
        }

        [TestClass]
        public class Task_UnitTests_Property_ThresholdDate
        {
            [TestMethod]
            public void Task_ThresholdDate_WhenTaskContainsThresholdDate_ShouldBeThresholdDate()
            {
                var task = new Task("(A) test task t:2015-12-31");
                Assert.AreEqual(new DateTime(2015, 12, 31), task.ThresholdDate);
            }

            [TestMethod]
            public void Task_ThresholdDate_WhenTaskContainsMulitpleThresholdDates_ShouldBeFirstThresholdDate()
            {
                var task = new Task("(A) test task t:2015-12-31 +Project1 t:2016-06-30");
                Assert.AreEqual(new DateTime(2015, 12, 31), task.ThresholdDate);
            }

            [TestMethod]
            public void Task_ThresholdDate_WhenTaskContainsNoThresholdDate_ShouldBeHighDate()
            {
                var task = new Task("(A) test task +Project1");
                Assert.AreEqual(new DateTime(9999, 12, 31), task.ThresholdDate);
            }

            [TestMethod]
            public void Task_ThresholdDate_WhenTaskContainsThresholdDateMissingLeadingSpace_ShouldBeHighDate()
            {
                var task = new Task("(A) test taskt:2015-12-31 +Project1");
                Assert.AreEqual(new DateTime(9999, 12, 31), task.ThresholdDate);
            }

            [TestMethod]
            public void Task_ThresholdDate_WhenTaskContainsThresholdDateMissingTrailingSpace_ShouldBeHighDate()
            {
                var task = new Task("(A) test task t:2015-12-31+Project1");
                Assert.AreEqual(new DateTime(9999, 12, 31), task.ThresholdDate);
            }

            [TestMethod]
            public void Task_ThresholdDate_WhenTaskContainsImproperlyFormattedThresholdDate_ShouldBeHighDate()
            {
                var task = new Task("(A) test task t:12/31/2015 +Project1");
                Assert.AreEqual(new DateTime(9999, 12, 31), task.ThresholdDate);
            }

            [TestMethod]
            public void Task_ThresholdDate_WhenThresholdDateMonthInvalid_ShouldBeHighDate()
            {
                var task = new Task("(A) test task t:9999-13-31 +Project1");
                Assert.AreEqual(new DateTime(9999, 12, 31), task.ThresholdDate);
            }

            [TestMethod]
            public void Task_ThresholdDate_WhenThresholdDateDayInvalid_ShouldBeHighDate()
            {
                var task = new Task("(A) test task t:9999-12-32 +Project1");
                Assert.AreEqual(new DateTime(9999, 12, 31), task.ThresholdDate);
            }

            [TestMethod]
            public void Task_ThresholdDate_WhenTaskContainsRelativeThresholdDate_ShouldBeHighDate()
            {
                var task = new Task("(A) test task t:Monday +Project1");
                Assert.AreEqual(new DateTime(9999, 12, 31), task.ThresholdDate);
            }

            [TestMethod]
            public void Task_ThresholdDate_WhenPropertyIsChanged_ShouldNotifyPropertyChanged()
            {
                var task = new Task("(A) This is a test task t:2015-12-31");
                List<String> changedProperties = new List<string>();
                task.PropertyChanged += (sender, e) =>
                {
                    changedProperties.Add(e.PropertyName);
                };
                task.RawText = "(A) This is a test task t:2016-01-01";
                CollectionAssert.Contains(changedProperties, "ThresholdDate");
            }
        }

        [TestClass]
        public class Task_UnitTests_Property_ThresholdState
        {
            [TestMethod]
            public void Task_ThresholdState_WhenTaskContainsNoThresholdDate_ShouldBeAfterThresholdDate()
            {
                var task = new Task("(A) test task");
                Assert.AreEqual(Task.TaskThresholdState.AfterThresholdDate, task.ThresholdState);
            }

            [TestMethod]
            public void Task_ThresholdState_WhenTaskContainsFutureThresholdDate_ShouldBeBeforeThresholdDate()
            {
                DateTime ThresholdDate = DateTime.Today.AddDays(1);
                string ThresholdDateText = ThresholdDate.ToString("yyyy-MM-dd");
                var task = new Task("(A) test task t:" + ThresholdDateText);
                Assert.AreEqual(Task.TaskThresholdState.BeforeThresholdDate, task.ThresholdState);
            }

            [TestMethod]
            public void Task_ThresholdState_WhenTaskContainsTodayThresholdDate_ShouldBeOnThresholdDate()
            {
                DateTime ThresholdDate = DateTime.Today;
                string ThresholdDateText = ThresholdDate.ToString("yyyy-MM-dd");
                var task = new Task("(A) test task t:" + ThresholdDateText);
                Assert.AreEqual(Task.TaskThresholdState.OnThresholdDate, task.ThresholdState);
            }

            [TestMethod]
            public void Task_ThresholdState_WhenTaskContainsPastThresholdDate_ShouldBeAfterThresholdDate()
            {
                DateTime ThresholdDate = DateTime.Today.AddDays(-1);
                string ThresholdDateText = ThresholdDate.ToString("yyyy-MM-dd");
                var task = new Task("(A) test task t:" + ThresholdDateText);
                Assert.AreEqual(Task.TaskThresholdState.AfterThresholdDate, task.ThresholdState);
            }

            [TestMethod]
            public void Task_ThresholdState_WhenPropertyIsChanged_ShouldNotifyPropertyChanged()
            {
                var task = new Task("(A) This is a test task");
                List<String> changedProperties = new List<string>();
                task.PropertyChanged += (sender, e) =>
                {
                    changedProperties.Add(e.PropertyName);
                };
                task.RawText = "(A) This is a test task t:9999-12-31";
                CollectionAssert.Contains(changedProperties, "ThresholdState");
            }
        }

        [TestClass]
        public class Task_UnitTests_Method_ToString
        {
            [TestMethod]
            public void Task_ToString_WhenTaskContainsTaskID_ShouldIncludeTaskId()
            {
                var task = new Task("(A) this is a test task", 50);
                Assert.AreEqual("[50] (A) this is a test task", task.ToString());
            }

            [TestMethod]
            public void Task_ToString_WhenTaskContainsNoTaskID_ShouldIncludeNoTaskId()
            {
                var task = new Task("(A) this is a test task");
                Assert.AreEqual("[] (A) this is a test task", task.ToString());
            }
        }

        [TestClass]
        public class Task_UnitTests_Method_AppendText
        {
            [TestMethod]
            public void Task_AppendText_WhenNotBlank_ShouldAppendAtEnd()
            {
                var task = new Task("(A) this is a test task");
                task.AppendText("[text to append]");
                Assert.AreEqual("(A) this is a test task [text to append]", task.RawText);
            }

            [TestMethod]
            public void Task_AppendText_WhenBlank_ShouldAppendAtEnd()
            {
                var task = new Task();
                task.AppendText("[text to append]");
                Assert.AreEqual("[text to append]", task.RawText);
            }
        }

        [TestClass]
        public class Task_UnitTests_Method_PrependText
        {
            [TestMethod]
            public void Task_PrependText_WhenCompleteWithCreationDate_ShouldPrependAfterCreationDate()
            {
                var task = new Task("x 2015-12-31 2015-12-01 test task");
                task.PrependText("[text to prepend]");
                Assert.AreEqual("x 2015-12-31 2015-12-01 [text to prepend] test task", task.RawText);
            }

            [TestMethod]
            public void Task_PrependText_WhenCompleteWithNoCreationDate_ShouldPrependAfterCreationDate()
            {
                var task = new Task("x 2015-12-01 test task");
                task.PrependText("[text to prepend]");
                Assert.AreEqual("x 2015-12-01 [text to prepend] test task", task.RawText);
            }

            [TestMethod]
            public void Task_PrependText_WhenIncompleteWithNoCreationDate_ShouldPrependAtBeginning()
            {
                var task = new Task("test task");
                task.PrependText("[text to prepend]");
                Assert.AreEqual("[text to prepend] test task", task.RawText);
            }

            [TestMethod]
            public void Task_PrependText_WhenIncompleteWithPriorityAndNoCreationDate_ShouldPrependAfterPriority()
            {
                var task = new Task("(A) test task");
                task.PrependText("[text to prepend]");
                Assert.AreEqual("(A) [text to prepend] test task", task.RawText);
            }

            [TestMethod]
            public void Task_PrependText_WhenIncompleteWithPriorityAndCreationDate_ShouldPrependAfterCreationDate()
            {
                var task = new Task("(A) 2015-12-01 test task");
                task.PrependText("[text to prepend]");
                Assert.AreEqual("(A) 2015-12-01 [text to prepend] test task", task.RawText);
            }

            [TestMethod]
            public void Task_PrependText_WhenBlank_ShouldPrependAtBeginning()
            {
                var task = new Task();
                task.PrependText("[text to prepend]");
                Assert.AreEqual("[text to prepend]", task.RawText);
            }
        }

        [TestClass]
        public class Task_UnitTests_Method_MarkComplete
        {
            [TestMethod]
            public void Task_MarkComplete_WhenTaskIncomplete_ShouldCompleteTask()
            {
                var task = new Task("(A) test task");
                task.MarkComplete();
                Assert.AreEqual("x " + DateTime.Today.ToString("yyyy-MM-dd") + " test task", task.RawText);
            }

            [TestMethod]
            public void Task_MarkComplete_WhenTaskComplete_ShouldDoNothing()
            {
                var task = new Task("x 2015-12-31 test task");
                task.MarkComplete();
                Assert.AreEqual("x 2015-12-31 test task", task.RawText);
            }
        }

        [TestClass]
        public class Task_UnitTests_Method_MarkIncomplete
        {
            [TestMethod]
            public void Task_MarkIncomplete_WhenTaskComplete_ShouldIncompleteTask()
            {
                var task = new Task("x 2015-12-31 test task");
                task.MarkIncomplete();
                Assert.AreEqual("test task", task.RawText);
            }

            [TestMethod]
            public void Task_MarkIncomplete_WhenTaskComplete_ShouldDoNothing()
            {
                var task = new Task("(A) test task");
                task.MarkIncomplete();
                Assert.AreEqual("(A) test task", task.RawText);
            }
        }

        [TestClass]
        public class Task_UnitTests_Method_ToggleCompletion
        {
            [TestMethod]
            public void Task_ToggleCompletion_WhenTaskComplete_ShouldIncompleteTask()
            {
                var task = new Task("x 2015-12-31 test task");
                task.ToggleCompletion();
                Assert.AreEqual("test task", task.RawText);
            }

            [TestMethod]
            public void Task_ToggleCompletion_WhenTaskIncomplete_ShouldCompleteTask()
            {
                var task = new Task("(A) test task");
                task.ToggleCompletion();
                Assert.AreEqual("x " + DateTime.Today.ToString("yyyy-MM-dd") + " test task", task.RawText);
            }
        }

        [TestClass]
        public class Task_UnitTests_Method_SetPriority
        {
            [TestMethod]
            public void Task_SetPriority_WhenNoPriority_ShouldPrependPriority()
            {
                var task = new Task("test task");
                task.SetPriority("C");
                Assert.AreEqual("(C) test task", task.RawText);
            }

            [TestMethod]
            public void Task_SetPriority_WhenNoPriority_ShouldChangePriority()
            {
                var task = new Task("(A) test task");
                task.SetPriority("C");
                Assert.AreEqual("(C) test task", task.RawText);
            }

            [TestMethod]
            public void Task_SetPriority_WhenInputIsNotAnUppercaseLetter_ShouldDoNothing()
            {
                var task = new Task("test task");
                task.SetPriority("a");
                Assert.AreEqual("test task", task.RawText);
            }

            [TestMethod]
            public void Task_SetPriority_WhenInputIsNotALetter_ShouldDoNothing()
            {
                var task = new Task("(A) test task");
                task.SetPriority("!");
                Assert.AreEqual("(A) test task", task.RawText);
            }
        }

        [TestClass]
        public class Task_UnitTests_Method_RemovePriority
        {
            [TestMethod]
            public void Task_RemovePriority_WhenPriority_ShouldRemovePriority()
            {
                var task = new Task("(A) test task");
                task.RemovePriority();
                Assert.AreEqual("test task", task.RawText);
            }

            [TestMethod]
            public void Task_RemovePriority_WhenNoPriority_ShouldDoNothing()
            {
                var task = new Task("test task");
                task.RemovePriority();
                Assert.AreEqual("test task", task.RawText);
            }
        }

        [TestClass]
        public class Task_UnitTests_Method_IncreasePriority
        {
            [TestMethod]
            public void Task_IncreasePriority_WhenPriorityIsC_ShouldSetPriorityToB()
            {
                var task = new Task("(C) test task");
                task.IncreasePriority();
                Assert.AreEqual("(B) test task", task.RawText);
            }

            [TestMethod]
            public void Task_IncreasePriority_WhenPriorityIsA_ShouldDoNothing()
            {
                var task = new Task("(A) test task");
                task.IncreasePriority();
                Assert.AreEqual("(A) test task", task.RawText);
            }

            [TestMethod]
            public void Task_IncreasePriority_WhenNoPriority_ShouldSetPriorityToA()
            {
                var task = new Task("test task");
                task.IncreasePriority();
                Assert.AreEqual("(A) test task", task.RawText);
            }

            [TestMethod]
            public void Task_IncreasePriority_WhenTaskIsComplete_ShouldDoNothing()
            {
                var task = new Task("x 2015-12-31 test task");
                task.IncreasePriority();
                Assert.AreEqual("x 2015-12-31 test task", task.RawText);
            }

            [TestMethod]
            public void Task_IncreasePriority_WhenTaskIsBlank_ShouldDoNothing()
            {
                var task = new Task();
                task.IncreasePriority();
                Assert.AreEqual(string.Empty, task.RawText);
            }
        }

        [TestClass]
        public class Task_UnitTests_Method_DecreasePriority
        {
            [TestMethod]
            public void Task_DecreasePriority_WhenPriorityIsC_ShouldSetPriorityToD()
            {
                var task = new Task("(C) test task");
                task.DecreasePriority();
                Assert.AreEqual("(D) test task", task.RawText);
            }

            [TestMethod]
            public void Task_DecreasePriority_WhenPriorityIsZ_ShouldDoNothing()
            {
                var task = new Task("(Z) test task");
                task.DecreasePriority();
                Assert.AreEqual("(Z) test task", task.RawText);
            }

            [TestMethod]
            public void Task_DecreasePriority_WhenNoPriority_ShouldSetPriorityToA()
            {
                var task = new Task("test task");
                task.DecreasePriority();
                Assert.AreEqual("(A) test task", task.RawText);
            }

            [TestMethod]
            public void Task_DecreasePriority_WhenTaskIsComplete_ShouldDoNothing()
            {
                var task = new Task("x 2015-12-31 test task");
                task.DecreasePriority();
                Assert.AreEqual("x 2015-12-31 test task", task.RawText);
            }

            [TestMethod]
            public void Task_IncreasePriority_WhenTaskIsBlank_ShouldDoNothing()
            {
                var task = new Task();
                task.DecreasePriority();
                Assert.AreEqual(string.Empty, task.RawText);
            }
        }

        [TestClass]
        public class Task_UnitTests_Method_SetDueDate
        {
            [TestMethod]
            public void Task_SetDueDate_WhenTaskHasNoDueDate_ShouldAppendDueDate()
            {
                var task = new Task("(A) test task");
                task.SetDueDate("2015-12-31");
                Assert.AreEqual("(A) test task due:2015-12-31", task.RawText);
            }

            [TestMethod]
            public void Task_SetDueDate_WhenTaskHasOneDueDate_ShouldReplaceDueDate()
            {
                var task = new Task("(A) test task due:2015-12-01");
                task.SetDueDate("2015-12-31");
                Assert.AreEqual("(A) test task due:2015-12-31", task.RawText);
            }

            [TestMethod]
            public void Task_SetDueDate_WhenTaskHasMultipleDueDates_ShouldReplaceAllDueDates()
            {
                var task = new Task("(A) due:2015-12-01 test task due:2015-12-15 due:2015-12-31");
                task.SetDueDate("2015-12-31");
                Assert.AreEqual("(A) due:2015-12-31 test task due:2015-12-31 due:2015-12-31", task.RawText);
            }

            [TestMethod]
            public void Task_SetDueDate_WhenInputStringInWrongDateFormat_ShouldDoNothing()
            {
                var task = new Task("(A) test task");
                task.SetDueDate("12/31/2015");
                Assert.AreEqual("(A) test task", task.RawText);
            }

            [TestMethod]
            public void Task_SetDueDate_WhenInputStringIsNotADate_ShouldDoNothing()
            {
                var task = new Task("(A) test task");
                task.SetDueDate("Next Monday");
                Assert.AreEqual("(A) test task", task.RawText);
            }
        }

        [TestClass]
        public class Task_UnitTests_Method_RemoveDueDate
        {
            [TestMethod]
            public void Task_RemoveDueDate_WhenTaskHasDueDateAtEnd_ShouldRemoveDueDate()
            {
                var task = new Task("(A) test task due:2015-12-31");
                task.RemoveDueDate();
                Assert.AreEqual("(A) test task", task.RawText);
            }

            [TestMethod]
            public void Task_RemoveDueDate_WhenTaskHasDueDateAtStart_ShouldRemoveDueDate()
            {
                var task = new Task("due:2015-12-31 test task");
                task.RemoveDueDate();
                Assert.AreEqual("test task", task.RawText);
            }

            [TestMethod]
            public void Task_RemoveDueDate_WhenTaskHasDueDateInMiddle_ShouldRemoveDueDate()
            {
                var task = new Task("(A) test task due:2015-12-31 test task");
                task.RemoveDueDate();
                Assert.AreEqual("(A) test task test task", task.RawText);
            }

            [TestMethod]
            public void Task_RemoveDueDate_WhenTaskIsOnlyTheDueDate_ShouldRemoveDueDate()
            {
                var task = new Task("due:2015-12-31");
                task.RemoveDueDate();
                Assert.AreEqual(String.Empty, task.RawText);
            }
        }

        [TestClass]
        public class Task_UnitTests_Method_IncrementDueDate
        {
            [TestMethod]
            public void Task_IncrementDueDate_WhenTaskHasDueDate_ShouldIncrementDueDateByOneDay()
            {
                var task = new Task("(A) test task due:2015-12-31 test task");
                task.IncrementDueDate(1);
                Assert.AreEqual("(A) test task due:2016-01-01 test task", task.RawText);
            }

            [TestMethod]
            public void Task_IncrementDueDate_WhenTaskHasNoDueDate_ShouldAppendDueDateOfTomorrow()
            {
                var task = new Task("(A) test task");
                task.IncrementDueDate(1);
                DateTime dueDate = DateTime.Today.AddDays(1);
                Assert.AreEqual("(A) test task due:" + dueDate.ToString("yyyy-MM-dd"),
                    task.RawText);
            }
        }

        [TestClass]
        public class Task_UnitTests_Method_DecrementDueDate
        {
            [TestMethod]
            public void Task_DecrementDueDate_WhenTaskHasDueDate_ShouldIncrementDueDateByOneDay()
            {
                var task = new Task("(A) test task due:2015-12-31 test task");
                task.DecrementDueDate(1);
                Assert.AreEqual("(A) test task due:2015-12-30 test task", task.RawText);
            }

            [TestMethod]
            public void Task_DecrementDueDate_WhenTaskHasNoDueDate_ShouldAppendDueDateOfTomorrow()
            {
                var task = new Task("(A) test task");
                task.DecrementDueDate(1);
                DateTime dueDate = DateTime.Today.AddDays(-1);
                Assert.AreEqual("(A) test task due:" + dueDate.ToString("yyyy-MM-dd"),
                    task.RawText);
            }
        }

        [TestClass]
        public class Task_UnitTests_Method_SetThresholdDate
        {
            [TestMethod]
            public void Task_SetThresholdDate_WhenTaskHasNoThresholdDate_ShouldAppendThresholdDate()
            {
                var task = new Task("(A) test task");
                task.SetThresholdDate("2015-12-31");
                Assert.AreEqual("(A) test task t:2015-12-31", task.RawText);
            }

            [TestMethod]
            public void Task_SetThresholdDate_WhenTaskHasOneThresholdDate_ShouldReplaceThresholdDate()
            {
                var task = new Task("(A) test task t:2015-12-01");
                task.SetThresholdDate("2015-12-31");
                Assert.AreEqual("(A) test task t:2015-12-31", task.RawText);
            }

            [TestMethod]
            public void Task_SetThresholdDate_WhenTaskHasMultipleThresholdDates_ShouldReplaceAllThresholdDates()
            {
                var task = new Task("(A) t:2015-12-01 test task t:2015-12-15 t:2015-12-31");
                task.SetThresholdDate("2015-12-31");
                Assert.AreEqual("(A) t:2015-12-31 test task t:2015-12-31 t:2015-12-31", task.RawText);
            }

            [TestMethod]
            public void Task_SetThresholdDate_WhenInputStringInWrongDateFormat_ShouldDoNothing()
            {
                var task = new Task("(A) test task");
                task.SetThresholdDate("12/31/2015");
                Assert.AreEqual("(A) test task", task.RawText);
            }

            [TestMethod]
            public void Task_SetThresholdDate_WhenInputStringIsNotADate_ShouldDoNothing()
            {
                var task = new Task("(A) test task");
                task.SetThresholdDate("Next Monday");
                Assert.AreEqual("(A) test task", task.RawText);
            }
        }

        [TestClass]
        public class Task_UnitTests_Method_RemoveThresholdDate
        {
            [TestMethod]
            public void Task_RemoveThresholdDate_WhenTaskHasThresholdDateAtEnd_ShouldRemoveThresholdDate()
            {
                var task = new Task("(A) test task t:2015-12-31");
                task.RemoveThresholdDate();
                Assert.AreEqual("(A) test task", task.RawText);
            }

            [TestMethod]
            public void Task_RemoveThresholdDate_WhenTaskHasThresholdDateAtStart_ShouldRemoveThresholdDate()
            {
                var task = new Task("t:2015-12-31 test task");
                task.RemoveThresholdDate();
                Assert.AreEqual("test task", task.RawText);
            }

            [TestMethod]
            public void Task_RemoveThresholdDate_WhenTaskHasThresholdDateInMiddle_ShouldRemoveThresholdDate()
            {
                var task = new Task("(A) test task t:2015-12-31 test task");
                task.RemoveThresholdDate();
                Assert.AreEqual("(A) test task test task", task.RawText);
            }

            [TestMethod]
            public void Task_RemoveThresholdDate_WhenTaskIsOnlyTheThresholdDate_ShouldRemoveThresholdDate()
            {
                var task = new Task("t:2015-12-31");
                task.RemoveThresholdDate();
                Assert.AreEqual(String.Empty, task.RawText);
            }
        }

        [TestClass]
        public class Task_UnitTests_Method_IncrementThresholdDate
        {
            [TestMethod]
            public void Task_IncrementThresholdDate_WhenTaskHasThresholdDate_ShouldIncrementThresholdDateByOneDay()
            {
                var task = new Task("(A) test task t:2015-12-31 test task");
                task.IncrementThresholdDate(1);
                Assert.AreEqual("(A) test task t:2016-01-01 test task", task.RawText);
            }

            [TestMethod]
            public void Task_IncrementThresholdDate_WhenTaskHasNoThresholdDate_ShouldAppendThresholdDateOfTomorrow()
            {
                var task = new Task("(A) test task");
                task.IncrementThresholdDate(1);
                DateTime ThresholdDate = DateTime.Today.AddDays(1);
                Assert.AreEqual("(A) test task t:" + ThresholdDate.ToString("yyyy-MM-dd"),
                    task.RawText);
            }
        }

        [TestClass]
        public class Task_UnitTests_Method_DecrementThresholdDate
        {
            [TestMethod]
            public void Task_DecrementThresholdDate_WhenTaskHasThresholdDate_ShouldIncrementThresholdDateByOneDay()
            {
                var task = new Task("(A) test task t:2015-12-31 test task");
                task.DecrementThresholdDate(1);
                Assert.AreEqual("(A) test task t:2015-12-30 test task", task.RawText);
            }

            [TestMethod]
            public void Task_DecrementThresholdDate_WhenTaskHasNoThresholdDate_ShouldAppendThresholdDateOfTomorrow()
            {
                var task = new Task("(A) test task");
                task.DecrementThresholdDate(1);
                DateTime ThresholdDate = DateTime.Today.AddDays(-1);
                Assert.AreEqual("(A) test task t:" + ThresholdDate.ToString("yyyy-MM-dd"),
                    task.RawText);
            }
        }
    }
}
