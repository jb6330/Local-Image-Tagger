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
            foreach (var record in database.PictureRecords)
            {
                AddRecord(record);
            }
        }

        private void AddRecord(PictureRecord record)
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

        public static string[] ConvertToReversePolishNotation(string equation)
        {
            List<string> output = new List<string>();
            Stack<OperatorTypes> operatorStack = new Stack<OperatorTypes>();

            string cleanedString = equation;
            cleanedString = cleanedString.Replace("(", " ( ");
            cleanedString = cleanedString.Replace(")", " ) ");
            string[] terms = cleanedString.Split(' ');

            string newTerm = "";
            foreach (string term in terms)
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
                        if (newTerm != "") { throw new InvalidOperationException("NOT operater found directly following term"); }
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
            if (newTerm != "")
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
            foreach (string entry in equation) {
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

        // TODO: split this off into a separate thread
        private void search(string text)
        {
            if(text.Trim() == string.Empty)
            {
                foreach (var entry in database.PictureRecords)
                {
                    displayedPictures[entry].Invoke((Action)(() => displayedPictures[entry].Visible = true));
                }
            }
            string[] equation = ConvertToReversePolishNotation(text);
            foreach (var entry in database.PictureRecords)
            {
                bool displayed = SolveEquation(equation, entry.tags);

                displayedPictures[entry].Invoke((Action)(
                    () => {
                        if (displayedPictures[entry].Visible != displayed)
                        {
                            displayedPictures[entry].Visible = displayed;
                        }
                    }));
            }
        }

        private void button1_Click(object sender, EventArgs eventArgs)
        {
            search(SearchBox.Text);
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs eventArgs)
        {
            if (eventArgs.KeyCode == Keys.Enter)
            {
                eventArgs.Handled = true;
                search(SearchBox.Text);
            }
        }

        private void addPictureToolStripMenuItem_Click(object sender, EventArgs eventArgs)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.CheckFileExists = true;
            if(dialog.ShowDialog() == DialogResult.OK && dialog.FileName.Trim() != string.Empty)
            {
                try
                {
                    var record = database.AddPicture(dialog.FileName);
                    AddRecord(record);
                }
                catch(Exception e)
                {
                    // todo: log exception
                }
            }
        }

        private void addFolderToolStripMenuItem_Click(object sender, EventArgs eventArgs)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if(dialog.ShowDialog() == DialogResult.OK && dialog.SelectedPath.Trim() != string.Empty)
            {
                database.AddFolder(dialog.SelectedPath);
                displayedPictures.Clear();
                ImageSelector.Controls.Clear();
                foreach (var record in database.PictureRecords)
                {
                    AddRecord(record);
                }
            }
        }

        private void saveChangesToolStripMenuItem_Click(object sender, EventArgs eventArgs)
        {
            database.SaveRecords();
        }
    }
}
