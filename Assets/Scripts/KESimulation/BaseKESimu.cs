using System;
using UnityEngine;
public delegate void EnergeBecomeZero();
/// <summary>
/// None StaticDir Destory Motivate
/// </summary>
public class BaseKESimu : ICloneable
{
    public KEDeliverType _KEType;

    protected int _Energe;
    protected Vector2 _Dir;

    public int Energe { get => _Energe; }
    public Vector2 Dir { get => _Dir; }

    public KEDeliverType KEType { get => _KEType; }

    public event EnergeBecomeZero OnEnergeBecomeZero;
    /// <summary>
    /// �ж����ٶ��ܣ��������¼���boxת��
    /// </summary>
    /// <param name="delta"></param>
    public void EnergeDesc(int delta)
    {
        this._Energe -= delta;
        //Debug.Log("_Energe:" + _Energe + "delta:" + delta);
        if (OnEnergeBecomeZero != null && _Energe == 0)
            OnEnergeBecomeZero();
    }

    public BaseKESimu(KEDeliverType kEType, Vector2 dir = default(Vector2))
    {
        _KEType = kEType;
        _Dir = dir;
    }



    /// <summary>
    /// ���ö��ܣ������ת����������������ײʹ��
    /// </summary>
    /// <param name="energe"></param>
    /// <param name="dir"></param>
    /// <param name="kEDeliverType"></param>
    public virtual void SetEnergeDir(int energe, Vector2 dir)
    {
        if (energe == 0)
        {
            Debug.LogError("err");
            return;
        }
        //Debug.Log("SetEnergeDir" + energe + dir);
        CalRealKE(energe, dir, _KEType);
    }

    public virtual void ClearEnerge()
    {
        //Debug.Log("clear _Energe:" + _Energe);
        _Energe = 0;
        if (OnEnergeBecomeZero != null)
            OnEnergeBecomeZero();
    }

    public virtual void CalRealKE(int energe, Vector2 dir, KEDeliverType deliverKEType)
    {
        switch (_KEType)
        {
            case KEDeliverType.None:
            case KEDeliverType.Motivate:
                _Dir = dir;
                _Energe = energe;
                break;
            case KEDeliverType.StaticDir:
                _Energe = energe;
                break;
            case KEDeliverType.Destory:
                break;
        }
    }

    public virtual object Clone()
    {
        return new BaseKESimu(_KEType, _Dir);
    }
}