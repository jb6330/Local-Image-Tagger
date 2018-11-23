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
            this.SearchControls = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.SearchControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // ImageSelector
            // 
            this.ImageSelector.AutoSize = true;
            this.ImageSelector.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ImageSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImageSelector.Location = new System.Drawing.Point(0, 23);
            this.ImageSelector.Name = "ImageSelector";
            this.ImageSelector.Size = new System.Drawing.Size(861, 479);
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
            this.SearchBox.Size = new System.Drawing.Size(786, 23);
            this.SearchBox.TabIndex = 0;
            this.SearchBox.Text = "Sample Text";
            this.SearchBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchBox_KeyDown);
            // 
            // SearchControls
            // 
            this.SearchControls.AutoSize = true;
            this.SearchControls.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.SearchControls.Controls.Add(this.SearchBox);
            this.SearchControls.Controls.Add(this.button1);
            this.SearchControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.SearchControls.Location = new System.Drawing.Point(0, 0);
            this.SearchControls.Name = "SearchControls";
            this.SearchControls.Size = new System.Drawing.Size(861, 23);
            this.SearchControls.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Right;
            this.button1.Location = new System.Drawing.Point(786, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Search";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // PictureViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(861, 502);
            this.Controls.Add(this.ImageSelector);
            this.Controls.Add(this.SearchControls);
            this.Name = "PictureViewer";
            this.Text = "Image Tagger";
            this.SearchControls.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel ImageSelector;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.RichTextBox SearchBox;
        private System.Windows.Forms.Panel SearchControls;
        private System.Windows.Forms.Button button1;
    }
}

