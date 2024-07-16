using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TeboCam
{
    public interface IFileInfo
    {
        Dictionary<string, string> GetFileTypes();
        void DeleteFiles(string id);
        void ArchiveFiles(string id);
        void AddFileType(string id, string fileType);
        void AddFileCount(string id);
        void AddFileSize(string id);
        void AddFileDirectory(string id, string directory);
        void AddFileNamePattern(string id, string pattern);
        void AddAggregates();
        string GetCountForId(string id);
        string GetSizeForId(string id);
        string GetTypeForId(string id);
    }

    public class FileInfoClass : IFileInfo
    {
        IException tebowebException;
        internal Dictionary<string, Dictionary<string, string>> store = new Dictionary<string, Dictionary<string, string>>();
        const string fileType = "FileType";
        const string fileDirectory = "FileDirectory";
        const string filePattern = "FilePattern";
        const string fileCount = "FileCount";
        const string fileSize = "FileSize";

        public FileInfoClass(IException exception)
        {
            tebowebException = exception;
        }

        public void ArchiveFiles(string id)
        {
            //https://stackoverflow.com/q/15291337
            var filesDirectory = store[fileDirectory][id];
            var dirName = store[fileType][id].ToLower().Replace(' ', '_');
            var timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
            var zipFileName = $"{dirName}_{timeStamp}";
            var patternToZip = store[filePattern][id];
            var files = Directory.EnumerateFiles(filesDirectory, patternToZip);

            if (files.Count() == 0) return;

            try
            {
                using (ZipFile zipFile = new ZipFile())
                {
                    zipFile.AddFiles(files, false, string.Empty);
                    zipFile.Save($"{filesDirectory}{zipFileName}.zip");
                }

                File.Move($"{filesDirectory}{zipFileName}.zip", $"{TebocamState.vaultFolder}{zipFileName}.zip");

                //todo unzip the file in a temporary folder to check the files have been correctly zipped
                //before deleting the files then also delete the temporary folder

                foreach (var f in Directory.EnumerateFiles(filesDirectory, patternToZip))
                {
                    File.Delete(f);
                }

                MessageDialog.messageInform("Files have been zipped and moved to the vault folder", "Files zipped");
            }
            catch (Exception e)
            {
                tebowebException.LogException(e);
            }
        }

        public void DeleteFiles(string id)
        {
            var check = MessageDialog.messageQuestionConfirm("Are you sure you want to delete these files?", "Delete files");

            if (check == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            var directoryToDeleteWithin = store[fileDirectory][id];
            var patternToDelete = store[filePattern][id];

            foreach (var f in Directory.EnumerateFiles(directoryToDeleteWithin, patternToDelete))
            {
                File.Delete(f);
            }
        }

        Dictionary<string, string> IFileInfo.GetFileTypes()
        {
            return store[fileType];
        }

        private void dictionaryDelete(string dataType, string id)
        {
            var dataTypeExists = store.ContainsKey(dataType);

            if (dataTypeExists)
            {
                store[dataType].Remove(id);
            }
        }


        private void dictionaryCreateOrAdd(string dataType, string id, string data)
        {
            var dataTypeExists = store.ContainsKey(dataType);

            if (!dataTypeExists)
            {
                var record = new Dictionary<string, string>();
                record.Add(id, data);
                store.Add(dataType, record);
            }
            else
            {
                store[dataType].Add(id, data);
            }
        }

        public void AddFileType(string id, string type)
        {
            dictionaryCreateOrAdd(fileType, id, type);
        }

        public void AddAggregates()
        {
            var ids = store[fileDirectory].Keys;

            foreach (var id in ids)
            {
                AddFileCount(id);
                AddFileSize(id);
            }
        }

        public void AddFileCount(string id)
        {
            dictionaryDelete(fileCount, id);
            var directoryToCountWithin = store[fileDirectory][id];
            var filePat = store[filePattern][id];
            var count = Directory.GetFiles(directoryToCountWithin, filePat, SearchOption.TopDirectoryOnly).Length;
            dictionaryCreateOrAdd(fileCount, id, count.ToString());
        }

        public void AddFileSize(string id)
        {
            dictionaryDelete(fileSize, id);
            var directoryToGetSizesWithin = store[fileDirectory][id];
            var filePat = store[filePattern][id];
            var files = Directory.GetFiles(directoryToGetSizesWithin, filePat, SearchOption.TopDirectoryOnly);
            var size = files.Sum(x => new FileInfo(x).Length) / 1024;
            dictionaryCreateOrAdd(fileSize, id, size.ToString());
        }

        public void AddFileDirectory(string id, string directory)
        {
            dictionaryCreateOrAdd(fileDirectory, id, directory);
        }

        public void AddFileNamePattern(string id, string pattern)
        {
            dictionaryCreateOrAdd(filePattern, id, pattern);
        }
        public string GetCountForId(string id)
        {
            return store[fileCount][id];
        }
        public string GetSizeForId(string id)
        {
            return store[fileSize][id];
        }
        public string GetTypeForId(string id)
        {
            return store[fileType][id];
        }
    }
}
