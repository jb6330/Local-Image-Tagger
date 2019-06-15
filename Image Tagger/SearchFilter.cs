namespace Image_Tagger
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>A filter that returns records whose tags match the search equation.</summary>
    internal class SearchFilter
    {
        /// <summary>Operators that can be used in the search equation.</summary>
        private enum OperatorTypes
        {
            NONE = 0,
            AND,
            OR,
            NOT,
            OpenBracket,
            CloseBracket,
        }

        /// <summary>Filter a list of records based on a search equation.</summary>
        /// <param name="pictures">The list of records.</param>
        /// <param name="equation">An equation that the desired records must match.</param>
        /// <returns>A list of matching records.</returns>
        public List<PictureRecord> CreateFilteredList(List<PictureRecord> pictures, string equation)
        {
            string[] parsedEquation = ConvertToReversePolishNotation(equation);
            var output = new List<PictureRecord>();
            foreach (PictureRecord record in pictures)
            {
                if (this.SolveEquation(parsedEquation, record.Tags))
                {
                    output.Add(record);
                }
            }

            return output;
        }

        /// <summary>Parses an equation into a list of terms and operators in Reverse Polish Notation.</summary>
        /// <param name="equation">The equation to parse.</param>
        /// <returns>The terms and operators in order of Reverse Polish Notation.</returns>
        private static string[] ConvertToReversePolishNotation(string equation)
        {
            List<string> output = new List<string>();
            Stack<OperatorTypes> operatorStack = new Stack<OperatorTypes>();

            string cleanedString = equation;
            cleanedString = cleanedString.Replace("(", " ( ");
            cleanedString = cleanedString.Replace(")", " ) ");
            string[] terms = cleanedString.Split(' ');

            string newTerm = string.Empty;
            foreach (string term in terms)
            {
                switch (term)
                {
                    case "(":
                        if (newTerm != string.Empty)
                        {
                            throw new InvalidOperationException("( operater found directly following term");
                        }

                        operatorStack.Push(OperatorTypes.OpenBracket);
                        break;
                    case ")":
                        if (newTerm != string.Empty)
                        {
                            output.Add(newTerm.Trim().ToLowerInvariant());
                        }

                        newTerm = string.Empty;
                        while (operatorStack.Count > 0 && operatorStack.Peek() != OperatorTypes.OpenBracket)
                        {
                            output.Add(operatorStack.Pop().ToString());
                        }

                        operatorStack.Pop();
                        break;
                    case "NOT":
                        if (newTerm != string.Empty)
                        {
                            throw new InvalidOperationException("NOT operater found directly following term");
                        }

                        operatorStack.Push(OperatorTypes.NOT);
                        break;
                    case "AND":
                        if (newTerm != string.Empty)
                        {
                            output.Add(newTerm.Trim().ToLowerInvariant());
                        }

                        newTerm = string.Empty;
                        while (operatorStack.Count > 0 && operatorStack.Peek() == OperatorTypes.NOT)
                        {
                            output.Add(operatorStack.Pop().ToString());
                        }

                        operatorStack.Push(OperatorTypes.AND);
                        break;
                    case "OR":
                        if (newTerm != string.Empty)
                        {
                            output.Add(newTerm.Trim().ToLowerInvariant());
                        }

                        newTerm = string.Empty;
                        while (operatorStack.Count > 0 && operatorStack.Peek() == OperatorTypes.NOT)
                        {
                            output.Add(operatorStack.Pop().ToString());
                        }

                        operatorStack.Push(OperatorTypes.OR);
                        break;
                    case "":
                        break;
                    default:
                        newTerm += " " + term;
                        break;
                }
            }

            if (newTerm != string.Empty)
            {
                output.Add(newTerm.Trim().ToLowerInvariant());
            }

            while (operatorStack.Count > 0)
            {
                output.Add(operatorStack.Pop().ToString());
            }

            return output.ToArray();
        }

        /// <summary>Evaluate the equation on against a list of tags.</summary>
        /// <param name="equation">The search equation to evaluate.</param>
        /// <param name="tags">The list of tags that the image has.</param>
        /// <returns>Whether or not the tags satisfy the search equation.</returns>
        private bool SolveEquation(string[] equation, HashSet<string> tags)
        {
            Stack<bool> buffer = new Stack<bool>();
            bool operand1, operand2;
            foreach (string entry in equation)
            {
                switch (entry)
                {
                    case "NOT":
                        buffer.Push(!buffer.Pop());
                        break;
                    case "AND":
                        operand1 = buffer.Pop();
                        operand2 = buffer.Pop();
                        buffer.Push(operand1 && operand2);
                        break;
                    case "OR":
                        operand1 = buffer.Pop();
                        operand2 = buffer.Pop();
                        buffer.Push(operand1 || operand2);
                        break;
                    default:
                        buffer.Push(tags.Contains(entry));
                        break;
                }
            }

            if (buffer.Count != 1)
            {
                throw new ArgumentException("Invalid equation");
            }

            return buffer.Pop();
        }
    }
}
