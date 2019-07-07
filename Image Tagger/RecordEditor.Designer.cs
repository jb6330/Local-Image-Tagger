namespace Image_Tagger
{
    partial class RecordEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Picture = new System.Windows.Forms.PictureBox();
            this.Tags = new System.Windows.Forms.TextBox();
            this.FileName = new System.Windows.Forms.Label();
            this.Folder = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Picture)).BeginInit();
            this.SuspendLayout();
            // 
            // Picture
            // 
            this.Picture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Picture.Location = new System.Drawing.Point(0, 0);
            this.Picture.Name = "Picture";
            this.Picture.Size = new System.Drawing.Size(347, 229);
            this.Picture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Picture.TabIndex = 0;
            this.Picture.TabStop = false;
            // 
            // Tags
            // 
            this.Tags.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Tags.Location = new System.Drawing.Point(0, 255);
            this.Tags.Name = "Tags";
            this.Tags.Size = new System.Drawing.Size(347, 20);
            this.Tags.TabIndex = 2;
            this.Tags.Text = "Tags";
            // 
            // FileName
            // 
            this.FileName.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.FileName.Location = new System.Drawing.Point(0, 229);
            this.FileName.Name = "FileName";
            this.FileName.Size = new System.Drawing.Size(347, 13);
            this.FileName.TabIndex = 3;
            this.FileName.Text = "File";
            // 
            // Folder
            // 
            this.Folder.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Folder.Location = new System.Drawing.Point(0, 242);
            this.Folder.Name = "Folder";
            this.Folder.Size = new System.Drawing.Size(347, 13);
            this.Folder.TabIndex = 4;
            this.Folder.Text = "Folder";
            // 
            // RecordEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Picture);
            this.Controls.Add(this.FileName);
            this.Controls.Add(this.Folder);
            this.Controls.Add(this.Tags);
            this.Name = "RecordEditor";
            this.Size = new System.Drawing.Size(347, 275);
            ((System.ComponentModel.ISupportInitialize)(this.Picture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox Picture;
        private System.Windows.Forms.TextBox Tags;
        private System.Windows.Forms.Label FileName;
        private System.Windows.Forms.Label Folder;
    }
}
