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

namespace RemoveComments
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string folder = textBox1.Text;

            RemoveCommentsFromFolder(folder);
        }

        /// <summary>
        /// Substitui os nomes do arquivos da pasta
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="oldString"></param>
        /// <param name="newString"></param>
        /// <param name="mainFolder"></param>
        private void RemoveCommentsFromFolder(string folder)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(folder);

            var files = directoryInfo.GetFiles();

            RemoveCommentsFromFiles(files);

            var folders = directoryInfo.GetDirectories();

            foreach (var subFolder in folders)
            {
                RemoveCommentsFromFolder(subFolder.FullName);
            }
        }

        private static void RemoveCommentsFromFiles(FileInfo[] files)
        {
            string[] extensions = { ".js", ".css", ".cshtml", ".cs" };

            foreach (var extension in extensions)
            {
                foreach (FileInfo file in files.Where(x => x.FullName.EndsWith(extension)))
                {
                    RemoveComments(file);
                }
            }
        }

        private static void RemoveComments(FileInfo file)
        {
            var lines = new List<string>(File.ReadAllLines(file.FullName));

            if (lines.Count > 0)
            {
                while ((lines.Count > 0) && (lines.First()?.Trim() == String.Empty))
                {
                    lines.RemoveAt(0);
                }

                lines.RemoveAll(
                    x => (x.Contains("//")) ||
                         (x.Contains("@*")) ||
                         (x.Contains("< !--")) ||
                         (x.StartsWith("/*")) ||
                         (x.StartsWith(" *")));

                file.IsReadOnly = false;
                File.WriteAllLines(file.FullName, lines);
            }
        }
    }
}
