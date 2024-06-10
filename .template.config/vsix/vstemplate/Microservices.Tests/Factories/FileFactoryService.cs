using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace $safeprojectname$
{
    public class FileFactoryService
    {
        private static readonly List<Tuple<string, byte[]>> files = new();

        private static readonly FileFactoryService _fileFactoryInstance = new();

        private FileFactoryService()
        {
        }

        public static FileFactoryService GetInstance() => _fileFactoryInstance;

        public async Task<byte[]> ReadAllBytesAsync(string fileName)
        {
            byte[] returnBytes;

            var existingItem = files.FirstOrDefault(f => f.Item1 == fileName);
            if (existingItem?.Item1 != fileName)
            {
                try
                {
                    returnBytes = await File.ReadAllBytesAsync(fileName);
                    files.Add(new Tuple<string, byte[]>(fileName, returnBytes));
                }
                catch
                {
                    returnBytes = null;
                }
                
            }
            else
                returnBytes = existingItem.Item2;

            return returnBytes;
        }
    }
}
