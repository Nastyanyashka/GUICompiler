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
        Identifier,
        ASSIGNTMENT,
        New,
        Dictionary2,
        Open,
        TypeKey2,
        Comma2,
        TypeValue2,
        Close2,
        Left_Bracket,
        Right_Bracket,
        End,
        ERROR,
        Whitespace

    };

    internal class Parser
    {
        States currentState;
        States previousState;
        public Parser()
        {
            currentState = States.None;
        }
        public States CurrentState { get { return currentState; } set { currentState = value; } }


        public States PreviousState { get { return previousState; } set { previousState = value; } }

        public States MatchToken(TokenType type, States state)
        {
            if (type == TokenType.TOKEN_WHITESPACE || type == TokenType.TOKEN_WHITESPACE_R || type == TokenType.TOKEN_WHITESPACE_N)
            {
                return States.Whitespace;
            }

            if (state == States.None && type == TokenType.TOKEN_DICTIONARY)
            {
                return States.Dictionary;
            }

            if (state == States.Dictionary && type == TokenType.TOKEN_LEFT_ANGLE_BRACKET)
            {
                return States.TypeKey;
            }

            if (state == States.TypeKey && (type == TokenType.TOKEN_INT || type == TokenType.TOKEN_STRING))
            {
                return States.Comma;
            }

            if (state == States.Comma && type == TokenType.TOKEN_COMMA)
            {
                return States.TypeValue;
            }

            if (state == States.TypeValue && (type == TokenType.TOKEN_INT || type == TokenType.TOKEN_STRING))
            {
                return States.Close;
            }

            if (state == States.Close && type == TokenType.TOKEN_RIGHT_ANGLE_BRACKET)
            {
                return States.Identifier;
            }

            if (state == States.Identifier && type == TokenType.TOKEN_IDENTIFIER)
            {
                return States.ASSIGNTMENT;
            }


            if (state == States.ASSIGNTMENT && type == TokenType.TOKEN_EQUALS)
            {
                return States.New;
            }


            if (state == States.New && type == TokenType.TOKEN_NEW)
            {
                return States.Dictionary2;
            }

            if (state == States.Dictionary2 && type == TokenType.TOKEN_DICTIONARY)
            {
                return States.Open;
            }


            if (state == States.Open && type == TokenType.TOKEN_LEFT_ANGLE_BRACKET)
            {
                return States.TypeKey2;
            }

            if (state == States.TypeKey2 && (type == TokenType.TOKEN_INT || type == TokenType.TOKEN_STRING))
            {
                return States.Comma2;
            }

            if (state == States.Comma2 && type == TokenType.TOKEN_COMMA)
            {
                return States.TypeValue2;
            }

            if (state == States.TypeValue2 && (type == TokenType.TOKEN_INT || type == TokenType.TOKEN_STRING))
            {
                return States.Close2;
            }

            if (state == States.Close2 && type == TokenType.TOKEN_RIGHT_ANGLE_BRACKET)
            {
                return States.Left_Bracket;
            }

            if (state == States.Left_Bracket && type == TokenType.TOKEN_LEFT_PARANTHESES)
            {
                return States.Right_Bracket;
            }

            if (state == States.Right_Bracket && type == TokenType.TOKEN_RIGHT_PARANTHESES)
            {
                return States.End;
            }

            if (state == States.End && type == TokenType.TOKEN_SEMICOLON) //currentState == States.End &&
            {
                return States.None;
            }

            return States.ERROR;
        }

        public States ParseError(TokenType type)
        {
            States temp = previousState;
            currentState = previousState;
            if (Parse(type) == States.ERROR)
            {
                currentState = States.ERROR;
                previousState = temp;
            }
            return currentState;
        }
        public States Parse(TokenType type)
        {


            if (type == TokenType.TOKEN_WHITESPACE || type == TokenType.TOKEN_WHITESPACE_R || type == TokenType.TOKEN_WHITESPACE_N)
            {
                return States.Whitespace;
            }

            if (currentState != States.ERROR)
            {
                previousState = currentState;
            }


            if (currentState == States.None && type == TokenType.TOKEN_DICTIONARY)
            {
                currentState = States.Dictionary;
                return States.Dictionary;
            }

            if (currentState == States.Dictionary && type == TokenType.TOKEN_LEFT_ANGLE_BRACKET)
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

            if (currentState == States.End && type == TokenType.TOKEN_SEMICOLON) //currentState == States.End &&
            {

                currentState = States.None;
                return States.None;
            }



            currentState = States.ERROR;
            return States.ERROR;
        }
    }
}
