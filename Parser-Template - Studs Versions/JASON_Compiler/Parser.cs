using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JASON_Compiler
{
    public class Node
    {
        public List<Node> Children = new List<Node>();

        public string Name;
        public Node(string N)
        {
            this.Name = N;
        }
    }
    public class Parser
    {
        int InputPointer = 0;
        List<Token> TokenStream;
        public Node root;

        public Node StartParsing(List<Token> TokenStream)
        {
            this.InputPointer = 0;
            this.TokenStream = TokenStream;
            root = new Node("Program");
            root.Children.Add(Program());
            return root;
        }

        Node Reserved_Keywords()
        {
            Node reserved_Keywords = new Node("Reserved_Keywords");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Int)
            {
                reserved_Keywords.Children.Add(match(Token_Class.Int));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Float)
            {
                reserved_Keywords.Children.Add(match(Token_Class.Float));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.String)
            {
                reserved_Keywords.Children.Add(match(Token_Class.String));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Read)
            {
                reserved_Keywords.Children.Add(match(Token_Class.Read));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Write)
            {
                reserved_Keywords.Children.Add(match(Token_Class.Write));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Repeat)
            {
                reserved_Keywords.Children.Add(match(Token_Class.Repeat));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Until)
            {
                reserved_Keywords.Children.Add(match(Token_Class.Until));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.If)
            {
                reserved_Keywords.Children.Add(match(Token_Class.If));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Elseif)
            {
                reserved_Keywords.Children.Add(match(Token_Class.Elseif));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Else)
            {
                reserved_Keywords.Children.Add(match(Token_Class.Else));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Then)
            {
                reserved_Keywords.Children.Add(match(Token_Class.Then));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Return)
            {
                reserved_Keywords.Children.Add(match(Token_Class.Return));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Endl)
            {
                reserved_Keywords.Children.Add(match(Token_Class.Endl));
            }
            return reserved_Keywords;
        }

        Node Id()
        {
            Node id = new Node("Id");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Idenifier)
            {
                id.Children.Add(match(Token_Class.Idenifier));
                id.Children.Add(Iden());
                return id;
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Number)
            {
                id.Children.Add(match(Token_Class.Number));
                id.Children.Add(Iden());
                return id;
            }
            else
            {
                return null;
            }
        }

        Node Iden()
        {
            Node iden = new Node("Iden");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                iden.Children.Add(match(Token_Class.Comma));
                iden.Children.Add(Id());
                return iden;
            }
            else
            {
                return null;
            }
        }

        Node FunctionCall()
        {
            Node functionCall = new Node("functionCall");
            functionCall.Children.Add(match(Token_Class.Idenifier));
            functionCall.Children.Add(match(Token_Class.LParanthesis));
            functionCall.Children.Add(Id());
            functionCall.Children.Add(match(Token_Class.RParanthesis));
            return functionCall;
        }

        Node Term()
        {
            Node term = new Node("Term");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Number)
            {
                term.Children.Add(match(Token_Class.Number));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Idenifier)
            {
                term.Children.Add(match(Token_Class.Idenifier));
                term.Children.Add(Ter());
            }
            return term;
        }

        Node Ter()
        {
            Node ter = new Node("Ter");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.LParanthesis)
            {
                ter.Children.Add(match(Token_Class.LParanthesis));
                ter.Children.Add(Id());
                ter.Children.Add(match(Token_Class.RParanthesis));
            }
            else
            {
                return null;
            }
            return ter;
        }

        Node ArithmeticOperator()
        {
            Node arithmeticOperator = new Node("ArithmeticOperator");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.PlusOp)
            {
                arithmeticOperator.Children.Add(match(Token_Class.PlusOp));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.MinusOp)
            {
                arithmeticOperator.Children.Add(match(Token_Class.MinusOp));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.MultiplyOp)
            {
                arithmeticOperator.Children.Add(match(Token_Class.MultiplyOp));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.DivideOp)
            {
                arithmeticOperator.Children.Add(match(Token_Class.DivideOp));
            }
            return arithmeticOperator;
        }

        Node EQ()
        {
            Node eq = new Node("EQ");
            eq.Children.Add(ArithmeticOperator());
            eq.Children.Add(Term());
            eq.Children.Add(E());
            return eq;
        }

        Node E()
        {
            Node e = new Node("E");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.PlusOp || TokenStream[InputPointer].token_type == Token_Class.MinusOp || TokenStream[InputPointer].token_type == Token_Class.MultiplyOp || TokenStream[InputPointer].token_type == Token_Class.DivideOp)
            {
                e.Children.Add(EQ());
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.LParanthesis)
            {
                e.Children.Add(Eq());
            }
            else
            {
                return null;
            }
            return e;
        }

        Node Eq()
        {
            Node eq = new Node("Eq");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.LParanthesis)
            {
                eq.Children.Add(match(Token_Class.LParanthesis));
                eq.Children.Add(Term());
                eq.Children.Add(EQ());
                eq.Children.Add(match(Token_Class.RParanthesis));
                eq.Children.Add(E());
            }
            else
            {
                return null;
            }
            return eq;
        }

        Node Equation()
        {
            Node equation = new Node("Equation");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.LParanthesis)
            {
                equation.Children.Add(E());
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Number || TokenStream[InputPointer].token_type == Token_Class.Idenifier)
            {
                equation.Children.Add(Term());
                equation.Children.Add(E());
            }
            return equation;
        }

        Node Expression()
        {
            Node expression = new Node("Expression");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.String)
            {
                expression.Children.Add(match(Token_Class.String));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Number || TokenStream[InputPointer].token_type == Token_Class.Idenifier)
            {
                expression.Children.Add(Term());
                expression.Children.Add(E());
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.LParanthesis)
            {
                expression.Children.Add(E());
            }
            return expression;
        }

        Node AssignmentStatement()
        {
            Node assignmentStatement = new Node("AssignmentStatement");
            if (InputPointer < TokenStream.Count)
            {
                assignmentStatement.Children.Add(match(Token_Class.Idenifier));
                assignmentStatement.Children.Add(match(Token_Class.AssignmentOp));
                assignmentStatement.Children.Add(Expression());
            }
            return assignmentStatement;

        }

        Node DataType()
        {
            Node dataType = new Node("DataType");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Int)
            {
                dataType.Children.Add(match(Token_Class.Int));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Float)
            {
                dataType.Children.Add(match(Token_Class.Float));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.String)
            {
                dataType.Children.Add(match(Token_Class.String));
            }
            return dataType;
        }

        Node Ex()
        {
            Node ex = new Node("Ex");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.AssignmentOp)
            {
                ex.Children.Add(match((Token_Class.AssignmentOp)));
                ex.Children.Add(Expression());
            }
            else
            {
                return null;
            }
            return ex;
        }
        Node I()
        {
            Node i = new Node("I");
            i.Children.Add(match(Token_Class.Idenifier));
            i.Children.Add(Ex());
            i.Children.Add(Ide());
            return i;

        }

        Node Ide()
        {
            Node ide = new Node("Ide");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                ide.Children.Add(match(Token_Class.Comma));
                ide.Children.Add(I());
                return ide;
            }
            else
            {
                return null;
            }
        }

        Node Assign()
        {
            Node assign = new Node("Assign");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Idenifier)
            {
                assign.Children.Add(AssignmentStatement());
                assign.Children.Add(Assignment());
            }
            else
            {
                return null;
            }
            return assign;
        }

        Node Assignment()
        {
            Node assignment = new Node("Assignment");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                assignment.Children.Add(match(Token_Class.Comma));
                assignment.Children.Add(Assign());
            }
            else
            {
                return null;
            }
            return assignment;
        }

        Node DeclarationStatement()
        {
            Node declarationStatement = new Node("DeclarationStatement");
            declarationStatement.Children.Add(DataType());
            declarationStatement.Children.Add(I());
            declarationStatement.Children.Add(match(Token_Class.Semicolon));
            return declarationStatement;
        }

        Node WriteStatement()
        {
            Node writeStatement = new Node("WriteStatement");
            writeStatement.Children.Add(match(Token_Class.Write));
            writeStatement.Children.Add(Write());
            writeStatement.Children.Add(match(Token_Class.Semicolon));
            return writeStatement;
        }

        Node Write()
        {
            Node write = new Node("Write");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.String || TokenStream[InputPointer].token_type == Token_Class.Number || TokenStream[InputPointer].token_type == Token_Class.Idenifier || TokenStream[InputPointer].token_type == Token_Class.LParanthesis)
            {
                write.Children.Add(Expression());
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Endl)
            {
                write.Children.Add(match(Token_Class.Endl));
            }
            return write;
        }

        Node ReadStatement()
        {
            Node readStatement = new Node("ReadStatement");
            readStatement.Children.Add(match(Token_Class.Read));
            readStatement.Children.Add(match(Token_Class.Idenifier));
            readStatement.Children.Add(match(Token_Class.Semicolon));
            return readStatement;
        }

        Node ReturnStatement()
        {
            Node returnStatement = new Node("ReturnStatement");
            if (InputPointer < TokenStream.Count)
            {
                returnStatement.Children.Add(match(Token_Class.Return));
                returnStatement.Children.Add(Expression());
                returnStatement.Children.Add(match(Token_Class.Semicolon));
            }
            return returnStatement;
        }

        Node ConditionOperator()
        {
            Node conditionOperator = new Node("ConditionOperator");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.EqualOp)
            {
                conditionOperator.Children.Add(match(Token_Class.EqualOp));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.GreaterThanOp)
            {
                conditionOperator.Children.Add(match(Token_Class.GreaterThanOp));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.LessThanOp)
            {
                conditionOperator.Children.Add(match(Token_Class.LessThanOp));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.NotEqualOp)
            {
                conditionOperator.Children.Add(match(Token_Class.NotEqualOp));
            }
            return conditionOperator;
        }

        Node Condition()
        {
            Node condition = new Node("Condition");
            condition.Children.Add(match(Token_Class.Idenifier));
            condition.Children.Add(ConditionOperator());
            condition.Children.Add(Term());
            return condition;
        }

        Node BooleanOperator()
        {
            Node booleanOperator = new Node("BooleanOperator");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.AndOp)
            {
                booleanOperator.Children.Add(match(Token_Class.AndOp));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.OrOp)
            {
                booleanOperator.Children.Add(match(Token_Class.OrOp));
            }
            return booleanOperator;
        }

        Node Cond()
        {
            Node cond = new Node("Cond");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.AndOp || TokenStream[InputPointer].token_type == Token_Class.OrOp)
            {
                cond.Children.Add(BooleanOperator());
                cond.Children.Add(Condition());
                cond.Children.Add(Cond());
            }
            else
            {
                return null;
            }
            return cond;
        }

        Node ConditionStatement()
        {
            Node conditionStatement = new Node("ConditionStatement");
            conditionStatement.Children.Add(Condition());
            conditionStatement.Children.Add(Cond());
            return conditionStatement;
        }

        Node Statements()
        {
            Node statements = new Node("Statements");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Idenifier)
            {
                if (InputPointer + 1 < TokenStream.Count && TokenStream[InputPointer + 1].token_type == Token_Class.LParanthesis)
                {
                    statements.Children.Add(FunctionCall());
                    statements.Children.Add(match(Token_Class.Semicolon));
                }
                else if (InputPointer + 1 < TokenStream.Count && TokenStream[InputPointer + 1].token_type == Token_Class.AssignmentOp)
                {
                    statements.Children.Add(AssignmentStatement());
                    statements.Children.Add(match(Token_Class.Semicolon));
                }
                else if (InputPointer + 1 < TokenStream.Count && TokenStream[InputPointer + 1].token_type == Token_Class.EqualOp || TokenStream[InputPointer + 1].token_type == Token_Class.GreaterThanOp || TokenStream[InputPointer + 1].token_type == Token_Class.LessThanOp || TokenStream[InputPointer + 1].token_type == Token_Class.NotEqualOp)
                {
                    statements.Children.Add(ConditionStatement());
                }
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Int || TokenStream[InputPointer].token_type == Token_Class.Float || TokenStream[InputPointer].token_type == Token_Class.String)
            {
                statements.Children.Add(DeclarationStatement());
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Write)
            {
                statements.Children.Add(WriteStatement());
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Read)
            {
                statements.Children.Add(ReadStatement());
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.If)
            {
                statements.Children.Add(IfStatement());
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Repeat)
            {
                statements.Children.Add(RepeatStatement());
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Comment)
            {
                statements.Children.Add(match(Token_Class.Comment));
            }
            return statements;
        }

        Node SetOfStatements()
        {
            Node setOfStatements = new Node("SetOfStatements");
            if (InputPointer < TokenStream.Count)
            {
                setOfStatements.Children.Add(Statements());
                setOfStatements.Children.Add(State());
            }
            return setOfStatements;

        }

        Node State()
        {
            Node state = new Node("State");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Idenifier || TokenStream[InputPointer].token_type == Token_Class.Int || TokenStream[InputPointer].token_type == Token_Class.Float || TokenStream[InputPointer].token_type == Token_Class.String || TokenStream[InputPointer].token_type == Token_Class.Write || TokenStream[InputPointer].token_type == Token_Class.Read || TokenStream[InputPointer].token_type == Token_Class.If || TokenStream[InputPointer].token_type == Token_Class.Repeat)
            {
                state.Children.Add(SetOfStatements());
            }
            else
            {
                return null;
            }
            return state;
        }

        Node ElseStatements()
        {
            Node elseStatements = new Node("ElseStatements");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Elseif)
            {
                elseStatements.Children.Add(ElseIfStatement());
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Else)
            {
                elseStatements.Children.Add(ElseStatement());
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.End)
            {
                elseStatements.Children.Add(match(Token_Class.End));
            }
            return elseStatements;
        }

        Node IfStatement()
        {
            Node ifStatement = new Node("IfStatement");
            ifStatement.Children.Add(match(Token_Class.If));
            ifStatement.Children.Add(ConditionStatement());
            ifStatement.Children.Add(match(Token_Class.Then));
            ifStatement.Children.Add(SetOfStatements());
            ifStatement.Children.Add(ElseStatements());
            return ifStatement;
        }

        Node ElseIfStatement()
        {
            Node elseIfStatement = new Node("ElseIfStatement");
            elseIfStatement.Children.Add(match(Token_Class.Elseif));
            elseIfStatement.Children.Add(ConditionStatement());
            elseIfStatement.Children.Add(match(Token_Class.Then));
            elseIfStatement.Children.Add(SetOfStatements());
            elseIfStatement.Children.Add(ElseStatements());
            return elseIfStatement;
        }

        Node ElseStatement()
        {
            Node elseStatement = new Node("ElseStatement");
            elseStatement.Children.Add(match(Token_Class.Else));
            elseStatement.Children.Add(SetOfStatements());
            elseStatement.Children.Add(match(Token_Class.End));
            return elseStatement;
        }

        Node RepeatStatement()
        {
            Node repeatStatement = new Node("RepeatStatement");
            repeatStatement.Children.Add(match(Token_Class.Repeat));
            repeatStatement.Children.Add(SetOfStatements());
            repeatStatement.Children.Add(match(Token_Class.Until));
            repeatStatement.Children.Add(ConditionStatement());
            return repeatStatement;
        }

        Node FunctionName()
        {
            Node functionName = new Node("FunctionName");
            functionName.Children.Add(match(Token_Class.Idenifier));
            return functionName;
        }

        Node Parameter()
        {
            Node parameter = new Node("Parameter");
            parameter.Children.Add(DataType());
            parameter.Children.Add(match(Token_Class.Idenifier));
            return parameter;
        }

        Node Para()
        {
            Node para = new Node("Para");
            if ((InputPointer < TokenStream.Count) && (TokenStream[InputPointer].token_type == Token_Class.Int || TokenStream[InputPointer].token_type == Token_Class.Float || TokenStream[InputPointer].token_type == Token_Class.String))
            {
                para.Children.Add(Parameter());
                para.Children.Add(Par());
            }
            else
            {
                return null;
            }
            return para;
        }

        Node Par()
        {
            Node par = new Node("Par");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                par.Children.Add(match(Token_Class.Comma));
                par.Children.Add(Para());
            }
            else
            {
                return null;
            }
            return par;
        }

        Node FunctionDeclaration()
        {
            Node functionDeclaration = new Node("FunctionDeclaration");
            functionDeclaration.Children.Add(DataType());
            functionDeclaration.Children.Add(FunctionName());
            functionDeclaration.Children.Add(match(Token_Class.LParanthesis));
            functionDeclaration.Children.Add(Para());
            functionDeclaration.Children.Add(match(Token_Class.RParanthesis));
            return functionDeclaration;
        }

        Node FunctionBody()
        {
            Node functionBody = new Node("FunctionBody");
            functionBody.Children.Add(match(Token_Class.LCurlyBracket));
            functionBody.Children.Add(SetOfStatements());
            functionBody.Children.Add(ReturnStatement());
            functionBody.Children.Add(match(Token_Class.RCurlyBracket));
            return functionBody;
        }

        Node FunctionStatement()
        {
            Node functionStatement = new Node("FunctionStatement");
            functionStatement.Children.Add(FunctionDeclaration());
            functionStatement.Children.Add(FunctionBody());
            return functionStatement;
        }

        Node MainFunction()
        {
            Node mainFunction = new Node("MainFunction");
            mainFunction.Children.Add(DataType());
            mainFunction.Children.Add(match(Token_Class.Main));
            mainFunction.Children.Add(match(Token_Class.LParanthesis));
            mainFunction.Children.Add(match(Token_Class.RParanthesis));
            mainFunction.Children.Add(FunctionBody());
            return mainFunction;
        }

        Node Prog()
        {
            Node prog = new Node("Prog");
            if ((InputPointer + 1 < TokenStream.Count) && (TokenStream[InputPointer + 1].token_type != Token_Class.Main) && (TokenStream[InputPointer].token_type == Token_Class.Int || TokenStream[InputPointer].token_type == Token_Class.Float || TokenStream[InputPointer].token_type == Token_Class.String))
            {
                prog.Children.Add(FunctionStatement());
                prog.Children.Add(Prog());
            }
            else
            {
                return null;
            }
            return prog;
        }

        Node Program()
        {
            Node program = new Node("Program");

            program.Children.Add(Prog());
            program.Children.Add(MainFunction());
            MessageBox.Show("Success");
            return program;
        }

        public Node match(Token_Class ExpectedToken)
        {

            if (InputPointer < TokenStream.Count)
            {
                if (ExpectedToken == TokenStream[InputPointer].token_type)
                {
                    InputPointer++;
                    Node newNode = new Node(ExpectedToken.ToString());

                    return newNode;

                }

                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + " and " +
                        TokenStream[InputPointer].token_type.ToString() +
                        "  found\r\n");
                    InputPointer++;
                    return null;
                }
            }
            else
            {
                Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + "\r\n");
                InputPointer++;
                return null;
            }
        }

        public static TreeNode PrintParseTree(Node root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
        static TreeNode PrintTree(Node root)
        {
            if (root == null || root.Name == null)
                return null;
            TreeNode tree = new TreeNode(root.Name);
            if (root.Children.Count == 0)
                return tree;
            foreach (Node child in root.Children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }
    }
}
