using System.Collections.Generic;
using UnityEngine;

namespace LSystem
{
    public class FractalTree
    {
        public FractalTree()
        {
            Symbol zero = new Symbol("0", false);
            Symbol one = new Symbol("1", false);
            Symbol openBracket = new Symbol("[", true);
            Symbol closeBracket = new Symbol("]", true);
            
            Alphabet alphabet = new Alphabet
            (
                new List<Symbol>
                {
                    zero,
                    one
                },
                new List<Symbol>
                {
                    openBracket,
                    closeBracket
                }
            );

            List<Symbol> axiom = new List<Symbol> { zero };

            Rule rule1 = new Rule(one, new List<Symbol> { one, one });
            Rule rule2 = new Rule(zero, new List<Symbol> { one, openBracket, zero, closeBracket, zero });
            List<Rule> rules = new List<Rule> { rule1, rule2 };

            Grammar grammar = new Grammar(alphabet, axiom, rules);
            string output = "";
            foreach (var symbol in grammar.Produce(3))
            {
                output += symbol.value;
            }

            Debug.Log(output);
        }
    }
}