using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 受到管理的重做物体，在执行操作时向RedoManager添加操作
/// </summary>
/// <typeparam name="T"></typeparam>
//public class ManagedRedoObject<T> : BaseRedoObject<T>
//{
//    private RedoObjectType type;
//    public ManagedRedoObject(RedoObjectType redoObjectType):base()
//    {
//        type = redoObjectType;
//    }
//    public override void AppendRedoAction(ObjectAction<T> action,bool flag)
//    {
//        base.AppendRedoAction(action, flag);
//        if(flag)
//        {
//            RedoManager.Instance.AddActionObjectType(type);
//        }
//    }
//}
