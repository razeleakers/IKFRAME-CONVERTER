namespace IKFRAME_CONVERTER.Drawing
{
    public struct BodyPartData
    {
        public int PosX { get; }
        public int PosY { get; }
        public int SizeX { get; }
        public int SizeY { get; }

        public BodyPartData(int PosX, int PosY, int SizeX, int SizeY)
        {
            this.PosX = PosX;
            this.PosY = PosY;
            this.SizeX = SizeX;
            this.SizeY = SizeY;
        }
    }

    public static class DATA_HEAD
    {
        public static BodyPartData TOP => new BodyPartData(8, 0, 8, 8);
        public static BodyPartData BOTTOM => new BodyPartData(16, 0, 8, 8);
        public static BodyPartData RIGHT => new BodyPartData(0, 8, 8, 8);
        public static BodyPartData FRONT => new BodyPartData(8, 8, 8, 8);
        public static BodyPartData LEFT => new BodyPartData(16, 8, 8, 8);
        public static BodyPartData BACK => new BodyPartData(24, 8, 8, 8);
    }

    public static class DATA_TORSO
    {
        public static BodyPartData TOP => new BodyPartData(4, 0, 8, 4);
        public static BodyPartData BOTTOM => new BodyPartData(12, 0, 8, 4);
        public static BodyPartData RIGHT => new BodyPartData(0, 4, 4, 12);
        public static BodyPartData FRONT => new BodyPartData(4, 4, 8, 12);
        public static BodyPartData LEFT => new BodyPartData(12, 4, 4, 12);
        public static BodyPartData BACK => new BodyPartData(16, 4, 8, 12);
    }

    public static class DATA_ARMS_AND_LEGS
    {
        public static BodyPartData TOP => new BodyPartData(4, 0, 4, 4);
        public static BodyPartData BOTTOM => new BodyPartData(8, 0, 4, 4);
        public static BodyPartData RIGHT => new BodyPartData(0, 4, 4, 12);
        public static BodyPartData FRONT => new BodyPartData(4, 4, 4, 12);
        public static BodyPartData LEFT => new BodyPartData(8, 4, 4, 12);
        public static BodyPartData BACK => new BodyPartData(12, 4, 4, 12);
    }
}
