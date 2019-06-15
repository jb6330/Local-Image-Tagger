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
        private SearchFilter filter = new SearchFilter();

        /// <summary>Initializes a new instance of the <see cref="PictureViewer"/> class.</summary>
        public PictureViewer()
        {
            this.InitializeComponent();
            this.database = new Database();
            this.database.LoadRecords(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings.xml"));
            foreach (var record in this.database.PictureRecords)
            {
                this.AddRecord(record);
            }
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
        }

        // TODO: split this off into a separate thread
        private void Search(string text)
        {
            this.ImageSelector.Controls.Clear();
            text = text.Trim();

            if (text == string.Empty)
            {
                foreach (var entry in this.database.PictureRecords)
                {
                    this.AddRecord(entry);
                }
            }
            else
            {
                var records = this.filter.CreateFilteredList(this.database.PictureRecords.ToList(), text);
                foreach (var entry in records)
                {
                    this.AddRecord(entry);
                }
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
