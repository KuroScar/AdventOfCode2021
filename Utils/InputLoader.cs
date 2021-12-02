using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Utils
{
    public class InputLoader
    {
        public static IEnumerable<string> Load(int index = 0)
        {
            return File.ReadAllLines($"./Input/input{index}.txt");
        }
    }
}