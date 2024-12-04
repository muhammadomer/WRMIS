using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace PMIU.WRMIS.Common
{
    public class Encryption
    {
        string passPhrase = "bananax97";
        string saltValue = "pepper";
        string hashAlgorithm = "MD5";
        int passwordIterations = 1;
        string initVector = "koxskfruvdslbsxu";
        int keySize = 128;

        byte[] initVectorBytes;
        byte[] saltValueBytes;

        RijndaelManaged symmetricKey = new RijndaelManaged();
                
        public Encryption()
        {
            initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            symmetricKey.Mode = CipherMode.CBC;
        }

        public string EncryptString(string _PlainInput)
        {
            try
            {

                //string passPhrase = "bananax97";
                //string saltValue = "pepper";
                //string hashAlgorithm = "MD5";
                //int passwordIterations = 1;
                //string initVector = "koxskfruvdslbsxu";
                //int keySize = 128;

                //byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                //byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(_PlainInput);

                PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);
                byte[] keyBytes = password.GetBytes(keySize / 8);

                //RijndaelManaged symmetricKey = new RijndaelManaged();
                //symmetricKey.Mode = CipherMode.CBC;

                ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();

                byte[] cipherTextBytes = memoryStream.ToArray();

                memoryStream.Close();
                cryptoStream.Close();

                string cipherText = Convert.ToBase64String(cipherTextBytes);

                return cipherText;

            }
            catch (Exception e) 
            {
                return "";
            }

            
        }

        public string DecryptString(string _EncryptedInput)
        {
            try 
            {
                //string passPhrase = "bananax97";
                //string saltValue = "pepper";
                //string hashAlgorithm = "MD5";
                //int passwordIterations = 1;
                //string initVector = "koxskfruvdslbsxu";
                //int keySize = 128;

                //byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                //byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
                byte[] cipherTextBytes = Convert.FromBase64String(_EncryptedInput);

                PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase,saltValueBytes,hashAlgorithm,passwordIterations);
                byte[] keyBytes = password.GetBytes(keySize/8);

                //RijndaelManaged symmetricKey = new RijndaelManaged();
                //symmetricKey.Mode = CipherMode.CBC;

                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes,initVectorBytes);
                MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
                CryptoStream cryptoStream = new CryptoStream(memoryStream,decryptor,CryptoStreamMode.Read);

                byte[] plainTextBytes = new byte[cipherTextBytes.Length];

                int decryptedByteCount = cryptoStream.Read(plainTextBytes,0,plainTextBytes.Length);

                memoryStream.Close();
                cryptoStream.Close();

                string plainText = Encoding.UTF8.GetString(plainTextBytes,0,decryptedByteCount);

                return plainText;

            }
            catch (Exception e)
            {
                return "";
            }
        }
    }
}
