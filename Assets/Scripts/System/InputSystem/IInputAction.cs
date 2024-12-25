using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;


public enum MyInputInteraction
{
    None,
    Hold,
    Tap,
}
public struct MyInputCallbackContext
{
    public MyInputInteraction interaction;
    public MyInputCallbackContext(InputAction.CallbackContext cbContext)
    {
        this.interaction = cbContext.interaction is HoldInteraction ?
            MyInputInteraction.Hold : MyInputInteraction.Tap;
    }
    public MyInputCallbackContext(MyInputInteraction inputInteraction)
    {
        this.interaction = inputInteraction;
    }
}
public delegate void InputDelegate(MyInputCallbackContext cbContext);

public class MyInputAction
{
    public event InputDelegate started;
    public event InputDelegate performed;
    public event InputDelegate canceled;

    private bool enable = false;
    public void Enable()
    {
        enable = true;
    }

    public void Disable()
    {
        enable = false;
    }
    public void InvokeStarted(MyInputCallbackContext cbContext)
    {
        if (!enable)
        {
            return;
        }
        started?.Invoke(cbContext);
    }

    public void InvokePerformed(MyInputCallbackContext cbContext)
    {
        if (!enable)
        {
            return;
        }
        performed?.Invoke(cbContext);
    }

    public void InvokeCanceled(MyInputCallbackContext cbContext)
    {
        if (!enable)
        {
            return;
        }
        canceled?.Invoke(cbContext);
    }
}


public abstract class IInputAction
{
    protected bool isEnable = false;
    public virtual void Enable() 
    {
        isEnable = true;
        MoveUp.Enable();
        MoveDown.Enable();
        MoveLeft.Enable();
        MoveRight.Enable();
        Bomb.Enable();
        Redo.Enable();
        Undo.Enable();
        ReStart.Enable();
        ShowGrid.Enable();
    }
    public virtual void Disable() 
    { 
        isEnable = false;
        MoveUp.Disable();
        MoveDown.Disable();
        MoveLeft.Disable();
        MoveRight.Disable();
        Bomb.Disable();
        Redo.Disable();
        Undo.Disable();
        ReStart.Disable();
        ShowGrid.Disable();
    }

    #region player input
    public MyInputAction MoveUp;
    public MyInputAction MoveDown;
    public MyInputAction MoveLeft;
    public MyInputAction MoveRight;
    public MyInputAction Bomb;

    public IInputAction()
    {
        MoveUp = new MyInputAction();
        MoveDown = new MyInputAction();
        MoveLeft = new MyInputAction();
        MoveRight = new MyInputAction();
        Bomb = new MyInputAction();
        Redo = new MyInputAction();
        Undo = new MyInputAction();
        ReStart = new MyInputAction();
        ShowGrid = new MyInputAction();
    }

    protected void Bind()
    {
        BindMoveUp();
        BindMoveDown();
        BindMoveLeft();
        BindMoveRight();
        BindBomb();
        BindRedo();
        BindUndo();
        BindReStart();
        BindShowGrid();
    }

    protected abstract void BindMoveUp();

    protected abstract void BindMoveDown();

    protected abstract void BindMoveLeft();

    protected abstract void BindMoveRight();

    protected abstract void BindBomb();
    #endregion

    #region Game input
    public MyInputAction Redo;
    public MyInputAction Undo;
    public MyInputAction ReStart;
    public MyInputAction ShowGrid;

    protected abstract void BindRedo();

    protected abstract void BindUndo();

    protected abstract void BindReStart();

    protected abstract void BindShowGrid();
    #endregion
}
