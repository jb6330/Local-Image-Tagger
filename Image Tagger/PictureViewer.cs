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
        public PictureViewer()
        {
            InitializeComponent();
            database = new Database();
            foreach(var record in database.PictureRecords)
            {
                PictureBox pictureBox = new PictureBox();
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox.ImageLocation = record.fileLocation;
                pictureBox.Size = new Size(300, 300);
                ImageSelector.Controls.Add(pictureBox);
            }
        }
    }
}
