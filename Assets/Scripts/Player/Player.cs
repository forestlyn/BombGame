using MyInputSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player instance;
    public static Player Instance { get { return instance; } }

    public int Height;
    public int HoldBombNum;
    public int HoldBombMaxNum;
    public float moveTimeInterval;

    public Vector2 worldPos
    {
        get => transform.position;
    } 
    public Vector2 mapPos
    {
        get=> MapManager.CalMapPos( transform.position);
    }
    public GameObject bomb;

    private Coroutine[] continuousMove = new Coroutine[4];

    private List<Bomb> bombs;

    //private Rigidbody2D rb;
    //private PlayerState GetNowState()
    //{
    //    PlayerState state = new PlayerState
    //    {
    //        pos = transform.position,
    //        height = Height,
    //        holdBombNum = HoldBombNum,
    //        bombPos = new Vector2[bombs.Count]
    //    };
    //    for (int i = 0; i < bombs.Count; i++)
    //    {
    //        state.bombPos[i] = bombs[i].gameObject.transform.position;
    //    }
    //    return state;
    //}
    private ObjectAction<PlayerState> action;

    private int Index(Vector2 dir)
    {
        int idx = -1;
        if (dir == Vector2.up) idx = 0;
        else if (dir == Vector2.left) idx = 1;
        else if (dir == Vector2.down) idx = 2;
        else if (dir == Vector2.right) idx = 3;
        Debug.Assert(idx >= 0 && idx < 4);
        return idx;
    }

    private void Awake()
    {
        instance = this;
        bombs = new List<Bomb>();
        //rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        HoldBombNum = HoldBombMaxNum;
    }
    void Update()
    {

    }

    //public void Move(Vector2 dir, bool continuous)
    //{
    //    //Debug.Log("dir" + dir);
    //    StopAllMove();
    //    if (continuous)
    //    {
    //        //Debug.Log("持续移动" + dir);
    //        StartContinuousMove(dir);
    //    }
    //    else
    //    {
    //        Move(dir);
    //    }
    //}

    //public void StopMove(Vector2 dir)
    //{
    //    EndContinuousMove(Index(dir));
    //}

    //public void Bomb()
    //{
    //    if (HoldBombNum == 0)
    //    {
    //        action.type = (int)PlayerActionType.Bomb;
    //        action.startState = GetNowState();

    //        InvokeBomb();
    //        HoldBombNum = HoldBombMaxNum;

    //        action.endState = GetNowState();
    //        redo.AppendRedoAction(action, true);

    //    }
    //    else if (HoldBombNum > 0)
    //    {
    //        action.type = (int)PlayerActionType.PutBomb;
    //        action.startState = GetNowState();

    //        PutBomb(transform.position);
    //        HoldBombNum--;
            
    //        action.endState = GetNowState();
    //        redo.AppendRedoAction(action, true);
    //    }
    //}

    #region 炸弹
    public void PutBomb(Vector2 pos)
    {
        var b = MyGameObjectPool.Instance.Get<Bomb>();
        b.transform.position = pos;
        HoldBombNum--;
        bombs.Add(b.GetComponent<Bomb>());
    }
    public List<Vector2> InvokeBomb()
    {
        var bombPos = new List<Vector2>();
        foreach (var b in bombs)
        {
            bombPos.Add(b.transform.position);
            b.Explosion();
            MyGameObjectPool.Instance.Return<Bomb>(b.gameObject);
        }
        bombs.Clear();
        HoldBombNum = HoldBombMaxNum;
        return bombPos;
    }
    #endregion

    #region 移动

    public void Move(Vector2 dir)
    {
        //action.type = (int)PlayerActionType.Move;
        //action.startState = GetNowState();
        //Debug.Log("移动" + dir);
        if (MapManager.Instance.CanMove(new Vector2(transform.position.x, transform.position.y) + dir,Height))
        {
            transform.Translate(dir);
        }
        //action.endState = GetNowState();
        //redo.AppendRedoAction(action, true);
    }
    //private void StopAllMove()
    //{
    //    for (int i = 0; i < 4; i++)
    //    {
    //        if (continuousMove[i] != null)
    //        {
    //            EndContinuousMove(i);
    //        }
    //    }
    //}
    //private void StartContinuousMove(Vector2 dir)
    //{
    //    continuousMove[Index(dir)] = StartCoroutine(ContinuousMove(dir));
    //}
    //private void EndContinuousMove(int idx)
    //{
    //    if (continuousMove[idx] != null)
    //    {
    //        //Debug.Log("停止持续移动");
    //        StopCoroutine(continuousMove[idx]);
    //        continuousMove[idx] = null;
    //    }
    //}
    //private IEnumerator ContinuousMove(Vector2 dir)
    //{
    //    while (true)
    //    {
    //        Move(dir);
    //        yield return new WaitForSeconds(moveTimeInterval);
    //    }
    //}
    #endregion

    #region Undo
    //private void TransportToNewState(PlayerState start, PlayerState end, int playerActionType)
    //{
    //    transform.position = end.pos;
    //    Height = end.height;
    //    HoldBombNum = end.holdBombNum;

    //    //引爆或撤销炸弹
    //    if (end.holdBombNum > start.holdBombNum)
    //    {
    //        //Debug.Log("1");
    //        if (playerActionType == (int)PlayerActionType.Bomb)
    //        {
    //            InvokeBomb();
    //        }
    //        else if(playerActionType == (int)PlayerActionType.PutBomb)
    //        {
    //            UndoPutBomb();
    //        }
    //    }
    //    //放置炸弹或者撤销引爆
    //    else if(end.holdBombNum < start.holdBombNum)
    //    {
    //        //Debug.Log("2");

    //        if (playerActionType == (int)PlayerActionType.Bomb) 
    //        {
    //            if (end.holdBombNum == 0)
    //            {
    //                //Debug.Log("撤销引爆");
    //                for (int i = 0; i < end.bombPos.Length; i++)
    //                {
    //                    PutBomb(end.bombPos[i]);
    //                }
    //                foreach(var b in bombs)
    //                {
    //                    b.Explosion();
    //                }
    //            }
    //            else
    //            {
    //                Debug.LogError("err");
    //            }
    //        }
    //        else if(playerActionType == (int)PlayerActionType.PutBomb)
    //        {
    //            PutBomb(transform.position);
    //        }
    //    }

    //}
    public void UndoPutBomb()
    {
        if (bombs.Count > 0)
        {
            var b = bombs[bombs.Count - 1];
            bombs.RemoveAt(bombs.Count - 1);
            MyGameObjectPool.Instance.Return<Bomb>(b.gameObject);
        }
    }

    #endregion
}
