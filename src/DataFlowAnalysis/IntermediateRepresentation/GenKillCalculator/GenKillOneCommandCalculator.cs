using System;
using System.Collections.Generic;
using System.Linq;
using DataFlowAnalysis.ControlFlowGraph;
using DataFlowAnalysis.ThreeAddressCode.Model;

namespace DataFlowAnalysis.IntermediateRepresentation.GenKillCalculator
{
    class GenKillOneCommandCalculator
    {
        private Tuple<int, int> Gen;
        private HashSet<Tuple<int, int>> Kill;
        private Dictionary<string, HashSet<Tuple<int, int>>> PairStorage;
        private Graph graph;

        public GenKillOneCommandCalculator(Graph g)
        {
            graph = g;
            PairStorage = new Dictionary<string, HashSet<Tuple<int, int>>>();
            for (int i = 0; i < graph.CFG.Vertices.Count(); i++)
                for (int j = 0; j < graph.CFG.Vertices.ElementAt(i).Commands.Count; j++)
                {
                    var command = graph.CFG.Vertices.ElementAt(i).Commands[j];
                    if (command is Assignment)
                        if (PairStorage.ContainsKey((command as Assignment).Target))
                            PairStorage[(command as Assignment).Target].Add(new Tuple<int, int>(i, j));
                        else
                            PairStorage.Add((command as Assignment).Target,
                                new HashSet<Tuple<int, int>> { new Tuple<int, int>(i, j) });
                }
            Kill = new HashSet<Tuple<int, int>>();
        }

        public Tuple<Tuple<int, int>, HashSet<Tuple<int, int>>> CalculateGenAndKill(int blockId, int commandId)
        {
            Kill.Clear();
            var command = graph.CFG.Vertices.ElementAt(blockId).Commands[commandId];
            if (command is Assignment)
            {
                Gen = new Tuple<int, int>(blockId, commandId);

                string target = (command as Assignment).Target;
                foreach (var pair in PairStorage[target])
                    if (pair.Item1 != blockId && pair.Item2 != commandId)
                        Kill.Add(new Tuple<int, int>(pair.Item1, pair.Item2));
            }
            return new Tuple<Tuple<int, int>, HashSet<Tuple<int, int>>>(Gen, Kill);
        }
    }
}
