using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.AoC2017.Day7
{
    public class Tower
    {
        public string Name { get; set; }
        public int Weight { get; set; }
        public int TotalWeight { get; set; }
        public List<Tower> SubTowers { get; set; } = new();
        public Tower ParentTower { get; set; }
    }

    public class RecursiveCircus
    {
        public Tower BottomTower { get; private set; }

        public RecursiveCircus(Tower bottomTower)
        {
            BottomTower = bottomTower;
            TotalWeigth(bottomTower);
        }

        public static Tower GetBottomTowerFromAoCInput(string input)
        {
            string[] strings = input.Split(Environment.NewLine);

            List<string>[] tempSubtowerNames = new List<string>[strings.Length];
            Tower[] towers = new Tower[strings.Length];
            for (int i = 0; i < strings.Length; i++)
            {
                towers[i] = new();
                tempSubtowerNames[i] = new();
            }

            for (int i = 0; i < strings.Length; i++)
            {
                Tower tower = towers[i];
                string t = strings[i];

                if (t.Contains("->"))
                {
                    foreach (string subTowerName in t.Split("->")[1].Split(", "))
                    {
                        string trimmedSubTowerName = subTowerName.Trim();
                        Tower subTower = towers.FirstOrDefault(tower => tower.Name == trimmedSubTowerName);
                        if (subTower != null)
                        {
                            tower.SubTowers.Add(subTower);
                            subTower.ParentTower = tower;
                        }
                        else tempSubtowerNames[i].Add(trimmedSubTowerName);
                    }
                }

                string[] infos = t.Split(' ');

                tower.Name = infos[0];
                var c = infos[1][1..^1];
                tower.Weight = int.Parse(c);

                for (int j = 0; j < tempSubtowerNames.Length; j++)
                    for (int k = 0; k < tempSubtowerNames[j].Count; k++)
                        if (tower.Name == tempSubtowerNames[j][k])
                        {
                            towers[j].SubTowers.Add(tower);
                            tower.ParentTower = towers[j];
                            tempSubtowerNames[j].RemoveAt(k);
                            break;
                        }
            }

            Tower bottomTower = towers[0];
            while (true)
                if (bottomTower.ParentTower != null)
                    bottomTower = bottomTower.ParentTower;
                else return bottomTower;
        }

        public int TotalWeigth(Tower tower)
        {
            tower.TotalWeight = tower.Weight + tower.SubTowers.Sum(subtower => TotalWeigth(subtower));
            return tower.TotalWeight;
        }

        public int ParityWeight(Tower tower, int parityWeight = 0)
        {
            Tower wrongTower = GetWrongTower(tower.SubTowers);

            if (wrongTower == null || wrongTower.SubTowers.TrueForAll(t =>
            t.TotalWeight == wrongTower.SubTowers[0].TotalWeight)) return GetParityWeight(tower);
            return ParityWeight(wrongTower, parityWeight);
        }

        private Tower GetWrongTower(List<Tower> towers)
            => towers.FirstOrDefault(subTower =>
                towers
                    .Where(t => t != subTower)
                    .ToList()
                    .TrueForAll(t => t.TotalWeight != subTower.TotalWeight));

        public int GetParityWeight(Tower tower)
        {
            var wrongTower = GetWrongTower(tower.SubTowers);
            if (wrongTower == null) return tower.SubTowers[0].TotalWeight;

            int parityWeight = (int)tower.SubTowers.Where(tower => tower != wrongTower).Average(tower => tower.TotalWeight);

            return parityWeight - wrongTower.SubTowers.Count * GetParityWeight(wrongTower);
        }
    }
}