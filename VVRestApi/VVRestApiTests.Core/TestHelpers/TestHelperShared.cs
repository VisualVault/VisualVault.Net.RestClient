using System;
using System.IO;
using System.Reflection;

namespace VVRestApiTests.TestHelpers
{
    public static class TestHelperShared
    {
        public static Stream GetSearchWordTextFileStream()
        {
            const string filename = "VVRestApiTests.Core.TestResources.SearchWordTextFile.txt";

            Assembly assembly = Assembly.GetExecutingAssembly();
            var info = assembly.GetManifestResourceInfo(filename);
            Stream fileStream = assembly.GetManifestResourceStream(filename);
            if (fileStream == null)
            {
                throw new Exception("Could not get the embedded file: " + filename);
            }

            return fileStream;
        }

        public static byte[] GetSearchWordTextFile()
        {
            var fileStream = GetSearchWordTextFileStream();
            using (var ms = new MemoryStream())
            {
                fileStream.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public static Stream GetSampleUploadFileStream()
        {
            const string filename = "VVRestApiTests.TestResources.SampleUploadFile.txt";

            Assembly assembly = Assembly.GetExecutingAssembly();
            var info = assembly.GetManifestResourceInfo(filename);

            Stream fileStream = assembly.GetManifestResourceStream(filename);
            if (fileStream == null)
            {
                throw new Exception("Could not get the embedded file: " + filename);
            }

            return fileStream;
        }

        public static byte[] GetSampleUploadFile()
        {
            var fileStream = GetSampleUploadFileStream();
            using (var ms = new MemoryStream())
            {
                fileStream.CopyTo(ms);
                return ms.ToArray();
            }

        }

        public static Stream GetFileStream(string filePath)
        {
            Stream fileStream = null;

            if (File.Exists(filePath))
            {
                fileStream = File.OpenRead(filePath);
            }
            else
            {
                throw new Exception("Could not get the file: " + filePath);
            }

            return fileStream;
        }

    }
}