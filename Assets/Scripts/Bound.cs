namespace PCG_SearchBased_Dungeon
{
    public struct Bound
    {
        public int x;
        public int y;
        public int w;
        public int h;

        public Bound(int x, int y, int w, int h)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
        }

        public static bool Collide(Bound rect1, Bound rect2)
        {
            return rect1.x < rect2.x + rect2.w &&
                   rect1.x + rect1.w > rect2.x &&
                   rect1.y < rect2.y + rect2.h &&
                   rect1.h + rect1.y > rect2.y;
        }
        
        public static bool Inside(Bound rect1, Bound rect2)
        {
            return rect1.x + rect1.w <= rect2.w &&
                   rect1.y + rect1.h <= rect2.h &&
                   rect1.x >= rect2.x &&
                   rect1.y >= rect2.y;
        }
    }
}