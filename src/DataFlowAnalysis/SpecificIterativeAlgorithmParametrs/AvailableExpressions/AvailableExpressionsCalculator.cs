using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;
using DataFlowAnalysis.IntermediateRepresentation.ControlFlowGraph;
using DataFlowAnalysis.IterativeAlgorithm.IterativeAlgorithmParameters;
using DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model;
using DataFlowAnalysis.Utilities;

namespace DataFlowAnalysis.SpecificIterativeAlgorithmParametrs.AvailableExpressions
{
    public class AvailableExpressionsCalculator : SetIterativeAlgorithmParameters<Expression>
    {
        private Graph Graph;
        public AvailableExpressionsCalculator(Graph g)
        {
            Graph = g;
        }

        public override ISet<Expression> GatherOperation(IEnumerable<ISet<Expression>> blocks)
        {
            ISet<Expression> intersection = SetFactory.GetSet((IEnumerable<Expression>)blocks.First());
            foreach (var block in blocks.Skip(1))
            {
                intersection.IntersectWith(block);
            }

            return intersection;
        }

        public override ISet<Expression> GetGen(BasicBlock block)
        {
            return SetFactory.GetSet(
                block.Commands
                    .OfType<Assignment>()
                    .Select(x => x.Value));
        }

        public override ISet<Expression> GetKill(BasicBlock block)
        {
            return SetFactory.GetSet(
                block.Commands
                    .OfType<Assignment>()
                    .Select(x => x.Target as Expression));
        }

        public override ISet<Expression> TransferFunction(ISet<Expression> input, BasicBlock block)
        {
            var kill = GetKill(block).Cast<Identifier>();
            var result = SetFactory.GetSet(
                input.Where(inputExpression =>
                !kill.Any(inputExpression.HasIdentifiedSubexpression)));
            result.UnionWith(GetGen(block));

            return result;
            /*
             * Non-LINQ realization
            var difference = SetFactory.GetSet();
            var foundInKill = false;
            foreach (var inputExpression in input)
            {
                foreach (var killExpression in kill)
                {
                    if (!inputExpression.HasIdentifiedSubexpression(killExpression as Identifier))
                        continue;

                    foundInKill = true;
                    break;
                }

                if (!foundInKill)
                    difference.Add(inputExpression);

                foundInKill = false;
            }
            return SetFactory.GetSet(GetGen(block).Union(difference));
            */
        }

        public override bool ForwardDirection { get { return true; } }

        public override ISet<Expression> FirstValue { get { return SetFactory.GetSet<Expression>(); } }

        public override ISet<Expression> StartingValue
        {
            get
            {
                HashSet<Expression> result = new HashSet<Expression>();
                foreach (BasicBlock b in Graph)
                    foreach (ThreeAddressCommand c in b.Commands)
                        if (c.GetType() == typeof(Assignment))
                        {
                            result.Add((c as Assignment).Value);
                            result.Add((c as Assignment).Target);
                        }
                return SetFactory.GetSet<Expression>(result);
            }
        }
    }
}
