namespace Image_Tagger
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    /// <summary>A class that manages the display of the pictures.</summary>
    public partial class PictureViewer : Form
    {
        private Database database;
        private Dictionary<PictureRecord, PictureBox> displayedPictures;

        /// <summary>Initializes a new instance of the <see cref="PictureViewer"/> class.</summary>
        public PictureViewer()
        {
            this.InitializeComponent();
            this.database = new Database();
            this.database.LoadRecords(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings.xml"));
            this.displayedPictures = new Dictionary<PictureRecord, PictureBox>();
            foreach (var record in this.database.PictureRecords)
            {
                this.AddRecord(record);
            }
        }

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

        /// <summary>Parses an equation into a list of terms and operators in Reverse Polish Notation.</summary>
        /// <param name="equation">The equation to parse.</param>
        /// <returns>The terms and operators in order of Reverse Polish Notation.</returns>
        public static string[] ConvertToReversePolishNotation(string equation)
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
        public bool SolveEquation(string[] equation, HashSet<string> tags)
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

        private void AddRecord(PictureRecord record)
        {
            PictureBox pictureBox = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                ImageLocation = record.FileLocation,
                Size = new Size(300, 300),
            };
            this.ImageSelector.Controls.Add(pictureBox);
            this.displayedPictures.Add(record, pictureBox);
        }

        // TODO: split this off into a separate thread
        private void Search(string text)
        {
            if (text.Trim() == string.Empty)
            {
                foreach (var entry in this.database.PictureRecords)
                {
                    this.displayedPictures[entry].Invoke((Action)(() => this.displayedPictures[entry].Visible = true));
                }
            }

            string[] equation = ConvertToReversePolishNotation(text);
            foreach (var entry in this.database.PictureRecords)
            {
                bool displayed = this.SolveEquation(equation, entry.Tags);

                this.displayedPictures[entry].Invoke((Action)(
                    () =>
                    {
                        if (this.displayedPictures[entry].Visible != displayed)
                        {
                            this.displayedPictures[entry].Visible = displayed;
                        }
                    }));
            }
        }

        private void Button1_Click(object sender, EventArgs eventArgs)
        {
            this.Search(this.SearchBox.Text);
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs eventArgs)
        {
            if (eventArgs.KeyCode == Keys.Enter)
            {
                eventArgs.Handled = true;
                this.Search(this.SearchBox.Text);
            }
        }

        private void AddPictureToolStripMenuItem_Click(object sender, EventArgs eventArgs)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.CheckFileExists = true;
            if (dialog.ShowDialog() == DialogResult.OK && dialog.FileName.Trim() != string.Empty)
            {
                try
                {
                    var record = this.database.AddPicture(dialog.FileName);
                    this.AddRecord(record);
                }
                catch (Exception)
                {
                    // todo: log exception
                }
            }
        }

        private void AddFolderToolStripMenuItem_Click(object sender, EventArgs eventArgs)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK && dialog.SelectedPath.Trim() != string.Empty)
            {
                this.database.AddFolder(dialog.SelectedPath);
                this.displayedPictures.Clear();
                this.ImageSelector.Controls.Clear();
                foreach (var record in this.database.PictureRecords)
                {
                    this.AddRecord(record);
                }
            }
        }

        private void SaveChangesToolStripMenuItem_Click(object sender, EventArgs eventArgs)
        {
            this.database.SaveRecords();
        }
    }
}
