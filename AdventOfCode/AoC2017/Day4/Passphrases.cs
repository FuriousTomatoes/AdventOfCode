using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.AoC2017.Day4
{
    public class Passphrases
    {
        public static int Part1(string input)
        {
            return input.Split(Environment.NewLine).ToList().Count(passphrase =>
            {
                var list = passphrase.Split(' ').ToList();
                return !list.Any(password => list.Count(p => p == password) > 1);
            });
        }

        public static int Part2(string input)
        {
            return input.Split(Environment.NewLine).ToList().Count(passphrase =>
            {
                var list = passphrase.Split(' ').ToList();
                return !list.Any(password => list.Count(p => AreAnagrams(p, password)) > 1);
            });
        }

        private static bool AreAnagrams(string s1, string s2)
        {
            if (s1.Length != s2.Length) return false;

            for (int i = 0; i < s1.Length; i++)
                if (s2.Contains(s1[i]))
                    s2 = s2.Remove(s2.IndexOf(s1[i]), 1);
                else return false;
            return true;
        }
    }
}