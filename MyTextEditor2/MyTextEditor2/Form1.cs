using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyTextEditor2
{
    public partial class Form1 : Form
    {

        private bool IsChanged;

        private string CurrentFilename;

        public Form1()
        {
            InitializeComponent();
            IsChanged = false;
            this.CurrentFilename = "";
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("WUT");
            this.Close(); //close "this" window
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.IsChanged)
            {
                DialogResult dr;

                dr = MessageBox.Show("Do you want to save the current changes?",
                    "MyEditor", MessageBoxButtons.YesNo);

                if (dr == DialogResult.Yes)
                {
                    this.saveToolStripMenuItem_Click(sender, e);
                }
            }
           // MessageBox.Show("Closing");

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DialogResult dr;
            //
            // has text changed?
            //
            if (!this.IsChanged)
                return;

            if (this.CurrentFilename == "")
            {
                this.saveFileDialog1.FileName = "mytextfile.txt";
                this.saveFileDialog1.Filter = "*.txt|*.txt";
                this.saveFileDialog1.InitialDirectory =
                  System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                dr = this.saveFileDialog1.ShowDialog();
                if (dr == DialogResult.OK)  // save file:
                {
                    System.IO.File.WriteAllText(this.saveFileDialog1.FileName, this.txtEditorPane.Text);

                    this.CurrentFilename = this.saveFileDialog1.FileName;
                    this.IsChanged = false;  // reset flag:
                }
            }
            else
            {
                System.IO.File.WriteAllText(this.CurrentFilename, this.txtEditorPane.Text);
                this.IsChanged = false;  // reset flag:
            }
      

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (this.IsChanged)
            {

                DialogResult dr2;
                dr2 = MessageBox.Show("Do you want to save the current changes?",
                    "MyEditor", MessageBoxButtons.YesNo);

                if (dr2 == DialogResult.Yes)
                {
                    this.saveToolStripMenuItem_Click(sender, e);
                }
            }

            this.openFileDialog1.CheckFileExists = true;
            this.openFileDialog1.FileName = "";
            this.openFileDialog1.Filter = "*.txt|*.txt";
            this.openFileDialog1.InitialDirectory =
                    System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            DialogResult dr = this.openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                this.txtEditorPane.Text = System.IO.File.ReadAllText(this.openFileDialog1.FileName);
                this.CurrentFilename = this.openFileDialog1.FileName;
                IsChanged = false;
            }
        }

        private void txtEditorPane_TextChanged(object sender, EventArgs e)
        {
            IsChanged = true;
            this.toolStripStatusLabel2.Text = "Chars: " + this.txtEditorPane.Text.Length;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            if (IsChanged && this.CurrentFilename != "")
            {
                System.IO.File.WriteAllText(this.CurrentFilename, this.txtEditorPane.Text);
                this.IsChanged = false;
            }
        }

        private void enableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.txtEditorPane.TextChanged += new System.EventHandler(Backup_TextChanged);
        }

        private void Backup_TextChanged(object sender, EventArgs e)
        {
            Console.WriteLine("backup......");
            System.IO.File.WriteAllText("_backup.txt", this.txtEditorPane.Text);
        }

    }
}
