using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.AoC2017.Day12
{
    public class Program
    {
        public string Name { get; set; }
        public List<Program> ConnectedPrograms { get; set; } = new();
    }

    public class DigitalPlumber
    {
        public List<Program> Programs = new();

        public void Populate(string input)
        {
            foreach (string programString in input.Split(Environment.NewLine))
            {
                string[] data = programString.Split(" <-> ");
                string[] connectedPrograms = data[1].Split(", ");

                Program program = GetProgram(data[0]);

                foreach (string connectedProgram in connectedPrograms)
                    program.ConnectedPrograms.Add(GetProgram(connectedProgram));
            }
        }

        public Program GetProgram(string name)
        {
            Program program = Programs.FirstOrDefault(p => p.Name == name);
            if (program == null)
            {
                program = new() { Name = name };
                Programs.Add(program);
            }
            return program;
        }

        public List<Program> ProgramsConnectedTo(string name, List<Program> alreadyConnectedPrograms = null)
        {
            if (alreadyConnectedPrograms == null) alreadyConnectedPrograms = new();
            Program program = GetProgram(name);

            List<Program> newConnections = program.ConnectedPrograms.Except(alreadyConnectedPrograms).ToList();
            alreadyConnectedPrograms.AddRange(newConnections);

            foreach (Program connectedProgram in newConnections)
                ProgramsConnectedTo(connectedProgram.Name, alreadyConnectedPrograms);

            return alreadyConnectedPrograms;
        }

        public int GroupsCount()
        {
            int count = 0;
            List<Program> programs = new(Programs);

            while (programs.Count > 0)
            {
                programs = programs.Except(ProgramsConnectedTo(programs[0].Name)).ToList();
                count++;
            }

            return count;
        }
    }
}