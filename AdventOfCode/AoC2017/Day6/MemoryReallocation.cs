using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.AoC2017.Day6
{
    public class MemoryReallocation
    {
        public static int Part1(string input)
        {
            var state = input.Split('\t').ToList().ConvertAll(int.Parse);
            var seenStates = new List<int[]>().ConvertAll(x => new int[state.Count]);
            seenStates.Add(state.ToArray());

            while (true)
            {
                int extraBlocks = state.Max();
                int maxBankIndex = state.IndexOf(extraBlocks);
                state[maxBankIndex] = 0;

                for (int i = maxBankIndex + 1; extraBlocks > 0; i++)
                {
                    if (i == state.Count) i = 0;
                    state[i]++;
                    extraBlocks--;
                }

                bool hasBeenAlreadySeen = true;
                foreach (var s in seenStates)
                {
                    for (int i = 0; i < s.Length; i++)
                    {
                        hasBeenAlreadySeen = true;
                        if (s[i] != state[i])
                        {
                            hasBeenAlreadySeen = false;
                            break;
                        }
                    }
                    if (hasBeenAlreadySeen) return seenStates.Count;
                }

                seenStates.Add(state.ToArray());
            }
        }

        public static int Part2(string input)
        {
            var state = input.Split('\t').ToList().ConvertAll(int.Parse);
            var seenStates = new List<int[]>().ConvertAll(x => new int[state.Count]);
            seenStates.Add(state.ToArray());

            while (true)
            {
                int extraBlocks = state.Max();
                int maxBankIndex = state.IndexOf(extraBlocks);
                state[maxBankIndex] = 0;

                for (int i = maxBankIndex + 1; extraBlocks > 0; i++)
                {
                    if (i == state.Count) i = 0;
                    state[i]++;
                    extraBlocks--;
                }

                bool hasBeenAlreadySeen = true;
                for (int s = 0; s < seenStates.Count; s++)
                {
                    for (int i = 0; i < seenStates[s].Length; i++)
                    {
                        hasBeenAlreadySeen = true;
                        if (seenStates[s][i] != state[i])
                        {
                            hasBeenAlreadySeen = false;
                            break;
                        }
                    }
                    if (hasBeenAlreadySeen) return seenStates.Count - s;
                }

                seenStates.Add(state.ToArray());
            }
        }
    }
}