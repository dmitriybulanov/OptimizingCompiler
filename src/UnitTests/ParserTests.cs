using System;
using GPPGParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyntaxTree;
using SyntaxTree.SyntaxNodes;

namespace UnitTests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void ParserTest()
        {
            string text = @"
            a = 40;
            b = a + 2;
            print(b);";
            SyntaxNode root = ParserWrap.Parse(text);

            var statements = new StatementList();
            statements.Add(new AssignmentStatement(new Identifier("a"), new Int32Const(40)));
            statements.Add(new AssignmentStatement(
                new Identifier("b"),
                new BinaryExpression(
                    new Identifier("a"),
                    Operation.Add,
                    new Int32Const(2))));
            statements.Add(new PrintStatement(new Identifier("b"), false));

            Assert.AreEqual(root, new Program(statements));
        }
    }
}
