using System;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Collections.Generic;

namespace IKFRAME_CONVERTER
{
    public partial class IKForm : Form
    {
        private readonly string pathSave = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "SkinData");
        private string pathSkin = string.Empty;

        public IKForm()
        {
            InitializeComponent();
        }

        private void IKForm_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(pathSave)) Directory.CreateDirectory(pathSave);
        }

        private void BTN_Upload_Click(object sender, System.EventArgs e)
        {
            BTN_Convert.Enabled = false;

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select a skin";
                ofd.CheckFileExists = true;
                ofd.Multiselect = false;
                ofd.Filter = "PNG Images|*.png";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Image.FromFile(ofd.FileName).Dispose();

                        pathSkin = ofd.FileName;
                        PB_Skin.ImageLocation = ofd.FileName;
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message,"Image Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                }
            }

            BTN_Convert.Enabled = true;
        }

        private void BTN_Convert_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(pathSkin) || !File.Exists(pathSkin)) return;

            BTN_Upload.Enabled = false;
            BTN_Convert.Enabled = false;
            BTN_Copy.Enabled = false;
            BTN_Download.Enabled = false;
            BTN_Clear.Enabled = false;

            CB_Head.Enabled = false;
            CB_Torso.Enabled = false;
            CB_LeftArm.Enabled = false;
            CB_RightArm.Enabled = false;
            CB_LeftLeg.Enabled = false;
            CB_RightLeg.Enabled= false;

            RTB_Content.Clear();

            Task.Run(() =>
            {
                using (Bitmap bpSkin = new Bitmap(pathSkin))
                {
                    if (bpSkin.Width == 64 && bpSkin.Height == 64)
                    {
                        if (CB_Head.Checked)
                        {
                            string head = GetBodySection("Head", bpSkin, 0, 0, 32, 16);

                            GetBodyPartColors("Top", head, 8, 0, 8, 8);
                            GetBodyPartColors("Bottom", head, 16, 0, 8, 8);
                            GetBodyPartColors("Right", head, 0, 8, 8, 8);
                            GetBodyPartColors("Front", head, 8, 8, 8, 8);
                            GetBodyPartColors("Left", head, 16, 8, 8, 8);
                            GetBodyPartColors("Back", head, 24, 8, 8, 8);
                        }

                        if (CB_RightLeg.Checked)
                        {
                            string legTop = GetBodySection("RightLeg", bpSkin, 0, 16, 16, 16);

                            GetBodyPartColors("Top", legTop, 4, 0, 4, 4);
                            GetBodyPartColors("Bottom", legTop, 8, 0, 4, 4);
                            GetBodyPartColors("Right", legTop, 0, 4, 4, 12);
                            GetBodyPartColors("Front", legTop, 4, 4, 4, 12);
                            GetBodyPartColors("Left", legTop, 8, 4, 4, 12);
                            GetBodyPartColors("Back", legTop, 12, 4, 4, 12);
                        }

                        if (CB_Torso.Checked)
                        {
                            string torso = GetBodySection("Torso", bpSkin, 16, 16, 24, 16);

                            GetBodyPartColors("Top", torso, 4, 0, 8, 4);
                            GetBodyPartColors("Bottom", torso, 12, 0, 8, 4);
                            GetBodyPartColors("Right", torso, 0, 4, 4, 12);
                            GetBodyPartColors("Front", torso, 4, 4, 8, 12);
                            GetBodyPartColors("Left", torso, 12, 4, 4, 12);
                            GetBodyPartColors("Back", torso, 16, 4, 8, 12);
                        }

                        if (CB_RightArm.Checked)
                        {
                            string armTop = GetBodySection("RightArm", bpSkin, 40, 16, 16, 16);

                            GetBodyPartColors("Top", armTop, 4, 0, 4, 4);
                            GetBodyPartColors("Bottom", armTop, 8, 0, 4, 4);
                            GetBodyPartColors("Right", armTop, 0, 4, 4, 12);
                            GetBodyPartColors("Front", armTop, 4, 4, 4, 12);
                            GetBodyPartColors("Left", armTop, 8, 4, 4, 12);
                            GetBodyPartColors("Back", armTop, 12, 4, 4, 12);
                        }

                        if (CB_LeftLeg.Checked)
                        {
                            string legBottom = GetBodySection("LeftLeg", bpSkin, 16, 48, 16, 16);

                            GetBodyPartColors("Top", legBottom, 4, 0, 4, 4);
                            GetBodyPartColors("Bottom", legBottom, 8, 0, 4, 4);
                            GetBodyPartColors("Right", legBottom, 0, 4, 4, 12);
                            GetBodyPartColors("Front", legBottom, 4, 4, 4, 12);
                            GetBodyPartColors("Left", legBottom, 8, 4, 4, 12);
                            GetBodyPartColors("Back", legBottom, 12, 4, 4, 12);
                        }

                        if (CB_LeftArm.Checked)
                        {
                            string armBottom = GetBodySection("LeftArm", bpSkin, 32, 48, 16, 16);

                            GetBodyPartColors("Top", armBottom, 4, 0, 4, 4);
                            GetBodyPartColors("Bottom", armBottom, 8, 0, 4, 4);
                            GetBodyPartColors("Right", armBottom, 0, 4, 4, 12);
                            GetBodyPartColors("Front", armBottom, 4, 4, 4, 12);
                            GetBodyPartColors("Left", armBottom, 8, 4, 4, 12);
                            GetBodyPartColors("Back", armBottom, 12, 4, 4, 12);
                        }
                    }
                    else MessageBox.Show("The image must be 64 x 64 pixels", "Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }

                BTN_Clear.Invoke(new MethodInvoker(() =>
                {
                    BTN_Clear.Enabled = true;
                    BTN_Download.Enabled = true;
                    BTN_Copy.Enabled = true;
                    BTN_Convert.Enabled = true;
                    BTN_Upload.Enabled = true;

                    CB_Head.Enabled = true;
                    CB_Torso.Enabled = true;
                    CB_LeftArm.Enabled = true;
                    CB_RightArm.Enabled = true;
                    CB_LeftLeg.Enabled = true;
                    CB_RightLeg.Enabled = true;
                }));
            });
        }

        private string GetBodySection(string sectionName,Bitmap bpSkin,int skinPosX,int skinPosY,int skinWidth,int skinHeight)
        {
            string savePath = Path.Combine(pathSave, $"{sectionName}.png");

            using (Bitmap bp = new Bitmap(skinWidth, skinHeight))
            {
                using (Graphics g = Graphics.FromImage(bp))
                {
                    g.DrawImage(bpSkin, new Rectangle(0, 0, bp.Width, bp.Height),new Rectangle(skinPosX, skinPosY, skinWidth, skinHeight), GraphicsUnit.Pixel);
                }

                bp.Save(savePath,ImageFormat.Png);
            }

            return savePath;
        }

        private void GetBodyPartColors(string partName,string bpSection,int secPosX,int secPosY,int secWidth,int secHeight)
        {
            string sectionName = Path.GetFileNameWithoutExtension(bpSection);
            
            using (Bitmap bp = new Bitmap(secWidth, secHeight))
            {
                using (Graphics g = Graphics.FromImage(bp))
                {
                    using (Image imgSection = Image.FromFile(bpSection))
                    {
                        g.DrawImage(imgSection, new Rectangle(0, 0, bp.Width, bp.Height), new Rectangle(secPosX, secPosY, secWidth, secHeight), GraphicsUnit.Pixel);
                    }
                }

                int acc = 1;

                if(partName == "Top" || partName == "Bottom")
                {
                    List<List<Color>> colors = new List<List<Color>>();

                    for (int x = 0; x < bp.Width; x++) 
                    {
                        List<Color> rowColors = new List<Color>();

                        for (int y = bp.Height - 1; y >= 0; y--)
                        {
                            rowColors.Add(bp.GetPixel(x, y));
                        }

                        colors.Add(rowColors);
                    }

                    RTB_Content.Invoke(new MethodInvoker(() =>
                    {
                        for (int i = 0; i < colors.Count; i++)
                        {
                            colors[i].Reverse();

                            for (int j = 0; j < colors[i].Count; j++)
                            {
                                RTB_Content.AppendText($"{sectionName}-{partName}={acc}&'{colors[i][j].R},{colors[i][j].G},{colors[i][j].B}'+" + "\n");

                                acc++;
                            }
                        }
                    }));
                }
                else
                {
                    RTB_Content.Invoke(new MethodInvoker(() =>
                    {
                        for (int y = 0; y < bp.Height; y++)
                        {
                            for (int x = 0; x < bp.Width; x++)
                            {
                                Color color = bp.GetPixel(x, y);

                                RTB_Content.AppendText($"{sectionName}-{partName}={acc}&'{color.R},{color.G},{color.B}'+" + "\n");

                                acc++;
                            }
                        }
                    }));
                }
            }
        }

        private async void BTN_Copy_Click(object sender, EventArgs e)
        {
            if (RTB_Content.TextLength < 1) return;

            BTN_Copy.Enabled = false;

            Clipboard.SetText(RTB_Content.Text);

            BTN_Copy.Text = "Copied!";

            await Task.Delay(200);

            BTN_Copy.Text = "Copy";

            BTN_Copy.Enabled = true;
        }

        private void BTN_Download_Click(object sender, EventArgs e)
        {
            if (RTB_Content.TextLength < 1) return;

            BTN_Convert.Enabled = false;

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Save Skin Data";
                sfd.Filter = "Text files (*.txt)|*.txt";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(sfd.FileName, RTB_Content.Text);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message,"Download Failed",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                }
            }

            BTN_Convert.Enabled = true;
        }

        private void BTN_Clear_Click(object sender, EventArgs e)
        {
            RTB_Content.Clear();
        }
    }
}
