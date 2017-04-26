%using QUT.Gppg;
%using System.Linq;
%using SyntaxTree;
%using SyntaxTree.SyntaxNodes;

%namespace GPPGParser

Alpha 	[a-zA-Z_]
Digit   [0-9] 
AlphaDigit {Alpha}|{Digit}
INTNUM  {Digit}+
ID {Alpha}{AlphaDigit}* 

%%

{INTNUM} { 
  yylval = new Union();
  yylval.Int = int.Parse(yytext);
  return (int)Tokens.INUM; 
}

{ID}  {
  int res = ScannerHelper.GetIDToken(yytext);
  yylval = new Union();
  yylval.String = yytext;
  return res;
}

"="		{ return (int)Tokens.ASSIGN; }
";"		{ return (int)Tokens.SEMICOLON; }
"{"		{ return (int)Tokens.BEGIN; }
"}"		{ return (int)Tokens.END; }
".."	{ return (int)Tokens.DOUBLEDOT; }
"!="	{ return (int)Tokens.NOTEQ; }
"=="	{ return (int)Tokens.EQ; }
"<"		{ return (int)Tokens.LESSER; }
">"		{ return (int)Tokens.GREATER; }
">="	{ return (int)Tokens.GREATEREQ; }
"<="	{ return (int)Tokens.LESSEREQ; }
"&"		{ return (int)Tokens.AND; }
"|"		{ return (int)Tokens.OR; }
"+"		{ return (int)Tokens.PLUS; }
"-"		{ return (int)Tokens.MINUS; }
"*"		{ return (int)Tokens.MULT; }
"/"		{ return (int)Tokens.DIVIDE; }
"("		{ return (int)Tokens.OPENROUND; }
")"		{ return (int)Tokens.CLOSEROUND; }
"!"     { return (int)Tokens.NOT; }
":"		{ return (int)Tokens.COLON; }

[^ \r\n] {
	LexError();
	return (int)Tokens.EOF; // ����� �������
}

%{
  yylloc = new LexLocation(tokLin, tokCol, tokELin, tokECol); // ������� ������� (������������� ��� ���������������), ������������ @1 @2 � �.�.
%}

%%

public override void yyerror(string format, params object[] args) // ��������� �������������� ������
{
  var ww = args.Skip(1).Cast<string>().ToArray();
  string errorMsg = string.Format("({0},{1}): ��������� {2}, � ��������� {3}", yyline, yycol, args[0], string.Join(" ��� ", ww));
  throw new SyntaxException(errorMsg);
}

public void LexError()
{
	string errorMsg = string.Format("({0},{1}): ����������� ������ {2}", yyline, yycol, yytext);
    throw new LexException(errorMsg);
}

class ScannerHelper 
{
  private static Dictionary<string,int> keywords;

  static ScannerHelper() 
  {
    keywords = new Dictionary<string,int>();
    keywords.Add("while",(int)Tokens.WHILE);
    keywords.Add("for",(int)Tokens.FOR);
    keywords.Add("if",(int)Tokens.IF);
    keywords.Add("else",(int)Tokens.ELSE);
	keywords.Add("print",(int)Tokens.PRINT);
	keywords.Add("println",(int)Tokens.PRINTLN);
	keywords.Add("goto", (int)Tokens.GOTO);
  }
  public static int GetIDToken(string s)
  {
    if (keywords.ContainsKey(s.ToLower())) // ���� �������������� � ��������
      return keywords[s];
    else
      return (int)Tokens.ID;
  }
}
