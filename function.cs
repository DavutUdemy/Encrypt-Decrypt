using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace ConsoleApp3
{
    internal class EncryptMail
    {
        static void Main(string[] args)
        {
            EncryptFile("C:\\Users\\denem\\Downloads\\encrypt\\dkdkd.docx");
            
        }
        public static void EncryptFile(string filePath)
        {
            ReadOnlySpan<byte> clearBytes = File.ReadAllBytes(filePath).AsSpan();
            string EncryptionKey = "753ovb050y7hdd9ly2f7r4h3lq7jj2c89qbf";
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x47, 0x76, 0x62, 0x6f, 0x20, 0x4d, 0x73, 0x64, 0x73, 0x65, 0x55, 0x20, 0x33 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes.ToArray(), 0, clearBytes.Length);
                        cs.FlushFinalBlock();
                        cs.Close();
                    }
                    clearBytes = ms.ToArray();
                }
            }
            File.WriteAllBytes(filePath, clearBytes.ToArray());
        }
        public static void DecryptFile(string filePath)
        {
            ReadOnlySpan<byte> cipherBytes = File.ReadAllBytes(filePath).AsSpan();
            // Get encryption key from config file
            string EncryptionKey = "753ovb050y7hdd9ly2f7r4h3lq7jj2c89qbf";
            //string EncryptionKey = ConfigurationManager.AppSettings["EncryptionKey"];            
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x47, 0x76, 0x62, 0x6f, 0x20, 0x4d, 0x73, 0x64, 0x73, 0x65, 0x55, 0x20, 0x33 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes.ToArray(), 0, cipherBytes.Length);
                        cs.FlushFinalBlock();
                        cs.Close();
                    }
                    cipherBytes = ms.ToArray();
                }
            }
            File.WriteAllBytes(filePath, cipherBytes.ToArray());
        }
        public static void EncryptAllFilesInFolder(string folderPath, string fileType)
        {
            DirectoryInfo d = new DirectoryInfo(folderPath);
            FileInfo[] Files = d.GetFiles(fileType);
            string EncryptionKey = "753ovb050y7hdd9ly2f7r4h3lq7jj2c89qbf";
            foreach (FileInfo file in Files)
            {
                string filePath = $"{folderPath}\\{file.Name}";

                FileAttributes attributes = File.GetAttributes(filePath);
                //System, Offline, Readonly
                if (!((attributes & FileAttributes.Offline) == FileAttributes.Offline))
                {
                    ReadOnlySpan<byte> clearBytes = File.ReadAllBytes(filePath).AsSpan();
                    using (Aes encryptor = Aes.Create())
                    {
                        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                        encryptor.Key = pdb.GetBytes(32);
                        encryptor.IV = pdb.GetBytes(16);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                            {
                                cs.Write(clearBytes.ToArray(), 0, clearBytes.Length);
                                cs.FlushFinalBlock();
                                cs.Close();
                            }
                            clearBytes = ms.ToArray();
                        }
                    }
                    File.WriteAllBytes(filePath, clearBytes.ToArray());
                    File.SetAttributes(filePath, File.GetAttributes(filePath) | FileAttributes.Offline);
                }
            }
        }
        public static void DecryptAllFilesInFolder(string folderPath, string fileType)
        {
            DirectoryInfo d = new DirectoryInfo(folderPath);
            FileInfo[] Files = d.GetFiles(fileType);
            string EncryptionKey = "753ovb050y7hdd9ly2f7r4h3lq7jj2c89qbf";
            foreach (FileInfo file in Files)
            {
                try
                {
                    string filePath = $"{folderPath}\\{file.Name}";
                    FileAttributes attributes = File.GetAttributes(filePath);
                    if (((attributes & FileAttributes.Offline) == FileAttributes.Offline))
                    {
                        attributes = attributes & ~FileAttributes.Offline;
                        File.SetAttributes(filePath, attributes);

                        ReadOnlySpan<byte> cipherBytes = File.ReadAllBytes(filePath).AsSpan();
                        using (Aes encryptor = Aes.Create())
                        {
                            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                            encryptor.Key = pdb.GetBytes(32);
                            encryptor.IV = pdb.GetBytes(16);
                            using (MemoryStream ms = new MemoryStream())
                            {
                                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                                {
                                    cs.Write(cipherBytes.ToArray(), 0, cipherBytes.Length);
                                    cs.FlushFinalBlock();
                                    cs.Close();
                                }
                                cipherBytes = ms.ToArray();
                            }
                        }
                        File.WriteAllBytes(filePath, cipherBytes.ToArray());
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
        public static bool MatchFiles(string sourceFilePath, string targetFilePath)
        {
            return File.ReadAllBytes(sourceFilePath).SequenceEqual(File.ReadAllBytes(targetFilePath));
        }
        public static List<string> MatchAllFilesInFolder(string sourcePath, string targetPath)
        {
            DirectoryInfo d = new DirectoryInfo(sourcePath);

            FileInfo[] Files = d.GetFiles();
            List<string> diffFilesList = new List<string>();
            foreach (FileInfo file in Files)
            {
                string sourceFilePath = $"{sourcePath}\\{file.Name}";
                string targetFilePath = $"{targetPath}\\{file.Name}";
                if (!MatchFiles(sourceFilePath, targetFilePath))
                {
                    diffFilesList.Add(file.Name);
                }
                else
                {
                    Console.WriteLine($"Şifrelenmemiş Dosya:{file.Name}");
                }
            }
            return diffFilesList;
        }
    }
}
