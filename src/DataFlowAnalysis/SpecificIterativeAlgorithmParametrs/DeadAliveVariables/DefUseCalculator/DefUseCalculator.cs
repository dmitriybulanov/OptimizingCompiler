using DataFlowAnalysis.BasicBlockCode.Model;
using DataFlowAnalysis.ControlFlowGraph;
using DataFlowAnalysis.Utilities;
using DataFlowAnalysis.IterativeAlgorithmParameters.Model;
using DataFlowAnalysis.ThreeAddressCode.Model;
using System.Collections.Generic;
using System.Linq;
using System;


namespace DataFlowAnalysis.DefUseCalculator
{
	public class DefUseCalculator
	{
		private Dictionary<int, Tuple<ISet<string>, ISet<string>>> SetStorage;

		public DefUseCalculator(Graph g)
		{
			SetStorage = new Dictionary<int, Tuple<ISet<string>, ISet<string>>>();
			foreach (BasicBlock block in g) {
				//SetStorage.Add(block.BlockId, CreateDefUseSets(v));
			}
		}

		private Tuple<ISet<string>, ISet<string>> CreateDefUseSets(BasicBlock block)
		{
			ISet<string> Def = new HashSet<string>();
			ISet<string> Use = new HashSet<string>();

			foreach (var command in block.Commands)
			{
				if (command is Assignment)  // todo switch
				{   
					Def.Add(((Assignment)command).Target);
					// todo parsing expression
				}
				if (command is ConditionalGoto)
				{
					// todo parsing expression
				}
				if (command is Print)
				{
					// todo parsing expression
				}
			}
			return new Tuple<ISet<string>, ISet<string>>(Def, Use);
		}


		public Tuple<ISet<string>, ISet<string>> GetDefUseSetsById(int id)
		{
			return SetStorage[id];
		}
	}
}
