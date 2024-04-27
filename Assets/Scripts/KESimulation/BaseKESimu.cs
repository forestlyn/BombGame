using System;
using UnityEngine;
/// <summary>
/// None StaticDir Destory
/// </summary>
public class BaseKESimu
{
    public KEDeliverType _KEType;

    protected int _Energe;
    protected Vector2 _Dir;

    public int Energe { get => _Energe; }
    public Vector2 Dir { get => _Dir; }

    public KEDeliverType KEType { get => _KEType; }

    /// <summary>
    /// 行动减少动能，不会重新计算box转换
    /// </summary>
    /// <param name="delta"></param>
    public void EnergeDesc(int delta)
    {
        this._Energe -= delta;
    } 

    public BaseKESimu(KEDeliverType kEType, Vector2 dir = default(Vector2))
    {
        _KEType = kEType;
        _Dir = dir;
    }

    /// <summary>
    /// 设置动能，会计算转换后真正能量，碰撞使用
    /// </summary>
    /// <param name="energe"></param>
    /// <param name="dir"></param>
    public void SetEnergeDir(int energe, Vector2 dir)
    {
        if (energe == 0) return;
        CalRealKE(energe, dir, _KEType);
    }

    public void ClearEnergeDir()
    {
        _Energe = 0;
        _Dir = default;
    }

    protected virtual void CalRealKE(int energe, Vector2 dir, KEDeliverType deliverKEType)
    {
        switch (_KEType)
        {
            case KEDeliverType.None: 
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
}