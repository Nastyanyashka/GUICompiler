using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUICompiler
{
    internal class Error
    {
        int start_pos;
        int end_pos;
        int current_line;
        string text;
        TokenType token;
        States previousState;
        public Error(int start_pos, int end_pos, string text, TokenType token, int current_line, States previousState)
        {
            this.start_pos = start_pos;
            this.end_pos = end_pos;
            this.text = text;
            this.token = token;
            this.current_line = current_line;
            this.previousState = previousState;
        }

        public int Start_pos { get { return start_pos; } set { start_pos = value; } }
        public int End_pos { get { return end_pos; } set { end_pos = value; }}
        public string Text { get { return text; } set { text = value; } }
        public TokenType Token { get { return token; } set { token = value; } }
        public int Current_line { get { return current_line; } set { current_line = value; } }

        public States PreviousState { get {  return previousState; } set {  previousState = value; } }


    }
}
