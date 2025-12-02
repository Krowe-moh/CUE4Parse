using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using CUE4Parse.UE4.Exceptions;

namespace CUE4Parse.GameTypes.RL.Encryption.Aes
{
    public static class RocketLeagueAes
    {
        private static readonly byte[][] KeyList;
        private static readonly HttpClient HttpClient = new();

        static RocketLeagueAes()
        {
            var defaultKey = new[]
            {
                new byte[]
                {
                    0xC7, 0xDF, 0x6B, 0x13, 0x25, 0x2A, 0xCC, 0x71,
                    0x47, 0xBB, 0x51, 0xC9, 0x8A, 0xD7, 0xE3, 0x4B,
                    0x7F, 0xE5, 0x00, 0xB7, 0x7F, 0xA5, 0xFA, 0xB2,
                    0x93, 0xE2, 0xF2, 0x4E, 0x6B, 0x17, 0xE7, 0x79
                }
            };

            // I'd like to move this to fmodel but it's not simple, either a text field with all keys or url
            string[] remoteKeys = HttpClient.GetStringAsync("https://raw.githubusercontent.com/ShinyEmii/Toga-Files/refs/heads/master/aes.txt").GetAwaiter().GetResult().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            var allKeys = new System.Collections.Generic.List<byte[]>(defaultKey);
            foreach (var line in remoteKeys)
            {
                if (!string.IsNullOrWhiteSpace(line))
                    allKeys.Add(Convert.FromBase64String(line.Trim()));
            }

            KeyList = allKeys.ToArray();
        }

        public static bool Decrypt(byte[] inputData, int offset, bool upk, out byte[] outputData)
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

                    if (upk)
                    {
                        byte[] decrypted = decryptor.TransformFinalBlock(inputData, 0, inputData.Length);

                        using var ms = new MemoryStream(decrypted);
                        using var br = new BinaryReader(ms);
                        br.ReadBytes(offset);
                        int count = br.ReadInt32();
                        long expectedSize = 4 + count * 24L;

                        if (expectedSize > 0 && expectedSize <= decrypted.Length)
                        {
                            outputData = decrypted;
                            return true;
                        }
                    }
                    else
                    {
                        int headerLen = Math.Min(256, inputData.Length);
                        byte[] decrypted = decryptor.TransformFinalBlock(inputData, 0, headerLen);
                        if (Encoding.ASCII.GetString(decrypted, 0, 4) == "RIFF")
                        {
                            outputData = decryptor.TransformFinalBlock(inputData, 0, inputData.Length);
                            return true;
                        }
                    }
                }
                catch
                {
                    // ignore
                }
            }
            throw new InvalidAesKeyException("No valid decryption key found");
        }
    }
}
