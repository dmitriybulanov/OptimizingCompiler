%{
// Ёти объ€влени€ добавл€ютс€ в класс GPPGParser, представл€ющий собой парсер, генерируемый системой gppg
    public Parser(AbstractScanner<Union, LexLocation> scanner) : base(scanner) { }
    public SyntaxNode Root;
%}
%using SyntaxTree;
%using SyntaxTree.SyntaxNodes;

%output = SimpleYacc.cs

%namespace GPPGParser

%YYSTYPE GPPGParser.Union

%token <String> ID
%token <Int> INUM
%token BEGIN END INUM ASSIGN SEMICOLON WHILE DOUBLEDOT FOR IF ELSE PRINT PRINTLN OPENROUND CLOSEROUND GOTO COLON
%token AND OR LESSER GREATER LESSEREQ GREATEREQ AND OR EQ NOTEQ NOT
%token PLUS MINUS DIVIDE MULT

%type <SyntaxNode> progr 
%type <Statement> statement stlist assign block while_cycle for_cycle if print goto
%type <Expression> expr ident unaryop parenth mulop addop compar andop 
%type <String> goto_label

%start root

%%

root    : progr
            { Root = $1; }
        ;
        
progr   : stlist
            {
                $$ = new Program($1);
            }
		;

stlist	: statement
            { 
                $$ = new StatementList($1); 
            }
		| stlist statement
            {
                $$ = ($1 as StatementList).Add($2); 
            }
		;

statement: assign SEMICOLON
		| block 
		| while_cycle  
		| for_cycle
		| if
		| print SEMICOLON
		| goto SEMICOLON
		;

ident 	: ID 
            { 
                $$ = new Identifier($1); 
            }
		;
	
assign 	: ident ASSIGN expr 
            {
                $$ = new AssignmentStatement($1 as Identifier, $3);
            }
		;

expr	: andop
            { $$ = $1; }
		| expr OR andop
            {
                $$ = new BinaryExpression($1, Operation.LogicalOr, $3);
            }
		;

andop   : compar
            { $$ = $1; }
		| andop AND compar
            {
                $$ = new BinaryExpression($1, Operation.LogicalAnd, $3);
            }
		; 
		
compar	: addop 
            { $$ = $1; }
		| compar EQ addop
            {
                $$ = new BinaryExpression($1, Operation.Equal, $3);
            }
		| compar NOTEQ addop
            {
                $$ = new BinaryExpression($1, Operation.NotEqual, $3);
            }
		| compar LESSER addop
            {
                $$ = new BinaryExpression($1, Operation.Lesser, $3);
            }
		| compar GREATER addop
            {
                $$ = new BinaryExpression($1, Operation.Greater, $3);
            }
		| compar LESSEREQ addop
            {
                $$ = new BinaryExpression($1, Operation.LesserEqual, $3);
            }
		| compar GREATEREQ addop
            {
                $$ = new BinaryExpression($1, Operation.GreaterEqual, $3);
            }
		;
		
addop 	: mulop
            { $$ = $1; }
		| addop PLUS mulop
            {
                $$ = new BinaryExpression($1, Operation.Add, $3);
            }
		| addop MINUS mulop
            {
                $$ = new BinaryExpression($1, Operation.Subtract, $3);
            }
		;

mulop  	: unaryop
            { $$ = $1; }
		| mulop MULT parenth
            {
                $$ = new BinaryExpression($1, Operation.Multiply, $3);
            }
		| mulop DIVIDE parenth
            {
                $$ = new BinaryExpression($1, Operation.Divide, $3);
            }
		;

unaryop : parenth 
            { $$ = $1; }
        | NOT parenth
            {
                $$ = new UnaryExpression(Operation.UnaryNot, $2);
            }
        | MINUS parenth
            {
                $$ = new UnaryExpression(Operation.UnaryMinus, $2);
            }
        ;
        
parenth : ident 
            { $$ = $1; }
		| INUM
            { $$ = new Int32Const($1); }
		| OPENROUND expr CLOSEROUND
            { $$ = new ParenthesizedExpression($2); }
		;

block	: BEGIN stlist END 
            { $$ = new Block($2 as StatementList); }
		;

while_cycle	: WHILE expr statement 
            { 
                $$ = new WhileStatement($2, $3); 
            }
		;

for_cycle	: FOR ident ASSIGN addop DOUBLEDOT addop statement 
            {
                $$ = new ForStatement($2 as Identifier, $4, $6, $7);
            }
		;

if		: IF expr statement
            {
                $$ = new IfStatement($2, $3);
            }
		| IF expr statement ELSE statement
            {
                $$ = new IfStatement($2, $3, $5);
            }
		;

print	: PRINT OPENROUND expr CLOSEROUND
            {
                $$ = new PrintStatement($3, newLine: false);
            }
		| PRINTLN OPENROUND expr CLOSEROUND
            {
                $$ = new PrintStatement($3, newLine: true);
            }
		;

goto 	: GOTO goto_label
			{
				$$ = new GotoStatement($2);
			}
		;

goto_label 
	: ID
		{ $$ = $1; }
	| INUM
		{ $$ = $1.ToString(); }
	;
%%
