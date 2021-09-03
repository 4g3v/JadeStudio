namespace JadeStudio.BIG
{
    partial class TextForm
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
            this.textTreeView = new System.Windows.Forms.TreeView();
            this.textRichTextBox = new System.Windows.Forms.RichTextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadTextFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveTextFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.applyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.toggleViewModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeTextColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeBackgroundColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textTreeView
            // 
            this.textTreeView.Dock = System.Windows.Forms.DockStyle.Left;
            this.textTreeView.Location = new System.Drawing.Point(0, 27);
            this.textTreeView.Name = "textTreeView";
            this.textTreeView.Size = new System.Drawing.Size(212, 355);
            this.textTreeView.TabIndex = 0;
            this.textTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.textTreeView_AfterSelect);
            // 
            // textRichTextBox
            // 
            this.textRichTextBox.BackColor = System.Drawing.Color.CadetBlue;
            this.textRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textRichTextBox.Location = new System.Drawing.Point(212, 27);
            this.textRichTextBox.Name = "textRichTextBox";
            this.textRichTextBox.ReadOnly = true;
            this.textRichTextBox.Size = new System.Drawing.Size(595, 355);
            this.textRichTextBox.TabIndex = 1;
            this.textRichTextBox.Text = "";
            this.textRichTextBox.TextChanged += new System.EventHandler(this.textRichTextBox_TextChanged);
            this.textRichTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textRichTextBox_KeyDown);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {this.fileToolStripMenuItem, this.applyToolStripMenuItem, this.searchToolStripTextBox, this.toggleViewModeToolStripMenuItem, this.changeTextColorToolStripMenuItem, this.changeBackgroundColorToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(807, 27);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {this.loadTextFileToolStripMenuItem, this.saveTextFileToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 23);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadTextFileToolStripMenuItem
            // 
            this.loadTextFileToolStripMenuItem.Name = "loadTextFileToolStripMenuItem";
            this.loadTextFileToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.loadTextFileToolStripMenuItem.Text = "Load text file";
            this.loadTextFileToolStripMenuItem.Click += new System.EventHandler(this.loadTextFileToolStripMenuItem_Click);
            // 
            // saveTextFileToolStripMenuItem
            // 
            this.saveTextFileToolStripMenuItem.Enabled = false;
            this.saveTextFileToolStripMenuItem.Name = "saveTextFileToolStripMenuItem";
            this.saveTextFileToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.saveTextFileToolStripMenuItem.Text = "Save text file";
            this.saveTextFileToolStripMenuItem.Click += new System.EventHandler(this.saveTextFileToolStripMenuItem_Click);
            // 
            // applyToolStripMenuItem
            // 
            this.applyToolStripMenuItem.Enabled = false;
            this.applyToolStripMenuItem.Name = "applyToolStripMenuItem";
            this.applyToolStripMenuItem.Size = new System.Drawing.Size(50, 23);
            this.applyToolStripMenuItem.Text = "Apply";
            this.applyToolStripMenuItem.Click += new System.EventHandler(this.applyToolStripMenuItem_Click);
            // 
            // searchToolStripTextBox
            // 
            this.searchToolStripTextBox.Name = "searchToolStripTextBox";
            this.searchToolStripTextBox.Size = new System.Drawing.Size(233, 23);
            this.searchToolStripTextBox.Text = "Search";
            this.searchToolStripTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.searchToolStripTextBox_KeyDown);
            this.searchToolStripTextBox.Click += new System.EventHandler(this.searchToolStripTextBox_Click);
            // 
            // toggleViewModeToolStripMenuItem
            // 
            this.toggleViewModeToolStripMenuItem.Name = "toggleViewModeToolStripMenuItem";
            this.toggleViewModeToolStripMenuItem.Size = new System.Drawing.Size(115, 23);
            this.toggleViewModeToolStripMenuItem.Text = "Toggle view mode";
            this.toggleViewModeToolStripMenuItem.Click += new System.EventHandler(this.toggleViewModeToolStripMenuItem_Click);
            // 
            // changeTextColorToolStripMenuItem
            // 
            this.changeTextColorToolStripMenuItem.Enabled = false;
            this.changeTextColorToolStripMenuItem.Name = "changeTextColorToolStripMenuItem";
            this.changeTextColorToolStripMenuItem.Size = new System.Drawing.Size(113, 23);
            this.changeTextColorToolStripMenuItem.Text = "Change text color";
            this.changeTextColorToolStripMenuItem.Click += new System.EventHandler(this.changeTextColorToolStripMenuItem_Click);
            // 
            // changeBackgroundColorToolStripMenuItem
            // 
            this.changeBackgroundColorToolStripMenuItem.Name = "changeBackgroundColorToolStripMenuItem";
            this.changeBackgroundColorToolStripMenuItem.Size = new System.Drawing.Size(157, 23);
            this.changeBackgroundColorToolStripMenuItem.Text = "Change background color";
            this.changeBackgroundColorToolStripMenuItem.Click += new System.EventHandler(this.changeBackgroundColorToolStripMenuItem_Click);
            // 
            // TextForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 382);
            this.Controls.Add(this.textRichTextBox);
            this.Controls.Add(this.textTreeView);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "TextForm";
            this.Text = "Text file editor (FD*.bin)";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TreeView textTreeView;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadTextFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveTextFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem applyToolStripMenuItem;
        private System.Windows.Forms.RichTextBox textRichTextBox;
        private System.Windows.Forms.ToolStripTextBox searchToolStripTextBox;
        private System.Windows.Forms.ToolStripMenuItem toggleViewModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeBackgroundColorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeTextColorToolStripMenuItem;
    }
}