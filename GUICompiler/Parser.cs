using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUICompiler
{

    internal class Parser
    {
        string strToParse;
        int index;
        string result;
        public Parser(string strToParse)
        {
            this.strToParse = strToParse;
            result = string.Empty;
        }

        public string Result { get { return result; } }

        public void Parse()
        {
            index = -1;
            Program();
            

            
        }
        void Program()
        {
            Token token;
            index++; token = lexer(strToParse[index]);
            result += "Program-";
            switch (token.Type)
            {
                case TokenType.TOKEN_INSTR:  Instr(token);
                     Program(); break;

                default: result += "e-"; break;
            }
            
        }

        void Instr(Token token)
        {
            result += "Instr-";
            switch(token.Lexeme)
            {
                case '[': Program();
                    if (index < strToParse.Length) //++[++[++]+]+
                    {
                        if (strToParse[index] == ']') 
                        {
                            break;
                        }
                        else
                        {
                            index--;
                            //Program();
                        }
                    }
                    break;
                default:break;
            }
        }

        Token lexer(char strToLex)
        {

            switch (strToLex)
            {
                case '+':
                case '-':
                case '>':
                case '<':
                case ',':
                case '.':
                case '[': Token token = new Token("разделитель", TokenType.TOKEN_INSTR); token.Lexeme = strToLex; return token;
                case ' ': return new Token("разделитель", TokenType.TOKEN_WHITESPACE);
                case '\r': return new Token("разделитель", TokenType.TOKEN_WHITESPACE_R);
                case '\n': return new Token("разделитель", TokenType.TOKEN_WHITESPACE_N);
                default: break;
            }
            return new Token("недопустимый символ", TokenType.TOKEN_ERROR);
        }
    }
}
