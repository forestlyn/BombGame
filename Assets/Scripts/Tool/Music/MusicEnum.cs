using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTool.Music
{
    public enum MusicEnum
    {
        PlayerMoveSuccess,
        PlayerMoveFail,
        BoxMove,
        MoveToTarget,
        FallToWater,
        PutBomb,
        BombExplode,
        //暂无
        Collision,
        ButtonClick,
        ChangeLevel,
        StartLevel,
        Win,
    }
}
