## Version 0.1 ##

The prototype will implement the eight commands of the [Brainfuck](http://en.wikipedia.org/wiki/Brainfuck) programming language mapped to the following notation
| **RCN** | Brainfuck | description |
|:--------|:----------|:------------|
| **U** | **>** | increment the data pointer to the next cell to the right |
| **U'** | **<** | decrement the data pointer to the next cell to the right |
| **R** | **>** | increment the byte at the data pointer |
| **R'** | **>** | decrement the byte at the data pointer |
| **F** | **.** | output the byte at the data pointer |
| **F'** | **,** | accept one byte of input into the current data pointer |
| **(** | **[** | if the byte at the data pointer is zero, then instead of moving the instruction pointer forward to the next command, jump it forward to the command after the matching ] command |
| **)** | **]** | if the byte at the data pointer is nonzero, then instead of moving the instruction pointer forward to the next command, jump it back to the command after the matching [ command |


### Notes ###

  * Numbers can be added to the U, R, F commands. When appending numbers to a command with a prime symbol ( **'** ), the order of the prime symbol is irrelevant. For example **[R2](https://code.google.com/p/rndotnet/source/detail?r=2)'** is the same as **R'2**. The number appended is one byte in size.
  * Multiple prime symbols will toggle the _prime status_ of the command. For example **[R2](https://code.google.com/p/rndotnet/source/detail?r=2)** is the same as **[R2](https://code.google.com/p/rndotnet/source/detail?r=2)''**.

## Version 0.2 ##

The second version of the compiler will include all the function of version 0.1 with additional support for the following commands

|L\_X| if the byte at the current data pointer is equal to 0 then skip the next _X_ commands |
|:---|:--------------------------------------------------------------------------------------|
| L'X | if the byte at the current data pointer is **not** equal to 0 then skip the next _X_ commands |