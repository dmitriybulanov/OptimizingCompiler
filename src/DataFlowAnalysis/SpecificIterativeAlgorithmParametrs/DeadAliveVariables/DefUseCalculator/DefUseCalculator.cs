using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;
using DataFlowAnalysis.IntermediateRepresentation.ControlFlowGraph;
using DataFlowAnalysis.Utilities;
using DataFlowAnalysis.IterativeAlgorithm.IterativeAlgorithmParameters;
using DataFlowAnalysis.IterativeAlgorithm.IterativeAlgorithmParameters.Model;
using DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model;
using System.Collections.Generic;
using System.Linq;
using System;


namespace DataFlowAnalysis.SpecificIterativeAlgorithmParametrs.DefUseCalculator
{
	public class DefUseBlockCalculator
	{
		private Dictionary<int, Tuple<ISet<string>, ISet<string>>> SetStorage;

		public DefUseBlockCalculator()
		{
			SetStorage = new Dictionary<int, Tuple<ISet<string>, ISet<string>>>();
		}

		private void ExpressionParser(Expression expr, ISet<string> Def, ISet<string> Use)
		{
			if (expr.GetType() == typeof(BinaryOperation))
			{
				ExpressionParser(((BinaryOperation)expr).Left, Def, Use);
				ExpressionParser(((BinaryOperation)expr).Right, Def, Use);
			}
			if (expr.GetType() == typeof(Identifier))
			{
				if (!Def.Contains(((Identifier)expr).Name))
				{
					Use.Add(((Identifier)expr).Name);
				}
			}
			if (expr.GetType() == typeof(UnaryOperation))
			{
				ExpressionParser(((UnaryOperation)expr).Operand, Def, Use);
			}
		}

		private Tuple<ISet<string>, ISet<string>> CreateDefUseSets(BasicBlock block)
		{
			ISet<string> Def = new HashSet<string>();
			ISet<string> Use = new HashSet<string>();

			foreach (var command in block.Commands)
			{
				if (command.GetType() == typeof(Assignment))  // todo switch
				{
					Def.Add(((Assignment)command).Target.Name);
					ExpressionParser(((Assignment)command).Value, Def, Use);
				}
				if (command.GetType() == typeof(ConditionalGoto))
				{
					ExpressionParser(((ConditionalGoto)command).Condition, Def, Use);
				}
				if (command.GetType() == typeof(Print))
				{
					ExpressionParser(((Print)command).Argument, Def, Use);
				}
			}
			return new Tuple<ISet<string>, ISet<string>>(Def, Use);
		}


		public Tuple<ISet<string>, ISet<string>> GetDefUseSetsByBlock(BasicBlock block)
		{
			if (!SetStorage.Keys.Contains(block.BlockId))
			{
				SetStorage.Add(block.BlockId, CreateDefUseSets(block));
			}
			return SetStorage[block.BlockId];
		}
	}
}
