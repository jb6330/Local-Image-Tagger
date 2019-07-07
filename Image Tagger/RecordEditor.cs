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

    /// <summary>A class for editing a record.</summary>
    public partial class RecordEditor : UserControl
    {
        private PictureRecord record;

        /// <summary>Initializes a new instance of the <see cref="RecordEditor"/> class.</summary>
        public RecordEditor()
        {
            this.InitializeComponent();
        }

        /// <summary>Load the record that will be edited.</summary>
        /// <param name="record">The record to edit.</param>
        public void LoadRecord(PictureRecord record)
        {
            this.record = record;
            this.Picture.ImageLocation = record.FileLocation;
            this.Tags.Text = string.Join(", ", record.Tags);
            this.FileName.Text = Path.GetFileName(record.FileLocation);
            this.Folder.Text = Path.GetDirectoryName(record.FileLocation);
        }

        /// <summary>Gets a changed version of the record.</summary>
        /// <returns>The changed record.</returns>
        public PictureRecord GetChanges()
        {
            this.record.Tags = new HashSet<string>(this.Tags.Text.Split(',').Select(tag => tag.Trim().ToLowerInvariant()));
            return this.record;
        }
    }
}
