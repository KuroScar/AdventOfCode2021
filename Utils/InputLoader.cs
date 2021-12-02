using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Utils
{
    public class InputLoader
    {
        public static IEnumerable<string> Load(string index = "0")
        {
            return File.ReadLines($"./Input/input{index}.txt");
        }

        private static IEnumerable<string[]> LoadAndSplit(string index)
        {
            return Load(index).Select(item => item.Split(" "));
        }

        // Playing around with generic parsing
        public static IEnumerable<T> Load<T>(string index = "0") where T : ITuple
        {
            return LoadAndSplit(index).Select(ConvertType<T>);
        }

        private static T ConvertType<T>(string[] values)
                where T : ITuple
        {
            object result = Activator.CreateInstance(typeof(T));
            for (int i = 0; i < values.Length; i++)
            {
                var fieldInfo = typeof(T).GetField("Item" + (i + 1));
                fieldInfo?.SetValue(result, ConvertValue(values[i], fieldInfo.FieldType));
            }

            return (T)result;
        }

        private static object ConvertValue(string value, Type type)
        {
            if (type.IsEnum)
            {
                return Enum.Parse(type, value, true);
            }

            return Convert.ChangeType(value, type);
        }
    }
}