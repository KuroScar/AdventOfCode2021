using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Utils
{
    public class InputLoader
    {
        public static IEnumerable<string> Load(string index = "0")
        {
            return File.ReadAllLines($"./Input/input{index}.txt");
        }

        private static IEnumerable<string[]> LoadAndSplit(string index)
        {
            return Load(index).Select(item => item.Split(" "));
        }

        // Playing around with generic parsing
        public static IEnumerable<(T1, T2)> Load<T1, T2>(string index = "0")
        {
            return LoadAndSplit(index).Select(ConvertType<T1, T2>);
        }

        public static IEnumerable<(T1, T2, T3)> Load<T1, T2, T3>(string index = "0")
        {
            return LoadAndSplit(index).Select(ConvertType<T1, T2, T3>);
        }

        public static IEnumerable<(T1, T2, T3, T4)> Load<T1, T2, T3, T4>(string index = "0")
        {
            return LoadAndSplit(index).Select(ConvertType<T1, T2, T3, T4>);
        }

        private static T ConvertType<T>(string value)
        {
            if (typeof(T).IsEnum) {
                return (T)Enum.Parse(typeof(T), value, true);
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }

        private static (T1, T2) ConvertType<T1, T2>(string[] splitList)
        {
            return (ConvertType<T1>(splitList[0]), ConvertType<T2>(splitList[1]));
        }

        private static (T1, T2, T3) ConvertType<T1, T2, T3>(string[] splitList)
        {
            (T1, T2) firstPart = ConvertType<T1, T2>(splitList);
            return (firstPart.Item1, firstPart.Item2, ConvertType<T3>(splitList.Skip(2).First()));
        }

        private static (T1, T2, T3, T4) ConvertType<T1, T2, T3, T4>(string[] splitList)
        {
            (T1, T2, T3) firstPart = ConvertType<T1, T2, T3>(splitList);
            return (firstPart.Item1, firstPart.Item2, firstPart.Item3, ConvertType<T4>(splitList.Skip(3).First()));
        }
    }
}