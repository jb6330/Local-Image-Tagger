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
        private ToolTip toolTip = new ToolTip();
        private Database database;
        private SearchFilter filter = new SearchFilter();
        private ContextMenuStrip contextMenu;
        private string contextMenuPath = string.Empty;

        /// <summary>Initializes a new instance of the <see cref="PictureViewer"/> class.</summary>
        public PictureViewer()
        {
            this.InitializeComponent();

            ToolStripMenuItem copyPath = new ToolStripMenuItem("Copy Path");
            copyPath.Click += this.CopyPath_Click;
            copyPath.Name = "CopyPath";

            ToolStripMenuItem copyFile = new ToolStripMenuItem("Copy File");
            copyFile.Click += this.CopyFile_Click;
            copyFile.Name = "CopyFile";

            ToolStripMenuItem edit = new ToolStripMenuItem("Edit");
            edit.Click += this.Edit_Click;
            edit.Name = "Edit";

            this.contextMenu = new ContextMenuStrip();
            this.paginatedView1.ContextMenuStrip = this.contextMenu;
            this.contextMenu.Opening += this.ContextMenu_Opening;
            this.contextMenu.Items.Add(copyPath);
            this.contextMenu.Items.Add(copyFile);
            this.contextMenu.Items.Add(new ToolStripSeparator());
            this.contextMenu.Items.Add(edit);

            this.database = new Database();
            this.database.LoadRecords(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings.xml"));
            this.paginatedView1.DisplayItems(this.database.PictureRecords.Values.ToList());
        }

        private void CopyFile_Click(object sender, EventArgs e)
        {
            if (this.contextMenu.SourceControl is PictureBox picture && picture.Tag is Guid id)
            {
                var record = this.database.PictureRecords[id];
                Clipboard.SetData(DataFormats.FileDrop, new string[] { record.FileLocation });
            }
        }

        private void Edit_Click(object sender, EventArgs e)
        {
            if (this.contextMenu.SourceControl is PictureBox picture && picture.Tag is Guid id)
            {
                Form popup = new Form();
                RecordEditor editor = new RecordEditor();
                editor.LoadRecord(this.database.PictureRecords[id]);
                editor.Dock = DockStyle.Fill;
                popup.Controls.Add(editor);
                popup.ShowDialog();
                var newRecord = editor.GetChanges();
                this.database.UpdateRecord(newRecord);
            }
        }

        private void ContextMenu_Opening(object sender, CancelEventArgs e)
        {
            if (this.contextMenu.SourceControl is PictureBox picture && picture.Tag is Guid id)
            {
                this.contextMenuPath = this.database.PictureRecords[id].FileLocation;
            }
            else
            {
                e.Cancel = true;
            }
        }

        /*private void AddRecord(PictureRecord record)
        {
            PictureBox pictureBox = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                ImageLocation = record.FileLocation,
                Size = new Size(300, 300),
            };

            pictureBox.ContextMenuStrip = this.contextMenu;

            this.reverseLookup.Add(pictureBox, record);

            this.toolTip.SetToolTip(pictureBox, string.Join(", ", record.Tags.OrderBy(tag => tag)));
            this.ImageSelector.Controls.Add(pictureBox);
        }*/

        private void CopyPath_Click(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, this.contextMenuPath);
        }

        // TODO: split this off into a separate thread
        private void Search(string text)
        {
            text = text.Trim();
            List<PictureRecord> recordsToDisplay;

            if (text == string.Empty)
            {
                recordsToDisplay = this.database.PictureRecords.Values.ToList();
            }
            else
            {
                recordsToDisplay = this.filter.CreateFilteredList(this.database.PictureRecords.Values.ToList(), text);
            }

            this.paginatedView1.DisplayItems(recordsToDisplay);
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
                    this.database.AddPicture(dialog.FileName);
                    this.Search(this.SearchBox.Text);
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
                this.Search(this.SearchBox.Text);
            }
        }

        private void SaveChangesToolStripMenuItem_Click(object sender, EventArgs eventArgs)
        {
            this.database.SaveRecords();
        }
    }
}
