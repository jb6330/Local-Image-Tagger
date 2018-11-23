using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Image_Tagger
{
    enum OperatorTypes
    {
        NONE = 0,
        AND,
        OR,
        NOT,
        OpenBracket,
        CloseBracket
    }

    public partial class PictureViewer : Form
    {
        private Database database;
        private Dictionary<PictureRecord, PictureBox> displayedPictures;

        public PictureViewer()
        {
            InitializeComponent();
            database = new Database();
            displayedPictures = new Dictionary<PictureRecord, PictureBox>();
            foreach(var record in database.PictureRecords)
            {
                PictureBox pictureBox = new PictureBox
                {
                    SizeMode = PictureBoxSizeMode.Zoom,
                    ImageLocation = record.fileLocation,
                    Size = new Size(300, 300)
                };
                ImageSelector.Controls.Add(pictureBox);
                displayedPictures.Add(record, pictureBox);
            }
        }

        public static string[] ConvertToReversePolishNotation(string equation)
        {
            List<string> output = new List<string>();
            Stack<OperatorTypes> operatorStack = new Stack<OperatorTypes>();

            string cleanedString = equation;
            cleanedString = cleanedString.Replace("(", " ( ");
            cleanedString = cleanedString.Replace(")", " ) ");
            string[] terms = cleanedString.Split(' ');

            string newTerm = "";
            foreach(string term in terms)
            {
                switch (term)
                {
                    case "(":
                        if (newTerm != "") { throw new InvalidOperationException("( operater found directly following term"); }
                        operatorStack.Push(OperatorTypes.OpenBracket);
                        break;
                    case ")":
                        if (newTerm != "") { output.Add(newTerm.Trim().ToLowerInvariant()); }
                        newTerm = "";
                        while (operatorStack.Count > 0 && operatorStack.Peek() != OperatorTypes.OpenBracket)
                        {
                            output.Add(operatorStack.Pop().ToString());
                        }
                        operatorStack.Pop();
                        break;
                    case "NOT":
                        if(newTerm != "") { throw new InvalidOperationException("NOT operater found directly following term"); }
                        operatorStack.Push(OperatorTypes.NOT);
                        break;
                    case "AND":
                        if (newTerm != "") { output.Add(newTerm.Trim().ToLowerInvariant()); }
                        newTerm = "";
                        while (operatorStack.Count > 0 && operatorStack.Peek() == OperatorTypes.NOT) { output.Add(operatorStack.Pop().ToString()); }
                        operatorStack.Push(OperatorTypes.AND);
                        break;
                    case "OR":
                        if (newTerm != "") { output.Add(newTerm.Trim().ToLowerInvariant()); }
                        newTerm = "";
                        while (operatorStack.Count > 0 && operatorStack.Peek() == OperatorTypes.NOT) { output.Add(operatorStack.Pop().ToString()); }
                        operatorStack.Push(OperatorTypes.OR);
                        break;
                    case "":
                        break;
                    default:
                        newTerm += " " + term;
                        break;
                }
            }
            if(newTerm != "")
            {
                output.Add(newTerm.Trim().ToLowerInvariant());
            }
            while (operatorStack.Count > 0)
            {
                output.Add(operatorStack.Pop().ToString());
            }
            return output.ToArray();
        }

        public bool SolveEquation(string[] equation, HashSet<string> tags)
        {
            Stack<bool> buffer = new Stack<bool>();
            bool operand1, operand2;
            foreach (string entry in equation){
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
            if(buffer.Count != 1)
            {
                throw new ArgumentException("Invalid equation");
            }
            return buffer.Pop();
        }

        // TODO: split this off into a separate thread
        private void search(string text)
        {
            string[] equation = ConvertToReversePolishNotation(text);
            foreach (var entry in database.PictureRecords)
            {
                bool displayed = SolveEquation(equation, entry.tags);
                
                displayedPictures[entry].Invoke((Action)(() => displayedPictures[entry].Visible = displayed));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            search(SearchBox.Text);
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                search(SearchBox.Text);
            }
        }
    }
}
