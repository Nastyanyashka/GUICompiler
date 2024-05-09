using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUICompiler
{
    enum TokenType
    {
        TOKEN_INSTR,
        TOKEN_WHITESPACE,
        TOKEN_WHITESPACE_R,
        TOKEN_WHITESPACE_N,
        TOKEN_ERROR,
    };

    internal class Token
    {
        string name;
        char lexeme;
        TokenType type;
        public Token(string name, TokenType type)
        {
            this.name = name;
            this.type = type;
        }

        public string Name { get { return name; } set { name = value; } }
        public char Lexeme { get { return lexeme; }  set { lexeme = value; } }  
        public TokenType Type { get { return type; } set { type = value; } }
    }
}
