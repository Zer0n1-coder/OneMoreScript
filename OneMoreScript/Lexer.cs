using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections;
using System.Reflection.Metadata.Ecma335;

namespace OneMoreScript
{
    public class Lexer
    {
        public static string regexPat = "\\s*((//.*)|([0-9]+)|(\"(\\\\\"|\\\\\\\\|\\\\n|[^\"])*\")|[A-Z_a-z][A-Z_a-z0-9]*|==|<=|>=|&&|\\|\\|)?";
        private List<Token> queue = new List<Token>();
        private bool hasMore;
        private StreamReader reader;
        private int currentLineNo = -1;

        public Lexer(String filePath)
        {
            hasMore = true; 
            reader = new StreamReader(filePath);
        }
        public Token read()
        {
            if (fillQueue(0))
            {
                var ret = queue.ElementAt(0);
                queue.RemoveAt(0);
                return ret;
            }
            else
            {
                return Token.EOF;
            }
        }
        
        public Token peek(int i)
        {
            if (fillQueue(i))
            {
                return queue.ElementAt(i);
            }
            else
            {
                return Token.EOF;
            }
        }

        private bool fillQueue(int i)
        {
            while (i >= queue.Count)
            {
                if (hasMore)
                {
                    readLine(); 
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        
        protected void readLine()
        {
            String line;
            try
            {
                line = reader.ReadLine();
            }catch(IOException e)
            {
                throw new ParseException(e);
            }
            if(line == null)
            {
                hasMore = false;
                return;
            }
            ++currentLineNo;
            int lineNo = currentLineNo;
            var matches = Regex.Matches(line, regexPat, RegexOptions.Multiline| RegexOptions.Compiled);
            foreach (Match match in matches)
            {
                addToken(lineNo, match);
            }
        }

        protected void addToken(int lineNo, Match matcher)
        {
            if(matcher.Groups[1].Success)
            {
                if (!matcher.Groups[2].Success)
                {
                    Token token;
                    if(matcher.Groups[3].Success)
                    {
                        token = new NumToken(lineNo, int.Parse(matcher.Value)); 
                    }else if(matcher.Groups[4].Success)
                    {
                        token = new StrToken(lineNo, toStringLiteral(matcher.Value));
                    }
                    else
                    {
                        token = new IdToken(lineNo, matcher.Value);
                    }
                    queue.Add(token);   
                }
            }
        }

        protected String toStringLiteral(String s)
        {
            StringBuilder sb = new StringBuilder();
            int len = s.Length - 1;
            for(int i = 1; i < len; ++i)
            {
                char c = s[i];
                if(c == '\\' && i + 1 < len)
                {
                    int c2 = s[i + 1];
                    if(c2 == '"' || c2 == '\\')
                    {
                        c = s[++i];
                    }else if(c2 == 'n')
                    {
                        ++i;
                        c = '\n';
                    }
                }
                sb.Append(c);
            }
            return sb.ToString();
        }

        protected class NumToken : Token
        {
            private int value;
            public NumToken(int line, int v) : base(line) { value = v; }
            public override bool isNumber() { return true; }
            public override String getText() { return value.ToString(); }
            public override int getNumber() { return value; }
        }

        protected class IdToken : Token
        {
            private String text;
            public IdToken(int line, String id):base(line)
            {
                text = id;
            }
            public override bool isIdentifier() { return true; }
            public override string getText()
            {
                return text;
            }
        }

        protected class StrToken : Token
        {
            private String literal;
            public StrToken(int line, String str) : base(line) { literal = str; }
            public override bool isString()
            {
                return true;
            }
            public override string getText()
            {
                return literal;
            }
        }
    }
}
