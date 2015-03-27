**The Rubik's Cube Notation** programming language (aka **RCN**) is an esoteric programming language designed to look like [standard Rubik's Cube notation](http://www.worldcubeassociation.org/regulations/#notation).

The language aims to be isomorphic to [Brainfuck](http://www.muppetlabs.com/~breadbox/bf/) and therefore Turing-complete. It is currently in the development phase.

The following **RCN** program prints "Hello World!"

```
RRRRRRRRRR(URRRRRRRURRRRRRRRRRURRRURU'U'U'U'R')
URRFURFRRRRRRRFFRRRFURRFU'U'RRRRRRRRRRRRRRRFUFR
RRFR'R'R'R'R'R'FR'R'R'R'R'R'R'R'FURFUF
```

This is equivalent to the same Brainfuck program:

```
++++++++++[>+++++++>++++++++++>+++>+<<<<-]>++.>
+.+++++++..+++.>++.<<+++++++++++++++.>.+++.----
--.--------.>+.>.
```

Certain properties of the cubing notation carry over into **RCN**. As in Rubik's Cube notation, the prime symbol **'** _reverses_ the operation. For example, in **RCN**, _R_ by itself increments the byte at the current data pointer and _R'_ decrements it.

Also, numbering the commands will repeat the preceding command that many times. Thus the command _R3_ will increment the byte at the current data pointer three times (or by 3).

Using these properties, the above "Hello World!" program could be reduced to

```
R10 (U R7 U R10 U R3U R U4' R') U R2 F U R F R7 
F2 R3 F U RR F U2' R15 F U F R3 F R6' F R8' F U 
R F U F
```

Rubik's Notation.NET is a .NET implementation of the Rubik's Notation programming language.
