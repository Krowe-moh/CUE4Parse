using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace CUE4Parse.GameTypes.RL.Encryption.Aes
{
    public static class RocketLeagueAes
    {
        private static readonly List<byte[]> KeyList = new();

        static RocketLeagueAes()
        {
            KeyList.Add(new byte[]
            {
                0xC7, 0xDF, 0x6B, 0x13, 0x25, 0x2A, 0xCC, 0x71,
                0x47, 0xBB, 0x51, 0xC9, 0x8A, 0xD7, 0xE3, 0x4B,
                0x7F, 0xE5, 0x00, 0xB7, 0x7F, 0xA5, 0xFA, 0xB2,
                0x93, 0xE2, 0xF2, 0x4E, 0x6B, 0x17, 0xE7, 0x79
            });

            // temp
            if (File.Exists("keys.txt"))
            {
                foreach (var line in File.ReadAllLines("keys.txt"))
                {
                    if (!string.IsNullOrWhiteSpace(line))
                        KeyList.Add(Convert.FromBase64String(line.Trim()));
                }
            }
        }

        public static bool Decrypt(byte[] inputData, out byte[] outputData)
        {
            foreach (var key in KeyList)
            {
                try
                {
                    using var aes = new RijndaelManaged
                    {
                        KeySize = 256,
                        Key = key,
                        Mode = CipherMode.ECB,
                        Padding = PaddingMode.None
                    };
                    using var decryptor = aes.CreateDecryptor();
                    var decrypted = decryptor.TransformFinalBlock(inputData, 0, inputData.Length);

                    using var ms = new MemoryStream(decrypted);
                    using var br = new BinaryReader(ms);

                    int count = br.ReadInt32();
                    long expectedSize = 4 + (count * 24L);

                    if (expectedSize > 0 && expectedSize <= decrypted.Length)
                    {
                        outputData = decrypted;
                        return true;
                    }
                }
                catch
                {
                    // ignore
                }
            }

            outputData = null;
            return false;
        }
    }
}