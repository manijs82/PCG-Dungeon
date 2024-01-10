using System.Collections.Generic;

namespace LSystem
{
    public class Grammar
    {
        public Alphabet alphabet;
        public List<Symbol> axiom;
        public List<Rule> rules;
        
        public Grammar(Alphabet alphabet, List<Symbol> axiom, List<Rule> rules)
        {
            this.alphabet = alphabet;
            this.axiom = axiom;
            this.rules = rules;
        }

        public List<Symbol> Produce(int recursion)
        {
            List<Symbol> o = new List<Symbol>(axiom);
            
            for (int i = 0; i < recursion; i++)
            {
                int length = o.Count;
                int pos = 0;
                for (int j = 0; j < length; j++)
                {
                    Rule rule = GetRuleForSymbol(o[pos]);
                    o.RemoveAt(pos);
                    o.InsertRange(pos, rule.successor);
                    pos += rule.successor.Count;
                }
            }

            return o;
        }

        public Rule GetRuleForSymbol(Symbol symbol)
        {
            foreach (var rule in rules)
            {
                if (rule.predecessor == symbol)
                    return rule;
            }

            return new Rule(symbol, new List<Symbol> { symbol });
        }
    }
}