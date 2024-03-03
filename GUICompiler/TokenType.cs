using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUICompiler
{
    enum TokenType
    {
        TOKEN_INT = 1,
        TOKEN_STRING,
        TOKEN_DICTIONARY,
        TOKEN_NEW,
        TOKEN_IDENTIFIER,
        TOKEN_WHITESPACE,
        TOKEN_WHITESPACE_R,
        TOKEN_WHITESPACE_N,
        TOKEN_EQUALS,
        TOKEN_SEMICOLON,
        TOKEN_LEFT_ANGLE_BRACKET,
        TOKEN_RIGHT_ANGLE_BRACKET,
        TOKEN_LEFT_PARANTHESES,
        TOKEN_RIGHT_PARANTHESES,
        TOKEN_COMMA,
        TOKEN_ERROR,
    };

    internal class Token
    {
        string name;
        TokenType type;
        public Token(string name, TokenType type)
        {
            this.name = name;
            this.type = type;
        }

        public string Name { get { return name; } set { name = value; } }
        public TokenType Type { get { return type; } set { type = value; } }
    }
}
