using System;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Threading.Tasks;
using IKFRAME_CONVERTER.Drawing;

namespace IKFRAME_CONVERTER
{
    public partial class IKForm : Form
    {
        private readonly string pathSaveFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "SKINS_DATA");
        private string pathSkin = string.Empty;

        public IKForm()
        {
            InitializeComponent();
        }

        private void IKForm_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(pathSaveFolder)) Directory.CreateDirectory(pathSaveFolder);
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
                        MessageBox.Show(ex.Message,"Image Upload Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
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
            CB_RightLeg.Enabled = false;

            Task<string> task = Task.Run(() =>
            {
                Bitmap bp = new Bitmap(pathSkin);

                if (bp.Width == 64 && bp.Height == 64)
                {
                    using (DrawManager dw = new DrawManager(bp, pathSaveFolder))
                    {
                        if (CB_Head.Checked) dw.CutAndSaveImage(BodyPart.HEAD, new Rectangle(0, 0, 32, 16));

                        if (CB_RightLeg.Checked) dw.CutAndSaveImage(BodyPart.RIGHT_LEG, new Rectangle(0, 16, 16, 16));

                        if (CB_Torso.Checked) dw.CutAndSaveImage(BodyPart.TORSO, new Rectangle(16, 16, 24, 16));

                        if (CB_RightArm.Checked) dw.CutAndSaveImage(BodyPart.RIGHT_ARM, new Rectangle(40, 16, 16, 16));

                        if (CB_LeftLeg.Checked) dw.CutAndSaveImage(BodyPart.LEFT_LEG, new Rectangle(16, 48, 16, 16));

                        if (CB_LeftArm.Checked) dw.CutAndSaveImage(BodyPart.LEFT_ARM, new Rectangle(32, 48, 16, 16));

                        return dw.GetDrawData();
                    }
                }
                else
                {
                    bp.Dispose();

                    return string.Empty;
                }
            });

            task.ContinueWith((t) =>
            {
                if (task.Result != string.Empty) RTB_Content.Text = task.Result;
                else MessageBox.Show("The image must be 64 x 64 pixels", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                CB_Head.Enabled = true;
                CB_Torso.Enabled = true;
                CB_LeftArm.Enabled = true;
                CB_RightArm.Enabled = true;
                CB_LeftLeg.Enabled = true;
                CB_RightLeg.Enabled = true;

                BTN_Upload.Enabled = true;
                BTN_Convert.Enabled = true;
                BTN_Copy.Enabled = true;
                BTN_Download.Enabled = true;
                BTN_Clear.Enabled = true;
            },TaskScheduler.FromCurrentSynchronizationContext());
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
