using System.Collections.Generic;

namespace LSystem
{
    public class Alphabet
    {
        public List<Symbol> variables;
        public List<Symbol> constants;

        public Alphabet(List<Symbol> variables, List<Symbol> constants)
        {
            this.variables = variables;
            this.constants = constants;
        }
    }

    public struct Symbol
    {
        public string value;
        public bool constant;

        public Symbol(string value, bool constant)
        {
            this.value = value;
            this.constant = constant;
        }
        
        public static bool operator ==(Symbol a, Symbol b)
        {
            return a.value == b.value;
        }
        
        public static bool operator !=(Symbol a, Symbol b)
        {
            return !(a == b);
        }
    }

    public class Rule
    {
        public Symbol predecessor;
        public List<Symbol> successor;

        public Rule(Symbol predecessor, List<Symbol> successor)
        {
            this.predecessor = predecessor;
            this.successor = successor;
        }
    }
}