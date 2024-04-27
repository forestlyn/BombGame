using UnityEngine;

public class KECalculateSimu : BaseKESimu
{
    private KEMathematics kEMathematics;
    public KECalculateSimu(KEDeliverType kEType, Vector2 dir = default) : base(kEType, dir)
    {
    }

    public KECalculateSimu(KEDeliverType kEType, KEMathematics kEMathematics):
        base(kEType) 
    { 
        this.kEMathematics = kEMathematics;
    }

    protected override void CalRealKE(int energe, Vector2 dir, KEDeliverType deliverKEType)
    {
        Vector2 mydir = dir;
        int myEnerge = energe;
        kEMathematics.Calculate(ref myEnerge, ref mydir);
        this._Energe = myEnerge;
        this._Dir = mydir;
    }
}