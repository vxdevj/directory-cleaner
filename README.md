# directory-cleaner
A program for lazy, messy people.

This is a project I created awhile back for the purpose of sorting extremely messy directories with thousands of files in them.

Currently there are two different ways to sort,
Default Sort will take files in the targeted directory and attempt to put them in common sense places, such as .mp4 files go in the user's videos directory.

Isolate sort will take files in the targeted directory and separate them by file extension. It does this by creating a subfolder named after the file extension it found, and then throws all files with that extension into that folder.

Right now it can undo the last sorting that it completed until you close the application, but I plan on adding a way for it to remember every sorting session it has done so that you can always undo it later on, or look at where files went.
