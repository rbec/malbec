# Malbec
Data Flow Programming in C# .NET

Malbec is a functional reactive data flow programming library. The output of a program is modelled as the value of a pure function (or functions).

Key features
* Optional memoisation.
* Lazy and partial re-evaluation of nodes in the function composition graph.
* Higher order functions allowing self-modification of the graph.

Functions are represented as vertices in a directed acyclic graph with edges representing dependencies between the ouput of a function and it's use as the argument to other functions. External vertices or 'variables' are inputs to the program and might be a file on disk, an external data stream or user input events. When external nodes are modified the changes are automatically pushed through the graph in a topologically sorted order skipping the evaluation of any function whose arguments are unchanged.
