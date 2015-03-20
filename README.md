# TodoTxtWin

## Purpose

Provide a todo.txt format-compliant object representation of tasks and task lists.

## Project Goals

1. Provide a modern .NET implementation that improves on the ones used in Todotxt.net and TodoTxtLib.
2. Fully implement the todo.txt format specifications.
3. Include support for threshold date and arbitrary tags, on top of the standard todo.txt format specifications.
4. Optimize classes for performance rather than in-memory footprint.
5. Fully support data bindings, including sending notifications of property changes.
6. Include exhaustive unit tests of the Task and TaskList objects.

## Design Decisions

### Task object

1. Support a property indicating the Task's position in a file or list.
2. Use regular expressions for todo.txt format parsing.
3. Minimize re-running regular expression pattern matching unless the Task is modified. That is, class properties are saved to instance variables, rather than evaluated each time they are needed.
4. Notify consumers of property changes, to support data binding.
5. Provide methods for updating all properties.
6. Do not provide methods for building tasks from component parts.

### TaskList object

1. Notify consumers of task list changes, by sending CollectionChanged notifications, to support data binding.
2. Send CollectionChanged notifications when task properties are changed (for in-place modifications to the Task objects within the task list).
2. Provide methods for reading/writing task list to file/stream.
3. Support task list sorting.
4. Support task list filtering.

### FileBasedTaskList object

1. Encapsulate methods for loading and saving todo.txt files.
2. Encapsulate functionality for watching the todo.txt file for external modifications, and optionally reloading the file when it is externally modified.
3. Support an optional auto-save function, triggered by changes to the task list.