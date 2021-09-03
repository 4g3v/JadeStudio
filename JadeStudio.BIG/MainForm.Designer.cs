namespace JadeStudio.BIG
{
    partial class MainForm
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
            this.bigListView = new System.Windows.Forms.ListView();
            this.keyHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.offsetHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.nameHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.compressedHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.extractToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extractAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logRichTextBox = new System.Windows.Forms.RichTextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openBFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buildBFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.texturesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extractTexturesFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buildTexturesFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bigListView
            // 
            this.bigListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.keyHeader,
            this.offsetHeader,
            this.nameHeader,
            this.compressedHeader});
            this.bigListView.ContextMenuStrip = this.contextMenuStrip1;
            this.bigListView.Dock = System.Windows.Forms.DockStyle.Left;
            this.bigListView.FullRowSelect = true;
            this.bigListView.HideSelection = false;
            this.bigListView.Location = new System.Drawing.Point(0, 0);
            this.bigListView.Name = "bigListView";
            this.bigListView.Size = new System.Drawing.Size(477, 341);
            this.bigListView.TabIndex = 0;
            this.bigListView.UseCompatibleStateImageBehavior = false;
            this.bigListView.View = System.Windows.Forms.View.Details;
            // 
            // keyHeader
            // 
            this.keyHeader.Text = "Key";
            // 
            // offsetHeader
            // 
            this.offsetHeader.Text = "Offset";
            // 
            // nameHeader
            // 
            this.nameHeader.Text = "Name";
            // 
            // compressedHeader
            // 
            this.compressedHeader.Text = "Compressed";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.extractToolStripMenuItem,
            this.extractAllToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(126, 48);
            // 
            // extractToolStripMenuItem
            // 
            this.extractToolStripMenuItem.Name = "extractToolStripMenuItem";
            this.extractToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.extractToolStripMenuItem.Text = "Extract";
            this.extractToolStripMenuItem.Click += new System.EventHandler(this.extractToolStripMenuItem_Click);
            // 
            // extractAllToolStripMenuItem
            // 
            this.extractAllToolStripMenuItem.Name = "extractAllToolStripMenuItem";
            this.extractAllToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.extractAllToolStripMenuItem.Text = "Extract all";
            this.extractAllToolStripMenuItem.Click += new System.EventHandler(this.extractAllToolStripMenuItem_Click);
            // 
            // logRichTextBox
            // 
            this.logRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logRichTextBox.Location = new System.Drawing.Point(477, 0);
            this.logRichTextBox.Name = "logRichTextBox";
            this.logRichTextBox.Size = new System.Drawing.Size(381, 341);
            this.logRichTextBox.TabIndex = 1;
            this.logRichTextBox.Text = "";
            this.logRichTextBox.WordWrap = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.texturesToolStripMenuItem,
            this.textToolStripMenuItem,
            this.searchToolStripTextBox});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(858, 27);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openBFToolStripMenuItem,
            this.buildBFToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(52, 23);
            this.fileToolStripMenuItem.Text = "Bigfile";
            // 
            // openBFToolStripMenuItem
            // 
            this.openBFToolStripMenuItem.Name = "openBFToolStripMenuItem";
            this.openBFToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.openBFToolStripMenuItem.Text = "Open BF";
            this.openBFToolStripMenuItem.Click += new System.EventHandler(this.openBFToolStripMenuItem_Click);
            // 
            // buildBFToolStripMenuItem
            // 
            this.buildBFToolStripMenuItem.Name = "buildBFToolStripMenuItem";
            this.buildBFToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.buildBFToolStripMenuItem.Text = "Build BF";
            this.buildBFToolStripMenuItem.Click += new System.EventHandler(this.buildBFToolStripMenuItem_Click);
            // 
            // texturesToolStripMenuItem
            // 
            this.texturesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.extractTexturesFileToolStripMenuItem,
            this.buildTexturesFileToolStripMenuItem});
            this.texturesToolStripMenuItem.Name = "texturesToolStripMenuItem";
            this.texturesToolStripMenuItem.Size = new System.Drawing.Size(62, 23);
            this.texturesToolStripMenuItem.Text = "Textures";
            // 
            // extractTexturesFileToolStripMenuItem
            // 
            this.extractTexturesFileToolStripMenuItem.Name = "extractTexturesFileToolStripMenuItem";
            this.extractTexturesFileToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.extractTexturesFileToolStripMenuItem.Text = "Extract textures file";
            this.extractTexturesFileToolStripMenuItem.Click += new System.EventHandler(this.extractTexturesFileToolStripMenuItem_Click);
            // 
            // buildTexturesFileToolStripMenuItem
            // 
            this.buildTexturesFileToolStripMenuItem.Name = "buildTexturesFileToolStripMenuItem";
            this.buildTexturesFileToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.buildTexturesFileToolStripMenuItem.Text = "Build textures file";
            this.buildTexturesFileToolStripMenuItem.Click += new System.EventHandler(this.buildTexturesFileToolStripMenuItem_Click);
            // 
            // textToolStripMenuItem
            // 
            this.textToolStripMenuItem.Name = "textToolStripMenuItem";
            this.textToolStripMenuItem.Size = new System.Drawing.Size(74, 23);
            this.textToolStripMenuItem.Text = "Text Editor";
            this.textToolStripMenuItem.Click += new System.EventHandler(this.textToolStripMenuItem_Click);
            // 
            // searchToolStripTextBox
            // 
            this.searchToolStripTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.searchToolStripTextBox.Name = "searchToolStripTextBox";
            this.searchToolStripTextBox.Size = new System.Drawing.Size(200, 23);
            this.searchToolStripTextBox.Text = "Search";
            this.searchToolStripTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.searchToolStripTextBox_KeyDown);
            this.searchToolStripTextBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.searchToolStripTextBox_MouseDown);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.logRichTextBox);
            this.panel1.Controls.Add(this.bigListView);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(858, 341);
            this.panel1.TabIndex = 4;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(858, 368);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "JadeStudio BIG Extractor/Builder";
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView bigListView;
        private System.Windows.Forms.ColumnHeader keyHeader;
        private System.Windows.Forms.ColumnHeader offsetHeader;
        private System.Windows.Forms.ColumnHeader nameHeader;
        private System.Windows.Forms.RichTextBox logRichTextBox;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem extractToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extractAllToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openBFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem buildBFToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ColumnHeader compressedHeader;
        private System.Windows.Forms.ToolStripMenuItem texturesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extractTexturesFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem buildTexturesFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox searchToolStripTextBox;
        private System.Windows.Forms.ToolStripMenuItem textToolStripMenuItem;
    }
}