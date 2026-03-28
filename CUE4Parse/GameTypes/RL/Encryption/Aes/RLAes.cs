using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.GameTypes.RL.Encryption.Aes
{
    public static class RocketLeagueAes
    {
        private static readonly byte[][] KeyList;
        private static readonly HttpClient HttpClient = new();

        static RocketLeagueAes()
        {
            KeyList = LoadKeys();
        }

        private static byte[][] LoadKeys()
        {
            var defaultKey = new[] { new byte[] { 0xC7, 0xDF, 0x6B, 0x13, 0x25, 0x2A, 0xCC, 0x71, 0x47, 0xBB, 0x51, 0xC9, 0x8A, 0xD7, 0xE3, 0x4B, 0x7F, 0xE5, 0x00, 0xB7, 0x7F, 0xA5, 0xFA, 0xB2, 0x93, 0xE2, 0xF2, 0x4E, 0x6B, 0x17, 0xE7, 0x79 } };

            try
            {
                string content = HttpClient
                    .GetStringAsync("https://raw.githubusercontent.com/ShinyEmii/Toga-Files/refs/heads/master/aes.txt")
                    .GetAwaiter()
                    .GetResult();

                var lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                var allKeys = new System.Collections.Generic.List<byte[]>(defaultKey);

                foreach (var line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                        allKeys.Add(Convert.FromBase64String(line.Trim()));
                }

                return allKeys.ToArray();
            }
            catch
            {
                return defaultKey;
            }
        }

        private static bool TryDecryptWithKey(byte[] inputData, byte[] key, int CheckSumDataOffset, bool upk, out byte[]? outputData)
        {
            outputData = null;
            try
            {
                using var aes = new RijndaelManaged { KeySize = 256, Key = key, Mode = CipherMode.ECB, Padding = PaddingMode.None };

                using var decryptor = aes.CreateDecryptor();

                if (upk)
                {
                    byte[] decrypted = decryptor.TransformFinalBlock(inputData, 0, inputData.Length);

                    var Ar = new FByteArchive("Rocket League - Decrypted Package", decrypted);
                    Ar.Position = CheckSumDataOffset;
                    byte prev = Ar.Read<byte>();

                    for (long i = CheckSumDataOffset + 1; i < Ar.Length; ++i)
                    {
                        byte curr = Ar.Read<byte>();
                        if (curr != (byte) ((prev + 1) % 255))
                            return false;

                        prev = curr;
                    }

                    outputData = decrypted;
                    return true;
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
                // ignored
            }

            return false;
        }

        public static bool Decrypt(byte[] inputData, int CheckSumDataOffset, bool upk, out byte[] outputData)
        {
            foreach (var key in KeyList)
            {
                if (TryDecryptWithKey(inputData, key, CheckSumDataOffset, upk, out var result))
                {
                    outputData = result!;
                    return true;
                }
            }

            throw new InvalidAesKeyException("No valid decryption key found");
        }
    }
}
