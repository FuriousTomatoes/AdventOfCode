namespace AdventOfCode.AoC2017.Day1
{
    class InverseCaptcha
    {
        public static int Part1(string input)
        {
            input += input[0];
            int sum = 0;
            for (int i = 0; i < input.Length - 1; i++)
                if (input[i] == input[i + 1]) sum += input[i] - '0';
            return sum;
        }

        public static int Part2(string input)
        {
            int halfwayRound = input.Length / 2;
            input += input[..halfwayRound];
            int sum = 0;
            for (int i = 0; i < input.Length - halfwayRound; i++)
                if (input[i] == input[i + halfwayRound]) sum += input[i] - '0';
            return sum;
        }
    }
}