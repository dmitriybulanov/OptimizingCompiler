using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataFlowAnalysis.BasicBlockCode.Model;
using DataFlowAnalysis.IterativeAlgorithmParameters;
using DataFlowAnalysis.ThreeAddressCode.Model;
using DataFlowAnalysis.Utilities;

namespace DataFlowAnalysis.SpecificIterativeAlgorithmParametrs.AvailableExpressions
{
    public class AvailableExpressionsCalculator : SetIterativeAlgorithmParameters<Expression>
    {
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
                    .Select(x => new Identifier(x.Target) as Expression));
        }

        public override ISet<Expression> TransferFunction(ISet<Expression> input, BasicBlock block)
        {
            var difference = SetFactory.GetSet();
            var kill = GetKill(block);
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
        }
    }
}
