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
using JadeStudio.Core;
using JadeStudio.Core.FileFormats.Bigfile;
using JadeStudio.Core.FileFormats.Texture;

namespace JadeStudio.BIG
{
    public partial class MainForm : Form
    {
        private Bigfile _big;
        private List<ListViewItem> _listViewItems;

        public MainForm()
        {
            InitializeComponent();
        }

        public void Log(string s)
        {
            logRichTextBox.AppendText(s + Environment.NewLine);
            logRichTextBox.ScrollToCaret();
        }

        public void Log()
        {
            logRichTextBox.AppendText(Environment.NewLine);
            logRichTextBox.ScrollToCaret();
        }

        private void openBFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bigListView.Items.Clear();
            logRichTextBox.Clear();

            var ofd = new OpenFileDialog();
            ofd.Filter = "Bigfile (*.bf)|*.bf|All files (*.*)|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                _big = new Bigfile(ofd.FileName);
                _big.Read();

                if (_big.Header.Magic != "BIG")
                {
                    Log("Not a Bigfile (Maybe it has a \"BUG\" as the magic?)");
                    return;
                }

                Log("BigfileHeader:");
                Log("Magic: " + _big.Header.Magic);
                Log("ID: " + _big.Header.ID.Hex());
                Log("FilesCount: " + _big.Header.FilesCount);
                Log("UnknownCount: " + _big.Header.UnknownCount.Hex());
                Log("Unknown: " + _big.Header.Unknown.ToHex());
                Log("UnknownCount2: " + _big.Header.UnknownCount2.Hex());
                Log("FatsCount: " + _big.Header.FatsCount);
                Log("StartKey: " + _big.Header.StartKey.Hex());
                Log();

                Log("FATHeaders:");
                foreach (var fatHeader in _big.FATHeaders)
                {
                    Log("FilesCount: " + fatHeader.FilesCount);
                    Log("Unknown: " + fatHeader.Unknown.Hex());
                    Log("Offset: " + fatHeader.Offset.Hex());
                    Log("Unknown2: " + fatHeader.Unknown2.Hex());
                    Log("Unknown3: " + fatHeader.Unknown3.Hex());
                    Log("Unknown4: " + fatHeader.Unknown4.Hex());
                    Log();
                }

                _listViewItems = new List<ListViewItem>();
                foreach (var file in _big.Files)
                {
                    var lvi = new ListViewItem(file.Key.Hex());
                    lvi.SubItems.Add(file.Offset.Hex());
                    lvi.SubItems.Add(file.Name);
                    lvi.SubItems.Add(file.IsCompressed() ? "Yes" : "No");

                    _listViewItems.Add(lvi);
                }

                bigListView.Items.AddRange(_listViewItems.ToArray());
                bigListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }

        private void extractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (ListViewItem selectedItem in bigListView.SelectedItems)
                {
                    var fatFile = _big.Files.Where(f => f.Key == selectedItem.Text.IntFromHex()).ToArray()[0];

                    var name = fatFile.Key.Hex() + "_" + fatFile.Name;
                    Log("Writing " + name);
                    File.WriteAllBytes(folderBrowserDialog.SelectedPath + "\\" + name, fatFile.Read(_big.Reader));
                }
            }
        }

        private void extractAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Extracting bigfile. This may take a while and the program may seem unresponsive.", "JadeStudio", MessageBoxButtons.OK, MessageBoxIcon.Information);

                foreach (ListViewItem item in bigListView.Items)
                {
                    foreach (var fatFile in _big.Files.Where(f => f.Key.Hex() == item.Text).ToList())
                    {
                        var name = fatFile.Key.Hex() + "_" + fatFile.Name;
                        File.WriteAllBytes(folderBrowserDialog.SelectedPath + "\\" + name, fatFile.Read(_big.Reader));
                    }
                }

                Log("Successfully extracted " + bigListView.Items.Count + " files!");
            }
        }

        private void buildBFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                var sfd = new SaveFileDialog();
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Building bigfile. This may take a while and the program may seem unresponsive.", "JadeStudio", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    var files = new List<FATFile>();
                    var fileNames = Directory.GetFiles(folderBrowserDialog.SelectedPath);
                    foreach (var fileName in fileNames)
                    {
                        files.Add(new FATFile {Key = new FileInfo(fileName).Name.Split('_')[0].IntFromHex(), Path = fileName});
                    }

                    var newBig = new Bigfile(sfd.FileName)
                    {
                        Header = new BigfileHeader
                        {
                            Magic = "BIG",
                            ID = 0x22,
                            FilesCount = files.Count,
                            UnknownCount = 0x2B0F,
                            Unknown = new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF},
                            UnknownCount2 = 0x2B9E,
                            FatsCount = 1,
                            StartKey = 0x71003FF9
                        },
                        FATHeaders = new FATHeader[1]
                    };
                    newBig.FATHeaders[0] = new FATHeader
                    {
                        FilesCount = files.Count,
                        Unknown = 0x2B0F,
                        Offset = 0x44,
                        Unknown2 = 0xFFFFFFFF,
                        Unknown3 = 0x00,
                        Unknown4 = 0x2B9D
                    };

                    newBig.Files = files.ToArray();
                    newBig.Write();

                    newBig.Writer.Close();

                    Log("Finished building bigfile!");
                }
            }
        }

        private void searchToolStripTextBox_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    searchToolStripTextBox.SelectAll();
                    break;
                case MouseButtons.Middle:
                    searchToolStripTextBox.Clear();
                    break;
            }
        }

        private void searchToolStripTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;
            
            if (_listViewItems == null)
                return;

            var search = searchToolStripTextBox.Text;
            if (!string.IsNullOrEmpty(search))
            {
                var foundItems = new List<ListViewItem>();
                foreach (ListViewItem item in _listViewItems)
                {
                    if (item.Text.Contains(search))
                        foundItems.Add(item);
                    else
                    {
                        foreach (ListViewItem.ListViewSubItem itemSubItem in item.SubItems)
                        {
                            if (itemSubItem.Text.Contains(search))
                                foundItems.Add(item);
                        }
                    }
                }

                bigListView.Items.Clear();
                foreach (var listViewItem in foundItems)
                {
                    if (!bigListView.Items.Contains(listViewItem))
                    {
                        bigListView.Items.Add(listViewItem);
                    }
                }

                bigListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            else
            {
                bigListView.Items.Clear();
                bigListView.Items.AddRange(_listViewItems.ToArray());
            }
        }

        private void extractTexturesFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Choose the folder which will get filled with the extracted textures:";
            
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                var openFileDialog = new OpenFileDialog();
                openFileDialog.Multiselect = true;
                openFileDialog.Filter = "Texture file (FF8*.bin)|FF8*.bin";
                openFileDialog.Title = "Choose the texture files you want to extract.";
                
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (var fileName in openFileDialog.FileNames)
                    {
                        var folderPath = folderBrowserDialog.SelectedPath + "\\" + Path.GetFileNameWithoutExtension(fileName) + "\\";
                        
                        var texturesFile = new TexturesFile();
                        texturesFile.Read(fileName);
                        texturesFile.DumpTextures(folderPath);
                        
                        Log("Dumped textures from " + fileName + " to " + folderPath);
                    }
                    
                    Log("Finished dumping textures!");
                }
            }
        }

        private void buildTexturesFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var folderPaths = new List<string>();
            
            var lastResult = DialogResult.None;
            while (lastResult != DialogResult.Cancel)
            {
                var folderBrowserDialog = new FolderBrowserDialog();
                folderBrowserDialog.Description = "Choose the folder containing the extracted textures:";

                lastResult = folderBrowserDialog.ShowDialog();
                if (lastResult == DialogResult.OK)
                {
                    folderPaths.Add(folderBrowserDialog.SelectedPath);
                }   
            }
            
            var destinationFolderBrowserDialog = new FolderBrowserDialog();
            destinationFolderBrowserDialog.Description = "Choose the folder which will get filled with the rebuilt texture files. Existing ones get overwritten.";
            
            if (destinationFolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (var folderPath in folderPaths)
                {
                    var fileName = new DirectoryInfo(folderPath).Name + ".bin";
                    var destination = destinationFolderBrowserDialog.SelectedPath + "\\" + fileName;
                    
                    var texturesFile = new TexturesFile();
                    texturesFile.Write(folderPath + "\\", destination);
                    
                    Log("Wrote new texture file to " + destination);
                }
                
                Log("Finished building new texture files!");
            }
        }

        private void textToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new TextForm().Show();
        }
    }
}