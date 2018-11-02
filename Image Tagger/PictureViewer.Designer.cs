namespace Image_Tagger
{
    partial class PictureViewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ImageSelector = new System.Windows.Forms.FlowLayoutPanel();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SearchBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // ImageSelector
            // 
            this.ImageSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImageSelector.Location = new System.Drawing.Point(0, 0);
            this.ImageSelector.Name = "ImageSelector";
            this.ImageSelector.Size = new System.Drawing.Size(800, 450);
            this.ImageSelector.TabIndex = 0;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // SearchBox
            // 
            this.SearchBox.DetectUrls = false;
            this.SearchBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.SearchBox.Location = new System.Drawing.Point(0, 0);
            this.SearchBox.Name = "SearchBox";
            this.SearchBox.Size = new System.Drawing.Size(800, 22);
            this.SearchBox.TabIndex = 0;
            this.SearchBox.Text = "Sample Text";
            // 
            // PictureViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.SearchBox);
            this.Controls.Add(this.ImageSelector);
            this.Name = "PictureViewer";
            this.Text = "Image Tagger";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel ImageSelector;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.RichTextBox SearchBox;
    }
}

