using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace OneMoreScript
{
    public class Token
    {
        public static Token EOF = new Token(-1);
        public static String EOL = "\\n";
        private int lineNumber;

        protected Token(int line)
        {
            lineNumber = line;
        }

        public virtual int getLineNumber() { return lineNumber; }
        public virtual bool isIdentifier() { return false; }
        public virtual bool isNumber() { return false; }
        public virtual bool isString() { return false; }
        public virtual int getNumber() { throw new OMSException("not number token"); }
        public virtual String getText() { return ""; }
    }
}
