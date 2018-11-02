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
    }
}
