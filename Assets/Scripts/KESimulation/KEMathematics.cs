using UnityEngine;

public class KEMathematics
{
    //ÔËËã
    public int add;
    public int multi;

    //Ðý×ª
    public int rotateAngle;

    public KEMathematics(int add = 0, int multi = 0,int rotateAngle = 0)
    {
        this.add = add;
        this.multi = multi;
        this.rotateAngle = rotateAngle;
    }

    public void Calculate(ref int energe, ref Vector2 dir)
    {
        dir = CalRotate(dir, rotateAngle);
        energe = CalOperation(energe, add, multi);
        if (energe < 0)
        {
            dir = -dir;
            energe = -energe;
        }
        //Debug.Log("cal energe:" + energe + " dir:" + dir);
    }

    private int CalOperation(int energe, int add, int multi)
    {
        //Debug.Log("CalOperation:" + energe + " " + add + " " + multi);
        if (add == 0 && multi == 0)
        {
            return energe;
        }
        else if (add != 0 && multi == 0)
        {
            return energe + add;
        }
        else if (add == 0 && multi != 0)
        {
            return energe * multi;
        }
        return energe * multi + add;
    }

    private Vector2 CalRotate(Vector2 dir, int angleRotate)
    {
        if (angleRotate == 0)
        {
            return dir;
        }
        else if (angleRotate == 90)
        {
            if (dir == Vector2.left)
                return Vector2.up;
            else if (dir == Vector2.up)
                return Vector2.right;
            else if (dir == Vector2.right)
                return Vector2.down;
            else
                return Vector2.left;
        }
        else if (angleRotate == 180)
        {
            return new Vector2(-dir.x, -dir.y);
        }
        else if (angleRotate == 270)
        {
            if (dir == Vector2.left)
                return Vector2.down;
            else if (dir == Vector2.up)
                return Vector2.left;
            else if (dir == Vector2.right)
                return Vector2.up;
            else
                return Vector2.right;
        }
        Debug.LogError("angleRotate err:" + angleRotate);
        return dir;
    }
}