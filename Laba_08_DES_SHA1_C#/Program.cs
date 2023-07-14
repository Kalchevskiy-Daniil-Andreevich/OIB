using System.Security.Cryptography;
using System.Text;
using System;
using System.IO;
//пространство имён для выполнения криптографических операций под названием System.Security.Cryptography. Данное пространство имён предоставляет
//криптографические службы, включающие безопасное кодирование и декодирование данных, а также другие операции, такие как хеширование сообщений,
//генерация случайных чисел и проверка подлинности сообщений. 
// Данная библиотека предоставляет доступ для использования различных реализаций алгоритмов
internal class Program
{
class DESSample 
{
    public static void EncryptTextToFile(string text, string path, byte[] key, byte[] iv)
    {
        try
        {
            using (FileStream fStream = File.Open(path, FileMode.Create))
            using (DES des = DES.Create())
            using (ICryptoTransform encryptor = des.CreateEncryptor(key, iv))
            using (var cStream = new CryptoStream(fStream, encryptor, CryptoStreamMode.Write))
            {
                byte[] toEncrypt = Encoding.UTF8.GetBytes(text);
                cStream.Write(toEncrypt, 0, toEncrypt.Length);
            }
                using (StreamReader reader = new StreamReader(path, System.Text.Encoding.UTF8))
                {
                    string text1 = reader.ReadToEnd();
                    Console.WriteLine("Encrypted text: " + text1);
                }
            }
        catch (CryptographicException e)
        {
            Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
            throw;
        }
    }

    public static string DecryptTextFromFile(string path, byte[] key, byte[] iv)
    {
        try
        {
            using (FileStream fStream = File.OpenRead(path))
            using (DES des = DES.Create())
            using (ICryptoTransform decryptor = des.CreateDecryptor(key, iv))
            using (var cStream = new CryptoStream(fStream, decryptor, CryptoStreamMode.Read))
                using (StreamReader reader = new StreamReader(cStream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
        }
        catch (CryptographicException e)
        {
            Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
            throw;
        }
    }
        static void Main()
        {
            try
            {
                byte[] key;
                byte[] iv;

                using (DES des = DES.Create())
                {
                    key = des.Key;
                    iv = des.IV;
                }

                string original = "Kalchevskiy Daniil";

                string filename = "CText.txt";
                Console.WriteLine("Original text: {0}", original);
                EncryptTextToFile(original, filename, key, iv);
                
                string decrypted = DecryptTextFromFile(filename, key, iv);
                Console.WriteLine($"Decrypted text: {decrypted}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            byte[] hashValue = new byte[256];

            using (SHA1 mysha1 = SHA1.Create())
            {
                hashValue = mysha1.ComputeHash(File.ReadAllBytes("sha.txt"));
                File.WriteAllBytes("hash.txt", hashValue);
            }

        }
    }
}