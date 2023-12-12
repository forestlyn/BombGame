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
        get => MapManager.CalMapPos(transform.position);
    }
    public GameObject bomb;

    private List<Bomb> bombs;

    private void Awake()
    {
        instance = this;
        bombs = new List<Bomb>();
    }
    private void Start()
    {
        HoldBombNum = HoldBombMaxNum;
    }
    void Update()
    {

    }

    #region Õ¨µ¯
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

    #region ÒÆ¶¯

    public void Move(Vector2 dir)
    {
        if (MapManager.Instance.CanMove(new Vector2(transform.position.x, transform.position.y) + dir,Height))
        {
            transform.Translate(dir);
        }
    }
    #endregion

    #region Undo
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
