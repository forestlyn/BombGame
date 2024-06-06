using Unity.VisualScripting;
using UnityEngine;

public class WildCardKESimu : BaseKESimu
{
    private BaseKESimu tempKE;

    public WildCardKESimu(KEDeliverType kEType, Vector2 dir = default) : base(kEType, dir)
    {
    }

    public void SetHitKE(BaseKESimu hitKE)
    {
        object calKESimu = hitKE.KEType switch
        {
            KEDeliverType.WildCard => (hitKE as WildCardKESimu)?.tempKE?.Clone(),
            _ => hitKE.Clone(),
        };
        if (calKESimu == null)
        {
            return;
        }
        tempKE = calKESimu as BaseKESimu;
    }

    public override void CalRealKE(int energe, Vector2 dir, KEDeliverType deliverKEType)
    {
        if (tempKE != null)
        {
            tempKE.CalRealKE(energe, dir, tempKE.KEType);
            _Energe = tempKE.Energe;
            _Dir = tempKE.Dir;
        }
    }
    public override void ClearEnerge()
    {
        base.ClearEnerge();
        tempKE = null;
    }
}