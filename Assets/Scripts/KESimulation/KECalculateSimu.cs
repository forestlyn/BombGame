using UnityEngine;

public class KECalculateSimu : BaseKESimu
{
    private KEMathematics kEMathematics;
    private KECalculateSimu(KEDeliverType kEType, Vector2 dir = default) : base(kEType, dir)
    {
    }

    public KECalculateSimu(KEDeliverType kEType, KEMathematics kEMathematics):
        base(kEType) 
    { 
        this.kEMathematics = kEMathematics;
    }

    public override void CalRealKE(int energe, Vector2 dir, KEDeliverType deliverKEType)
    {
        Vector2 mydir = dir;
        int myEnerge = energe;
        kEMathematics.Calculate(ref myEnerge, ref mydir);
        this._Energe = myEnerge;
        this._Dir = mydir;
    }

    public override object Clone()
    {
        return new KECalculateSimu(_KEType, kEMathematics);
    }
}