namespace Image_Tagger
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    /// <summary>Displays picture records using pagination.</summary>
    public partial class PaginatedView : UserControl
    {
        private const int PerPage = 20;
        private ToolTip toolTip = new ToolTip();

        private List<PictureRecord> allRecords = new List<PictureRecord>();
        private int page = 0;

        /// <summary>Initializes a new instance of the <see cref="PaginatedView"/> class.</summary>
        public PaginatedView()
        {
            this.InitializeComponent();
        }

        /// <summary>Displays a new set of records. Clears previously displayed records.</summary>
        /// <param name="items">The records to display.</param>
        public void DisplayItems(IEnumerable<PictureRecord> items)
        {
            this.ClearEntries();
            this.allRecords = items.ToList();
            this.DisplayPage(0);
        }

        /// <summary>Clears all records from the display.</summary>
        public void ClearEntries()
        {
            this.allRecords.Clear();
            this.display.Controls.Clear();
            this.page = 0;
            this.PageNumberLabel.Text = "0/0";
        }

        private void DisplayPage(int page)
        {
            this.page = page;
            this.display.Controls.Clear();
            if (page < 0)
            {
                return;
            }

            int lower = page * PerPage;
            int upper = lower + PerPage;
            for (int i = lower; i < upper && i < this.allRecords.Count; i++)
            {
                PictureRecord record = this.allRecords[i];
                PictureBox pictureBox = new PictureBox
                {
                    SizeMode = PictureBoxSizeMode.Zoom,
                    ImageLocation = record.FileLocation,
                    Size = new Size(300, 300),
                };

                pictureBox.ContextMenuStrip = this.ContextMenuStrip;
                pictureBox.Tag = record.Id;
                this.toolTip.SetToolTip(pictureBox, string.Join(", ", record.Tags.OrderBy(tag => tag)));
                this.display.Controls.Add(pictureBox);
            }

            this.PageNumberLabel.Text = $"{page + 1}/{Math.Ceiling((double)this.allRecords.Count / PerPage)}";
        }

        private void Previous_Click(object sender, EventArgs e)
        {
            if (this.page > 0)
            {
                this.page--;
                this.DisplayPage(this.page);
            }
        }

        private void Next_Click(object sender, EventArgs e)
        {
            if (this.page + 1 < Math.Ceiling((double)this.allRecords.Count / PerPage))
            {
                this.page++;
                this.DisplayPage(this.page);
            }
        }
    }
}
