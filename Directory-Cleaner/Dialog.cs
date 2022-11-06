using System;
using System.IO;
using System.Windows.Forms;

namespace DirectoryCleaner
{
    public class Dialog
    {
        static int errorCount = 0;

        public static void IntroQuery()
        {
            Console.Clear();
            Console.WriteLine($"DirectoryCleaner v{MainClass.version}");
            Console.WriteLine("___________________________________________");
            Console.WriteLine("Welcome to DirectoryCleaner!");
            Console.WriteLine("Choose what you would like to do.");
            Console.WriteLine("1. Sort | 2. Undo last sort | 3. Exit");
            Console.WriteLine("------------------------------------------------------------------------------------");
            Console.WriteLine();
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D1:
                    {
                        DirectoryQuery();
                        break;
                    }
                case ConsoleKey.D2:
                    {
                        UndoQuery();
                        break;
                    }
                case ConsoleKey.D3:
                    {
                        Environment.Exit(0);
                        break;
                    }
                default:
                    {
                        IntroQuery();
                        break;
                    }
            }
        }

        public static void DirectoryQuery()
        {
            if (Sorting.movedFiles.Count != 0)
            {
                UndoLostWarning();
            }
            Console.Clear();
            Console.WriteLine("Choose a directory to sort.");
            Console.WriteLine("You will get to choose how you want the directory sorted before anything is moved.");
            Console.WriteLine("------------------------------------------------------------------------------------");
            Console.WriteLine();
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            if (fbd.SelectedPath != null)
            {
                SortQuery(fbd.SelectedPath);
            }
            else
            {
                DirectoryQuery();
            }
        }

        public static void UndoLostWarning()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Are you sure you want to start sorting again?");
            Console.WriteLine("This will overwrite the program's memory of your last sort, meaning you can NOT undo the last sort that was done.");
            Console.WriteLine("This does not matter if you're fine with the last sort.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("1. Yes | 2. Cancel");
            Console.WriteLine("------------------------------------------------------------------------------------");
            Console.WriteLine();
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D1:
                    {
                        break;
                    }
                case ConsoleKey.D2:
                    {
                        IntroQuery();
                        break;
                    }
                default:
                    {
                        UndoLostWarning();
                        break;
                    }
            }
        }

        public static void SortQuery(string directoryPath)
        {
            if (!Sorting.IsValidPath(directoryPath))
            {
                Console.WriteLine();
                Console.WriteLine($"This is not a valid path: {directoryPath}");
                Console.WriteLine("Try checking for any typos.");
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
                DirectoryQuery();
            }
            else
            {
                if (!Directory.Exists(directoryPath))
                {
                    Console.WriteLine();
                    Console.WriteLine($"This directory does not exist: {directoryPath}");
                    Console.WriteLine("Try checking for any typos.");
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                    DirectoryQuery();
                }
                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Directory found: {directoryPath}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine();
                    Console.WriteLine("What kind of sorting do you want done?");
                    Console.WriteLine("------------------------------------------------------------------------------------");
                    Console.WriteLine();
                    Console.WriteLine("1. Default");
                    Console.WriteLine("For each file in this directory, this will check the file's file extension (.jpg, .mp3, .zip, etc) and attempt to");
                    Console.WriteLine("recognize it from a list of different file types I've hard-coded into the program.");
                    Console.WriteLine();
                    Console.WriteLine("It will take the files it recognizes and try to put them in common sense places like, pictures will go into your");
                    Console.WriteLine("Pictures folder, text files will go into your Documents folder, etc.");
                    Console.WriteLine();
                    Console.WriteLine("If it sees a file that it recognizes and/or doesn't know where to put it, it will ask you where it put files of that");
                    Console.WriteLine("type from then on. It will also do this if it comes across a file it does not recognize.");
                    Console.WriteLine("------------------------------------------------------------------------------------");
                    Console.WriteLine();
                    Console.WriteLine("2. Isolate");
                    Console.WriteLine("For each file in this directory, this will check the file's file extension (.jpg, .mp3, .zip, etc) and make a");
                    Console.WriteLine("folder only for files with that exact file extension.");
                    Console.WriteLine();
                    Console.WriteLine("This can be useful for isolating all of the different files you have so you can move specific files you are");
                    Console.WriteLine("looking for out of a large mess.");
                    Console.WriteLine();
                    Console.WriteLine("If you have many different file types in this directory, this will make many different folders to look through");
                    Console.WriteLine("------------------------------------------------------------------------------------");
                    Console.WriteLine();
                    Console.WriteLine("3. Go back");
                    Console.WriteLine("------------------------------------------------------------------------------------");
                    Console.WriteLine();
                    switch (Console.ReadKey().Key)
                    {
                        case ConsoleKey.D1:
                            {
                                Console.WriteLine();
                                Sorting.StartDefaultSort(directoryPath, RecursiveQuery());
                                break;
                            }
                        case ConsoleKey.D2:
                            {
                                Console.WriteLine();
                                Sorting.StartIsolateSort(directoryPath, RecursiveQuery());
                                break;
                            }
                        case ConsoleKey.D3:
                            {
                                DirectoryQuery();
                                break;
                            }
                        default:
                            {
                                SortQuery(directoryPath);
                                break;
                            }
                    }
                }
            }
        }

        public static bool RecursiveQuery()
        {
            Console.Clear();
            Console.WriteLine($"Would you like the sorting to be recursive?");
            Console.WriteLine("This will make the program look through all sub-folders inside of the main folder you are wanting to organize.");
            Console.WriteLine();
            Console.WriteLine("1. Yes | 2. No");
            Console.WriteLine("------------------------------------------------------------------------------------");
            Console.WriteLine();
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D1:
                    {
                        return true;
                    }
                case ConsoleKey.D2:
                    {
                        return false;
                    }
            }
            return false;
        }

        public static void UndoQuery()
        {
            Console.Clear();
            Console.WriteLine("Are you sure that you want to undo the last sort?");
            Console.WriteLine("This program only remembers the last sort done during this session.");
            Console.WriteLine("1. Yes | 2. No");
            Console.WriteLine("------------------------------------------------------------------------------------");
            Console.WriteLine();
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D1:
                    {
                        if (Sorting.movedFiles.Count != 0)
                        {
                            Sorting.UndoLastSort();
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("There has been no sorting done this session.");
                            Console.WriteLine("Press any key to continue.");
                            Console.ReadKey();
                            IntroQuery();
                        }
                        break;
                    }
                case ConsoleKey.D2:
                    {
                        IntroQuery();
                        break;
                    }
                default:
                    {
                        UndoQuery();
                        break;
                    }
            }
        }

        public static void StoreFileTypeQuery(string filePath)
        {
            Console.WriteLine();
            Console.WriteLine($"Not sure where to put files of this type: {Path.GetExtension(filePath)} what would you like to do?");
            Console.WriteLine($"1. Specify a folder to store it into. | 2. Ignore this particular file. | 3. Ignore ALL files that end with {Path.GetExtension(filePath)}");
            Console.WriteLine();
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D1:
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Enter a full path to the directory you want to move all {Path.GetExtension(filePath)} files to from here on.");
                        FolderBrowserDialog fbd = new FolderBrowserDialog();
                        fbd.ShowDialog();
                        Sorting.StoreFileExtensionDestination(filePath, fbd.SelectedPath);
                        break;
                    }

                case ConsoleKey.D2:
                    {
                        Sorting.IgnoreFile();
                        break;
                    }
                case ConsoleKey.D3:
                    {
                        Sorting.IgnoreFile();
                        Sorting.IgnoreFileType(Path.GetExtension(filePath));
                        break;
                    }
                default:
                    {
                        StoreFileTypeQuery(filePath);
                        break;
                    }
            }
        }

        public static void DirectoryNotFound(string filePath, string directory)
        {
            if (Sorting.IsValidPath(directory))
            {
                Console.WriteLine();
                Console.WriteLine($"{directory} is not an existing directory.");
                Console.WriteLine("Would you like to create this directory?");
                Console.WriteLine();
                Console.WriteLine("1. Yes | 2. No");
                Console.WriteLine();
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.D1:
                        {
                            try
                            {
                                Directory.CreateDirectory(directory);
                                Sorting.MoveFile(filePath, directory);
                            }
                            catch (Exception e)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine();
                                Console.WriteLine(e.Message);
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine();
                                Console.WriteLine("Press any key to retry.");
                                Console.ReadKey();
                                DirectoryNotFound(filePath, directory);
                            }
                            break;
                        }
                    case ConsoleKey.D2:
                        {
                            StoreFileTypeQuery(filePath);
                            break;
                        }
                    default:
                        {
                            DirectoryNotFound(filePath, directory);
                            break;
                        }

                }
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("That is not a valid path.");
                StoreFileTypeQuery(filePath);
            }
        }

        public static void ErrorsQuery()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine("There were errors moving some files.");
            Console.WriteLine("This is fine, as the program will simply have ignored the files that it could not move.");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Would you like to see which files were unable to be moved and the error messages?");
            Console.WriteLine("1. Yes | Or press any other key to continue.");
            Console.WriteLine();
            if (Console.ReadKey().Key == ConsoleKey.D1)
            {
                PrintAllErrors();
            }
        }

        public static void PrintAllErrors()
        {
            Console.WriteLine("------------------------------------------------------------------------------------");
            foreach (string file in Sorting.exceptionsEncountered.Keys)
            {
                errorCount++;
                Exception error;
                Sorting.exceptionsEncountered.TryGetValue(file, out error);
                Console.WriteLine($"Error {errorCount}");
                Console.WriteLine($"File: {file}");
                Console.WriteLine();
                Console.WriteLine(error.Message);
                Console.WriteLine("------------------------------------------------------------------------------------");
                Console.WriteLine();
            }
            errorCount = 0;
        }
    }
}
