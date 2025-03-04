using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace IKFRAME_CONVERTER.Drawing
{
    public class DrawManager : IDisposable
    {
        private Bitmap bpSkin;
        private StringBuilder sb;
        private string pathSaveFolder;

        public DrawManager(Bitmap bpSkin, string pathSaveFolder)
        {
            this.bpSkin = bpSkin;
            this.sb = new StringBuilder();
            this.pathSaveFolder = pathSaveFolder;
        }

        public void CutAndSaveImage(BodyPart section, Rectangle data)
        {
            using (Bitmap bp = new Bitmap(data.Width, data.Height))
            {
                using (Graphics g = Graphics.FromImage(bp))
                {
                    g.DrawImage(bpSkin, new Rectangle(0, 0, bp.Width, bp.Height), data, GraphicsUnit.Pixel);
                }

                string savePath = Path.Combine(pathSaveFolder, $"{section}.png");

                bp.Save(savePath, ImageFormat.Png);

                using (Image img = Image.FromFile(savePath))
                {
                    if (section == BodyPart.HEAD)
                    {
                        ExtractAndSaveColors("Head", "Front", new Rectangle(DATA_HEAD.FRONT.PosX, DATA_HEAD.FRONT.PosY, DATA_HEAD.FRONT.SizeX, DATA_HEAD.FRONT.SizeY), img);
                        ExtractAndSaveColors("Head", "Back", new Rectangle(DATA_HEAD.BACK.PosX, DATA_HEAD.BACK.PosY, DATA_HEAD.BACK.SizeX, DATA_HEAD.BACK.SizeY), img);
                        ExtractAndSaveColors("Head", "Left", new Rectangle(DATA_HEAD.LEFT.PosX, DATA_HEAD.LEFT.PosY, DATA_HEAD.LEFT.SizeX, DATA_HEAD.LEFT.SizeY), img);
                        ExtractAndSaveColors("Head", "Right", new Rectangle(DATA_HEAD.RIGHT.PosX, DATA_HEAD.RIGHT.PosY, DATA_HEAD.RIGHT.SizeX, DATA_HEAD.RIGHT.SizeY), img);
                        ExtractAndSaveColors("Head", "Top", new Rectangle(DATA_HEAD.TOP.PosX, DATA_HEAD.TOP.PosY, DATA_HEAD.TOP.SizeX, DATA_HEAD.TOP.SizeY), img);
                        ExtractAndSaveColors("Head", "Bottom", new Rectangle(DATA_HEAD.BOTTOM.PosX, DATA_HEAD.BOTTOM.PosY, DATA_HEAD.BOTTOM.SizeX, DATA_HEAD.BOTTOM.SizeY), img);
                    }
                    else if (section == BodyPart.TORSO)
                    {
                        ExtractAndSaveColors("Torso", "Front", new Rectangle(DATA_TORSO.FRONT.PosX, DATA_TORSO.FRONT.PosY, DATA_TORSO.FRONT.SizeX, DATA_TORSO.FRONT.SizeY), img);
                        ExtractAndSaveColors("Torso", "Back", new Rectangle(DATA_TORSO.BACK.PosX, DATA_TORSO.BACK.PosY, DATA_TORSO.BACK.SizeX, DATA_TORSO.BACK.SizeY), img);
                        ExtractAndSaveColors("Torso", "Left", new Rectangle(DATA_TORSO.LEFT.PosX, DATA_TORSO.LEFT.PosY, DATA_TORSO.LEFT.SizeX, DATA_TORSO.LEFT.SizeY), img);
                        ExtractAndSaveColors("Torso", "Right", new Rectangle(DATA_TORSO.RIGHT.PosX, DATA_TORSO.RIGHT.PosY, DATA_TORSO.RIGHT.SizeX, DATA_TORSO.RIGHT.SizeY), img);
                        ExtractAndSaveColors("Torso", "Top", new Rectangle(DATA_TORSO.TOP.PosX, DATA_TORSO.TOP.PosY, DATA_TORSO.TOP.SizeX, DATA_TORSO.TOP.SizeY), img);
                        ExtractAndSaveColors("Torso", "Bottom", new Rectangle(DATA_TORSO.BOTTOM.PosX, DATA_TORSO.BOTTOM.PosY, DATA_TORSO.BOTTOM.SizeX, DATA_TORSO.BOTTOM.SizeY), img);
                    }
                    else
                    {
                        string sectionName = section == BodyPart.LEFT_ARM ? "LeftArm" :
                                             section == BodyPart.RIGHT_ARM ? "RightArm" :
                                             section == BodyPart.LEFT_LEG ? "LeftLeg" : "RightLeg";

                        ExtractAndSaveColors(sectionName, "Front", new Rectangle(DATA_ARMS_AND_LEGS.FRONT.PosX, DATA_ARMS_AND_LEGS.FRONT.PosY, DATA_ARMS_AND_LEGS.FRONT.SizeX, DATA_ARMS_AND_LEGS.FRONT.SizeY), img);
                        ExtractAndSaveColors(sectionName, "Back", new Rectangle(DATA_ARMS_AND_LEGS.BACK.PosX, DATA_ARMS_AND_LEGS.BACK.PosY, DATA_ARMS_AND_LEGS.BACK.SizeX, DATA_ARMS_AND_LEGS.BACK.SizeY), img);
                        ExtractAndSaveColors(sectionName, "Left", new Rectangle(DATA_ARMS_AND_LEGS.LEFT.PosX, DATA_ARMS_AND_LEGS.LEFT.PosY, DATA_ARMS_AND_LEGS.LEFT.SizeX, DATA_ARMS_AND_LEGS.LEFT.SizeY), img);
                        ExtractAndSaveColors(sectionName, "Right", new Rectangle(DATA_ARMS_AND_LEGS.RIGHT.PosX, DATA_ARMS_AND_LEGS.RIGHT.PosY, DATA_ARMS_AND_LEGS.RIGHT.SizeX, DATA_ARMS_AND_LEGS.RIGHT.SizeY), img);
                        ExtractAndSaveColors(sectionName, "Top", new Rectangle(DATA_ARMS_AND_LEGS.TOP.PosX, DATA_ARMS_AND_LEGS.TOP.PosY, DATA_ARMS_AND_LEGS.TOP.SizeX, DATA_ARMS_AND_LEGS.TOP.SizeY), img);
                        ExtractAndSaveColors(sectionName, "Bottom", new Rectangle(DATA_ARMS_AND_LEGS.BOTTOM.PosX, DATA_ARMS_AND_LEGS.BOTTOM.PosY, DATA_ARMS_AND_LEGS.BOTTOM.SizeX, DATA_ARMS_AND_LEGS.BOTTOM.SizeY), img);
                    }
                }
            }
        }

        private void ExtractAndSaveColors(string section,string face,Rectangle data,Image img)
        {
            using (Bitmap bp = new Bitmap(data.Width,data.Height))
            {
                using (Graphics g = Graphics.FromImage(bp))
                {
                    g.DrawImage(img, new Rectangle(0, 0, bp.Width, bp.Height), data, GraphicsUnit.Pixel);
                }

                int acc = 1;

                if (face == "Bottom")
                {
                    for (int y = bp.Height - 1; y >= 0; y--)
                    {
                        for (int x = 0; x < bp.Width; x++)
                        {
                            Color color = bp.GetPixel(x,y);

                            sb.AppendLine($"{section}-{face}={acc}&'{color.R},{color.G},{color.B}'+");

                            acc++;
                        }
                    }
                }
                else
                {
                    for (int y = 0; y < bp.Height; y++)
                    {
                        for (int x = 0; x < bp.Width; x++)
                        {
                            Color color = bp.GetPixel(x, y);

                            sb.AppendLine($"{section}-{face}={acc}&'{color.R},{color.G},{color.B}'+");

                            acc++;
                        }
                    }
                }
            }
        }

        public void ClearDrawData()
        {
            sb.Clear();
        }

        public string GetDrawData()
        {
            return sb.ToString();
        }

        public void Dispose()
        {
            bpSkin?.Dispose();
            sb.Clear();

            bpSkin = null;
            sb = null;
            pathSaveFolder = null;
        }
    }
}
