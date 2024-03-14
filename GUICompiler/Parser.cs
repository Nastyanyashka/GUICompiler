using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUICompiler
{
    enum States
    {
        None,
        Dictionary,
        TypeKey,
        Comma,
        TypeValue,
        Close,
        Open,
        Identifier,
        ASSIGNTMENT,
        New,
        Dictionary2,
        TypeKey2,
        Comma2,
        TypeValue2,
        Close2,
        Left_Bracket,
        Right_Bracket,
        Whitespace,
        End,
        ERROR
           
    };

    internal class Parser
    {
        States currentState;
        public Parser()
        {
            currentState = States.None;
        }

        public States Parse(Token token)
        {
            TokenType type = token.Type;
            if(type == TokenType.TOKEN_WHITESPACE || type == TokenType.TOKEN_WHITESPACE_R || type == TokenType.TOKEN_WHITESPACE_N)
            {
                return currentState;
            }
            
            if(currentState == States.None && type == TokenType.TOKEN_DICTIONARY)
            {
                currentState = States.Dictionary;
                return States.Dictionary;
            }

            if(currentState == States.Dictionary && type ==TokenType.TOKEN_LEFT_ANGLE_BRACKET)
            {
                currentState = States.TypeKey;
                return States.TypeKey;

            }

            if (currentState == States.TypeKey && (type == TokenType.TOKEN_INT || type == TokenType.TOKEN_STRING))
            {
                currentState = States.Comma;
                return States.Comma;
            }

            if (currentState == States.Comma && type == TokenType.TOKEN_COMMA)
            {
                currentState = States.TypeValue;
                return States.TypeValue;
            }

            if (currentState == States.TypeValue && (type == TokenType.TOKEN_INT || type == TokenType.TOKEN_STRING))
            {
                currentState = States.Close;
                return States.Close;
            }

            if (currentState == States.Close && type == TokenType.TOKEN_RIGHT_ANGLE_BRACKET)
            {
                currentState = States.Identifier;
                return States.Identifier;
            }

            if (currentState == States.Identifier && type == TokenType.TOKEN_IDENTIFIER)
            {
                currentState = States.ASSIGNTMENT;
                return States.ASSIGNTMENT;
            }


            if (currentState == States.ASSIGNTMENT && type == TokenType.TOKEN_EQUALS)
            {
                currentState = States.New;
                return States.New;
            }


            if (currentState == States.New && type == TokenType.TOKEN_NEW)
            {
                currentState = States.Dictionary2;
                return States.Dictionary2;
            }

            if (currentState == States.Dictionary2 && type == TokenType.TOKEN_DICTIONARY)
            {
                currentState = States.Open;
                return States.Open;
            }


            if (currentState == States.Open && type == TokenType.TOKEN_LEFT_ANGLE_BRACKET)
            {
                currentState = States.TypeKey2;
                return States.TypeKey2;
            }

            if (currentState == States.TypeKey2 && (type == TokenType.TOKEN_INT || type == TokenType.TOKEN_STRING))
            {
                currentState = States.Comma2;
                return States.Comma2;
            }

            if (currentState == States.Comma2 && type == TokenType.TOKEN_COMMA)
            {
                currentState = States.TypeValue2;
                return States.TypeValue2;
            }

            if (currentState == States.TypeValue2 && (type == TokenType.TOKEN_INT || type == TokenType.TOKEN_STRING))
            {
                currentState = States.Close2;
                return States.Close2;
            }

            if (currentState == States.Close2 && type == TokenType.TOKEN_RIGHT_ANGLE_BRACKET)
            {
                currentState = States.Left_Bracket;
                return States.Left_Bracket;
            }

            if (currentState == States.Left_Bracket && type == TokenType.TOKEN_LEFT_PARANTHESES)
            {
                currentState = States.Right_Bracket;
                return States.Right_Bracket;
            }

            if (currentState == States.Right_Bracket && type == TokenType.TOKEN_RIGHT_PARANTHESES)
            {
                currentState = States.End;
                return States.End;
            }

            if(currentState == States.End && type == TokenType.TOKEN_SEMICOLON)
            {
                currentState = States.None;
                return States.None;
            }
            currentState = States.ERROR;
            return States.ERROR;
        }
    }
}
