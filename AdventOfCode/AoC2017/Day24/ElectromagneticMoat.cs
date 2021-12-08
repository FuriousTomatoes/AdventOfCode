using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.AoC2017.Day24
{
    public class Component
    {
        public byte PortAPins { get; set; }
        public byte PortBPins { get; set; }

        public Component(byte portAPins, byte portBPins)
        {
            PortAPins = portAPins;
            PortBPins = portBPins;
        }
    }

    public class ComponentNode
    {
        public Component Component { get; set; }
        public List<ComponentNode> PossibleConnectedNodes { get; set; } = new();
        public bool IsInverted { get; set; }

        public ComponentNode(Component component, bool isInverted = false)
        {
            Component = component;
            IsInverted = isInverted;
        }
    }

    public class Bridge
    {
        public List<Component> Components { get; set; } = new();
        public ComponentNode Root { get; set; }

        public Bridge(string input)
        {
            foreach (string componentString in input.Split(Environment.NewLine))
            {
                var ports = componentString.Split('/');
                Components.Add(new(byte.Parse(ports[0]), byte.Parse(ports[1])));
            }

            Root = new(new(0, 0));
            BuildBridgeTree();
        }

        private void BuildBridgeTree() => AddPossibleConnections(Root, new());

        private void AddPossibleConnections(ComponentNode node, List<Component> except)
        {
            except.Add(node.Component);
            foreach (Component component in Components.Except(except))
            {
                if ((node.IsInverted ? node.Component.PortAPins : node.Component.PortBPins) == component.PortAPins) node.PossibleConnectedNodes.Add(new(component));
                if ((node.IsInverted ? node.Component.PortAPins : node.Component.PortBPins) == component.PortBPins) node.PossibleConnectedNodes.Add(new(component, true));
            }

            foreach (ComponentNode subnode in node.PossibleConnectedNodes)
                AddPossibleConnections(subnode, new(except));
        }

        public int GetStrenghtOfTheStronghestBridge() => GetStrenghtOfTheStronghestBranch(Root);

        private int GetStrenghtOfTheStronghestBranch(ComponentNode node)
        {
            int sum = 0;
            if (node.PossibleConnectedNodes.Count > 0)
                sum += node.PossibleConnectedNodes.Max(subNode => GetStrenghtOfTheStronghestBranch(subNode));
            sum += node.Component.PortBPins + node.Component.PortAPins;
            return sum;
        }

        public int GetStrenghtOfTheLonghestBridge() => GetStrenghtOfTheLonghestBranch(Root, out _);

        private int GetStrenghtOfTheLonghestBranch(ComponentNode node, out int length)
        {
            int maxStrenght = 0;
            int maxLength = 0;
            foreach (ComponentNode subnode in node.PossibleConnectedNodes)
            {
                int subnodeStrenght = GetStrenghtOfTheLonghestBranch(subnode, out int subnodeLength);
                if (subnodeLength > maxLength)
                {
                    maxLength = subnodeLength;
                    maxStrenght = subnodeStrenght;
                }
                else if (subnodeLength == maxLength) maxStrenght = Math.Max(subnodeStrenght, maxStrenght);
            }

            length = maxLength + 1;
            return maxStrenght += node.Component.PortBPins + node.Component.PortAPins;
        }
    }
}