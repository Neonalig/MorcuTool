using System.ComponentModel;
using System.Windows.Forms;

namespace MorcuTool
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.savePackageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.convertModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.utilityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.simsTPLToTPLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tPLToMSATPLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadVaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compressionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decompressQFSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compressToQFSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mSGTextEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.morcubusModeBox = new System.Windows.Forms.CheckBox();
            this.FileTree = new System.Windows.Forms.TreeView();
            this.subfileContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportSubfile = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backtrackToModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.packageRootContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportAllContextMenuStripButton = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.findByHashButton = new System.Windows.Forms.Button();
            this.findByHashTextBox = new System.Windows.Forms.TextBox();
            this.vaultSearchButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.vaultSearchTextBox = new System.Windows.Forms.TextBox();
            this.hashLabel = new System.Windows.Forms.Label();
            this.bulkConvertPackagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.subfileContextMenu.SuspendLayout();
            this.packageRootContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.utilityToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(641, 24);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.savePackageToolStripMenuItem,
            this.convertModelToolStripMenuItem,
            this.bulkConvertPackagesToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.openToolStripMenuItem.Text = "Open Package";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // savePackageToolStripMenuItem
            // 
            this.savePackageToolStripMenuItem.Name = "savePackageToolStripMenuItem";
            this.savePackageToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.savePackageToolStripMenuItem.Text = "Save Package";
            this.savePackageToolStripMenuItem.Click += new System.EventHandler(this.savePackageToolStripMenuItem_Click);
            // 
            // convertModelToolStripMenuItem
            // 
            this.convertModelToolStripMenuItem.Name = "convertModelToolStripMenuItem";
            this.convertModelToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.convertModelToolStripMenuItem.Text = "Convert Model";
            this.convertModelToolStripMenuItem.Click += new System.EventHandler(this.convertModelToolStripMenuItem_Click);
            // 
            // utilityToolStripMenuItem
            // 
            this.utilityToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.simsTPLToTPLToolStripMenuItem,
            this.tPLToMSATPLToolStripMenuItem,
            this.loadVaultToolStripMenuItem,
            this.compressionToolStripMenuItem,
            this.mSGTextEditorToolStripMenuItem});
            this.utilityToolStripMenuItem.Name = "utilityToolStripMenuItem";
            this.utilityToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.utilityToolStripMenuItem.Text = "Utility";
            // 
            // simsTPLToTPLToolStripMenuItem
            // 
            this.simsTPLToTPLToolStripMenuItem.Name = "simsTPLToTPLToolStripMenuItem";
            this.simsTPLToTPLToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.simsTPLToTPLToolStripMenuItem.Text = "Raw Sims TPL to TPL";
            this.simsTPLToTPLToolStripMenuItem.Click += new System.EventHandler(this.simsTPLToTPLToolStripMenuItem_Click);
            // 
            // tPLToMSATPLToolStripMenuItem
            // 
            this.tPLToMSATPLToolStripMenuItem.Name = "tPLToMSATPLToolStripMenuItem";
            this.tPLToMSATPLToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.tPLToMSATPLToolStripMenuItem.Text = "TPL to MSA TPL";
            this.tPLToMSATPLToolStripMenuItem.Click += new System.EventHandler(this.tPLToMSATPLToolStripMenuItem_Click);
            // 
            // loadVaultToolStripMenuItem
            // 
            this.loadVaultToolStripMenuItem.Name = "loadVaultToolStripMenuItem";
            this.loadVaultToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.loadVaultToolStripMenuItem.Text = "Load Vault";
            this.loadVaultToolStripMenuItem.Click += new System.EventHandler(this.loadVaultToolStripMenuItem_Click);
            // 
            // compressionToolStripMenuItem
            // 
            this.compressionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.decompressQFSToolStripMenuItem,
            this.compressToQFSToolStripMenuItem});
            this.compressionToolStripMenuItem.Name = "compressionToolStripMenuItem";
            this.compressionToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.compressionToolStripMenuItem.Text = "Compression";
            // 
            // decompressQFSToolStripMenuItem
            // 
            this.decompressQFSToolStripMenuItem.Name = "decompressQFSToolStripMenuItem";
            this.decompressQFSToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.decompressQFSToolStripMenuItem.Text = "Decompress QFS";
            this.decompressQFSToolStripMenuItem.Click += new System.EventHandler(this.decompressQFSToolStripMenuItem_Click_1);
            // 
            // compressToQFSToolStripMenuItem
            // 
            this.compressToQFSToolStripMenuItem.Name = "compressToQFSToolStripMenuItem";
            this.compressToQFSToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.compressToQFSToolStripMenuItem.Text = "Compress to QFS";
            this.compressToQFSToolStripMenuItem.Click += new System.EventHandler(this.compressToQFSToolStripMenuItem_Click);
            // 
            // mSGTextEditorToolStripMenuItem
            // 
            this.mSGTextEditorToolStripMenuItem.Name = "mSGTextEditorToolStripMenuItem";
            this.mSGTextEditorToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.mSGTextEditorToolStripMenuItem.Text = "MSG text editor";
            this.mSGTextEditorToolStripMenuItem.Click += new System.EventHandler(this.mSGTextEditorToolStripMenuItem_Click);
            // 
            // morcubusModeBox
            // 
            this.morcubusModeBox.AutoSize = true;
            this.morcubusModeBox.Location = new System.Drawing.Point(9, 358);
            this.morcubusModeBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.morcubusModeBox.Name = "morcubusModeBox";
            this.morcubusModeBox.Size = new System.Drawing.Size(320, 17);
            this.morcubusModeBox.TabIndex = 9;
            this.morcubusModeBox.Text = "Morcubus Mode (Needed for some Skyheroes models to work)";
            this.morcubusModeBox.UseVisualStyleBackColor = true;
            // 
            // FileTree
            // 
            this.FileTree.Location = new System.Drawing.Point(10, 33);
            this.FileTree.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.FileTree.Name = "FileTree";
            this.FileTree.Size = new System.Drawing.Size(452, 305);
            this.FileTree.TabIndex = 10;
            // 
            // subfileContextMenu
            // 
            this.subfileContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.subfileContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportSubfile,
            this.replaceToolStripMenuItem,
            this.backtrackToModelToolStripMenuItem});
            this.subfileContextMenu.Name = "subfileContextMenu";
            this.subfileContextMenu.Size = new System.Drawing.Size(177, 70);
            // 
            // exportSubfile
            // 
            this.exportSubfile.Name = "exportSubfile";
            this.exportSubfile.Size = new System.Drawing.Size(176, 22);
            this.exportSubfile.Text = "Export";
            this.exportSubfile.Click += new System.EventHandler(this.exportSubfile_Click);
            // 
            // replaceToolStripMenuItem
            // 
            this.replaceToolStripMenuItem.Name = "replaceToolStripMenuItem";
            this.replaceToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.replaceToolStripMenuItem.Text = "Replace";
            this.replaceToolStripMenuItem.Click += new System.EventHandler(this.replaceToolStripMenuItem_Click);
            // 
            // backtrackToModelToolStripMenuItem
            // 
            this.backtrackToModelToolStripMenuItem.Name = "backtrackToModelToolStripMenuItem";
            this.backtrackToModelToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.backtrackToModelToolStripMenuItem.Text = "Backtrack to model";
            this.backtrackToModelToolStripMenuItem.Click += new System.EventHandler(this.backtrackToModelToolStripMenuItem_Click);
            // 
            // packageRootContextMenu
            // 
            this.packageRootContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.packageRootContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportAllContextMenuStripButton});
            this.packageRootContextMenu.Name = "subfileContextMenu";
            this.packageRootContextMenu.Size = new System.Drawing.Size(126, 26);
            // 
            // exportAllContextMenuStripButton
            // 
            this.exportAllContextMenuStripButton.Name = "exportAllContextMenuStripButton";
            this.exportAllContextMenuStripButton.Size = new System.Drawing.Size(125, 22);
            this.exportAllContextMenuStripButton.Text = "Export All";
            this.exportAllContextMenuStripButton.Click += new System.EventHandler(this.exportAllContextMenuStripButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(488, 33);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Find file by hexadecimal hash";
            // 
            // findByHashButton
            // 
            this.findByHashButton.Location = new System.Drawing.Point(526, 72);
            this.findByHashButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.findByHashButton.Name = "findByHashButton";
            this.findByHashButton.Size = new System.Drawing.Size(56, 19);
            this.findByHashButton.TabIndex = 15;
            this.findByHashButton.Text = "Find";
            this.findByHashButton.UseVisualStyleBackColor = true;
            this.findByHashButton.Click += new System.EventHandler(this.findByHashButton_Click);
            // 
            // findByHashTextBox
            // 
            this.findByHashTextBox.Location = new System.Drawing.Point(490, 50);
            this.findByHashTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.findByHashTextBox.Name = "findByHashTextBox";
            this.findByHashTextBox.Size = new System.Drawing.Size(144, 20);
            this.findByHashTextBox.TabIndex = 13;
            // 
            // vaultSearchButton
            // 
            this.vaultSearchButton.Location = new System.Drawing.Point(526, 144);
            this.vaultSearchButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.vaultSearchButton.Name = "vaultSearchButton";
            this.vaultSearchButton.Size = new System.Drawing.Size(56, 19);
            this.vaultSearchButton.TabIndex = 18;
            this.vaultSearchButton.Text = "Find";
            this.vaultSearchButton.UseVisualStyleBackColor = true;
            this.vaultSearchButton.Click += new System.EventHandler(this.vaultSearchButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(483, 105);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(158, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Find vault entry by decimal hash";
            // 
            // vaultSearchTextBox
            // 
            this.vaultSearchTextBox.Location = new System.Drawing.Point(490, 121);
            this.vaultSearchTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.vaultSearchTextBox.Name = "vaultSearchTextBox";
            this.vaultSearchTextBox.Size = new System.Drawing.Size(144, 20);
            this.vaultSearchTextBox.TabIndex = 16;
            // 
            // hashLabel
            // 
            this.hashLabel.AutoSize = true;
            this.hashLabel.Location = new System.Drawing.Point(488, 202);
            this.hashLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.hashLabel.Name = "hashLabel";
            this.hashLabel.Size = new System.Drawing.Size(38, 13);
            this.hashLabel.TabIndex = 19;
            this.hashLabel.Text = "Hash: ";
            this.hashLabel.Click += new System.EventHandler(this.hashLabel_Click);
            // 
            // bulkConvertPackagesToolStripMenuItem
            // 
            this.bulkConvertPackagesToolStripMenuItem.Name = "bulkConvertPackagesToolStripMenuItem";
            this.bulkConvertPackagesToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.bulkConvertPackagesToolStripMenuItem.Text = "Bulk Convert Packages";
            this.bulkConvertPackagesToolStripMenuItem.Click += new System.EventHandler(this.bulkConvertPackagesToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 385);
            this.Controls.Add(this.hashLabel);
            this.Controls.Add(this.vaultSearchButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.vaultSearchTextBox);
            this.Controls.Add(this.findByHashButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.findByHashTextBox);
            this.Controls.Add(this.FileTree);
            this.Controls.Add(this.morcubusModeBox);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "MorcuTool";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.subfileContextMenu.ResumeLayout(false);
            this.packageRootContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem utilityToolStripMenuItem;
        private ToolStripMenuItem simsTPLToTPLToolStripMenuItem;
        private ToolStripMenuItem tPLToMSATPLToolStripMenuItem;
        private ToolStripMenuItem loadVaultToolStripMenuItem;
        private ToolStripMenuItem convertModelToolStripMenuItem;
        private ToolStripMenuItem compressionToolStripMenuItem;
        private ToolStripMenuItem decompressQFSToolStripMenuItem;
        private CheckBox morcubusModeBox;
        private TreeView FileTree;
        private ContextMenuStrip subfileContextMenu;
        private ToolStripMenuItem exportSubfile;
        private ToolStripMenuItem savePackageToolStripMenuItem;
        private ContextMenuStrip packageRootContextMenu;
        private ToolStripMenuItem exportAllContextMenuStripButton;
        private Label label1;
        private Button findByHashButton;
        private TextBox findByHashTextBox;
        private Button vaultSearchButton;
        private Label label2;
        private TextBox vaultSearchTextBox;
        private Label hashLabel;
        private ToolStripMenuItem compressToQFSToolStripMenuItem;
        private ToolStripMenuItem replaceToolStripMenuItem;
        private ToolStripMenuItem backtrackToModelToolStripMenuItem;
        private ToolStripMenuItem mSGTextEditorToolStripMenuItem;
        private ToolStripMenuItem bulkConvertPackagesToolStripMenuItem;
    }
}

