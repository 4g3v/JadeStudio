using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Cyotek.Windows.Forms;
using JadeStudio.Core;
using JadeStudio.Core.FileFormats.Text;

namespace JadeStudio.BIG
{
    public partial class TextForm : Form
    {
        private TextFile _textFile;
        private bool _parseText = true;
        private string _lastText;

        public TextForm()
        {
            InitializeComponent();
        }

        private void loadTextFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textTreeView.Nodes.Clear();
            _textFile = null;

            var ofd = new OpenFileDialog();
            ofd.Filter = "Text file (FD*.bin)|FD*.bin";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                _textFile = new TextFile();
                _textFile.Read(ofd.FileName);

                AddTextFileNodes();

                saveTextFileToolStripMenuItem.Enabled = true;
            }
        }

        private void AddTextFileNodes(Dictionary<TextGroup, List<int>> firstGroupDict = null, Dictionary<TextGroup, List<int>> groupDict = null)
        {
            textTreeView.Nodes.Clear();

            if (firstGroupDict == null && groupDict == null)
            {
                var firstTextGroupsNode = new TreeNode("First text groups");
                AddTextGroupsToNode(firstTextGroupsNode, _textFile.FirstTextGroups);
                textTreeView.Nodes.Add(firstTextGroupsNode);

                var textGroupsNode = new TreeNode("Text groups");
                AddTextGroupsToNode(textGroupsNode, _textFile.TextGroups);
                textTreeView.Nodes.Add(textGroupsNode);
            }
            else
            {
                var firstTextGroupsNode = new TreeNode("First text groups");
                AddTextGroupsToNode(firstTextGroupsNode, firstGroupDict);
                textTreeView.Nodes.Add(firstTextGroupsNode);

                var textGroupsNode = new TreeNode("Text groups");
                AddTextGroupsToNode(textGroupsNode, groupDict);
                textTreeView.Nodes.Add(textGroupsNode);
            }
        }

        private void AddTextGroupsToNode(TreeNode node, List<TextGroup> groups)
        {
            for (var i = 0; i < groups.Count; i++)
            {
                var group = groups[i];

                var groupNode = new TreeNode("Group " + i);
                groupNode.Tag = group;

                for (var j = 0; j < group.Texts.Count; j++)
                {
                    var entryNode = new TreeNode("Entry " + j + (string.IsNullOrEmpty(group.Texts[j]) ? " (Empty)" : string.Empty));
                    entryNode.Tag = j;

                    groupNode.Nodes.Add(entryNode);
                }

                node.Nodes.Add(groupNode);
            }
        }

        private void AddTextGroupsToNode(TreeNode node, Dictionary<TextGroup, List<int>> groupDict)
        {
            foreach (var group in groupDict.Keys)
            {
                var groupIndex = _textFile.FirstTextGroups.IndexOf(group);
                if (groupIndex == -1)
                {
                    groupIndex = _textFile.TextGroups.IndexOf(group);
                }
                
                var groupNode = new TreeNode("Group " + groupIndex);
                groupNode.Tag = group;

                foreach (var j in groupDict[group])
                {
                    var entryNode = new TreeNode("Entry " + j + (string.IsNullOrEmpty(group.Texts[j]) ? " (Empty)" : string.Empty));
                    entryNode.Tag = j;

                    groupNode.Nodes.Add(entryNode);
                }

                node.Nodes.Add(groupNode);
            }
        }

        private void textTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var selectedNode = textTreeView.SelectedNode;
            var tag = selectedNode.Tag;

            textRichTextBox.Clear();
            textRichTextBox.ReadOnly = true;

            if (tag == null)
                return;
            if (tag.GetType() != typeof(int))
                return;

            var textGroup = (TextGroup) selectedNode.Parent.Tag;
            _lastText = textGroup.Texts[(int) tag];
            ParseText(_lastText);
            textRichTextBox.ReadOnly = false;
            
            applyToolStripMenuItem.Enabled = false;
        }

        private void ParseText(string text)
        {
            textRichTextBox.Clear();
            
            if (_parseText == false || !text.Contains("\\c"))
            {
                textRichTextBox.Text = text;
                return;
            }

            var lastColor = Color.Black;
            for (var i = 0; i < text.Length; i++)
            {
                var c = text[i];
                if (c == '\\' && text[i + 1] == 'c')
                {
                    var colorBytes = text.Substring(i + 2, 8).ToByteArray();
                    lastColor = Color.FromArgb(colorBytes[0], colorBytes[3], colorBytes[2], colorBytes[1]);
                    
                    i += 10;
                    continue;
                }

                if (c == '\r')
                {
                    i += 1;
                    textRichTextBox.AppendText(Environment.NewLine);
                    continue;
                }

                textRichTextBox.AppendChar(c, lastColor);
            }
        }

        private void applyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedNode = textTreeView.SelectedNode;
            var tag = selectedNode?.Tag;

            if (tag == null)
                return;
            if (tag.GetType() != typeof(int))
                return;

            var textGroup = (TextGroup) selectedNode.Parent.Tag;
            textGroup.Texts[(int) tag] = _lastText;

            applyToolStripMenuItem.Enabled = false;
        }

        private void saveTextFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                var textFile = new TextFile {FirstTextGroups = _textFile.FirstTextGroups, TextGroups = _textFile.TextGroups};
                textFile.Write(sfd.FileName);
            }
        }

        private void textRichTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            applyToolStripMenuItem.Enabled = true;
        }

        private List<int> GetSearchIndexes(string searchText, TextGroup textGroup)
        {
            List<int> indexList = new List<int>();

            for (var i = 0; i < textGroup.Texts.Count; i++)
            {
                var text = textGroup.Texts[i];
                if (text.ToLower().Contains(searchText))
                {
                    indexList.Add(i);
                }
            }

            return indexList;
        }

        private void searchToolStripTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            var firstFoundTextDict = new Dictionary<TextGroup, List<int>>();
            var foundTextDict = new Dictionary<TextGroup, List<int>>();
            var searchText = searchToolStripTextBox.Text.ToLower();
            
            if (string.IsNullOrEmpty(searchText))
            {
                AddTextFileNodes();
                return;
            }

            foreach (var firstTextGroup in _textFile.FirstTextGroups)
            {
                var searchIndexes = GetSearchIndexes(searchText, firstTextGroup);
                if (searchIndexes.Count > 0)
                    firstFoundTextDict[firstTextGroup] = searchIndexes;
            }

            foreach (var textGroup in _textFile.TextGroups)
            {
                var searchIndexes = GetSearchIndexes(searchText, textGroup);
                if (searchIndexes.Count > 0)
                    foundTextDict[textGroup] = searchIndexes;
            }

            AddTextFileNodes(firstFoundTextDict, foundTextDict);
        }

        private void searchToolStripTextBox_Click(object sender, EventArgs e)
        {
            searchToolStripTextBox.SelectAll();
        }

        private void toggleViewModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _parseText = !_parseText;
            changeTextColorToolStripMenuItem.Enabled = !_parseText;
            ParseText(_lastText);
        }

        private void changeBackgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var colorPickerDialog = new ColorPickerDialog();
            colorPickerDialog.Color = textRichTextBox.BackColor;
            
            if (colorPickerDialog.ShowDialog() == DialogResult.OK)
            {
                textRichTextBox.BackColor = colorPickerDialog.Color;
            }
        }

        private void changeTextColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var colorPickerDialog = new ColorPickerDialog();

            if (colorPickerDialog.ShowDialog() == DialogResult.OK)
            {
                var color = colorPickerDialog.Color;
                
                var colorString = "\\c";
                colorString += color.A.ToString("X2");
                colorString += color.B.ToString("X2");
                colorString += color.G.ToString("X2");
                colorString += color.R.ToString("X2");
                colorString += "\\";

                var caret = textRichTextBox.SelectionStart;
                _lastText = _lastText.Insert(caret, colorString);
                ParseText(_lastText);

                applyToolStripMenuItem.Enabled = true;
            }
        }

        private void textRichTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!_parseText)
            {
                _lastText = textRichTextBox.Text;
            }
        }
    }
}