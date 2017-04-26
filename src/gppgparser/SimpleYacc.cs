// This code was generated by the Gardens Point Parser Generator
// Copyright (c) Wayne Kelly, QUT 2005-2010
// (see accompanying GPPGcopyright.rtf)

// GPPG version 1.3.6
// Machine:  MIL8A-315-05
// DateTime: ?? 26.04.17 17:28:56
// UserName: User
// Input file <SimpleYacc.y>

// options: no-lines gplex

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using QUT.Gppg;
using SyntaxTree;
using SyntaxTree.SyntaxNodes;

namespace GPPGParser
{
public enum Tokens {
    error=1,EOF=2,ID=3,INUM=4,BEGIN=5,END=6,
    ASSIGN=7,SEMICOLON=8,WHILE=9,DOUBLEDOT=10,FOR=11,IF=12,
    ELSE=13,PRINT=14,PRINTLN=15,OPENROUND=16,CLOSEROUND=17,GOTO=18,
    COLON=19,AND=20,OR=21,LESSER=22,GREATER=23,LESSEREQ=24,
    GREATEREQ=25,EQ=26,NOTEQ=27,NOT=28,PLUS=29,MINUS=30,
    DIVIDE=31,MULT=32};

// Abstract base class for GPLEX scanners
public abstract class ScanBase : AbstractScanner<GPPGParser.Union,LexLocation> {
  private LexLocation __yylloc = new LexLocation();
  public override LexLocation yylloc { get { return __yylloc; } set { __yylloc = value; } }
  protected virtual bool yywrap() { return true; }
}

public class Parser: ShiftReduceParser<GPPGParser.Union, LexLocation>
{
  // Verbatim content from SimpleYacc.y
// ��� ���������� ����������� � ����� GPPGParser, �������������� ����� ������, ������������ �������� gppg
    public Parser(AbstractScanner<Union, LexLocation> scanner) : base(scanner) { }
    public SyntaxNode Root;
  // End verbatim content from SimpleYacc.y

#pragma warning disable 649
  private static Dictionary<int, string> aliasses;
#pragma warning restore 649
  private static Rule[] rules = new Rule[48];
  private static State[] states = new State[90];
  private static string[] nonTerms = new string[] {
      "progr", "statement", "stlist", "assign", "block", "while_cycle", "for_cycle", 
      "if", "print", "goto", "expr", "ident", "unaryop", "parenth", "mulop", 
      "addop", "compar", "andop", "goto_label", "root", "$accept", };

  static Parser() {
    states[0] = new State(new int[]{3,22,5,52,9,56,11,60,12,68,14,75,15,79,18,85},new int[]{-20,1,-1,3,-3,4,-2,89,-4,6,-12,8,-5,51,-6,55,-7,59,-8,67,-9,73,-10,83});
    states[1] = new State(new int[]{2,2});
    states[2] = new State(-1);
    states[3] = new State(-2);
    states[4] = new State(new int[]{3,22,5,52,9,56,11,60,12,68,14,75,15,79,18,85,2,-3},new int[]{-2,5,-4,6,-12,8,-5,51,-6,55,-7,59,-8,67,-9,73,-10,83});
    states[5] = new State(-5);
    states[6] = new State(new int[]{8,7});
    states[7] = new State(-6);
    states[8] = new State(new int[]{7,9});
    states[9] = new State(new int[]{3,22,4,23,16,24,28,37,30,39},new int[]{-11,10,-18,27,-17,28,-16,50,-15,41,-13,35,-14,36,-12,21});
    states[10] = new State(new int[]{21,11,8,-14});
    states[11] = new State(new int[]{3,22,4,23,16,24,28,37,30,39},new int[]{-18,12,-17,28,-16,50,-15,41,-13,35,-14,36,-12,21});
    states[12] = new State(new int[]{20,13,21,-16,8,-16,17,-16,3,-16,5,-16,9,-16,11,-16,12,-16,14,-16,15,-16,18,-16});
    states[13] = new State(new int[]{3,22,4,23,16,24,28,37,30,39},new int[]{-17,14,-16,50,-15,41,-13,35,-14,36,-12,21});
    states[14] = new State(new int[]{26,15,27,29,22,42,23,44,24,46,25,48,20,-18,21,-18,8,-18,17,-18,3,-18,5,-18,9,-18,11,-18,12,-18,14,-18,15,-18,18,-18});
    states[15] = new State(new int[]{3,22,4,23,16,24,28,37,30,39},new int[]{-16,16,-15,41,-13,35,-14,36,-12,21});
    states[16] = new State(new int[]{29,17,30,31,26,-20,27,-20,22,-20,23,-20,24,-20,25,-20,20,-20,21,-20,8,-20,17,-20,3,-20,5,-20,9,-20,11,-20,12,-20,14,-20,15,-20,18,-20});
    states[17] = new State(new int[]{3,22,4,23,16,24,28,37,30,39},new int[]{-15,18,-13,35,-14,36,-12,21});
    states[18] = new State(new int[]{32,19,31,33,29,-27,30,-27,26,-27,27,-27,22,-27,23,-27,24,-27,25,-27,20,-27,21,-27,8,-27,17,-27,3,-27,5,-27,9,-27,11,-27,12,-27,14,-27,15,-27,18,-27,10,-27});
    states[19] = new State(new int[]{3,22,4,23,16,24},new int[]{-14,20,-12,21});
    states[20] = new State(-30);
    states[21] = new State(-35);
    states[22] = new State(-13);
    states[23] = new State(-36);
    states[24] = new State(new int[]{3,22,4,23,16,24,28,37,30,39},new int[]{-11,25,-18,27,-17,28,-16,50,-15,41,-13,35,-14,36,-12,21});
    states[25] = new State(new int[]{17,26,21,11});
    states[26] = new State(-37);
    states[27] = new State(new int[]{20,13,21,-15,8,-15,17,-15,3,-15,5,-15,9,-15,11,-15,12,-15,14,-15,15,-15,18,-15});
    states[28] = new State(new int[]{26,15,27,29,22,42,23,44,24,46,25,48,20,-17,21,-17,8,-17,17,-17,3,-17,5,-17,9,-17,11,-17,12,-17,14,-17,15,-17,18,-17});
    states[29] = new State(new int[]{3,22,4,23,16,24,28,37,30,39},new int[]{-16,30,-15,41,-13,35,-14,36,-12,21});
    states[30] = new State(new int[]{29,17,30,31,26,-21,27,-21,22,-21,23,-21,24,-21,25,-21,20,-21,21,-21,8,-21,17,-21,3,-21,5,-21,9,-21,11,-21,12,-21,14,-21,15,-21,18,-21});
    states[31] = new State(new int[]{3,22,4,23,16,24,28,37,30,39},new int[]{-15,32,-13,35,-14,36,-12,21});
    states[32] = new State(new int[]{32,19,31,33,29,-28,30,-28,26,-28,27,-28,22,-28,23,-28,24,-28,25,-28,20,-28,21,-28,8,-28,17,-28,3,-28,5,-28,9,-28,11,-28,12,-28,14,-28,15,-28,18,-28,10,-28});
    states[33] = new State(new int[]{3,22,4,23,16,24},new int[]{-14,34,-12,21});
    states[34] = new State(-31);
    states[35] = new State(-29);
    states[36] = new State(-32);
    states[37] = new State(new int[]{3,22,4,23,16,24},new int[]{-14,38,-12,21});
    states[38] = new State(-33);
    states[39] = new State(new int[]{3,22,4,23,16,24},new int[]{-14,40,-12,21});
    states[40] = new State(-34);
    states[41] = new State(new int[]{32,19,31,33,29,-26,30,-26,26,-26,27,-26,22,-26,23,-26,24,-26,25,-26,20,-26,21,-26,8,-26,17,-26,3,-26,5,-26,9,-26,11,-26,12,-26,14,-26,15,-26,18,-26,10,-26});
    states[42] = new State(new int[]{3,22,4,23,16,24,28,37,30,39},new int[]{-16,43,-15,41,-13,35,-14,36,-12,21});
    states[43] = new State(new int[]{29,17,30,31,26,-22,27,-22,22,-22,23,-22,24,-22,25,-22,20,-22,21,-22,8,-22,17,-22,3,-22,5,-22,9,-22,11,-22,12,-22,14,-22,15,-22,18,-22});
    states[44] = new State(new int[]{3,22,4,23,16,24,28,37,30,39},new int[]{-16,45,-15,41,-13,35,-14,36,-12,21});
    states[45] = new State(new int[]{29,17,30,31,26,-23,27,-23,22,-23,23,-23,24,-23,25,-23,20,-23,21,-23,8,-23,17,-23,3,-23,5,-23,9,-23,11,-23,12,-23,14,-23,15,-23,18,-23});
    states[46] = new State(new int[]{3,22,4,23,16,24,28,37,30,39},new int[]{-16,47,-15,41,-13,35,-14,36,-12,21});
    states[47] = new State(new int[]{29,17,30,31,26,-24,27,-24,22,-24,23,-24,24,-24,25,-24,20,-24,21,-24,8,-24,17,-24,3,-24,5,-24,9,-24,11,-24,12,-24,14,-24,15,-24,18,-24});
    states[48] = new State(new int[]{3,22,4,23,16,24,28,37,30,39},new int[]{-16,49,-15,41,-13,35,-14,36,-12,21});
    states[49] = new State(new int[]{29,17,30,31,26,-25,27,-25,22,-25,23,-25,24,-25,25,-25,20,-25,21,-25,8,-25,17,-25,3,-25,5,-25,9,-25,11,-25,12,-25,14,-25,15,-25,18,-25});
    states[50] = new State(new int[]{29,17,30,31,26,-19,27,-19,22,-19,23,-19,24,-19,25,-19,20,-19,21,-19,8,-19,17,-19,3,-19,5,-19,9,-19,11,-19,12,-19,14,-19,15,-19,18,-19});
    states[51] = new State(-7);
    states[52] = new State(new int[]{3,22,5,52,9,56,11,60,12,68,14,75,15,79,18,85},new int[]{-3,53,-2,89,-4,6,-12,8,-5,51,-6,55,-7,59,-8,67,-9,73,-10,83});
    states[53] = new State(new int[]{6,54,3,22,5,52,9,56,11,60,12,68,14,75,15,79,18,85},new int[]{-2,5,-4,6,-12,8,-5,51,-6,55,-7,59,-8,67,-9,73,-10,83});
    states[54] = new State(-38);
    states[55] = new State(-8);
    states[56] = new State(new int[]{3,22,4,23,16,24,28,37,30,39},new int[]{-11,57,-18,27,-17,28,-16,50,-15,41,-13,35,-14,36,-12,21});
    states[57] = new State(new int[]{21,11,3,22,5,52,9,56,11,60,12,68,14,75,15,79,18,85},new int[]{-2,58,-4,6,-12,8,-5,51,-6,55,-7,59,-8,67,-9,73,-10,83});
    states[58] = new State(-39);
    states[59] = new State(-9);
    states[60] = new State(new int[]{3,22},new int[]{-12,61});
    states[61] = new State(new int[]{7,62});
    states[62] = new State(new int[]{3,22,4,23,16,24,28,37,30,39},new int[]{-16,63,-15,41,-13,35,-14,36,-12,21});
    states[63] = new State(new int[]{10,64,29,17,30,31});
    states[64] = new State(new int[]{3,22,4,23,16,24,28,37,30,39},new int[]{-16,65,-15,41,-13,35,-14,36,-12,21});
    states[65] = new State(new int[]{29,17,30,31,3,22,5,52,9,56,11,60,12,68,14,75,15,79,18,85},new int[]{-2,66,-4,6,-12,8,-5,51,-6,55,-7,59,-8,67,-9,73,-10,83});
    states[66] = new State(-40);
    states[67] = new State(-10);
    states[68] = new State(new int[]{3,22,4,23,16,24,28,37,30,39},new int[]{-11,69,-18,27,-17,28,-16,50,-15,41,-13,35,-14,36,-12,21});
    states[69] = new State(new int[]{21,11,3,22,5,52,9,56,11,60,12,68,14,75,15,79,18,85},new int[]{-2,70,-4,6,-12,8,-5,51,-6,55,-7,59,-8,67,-9,73,-10,83});
    states[70] = new State(new int[]{13,71,3,-41,5,-41,9,-41,11,-41,12,-41,14,-41,15,-41,18,-41,2,-41,6,-41});
    states[71] = new State(new int[]{3,22,5,52,9,56,11,60,12,68,14,75,15,79,18,85},new int[]{-2,72,-4,6,-12,8,-5,51,-6,55,-7,59,-8,67,-9,73,-10,83});
    states[72] = new State(-42);
    states[73] = new State(new int[]{8,74});
    states[74] = new State(-11);
    states[75] = new State(new int[]{16,76});
    states[76] = new State(new int[]{3,22,4,23,16,24,28,37,30,39},new int[]{-11,77,-18,27,-17,28,-16,50,-15,41,-13,35,-14,36,-12,21});
    states[77] = new State(new int[]{17,78,21,11});
    states[78] = new State(-43);
    states[79] = new State(new int[]{16,80});
    states[80] = new State(new int[]{3,22,4,23,16,24,28,37,30,39},new int[]{-11,81,-18,27,-17,28,-16,50,-15,41,-13,35,-14,36,-12,21});
    states[81] = new State(new int[]{17,82,21,11});
    states[82] = new State(-44);
    states[83] = new State(new int[]{8,84});
    states[84] = new State(-12);
    states[85] = new State(new int[]{3,87,4,88},new int[]{-19,86});
    states[86] = new State(-45);
    states[87] = new State(-46);
    states[88] = new State(-47);
    states[89] = new State(-4);

    rules[1] = new Rule(-21, new int[]{-20,2});
    rules[2] = new Rule(-20, new int[]{-1});
    rules[3] = new Rule(-1, new int[]{-3});
    rules[4] = new Rule(-3, new int[]{-2});
    rules[5] = new Rule(-3, new int[]{-3,-2});
    rules[6] = new Rule(-2, new int[]{-4,8});
    rules[7] = new Rule(-2, new int[]{-5});
    rules[8] = new Rule(-2, new int[]{-6});
    rules[9] = new Rule(-2, new int[]{-7});
    rules[10] = new Rule(-2, new int[]{-8});
    rules[11] = new Rule(-2, new int[]{-9,8});
    rules[12] = new Rule(-2, new int[]{-10,8});
    rules[13] = new Rule(-12, new int[]{3});
    rules[14] = new Rule(-4, new int[]{-12,7,-11});
    rules[15] = new Rule(-11, new int[]{-18});
    rules[16] = new Rule(-11, new int[]{-11,21,-18});
    rules[17] = new Rule(-18, new int[]{-17});
    rules[18] = new Rule(-18, new int[]{-18,20,-17});
    rules[19] = new Rule(-17, new int[]{-16});
    rules[20] = new Rule(-17, new int[]{-17,26,-16});
    rules[21] = new Rule(-17, new int[]{-17,27,-16});
    rules[22] = new Rule(-17, new int[]{-17,22,-16});
    rules[23] = new Rule(-17, new int[]{-17,23,-16});
    rules[24] = new Rule(-17, new int[]{-17,24,-16});
    rules[25] = new Rule(-17, new int[]{-17,25,-16});
    rules[26] = new Rule(-16, new int[]{-15});
    rules[27] = new Rule(-16, new int[]{-16,29,-15});
    rules[28] = new Rule(-16, new int[]{-16,30,-15});
    rules[29] = new Rule(-15, new int[]{-13});
    rules[30] = new Rule(-15, new int[]{-15,32,-14});
    rules[31] = new Rule(-15, new int[]{-15,31,-14});
    rules[32] = new Rule(-13, new int[]{-14});
    rules[33] = new Rule(-13, new int[]{28,-14});
    rules[34] = new Rule(-13, new int[]{30,-14});
    rules[35] = new Rule(-14, new int[]{-12});
    rules[36] = new Rule(-14, new int[]{4});
    rules[37] = new Rule(-14, new int[]{16,-11,17});
    rules[38] = new Rule(-5, new int[]{5,-3,6});
    rules[39] = new Rule(-6, new int[]{9,-11,-2});
    rules[40] = new Rule(-7, new int[]{11,-12,7,-16,10,-16,-2});
    rules[41] = new Rule(-8, new int[]{12,-11,-2});
    rules[42] = new Rule(-8, new int[]{12,-11,-2,13,-2});
    rules[43] = new Rule(-9, new int[]{14,16,-11,17});
    rules[44] = new Rule(-9, new int[]{15,16,-11,17});
    rules[45] = new Rule(-10, new int[]{18,-19});
    rules[46] = new Rule(-19, new int[]{3});
    rules[47] = new Rule(-19, new int[]{4});
  }

  protected override void Initialize() {
    this.InitSpecialTokens((int)Tokens.error, (int)Tokens.EOF);
    this.InitStates(states);
    this.InitRules(rules);
    this.InitNonTerminals(nonTerms);
  }

  protected override void DoAction(int action)
  {
    switch (action)
    {
      case 2: // root -> progr
{ Root = ValueStack[ValueStack.Depth-1].SyntaxNode; }
        break;
      case 3: // progr -> stlist
{
                CurrentSemanticValue.SyntaxNode = new Program(ValueStack[ValueStack.Depth-1].Statement);
            }
        break;
      case 4: // stlist -> statement
{ 
                CurrentSemanticValue.Statement = new StatementList(ValueStack[ValueStack.Depth-1].Statement); 
            }
        break;
      case 5: // stlist -> stlist, statement
{
                CurrentSemanticValue.Statement = (ValueStack[ValueStack.Depth-2].Statement as StatementList).Add(ValueStack[ValueStack.Depth-1].Statement); 
            }
        break;
      case 13: // ident -> ID
{ 
                CurrentSemanticValue.Expression = new Identifier(ValueStack[ValueStack.Depth-1].String); 
            }
        break;
      case 14: // assign -> ident, ASSIGN, expr
{
                CurrentSemanticValue.Statement = new AssignmentStatement(ValueStack[ValueStack.Depth-3].Expression as Identifier, ValueStack[ValueStack.Depth-1].Expression);
            }
        break;
      case 15: // expr -> andop
{ CurrentSemanticValue.Expression = ValueStack[ValueStack.Depth-1].Expression; }
        break;
      case 16: // expr -> expr, OR, andop
{
                CurrentSemanticValue.Expression = new BinaryExpression(ValueStack[ValueStack.Depth-3].Expression, Operation.LogicalOr, ValueStack[ValueStack.Depth-1].Expression);
            }
        break;
      case 17: // andop -> compar
{ CurrentSemanticValue.Expression = ValueStack[ValueStack.Depth-1].Expression; }
        break;
      case 18: // andop -> andop, AND, compar
{
                CurrentSemanticValue.Expression = new BinaryExpression(ValueStack[ValueStack.Depth-3].Expression, Operation.LogicalAnd, ValueStack[ValueStack.Depth-1].Expression);
            }
        break;
      case 19: // compar -> addop
{ CurrentSemanticValue.Expression = ValueStack[ValueStack.Depth-1].Expression; }
        break;
      case 20: // compar -> compar, EQ, addop
{
                CurrentSemanticValue.Expression = new BinaryExpression(ValueStack[ValueStack.Depth-3].Expression, Operation.Equal, ValueStack[ValueStack.Depth-1].Expression);
            }
        break;
      case 21: // compar -> compar, NOTEQ, addop
{
                CurrentSemanticValue.Expression = new BinaryExpression(ValueStack[ValueStack.Depth-3].Expression, Operation.NotEqual, ValueStack[ValueStack.Depth-1].Expression);
            }
        break;
      case 22: // compar -> compar, LESSER, addop
{
                CurrentSemanticValue.Expression = new BinaryExpression(ValueStack[ValueStack.Depth-3].Expression, Operation.Lesser, ValueStack[ValueStack.Depth-1].Expression);
            }
        break;
      case 23: // compar -> compar, GREATER, addop
{
                CurrentSemanticValue.Expression = new BinaryExpression(ValueStack[ValueStack.Depth-3].Expression, Operation.Greater, ValueStack[ValueStack.Depth-1].Expression);
            }
        break;
      case 24: // compar -> compar, LESSEREQ, addop
{
                CurrentSemanticValue.Expression = new BinaryExpression(ValueStack[ValueStack.Depth-3].Expression, Operation.LesserEqual, ValueStack[ValueStack.Depth-1].Expression);
            }
        break;
      case 25: // compar -> compar, GREATEREQ, addop
{
                CurrentSemanticValue.Expression = new BinaryExpression(ValueStack[ValueStack.Depth-3].Expression, Operation.GreaterEqual, ValueStack[ValueStack.Depth-1].Expression);
            }
        break;
      case 26: // addop -> mulop
{ CurrentSemanticValue.Expression = ValueStack[ValueStack.Depth-1].Expression; }
        break;
      case 27: // addop -> addop, PLUS, mulop
{
                CurrentSemanticValue.Expression = new BinaryExpression(ValueStack[ValueStack.Depth-3].Expression, Operation.Add, ValueStack[ValueStack.Depth-1].Expression);
            }
        break;
      case 28: // addop -> addop, MINUS, mulop
{
                CurrentSemanticValue.Expression = new BinaryExpression(ValueStack[ValueStack.Depth-3].Expression, Operation.Subtract, ValueStack[ValueStack.Depth-1].Expression);
            }
        break;
      case 29: // mulop -> unaryop
{ CurrentSemanticValue.Expression = ValueStack[ValueStack.Depth-1].Expression; }
        break;
      case 30: // mulop -> mulop, MULT, parenth
{
                CurrentSemanticValue.Expression = new BinaryExpression(ValueStack[ValueStack.Depth-3].Expression, Operation.Multiply, ValueStack[ValueStack.Depth-1].Expression);
            }
        break;
      case 31: // mulop -> mulop, DIVIDE, parenth
{
                CurrentSemanticValue.Expression = new BinaryExpression(ValueStack[ValueStack.Depth-3].Expression, Operation.Divide, ValueStack[ValueStack.Depth-1].Expression);
            }
        break;
      case 32: // unaryop -> parenth
{ CurrentSemanticValue.Expression = ValueStack[ValueStack.Depth-1].Expression; }
        break;
      case 33: // unaryop -> NOT, parenth
{
                CurrentSemanticValue.Expression = new UnaryExpression(Operation.UnaryNot, ValueStack[ValueStack.Depth-1].Expression);
            }
        break;
      case 34: // unaryop -> MINUS, parenth
{
                CurrentSemanticValue.Expression = new UnaryExpression(Operation.UnaryMinus, ValueStack[ValueStack.Depth-1].Expression);
            }
        break;
      case 35: // parenth -> ident
{ CurrentSemanticValue.Expression = ValueStack[ValueStack.Depth-1].Expression; }
        break;
      case 36: // parenth -> INUM
{ CurrentSemanticValue.Expression = new Int32Const(ValueStack[ValueStack.Depth-1].Int); }
        break;
      case 37: // parenth -> OPENROUND, expr, CLOSEROUND
{ CurrentSemanticValue.Expression = new ParenthesizedExpression(ValueStack[ValueStack.Depth-2].Expression); }
        break;
      case 38: // block -> BEGIN, stlist, END
{ CurrentSemanticValue.Statement = new Block(ValueStack[ValueStack.Depth-2].Statement as StatementList); }
        break;
      case 39: // while_cycle -> WHILE, expr, statement
{ 
                CurrentSemanticValue.Statement = new WhileStatement(ValueStack[ValueStack.Depth-2].Expression, ValueStack[ValueStack.Depth-1].Statement); 
            }
        break;
      case 40: // for_cycle -> FOR, ident, ASSIGN, addop, DOUBLEDOT, addop, statement
{
                CurrentSemanticValue.Statement = new ForStatement(ValueStack[ValueStack.Depth-6].Expression as Identifier, ValueStack[ValueStack.Depth-4].Expression, ValueStack[ValueStack.Depth-2].Expression, ValueStack[ValueStack.Depth-1].Statement);
            }
        break;
      case 41: // if -> IF, expr, statement
{
                CurrentSemanticValue.Statement = new IfStatement(ValueStack[ValueStack.Depth-2].Expression, ValueStack[ValueStack.Depth-1].Statement);
            }
        break;
      case 42: // if -> IF, expr, statement, ELSE, statement
{
                CurrentSemanticValue.Statement = new IfStatement(ValueStack[ValueStack.Depth-4].Expression, ValueStack[ValueStack.Depth-3].Statement, ValueStack[ValueStack.Depth-1].Statement);
            }
        break;
      case 43: // print -> PRINT, OPENROUND, expr, CLOSEROUND
{
                CurrentSemanticValue.Statement = new PrintStatement(ValueStack[ValueStack.Depth-2].Expression, newLine: false);
            }
        break;
      case 44: // print -> PRINTLN, OPENROUND, expr, CLOSEROUND
{
                CurrentSemanticValue.Statement = new PrintStatement(ValueStack[ValueStack.Depth-2].Expression, newLine: true);
            }
        break;
      case 45: // goto -> GOTO, goto_label
{
				CurrentSemanticValue.Statement = new GotoStatement(ValueStack[ValueStack.Depth-1].String);
			}
        break;
      case 46: // goto_label -> ID
{ CurrentSemanticValue.String = ValueStack[ValueStack.Depth-1].String; }
        break;
      case 47: // goto_label -> INUM
{ CurrentSemanticValue.String = ValueStack[ValueStack.Depth-1].Int.ToString(); }
        break;
    }
  }

  protected override string TerminalToString(int terminal)
  {
    if (aliasses != null && aliasses.ContainsKey(terminal))
        return aliasses[terminal];
    else if (((Tokens)terminal).ToString() != terminal.ToString(CultureInfo.InvariantCulture))
        return ((Tokens)terminal).ToString();
    else
        return CharToString((char)terminal);
  }

}
}
