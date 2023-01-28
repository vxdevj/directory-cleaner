using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace DirectoryCleaner
{
    class Sorting
    {
        static int filesFound = 0;
        static int filesMoved = 0;
        static int filesIgnored = 0;
        static int errors = 0;
        public static bool verbose = true;
        public static bool recursive = false;

        public static Dictionary<string, Exception> exceptionsEncountered = new Dictionary<string, Exception>();
        public static List<string> createdDirectories = new List<string>();
        public static Dictionary<string, string> movedFiles = new Dictionary<string, string>();
        public static List<string> recursiveFolderPaths = new List<string>();
        public static string initalDirectory = "";

        static string myDocuments;
        static string myPictures;
        static string myMusic;
        static string myVideos;
        static string desktop;

        static List<string> imageFileTypes;
        static List<string> audioFileTypes;
        static List<string> videoFileTypes;
        static List<string> shortcutFileTypes;
        static List<string> textFileTypes;

        //this is where we will store where the user decides they want specific file types to go
        //key is file type, value is where that file type should go
        public static Dictionary<string, string> fileTypeLocations = new Dictionary<string, string>();

        public static void Initialize()
        {
            Console.Clear();
            Console.WriteLine("Initializing...");

            myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            myPictures = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            myMusic = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            myVideos = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
            desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            imageFileTypes = new List<string>();
            audioFileTypes = new List<string>();
            videoFileTypes = new List<string>();
            shortcutFileTypes = new List<string>();
            textFileTypes = new List<string>();

            #region Image Files
            imageFileTypes.Add(".jpg");
            imageFileTypes.Add(".jpeg");
            imageFileTypes.Add(".png");
            imageFileTypes.Add(".bmp");
            imageFileTypes.Add(".gif");
            imageFileTypes.Add(".jfif");
            imageFileTypes.Add(".webp");
            imageFileTypes.Add(".ppm");
            imageFileTypes.Add(".pgm");
            imageFileTypes.Add(".pbm");
            imageFileTypes.Add(".tiff");
            imageFileTypes.Add(".pdn");
            imageFileTypes.Add(".psd");
            imageFileTypes.Add(".cpt");
            imageFileTypes.Add(".kra");
            imageFileTypes.Add(".psp");
            imageFileTypes.Add(".tga");
            #endregion

            #region Audio Files
            audioFileTypes.Add(".aac");
            audioFileTypes.Add(".act");
            audioFileTypes.Add(".aiff");
            audioFileTypes.Add(".flac");
            audioFileTypes.Add(".m4a");
            audioFileTypes.Add(".mp3");
            audioFileTypes.Add(".mpc");
            audioFileTypes.Add(".nmf");
            audioFileTypes.Add(".ogg");
            audioFileTypes.Add(".oga");
            audioFileTypes.Add(".mogg");
            audioFileTypes.Add(".opus");
            audioFileTypes.Add(".org");
            audioFileTypes.Add(".wav");
            audioFileTypes.Add(".wma");
            audioFileTypes.Add(".mid");
            audioFileTypes.Add(".midi");
            #endregion

            #region Video Files
            videoFileTypes.Add(".webm");
            videoFileTypes.Add(".mkv");
            videoFileTypes.Add(".flv");
            videoFileTypes.Add(".ogv");
            videoFileTypes.Add(".gifv");
            videoFileTypes.Add(".avi");
            videoFileTypes.Add(".mov");
            videoFileTypes.Add(".qt");
            videoFileTypes.Add(".wmv");
            videoFileTypes.Add(".amv");
            videoFileTypes.Add(".mpg");
            videoFileTypes.Add(".mp2");
            videoFileTypes.Add(".mpeg");
            videoFileTypes.Add(".mpe");
            videoFileTypes.Add(".mpv");
            videoFileTypes.Add(".mp4");
            videoFileTypes.Add(".m2v");
            videoFileTypes.Add(".m4v");
            videoFileTypes.Add(".3gp");
            videoFileTypes.Add(".3g2");
            videoFileTypes.Add(".f4v");
            videoFileTypes.Add(".f4p");
            videoFileTypes.Add(".f4a");
            videoFileTypes.Add(".f4b");
            #endregion

            #region Shortcut Files
            shortcutFileTypes.Add(".lnk");
            shortcutFileTypes.Add(".url");
            #endregion

            #region Text Files
            textFileTypes.Add(".txt");
            textFileTypes.Add(".doc");
            textFileTypes.Add(".asc");
            textFileTypes.Add(".docx");
            #endregion

            Console.Clear();
        }

        public static void DefaultSortInitialize()
        {
            for (int i = 0; i < imageFileTypes.Count; i++)
            {
                fileTypeLocations.Add(imageFileTypes[i], myPictures);
            }

            for (int i = 0; i < audioFileTypes.Count; i++)
            {
                fileTypeLocations.Add(audioFileTypes[i], myMusic);
            }

            for (int i = 0; i < videoFileTypes.Count; i++)
            {
                fileTypeLocations.Add(videoFileTypes[i], myVideos);
            }

            for (int i = 0; i < shortcutFileTypes.Count; i++)
            {
                fileTypeLocations.Add(shortcutFileTypes[i], desktop);
            }

            for (int i = 0; i < textFileTypes.Count; i++)
            {
                fileTypeLocations.Add(textFileTypes[i], myDocuments);
            }
        }

        public static void DefaultSort(string directoryPath)
        {
            foreach (string filePath in Directory.EnumerateFiles(directoryPath))
            {
                FileFound(filePath);
                string fileExtension = Path.GetExtension(filePath);
                string pathToMoveTo = null;

                if (fileTypeLocations.ContainsKey(fileExtension))
                {
                    fileTypeLocations.TryGetValue(fileExtension, out pathToMoveTo);
                    if (pathToMoveTo == "DO NOT MOVE")
                    {
                        IgnoreFile();
                    }
                    else
                    {
                        MoveFile(filePath, pathToMoveTo);
                    }
                }
                else
                {
                    Dialog.StoreFileTypeQuery(filePath);
                }

            }
        }

        public static void RecursiveFolderSearch(string directoryPath)
        {
            foreach (string directory in Directory.EnumerateDirectories(directoryPath))
            {
                recursiveFolderPaths.Add(directory);
                RecursiveFolderSearch(directory);
            }
        }

        public static void StartDefaultSort(string directoryPath, bool recursive)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Initializing...");
            Console.ForegroundColor = ConsoleColor.White;
            DefaultSortInitialize();

            Console.Clear();
            movedFiles.Clear();
            createdDirectories.Clear();

            initalDirectory = directoryPath;

            DefaultSort(directoryPath);

            if (recursive == true)
            {
                RecursiveFolderSearch(directoryPath);
                foreach(string path in recursiveFolderPaths)
                {
                    DefaultSort(path);
                }
            }

            SortingFinished();
        }

        public static void IsolateSort(string directoryPath)
        {
            foreach (string filePath in Directory.EnumerateFiles(directoryPath))
            {
                //we do this check here so that if somehow a file was moved around while we're sorting, we won't actually even mark it as found or move it
                if (movedFiles.ContainsKey(filePath))
                {
                    if (verbose == true)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Already sorted this file before...that's weird I'm seeing it again.");
                    }
                    break;
                }

                FileFound(filePath);
                string fileExtension = Path.GetExtension(filePath);
                string destination = $@"{directoryPath}\{fileExtension} Folder";

                if(recursive == true)
                {
                    destination = $@"{initalDirectory}\{fileExtension} Folder";
                }

                if (Directory.Exists(destination))
                {
                    MoveFile(filePath, destination);
                }
                else
                {
                    Directory.CreateDirectory(destination);
                    createdDirectories.Add(destination);
                    if(verbose == true)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Created directory: {destination}");
                    }
                    MoveFile(filePath, destination);
                }
            }
        }

        public static void StartIsolateSort(string directoryPath, bool recursive)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Initializing...");
            Console.ForegroundColor = ConsoleColor.White;
            movedFiles.Clear();
            createdDirectories.Clear();

            initalDirectory = directoryPath;

            Console.Clear();
            //we sort the initial subfolders found first because during an isolate sort we create our own subfolders and we don't want to sort our own subfolders that we made
            if (recursive == true)
            {
                RecursiveFolderSearch(directoryPath);
                foreach (string path in recursiveFolderPaths)
                {
                    IsolateSort(path);
                }
            }

            IsolateSort(directoryPath);

            SortingFinished();
        }

        public static void CustomSort()
        {

        }

        public static void UndoLastSort()
        {
            string originalFilePath;
            if(movedFiles.Count() == 0)
            {
                Console.WriteLine();
                Console.WriteLine("No files have been sorted!");
                Dialog.IntroQuery();
            }

            //moving files back where they came from
            foreach(string movedFile in movedFiles.Keys)
            {
                movedFiles.TryGetValue(movedFile, out originalFilePath);
                if(originalFilePath != null)
                {
                    if(verbose == true)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Moving {movedFile} to {originalFilePath}");
                    }
                    File.Move(movedFile, originalFilePath);
                }
            }

            //cleaning up directories we made
            foreach(string directory in createdDirectories)
            {
                if(Directory.EnumerateFiles(directory).Count() == 0)
                {
                    if (verbose == true)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Deleting directory: {directory}");
                    }
                    Directory.Delete(directory);
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine($"There are files found in {directory}");
                    Console.WriteLine("Not deleting this directory because of them.");
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine("Undo sort complete.");
            movedFiles.Clear();
            createdDirectories.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();

            Dialog.IntroQuery();
        }

        //i wasn't sure a short way to put this so that's why the method name is really long
        //this is just the method that lists where certain file extensions should go to
        public static void StoreFileExtensionDestination(string filePath, string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                fileTypeLocations.Add(Path.GetExtension(filePath), directoryPath);
                MoveFile(filePath, directoryPath);
            }
            else
            {
                Dialog.DirectoryNotFound(filePath, directoryPath);
            }
        }

        public static void FileFound(string filePath)
        {
            filesFound++;
            if(verbose == true)
            {
                Console.WriteLine();
                Console.WriteLine($"File found: {filePath}");
            }
        }

        public static void FileMoved(string fileToMove, string destination)
        {
            filesMoved++;
            movedFiles.Add(destination, Path.GetFullPath(fileToMove));
        }

        public static void IgnoreFile()
        {
            filesIgnored++;
            if(verbose == true)
            {
                Console.WriteLine();
                Console.WriteLine("File ignored.");
            }
        }

        public static void IgnoreFileType(string fileExtension)
        {
            fileTypeLocations.Add(fileExtension, "DO NOT MOVE");
            if(verbose == true)
            {
                Console.WriteLine();
                Console.WriteLine($"Added {fileExtension} files to ignore list.");
            }
        }

        public static void ExceptionOccurred(string filePath, Exception e)
        {
            errors++;
            exceptionsEncountered.Add(filePath, e);
            Console.ForegroundColor = ConsoleColor.Red;
            if(verbose == true)
            {
                Console.WriteLine();
                Console.WriteLine(e.Message);
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static bool MoveFile(string fileToMove, string destinationDirectory)
        {
            //just another check to make sure we don't move the same file twice if for some reason it gets moved mid-sort
            if (movedFiles.ContainsKey(fileToMove))
            {
                if (verbose == true)
                {
                    Console.WriteLine();
                    Console.WriteLine("Hmm...I've already moved this file.");
                }
                return false;
            }

            string destinationFilePath = $@"{destinationDirectory}\{Path.GetFileName(fileToMove)}";

            if (verbose == true)
            {
                Console.WriteLine();
                Console.WriteLine($"Moving {Path.GetFullPath(fileToMove)} to {destinationDirectory}");
            }

            try
            {
                File.Move(fileToMove, destinationFilePath);
                FileMoved(Path.GetFullPath(fileToMove), destinationFilePath);
                return true;
            }
            catch(Exception e)
            {
                ExceptionOccurred(fileToMove, e);
                return false;
            }
        }

        public static void SortingFinished()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Done'd.");
            Console.WriteLine($"Files found: {filesFound}");
            Console.WriteLine($"Files moved: {filesMoved}");
            Console.WriteLine($"Files ignored: {filesIgnored}");
            if (errors > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Errors moving files: {errors}");
                Console.ForegroundColor = ConsoleColor.White;
                Dialog.ErrorsQuery();
            }
            else
            {
                Console.WriteLine($"Errors moving files: {errors}");
            }
            filesFound = 0;
            filesMoved = 0;
            filesIgnored = 0;
            errors = 0;
            fileTypeLocations.Clear();
            recursiveFolderPaths.Clear();
            exceptionsEncountered.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press any key to return.");
            Console.ReadKey();
            Console.WriteLine();
            Dialog.IntroQuery();
        }

        public static bool IsValidPath(string path, bool allowRelativePaths = false)
        {
            bool isValid = true;

            try
            {
                string fullPath = Path.GetFullPath(path);

                if (allowRelativePaths)
                {
                    isValid = Path.IsPathRooted(path);
                }
                else
                {
                    string root = Path.GetPathRoot(path);
                    isValid = string.IsNullOrEmpty(root.Trim(new char[] { '\\', '/' })) == false;
                }
            }
            catch (Exception ex)
            {
                isValid = false;
            }

            return isValid;
        }
    }
}
