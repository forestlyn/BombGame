using Unity.VisualScripting;
using UnityEngine;

public class BoxFactory : MonoBehaviour
{
    private static BoxFactory instance;
    public static BoxFactory Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("÷ÿ∏¥BoxFactory");
        }
    }
    public GameObject GenerateBox(BaseMapObjectState baseMapObject)
    {
        Box box = MyGameObjectPool.Instance.GetByMapObjectType(MapObjectType.Box).GetComponent<Box>();
        box.boxMaterial = (BoxMaterialType)baseMapObject.boxMaterialType;
        KEDeliverType keType = (KEDeliverType)baseMapObject.boxkEType;
        box.kESimu = keType switch
        {
            KEDeliverType.ClockWise =>
                new KECalculateSimu(keType, new KEMathematics(rotateAngle: baseMapObject.boxRotateAngle)),
            KEDeliverType.Calculate =>
                new KECalculateSimu(keType, new KEMathematics(add: baseMapObject.boxAdd, multi: baseMapObject.boxMulti)),
            KEDeliverType.WildCard => new WildCardKESimu(keType),
            _ => new BaseKESimu(keType, GetDir(baseMapObject.boxDir))
        };
        box.Init();
        return box.gameObject;
    }

    private Vector2 GetDir(int dir)
    {
        if (dir == 0) { return Vector2.left; }
        else if (dir == 1) { return Vector2.up; }
        else if (dir == 2) { return Vector2.right; }
        else if (dir == 3) { return Vector2.down; }
        return Vector2.left;
    }
}