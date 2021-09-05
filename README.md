# JadeStudio

JadeStudio is a wip (currently not in progress) tool that allows you to work with Beyond Good and Evil's files.
The solution contains three projects:
JadeStudio.Core: A dynamically linked library for working with the game files
JadeStudio.Console: A console utility for debugging the library / using parts of the library
JadeStudio.BIG: The main utility acting as a frontend of the core library

## Features
Currently you can extract and rebuild BIG files (sally_clean.bf) which means you can change files in the game.

There is support for extracting and rebuilding texture files (ff8*.bin) although not every texture type is supported right now.

There's also a text editor which allows you to look at and change all of the text in the game.

## Built With

* [Cyotek.Windows.Forms.ColorPickers](https://github.com/cyotek/Cyotek.Windows.Forms.ColorPicker) - Used for picking colors in the text editor

## Thanks to
* ***Zeli*** You are the goat and you helped me so much in understanding the file formats!
