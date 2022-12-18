// See https://aka.ms/new-console-template for more information
namespace OneMoreScript
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var l = new Lexer("./test.txt");

            for(Token t; (t = l.read()) != Token.EOF;)
            {
                Console.WriteLine("=> " + t.getText());
            }
        }
    }
}
