using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Sssh.FileManagement.FileHelper;

namespace Sssh.FileManagement
{
    public interface IFileHelper
    {
        FileType Type { get; }
        IDictionary<int, double> loadFile(string filePath);
        IDictionary<int, double> Data { get; set; }
    }

    public class FileHelper : IFileHelper
    {
        private FileType type;
        protected IDictionary<int, double> rawData;

        public IDictionary<int,double> loadFile(string filePath)
        {
            IDictionary<int, double> rawData = new Dictionary<int, double>();
            string rawContent = string.Empty;
            int count = 1;

            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    rawContent = line;
                }
            }

            string[] split = rawContent.Split(',');

            foreach (string s in split)
            {
                rawData.Add(count, double.Parse(s));
                count++;
            }

            return rawData;
        }



        public IDictionary<int, double> Data { get => rawData; set => rawData = value; }
        FileType IFileHelper.Type { get => type; }

        public enum FileType
        {
            csv,
            txt,
            None
        }
    }
}
