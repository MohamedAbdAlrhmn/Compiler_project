using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

public enum Token_Class
{
    If, Int, Float, String, Read, Write, Repeat, Until, Elseif, Else, Then, Return, Endl, Comment,
    Semicolon, Comma, LParanthesis, RParanthesis, LCurlyBracket, RCurlyBracket,
    EqualOp, LessThanOp, GreaterThanOp, NotEqualOp, AssignmentOp, AndOp, OrOp,
    PlusOp, MinusOp, MultiplyOp, DivideOp,
    Idenifier, Constant, Main, End, Number


}

namespace JASON_Compiler
{


    public class Token
    {
        public string lex;
        public Token_Class token_type;
    }

    public class Scanner
    {
        public List<Token> Tokens = new List<Token>();
        Dictionary<string, Token_Class> ReservedWords = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Operators = new Dictionary<string, Token_Class>();

        public Scanner()
        {
            // List of Reserved Words
            ReservedWords.Add("if", Token_Class.If);
            ReservedWords.Add("int", Token_Class.Int);
            ReservedWords.Add("Number", Token_Class.Number);
            ReservedWords.Add("float", Token_Class.Float);
            ReservedWords.Add("string", Token_Class.String);
            ReservedWords.Add("read", Token_Class.Read);
            ReservedWords.Add("write", Token_Class.Write);
            ReservedWords.Add("repeat", Token_Class.Repeat);
            ReservedWords.Add("until", Token_Class.Until);
            ReservedWords.Add("elseif", Token_Class.Elseif);
            ReservedWords.Add("else", Token_Class.Else);
            ReservedWords.Add("then", Token_Class.Then);
            ReservedWords.Add("return", Token_Class.Return);
            ReservedWords.Add("endl", Token_Class.Endl);
            ReservedWords.Add("main", Token_Class.Main);
            ReservedWords.Add("end", Token_Class.End);



            // List of Operators
            Operators.Add(";", Token_Class.Semicolon);
            Operators.Add(",", Token_Class.Comma);
            Operators.Add("(", Token_Class.LParanthesis);
            Operators.Add(")", Token_Class.RParanthesis);
            Operators.Add("{", Token_Class.LCurlyBracket);
            Operators.Add("}", Token_Class.RCurlyBracket);
            Operators.Add("=", Token_Class.EqualOp);
            Operators.Add("<", Token_Class.LessThanOp);
            Operators.Add(">", Token_Class.GreaterThanOp);
            Operators.Add("<>", Token_Class.NotEqualOp);
            Operators.Add(":=", Token_Class.AssignmentOp);
            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("-", Token_Class.MinusOp);
            Operators.Add("/", Token_Class.DivideOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            Operators.Add("&&", Token_Class.AndOp);
            Operators.Add("||", Token_Class.OrOp);
        }

        public void StartScanning(string SourceCode)
        {
            for (int i = 0; i < SourceCode.Length; i++)
            {
                int j = i;
                char CurrentChar = SourceCode[i];
                string CurrentLexeme = CurrentChar.ToString();

                if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n')
                    continue;
                if (char.IsLetter(CurrentChar))
                {



                    while (SourceCode.Length > j && char.IsLetterOrDigit(SourceCode[j]))
                    {



                        if (j != i)
                        {

                            CurrentLexeme += SourceCode[j].ToString();
                        }
                        j++;


                    }
                    FindTokenClass(CurrentLexeme);
                    i = j - 1;

                }
                else if (CurrentChar >= '0' && CurrentChar <= '9')//number
                {


                    while (j < SourceCode.Length && (char.IsLetterOrDigit(SourceCode[j]) || SourceCode[j] == '.'))
                    {


                        if (j != i)
                        {

                            CurrentLexeme += SourceCode[j].ToString();
                        }
                        j++;


                    }
                    FindTokenClass(CurrentLexeme);
                    i = j - 1;
                }

                else if (CurrentChar == '\"')
                {

                    j++;
                    while (j < SourceCode.Length && SourceCode[j] != '\"')
                    {
                        CurrentLexeme += SourceCode[j];
                        j++;
                    }
                    if (j < SourceCode.Length)
                        CurrentLexeme += SourceCode[j];
                    FindTokenClass(CurrentLexeme);
                    i = j;
                }



                else if (CurrentChar == '/' && SourceCode[i + 1] == '*')//comment
                {

                    while (true)
                    {

                        i++;
                        if (i >= SourceCode.Length)
                            break;

                        CurrentChar = SourceCode[i];
                        if (CurrentChar == '*' && SourceCode[i + 1] == '/')
                        {


                            CurrentLexeme += CurrentChar.ToString();
                            CurrentLexeme += SourceCode[i + 1].ToString();

                            break;

                        }
                        CurrentLexeme += CurrentChar;
                    }
                    i++;
                    FindTokenClass(CurrentLexeme);

                }
                else if (CurrentChar == '{' || CurrentChar == '}')
                {

                    FindTokenClass(CurrentLexeme);

                }


                else if (!char.IsLetterOrDigit(CurrentChar)) //operator
                {

                    CurrentChar = SourceCode[j];

                    while (j < SourceCode.Length - 1 && !char.IsLetterOrDigit(CurrentChar))
                    {
                        char k = SourceCode[j + 1];
                        if (CurrentChar == '<' && k == '>')
                        {
                            CurrentLexeme += k;
                            j += 2;
                            break;

                        }
                        if (Operators.ContainsKey(CurrentLexeme))
                        {
                            j++;
                            break;
                        }
                        if (j != i)
                        {

                            CurrentChar = SourceCode[j];
                            CurrentLexeme += CurrentChar.ToString();

                        }

                        j++;
                    }

                    FindTokenClass(CurrentLexeme);
                    i = j - 1;


                }

            }

            JASON_Compiler.TokenStream = Tokens;
        }
        void FindTokenClass(string Lex)
        {
            Token_Class TC;
            Token Tok = new Token();
            Tok.lex = Lex;
            //Is it a reserved word?

            if (ReservedWords.ContainsKey(Lex))
            {
                Tok.token_type = ReservedWords[Lex];

                Tokens.Add(Tok);

            }


            //Is it an identifier?

            else if (isIdentifier(Lex))
            {
                Tok.token_type = Token_Class.Idenifier;
                Tokens.Add(Tok);

            }

            //Is it a Number?

            else if (isConstant(Lex))
            {
                Tok.token_type = Token_Class.Number;
                Tokens.Add(Tok);
            }
            //Is it an operator?

            else if (Operators.ContainsKey(Lex))
            {

                Tok.token_type = Operators[Lex];

                Tokens.Add(Tok);
            }
            //Is it a String?
            else if (String(Lex))
            {
                Tok.token_type = Token_Class.String;
                Tokens.Add(Tok);
            }
            // is it comment?
            else if (Comment(Lex))
            {
                Tok.token_type = Token_Class.Comment;
                Tokens.Add(Tok);
            }
            //Is it an undefined?
            else
            {
                Errors.Error_List.Add("Undefined Token " + Lex);
            }
        }
        bool isIdentifier(string lex)
        {
            bool isValid = true;


            var regex = new Regex(@"^[A-Za-z ][-a-zA-Z0-9]*$", RegexOptions.Compiled);
            // Check if the lex is an identifier or not.
            isValid = regex.IsMatch(lex);
            return isValid;
        }
        bool isConstant(string lex)
        {
            bool isValid = true;
            // Check if the lex is a constant (Number) or not.

            var regex = new Regex(@"^[0-9]+(\.[0-9]+)?$", RegexOptions.Compiled);
            isValid = regex.IsMatch(lex);
            return isValid;
        }

        bool String(string lex)//string
        {
            bool isvalid = true;

            var regex = new Regex("^(\")(.*)(\")$", RegexOptions.Compiled);
            isvalid = regex.IsMatch(lex);

            return isvalid;

        }

        bool Comment(string lex)//comment
        {
            bool isvalid = true;
            var regex = new Regex("^(\\/*).*(\\*/)$", RegexOptions.Compiled);
            isvalid = regex.IsMatch(lex);
            return isvalid;
        }
    }
}
