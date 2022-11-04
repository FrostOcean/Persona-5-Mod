﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DarkUI;
using ShrineFox.IO;

namespace ModMenuBuilder
{
    public partial class BuilderForm : DarkUI.Forms.DarkForm
    {
        public BuilderForm()
        {
            InitializeComponent();
            Output.LogControl = rtb_Log;
            if (File.Exists("compilerPath.txt"))
                txt_Path.Text = File.ReadAllText("compilerPath.txt");
            if (File.Exists("outputPath.txt"))
                txt_OutPath.Text = File.ReadAllText("outputPath.txt");
#if DEBUG
            Program.Show();
            System.Threading.Thread.Sleep(200);
            Output.LogControl = null;
            chk_Reindex.Checked = false;
            chk_Decompile.Checked = true;
#endif
            rtb_Log.Text += $"{this.Text} by ShrineFox\nProcesses and compiles scripts for Persona 5 on PS3, PS4, PC and Switch.";
        }

        private void BuildButton_Click(object sender, EventArgs e)
        {
            rtb_Log.Clear();

            List<string> args = new List<string>();
           
            args.Add("-c"); args.Add(txt_Path.Text);
            if (chk_Decompile.Checked)
            {
                args.Add("-d"); args.Add("true");
            }
            if (chk_Reindex.Checked)
            {
                args.Add("-r"); args.Add("true");
            }
            if (!radio_Old.Checked)
            {
                args.Add("-n"); args.Add("true");
            }
            if (chk_RepackPACs.Checked)
            {
                args.Add("-p"); args.Add("true");
            }
            if (radio_Vanilla.Checked)
            {
                args.Add("-g"); args.Add("P5");
                args.Add("-e"); args.Add("P5");
            }
            if (radio_Nintendo.Checked)
            {
                args.Add("-j"); args.Add("NX");
            }
            if (radio_Playstation.Checked)
            {
                args.Add("-j"); args.Add("PS");
            }
            if (radio_Xbox.Checked)
            {
                args.Add("-j"); args.Add("MS");
            }
            args.Add("-o"); args.Add(txt_OutPath.Text);

            btn_Build.Enabled = false;
            Task.Run(() => {
                Program.StartWithOptions(args.ToArray());
            });
            btn_Build.Enabled = true;
        }

        private void Path_Changed(object sender, EventArgs e)
        {
            if (File.Exists(txt_Path.Text) && Directory.Exists(txt_OutPath.Text))
                btn_Build.Enabled = true;
            else
                btn_Build.Enabled = false;
        }

        private void Path_Click(object sender, EventArgs e)
        {
            var paths = WinFormsEvents.FilePath_Click("Choose AtlusScriptCompiler.exe", false, new string[] { "Executable File (*.exe)" });
            if (paths.Count > 0)
            {
                if (File.Exists(paths.First()))
                {
                    txt_Path.Text = paths.First();
                    File.WriteAllText("compilerPath.txt", paths.First());
                }
            }
        }

        private void OutPath_Click(object sender, EventArgs e)
        {
            var path = WinFormsEvents.FolderPath_Click("Choose AtlusScriptCompiler.exe");
            txt_OutPath.Text = path;
            File.WriteAllText("outputPath.txt", path);
        }

        private void Version_Changed(object sender, EventArgs e)
        {
            if (radio_Vanilla.Checked)
            {
                radio_Old.Checked = true;
                groupBox_Platform.Enabled = false;
            }
            else
            {
                groupBox_Platform.Enabled = true;
            }
        }

        private void Platform_Changed(object sender, EventArgs e)
        {
            if (radio_New.Checked)
            {
                chk_RepackPACs.Checked = false;
                chk_RepackPACs.Enabled = false;
            }
            else
            {
                chk_RepackPACs.Enabled = true;
            }
        }
    }
}
