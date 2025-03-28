using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DetekcijaIvica
{
    public sealed class KonvMatrica
    {
        public int TopL, TopM, TopR, MidL, MidM, MidR, BotL, BotM, BotR, Fac, Offset;
        public KonvMatrica(int TopL = 0, int TopM = 0, int TopR = 0, int MidL = 0, int MidM = 1, int MidR = 0, int BotL = 0, int BotM = 0, int BotR = 0, int Fac=1, int Offset=0)
        {
            this.TopL = TopL;   this.TopM = TopM;     this.TopR = TopR;
            this.MidL = MidL;   this.MidM = MidM;     this.MidR = MidR;
            this.BotL = BotL;   this.BotM = BotL;     this.BotR = BotR;

            this.Fac = Fac; this.Offset = Offset;
        }
        public int Fact
        {
            get { return Fac; }
            set { Fac = value; }
        }
        public int OffS
        {
            get { return Offset; }
            set { Offset = value; }
        }
    }
}
