using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class UnityInputAction : IInputAction
{
    private PCInputActions pcInputActions;
    public UnityInputAction() : base()
    {
        pcInputActions = new PCInputActions();
        Bind();
    }
    public override void Enable()
    {
        base.Enable();
        pcInputActions.Enable();
    }

    public override void Disable()
    {
        base.Disable();
        pcInputActions.Disable();
    }

    private MyInputCallbackContext Transfer(InputAction.CallbackContext ctx)
    {
        MyInputCallbackContext cbContext = new MyInputCallbackContext();
        if (ctx.interaction is HoldInteraction)
        {
            cbContext.interaction = MyInputInteraction.Hold;
        }
        else if (ctx.interaction is TapInteraction)
        {
            cbContext.interaction = MyInputInteraction.Tap;
        }
        else if(ctx.interaction is null)
        {
            cbContext.interaction = MyInputInteraction.None;
        }
        else
        {
            UnityEngine.Debug.LogError("unhanled type " + ctx.interaction);
            cbContext.interaction = MyInputInteraction.None;
        }
        return cbContext;
    }
    protected override void BindBomb()
    {
        //Debug.Log("BindBomb");
        pcInputActions.Player.Bomb.started += ctx => Bomb.InvokeStarted(Transfer(ctx));
        pcInputActions.Player.Bomb.performed += ctx => Bomb.InvokePerformed(Transfer(ctx));
        pcInputActions.Player.Bomb.canceled += ctx => Bomb.InvokeCanceled(Transfer(ctx));
        //pcInputActions.Player.Bomb.started += ctx => BindTest();
    }

    private void BindTest()
    {
        Debug.Log("Invoke!");
    }

    protected override void BindMoveDown()
    {
        pcInputActions.Player.MoveDown.started += ctx => MoveDown.InvokeStarted(Transfer(ctx));
        pcInputActions.Player.MoveDown.performed += ctx => MoveDown.InvokePerformed(Transfer(ctx));
        pcInputActions.Player.MoveDown.canceled += ctx => MoveDown.InvokeCanceled(Transfer(ctx));
    }

    protected override void BindMoveLeft()
    {
        pcInputActions.Player.MoveLeft.started += ctx => MoveLeft.InvokeStarted(Transfer(ctx));
        pcInputActions.Player.MoveLeft.performed += ctx => MoveLeft.InvokePerformed(Transfer(ctx));
        pcInputActions.Player.MoveLeft.canceled += ctx => MoveLeft.InvokeCanceled(Transfer(ctx));
    }

    protected override void BindMoveRight()
    {
        pcInputActions.Player.MoveRight.started += ctx => MoveRight.InvokeStarted(Transfer(ctx));
        pcInputActions.Player.MoveRight.performed += ctx => MoveRight.InvokePerformed(Transfer(ctx));
        pcInputActions.Player.MoveRight.canceled += ctx => MoveRight.InvokeCanceled(Transfer(ctx));
    }

    protected override void BindMoveUp()
    {
        pcInputActions.Player.MoveUp.started += ctx => MoveUp.InvokeStarted(Transfer(ctx));
        pcInputActions.Player.MoveUp.performed += ctx => MoveUp.InvokePerformed(Transfer(ctx));
        pcInputActions.Player.MoveUp.canceled += ctx => MoveUp.InvokeCanceled(Transfer(ctx));
    }

    protected override void BindRedo()
    {
        pcInputActions.Game.Redo.started += ctx => Redo.InvokeStarted(Transfer(ctx));
        pcInputActions.Game.Redo.performed += ctx => Redo.InvokePerformed(Transfer(ctx));
        pcInputActions.Game.Redo.canceled += ctx => Redo.InvokeCanceled(Transfer(ctx));
    }

    protected override void BindReStart()
    {
        pcInputActions.Game.ReStart.started += ctx => ReStart.InvokeStarted(Transfer(ctx));
        pcInputActions.Game.ReStart.performed += ctx => ReStart.InvokePerformed(Transfer(ctx));
        pcInputActions.Game.ReStart.canceled += ctx => ReStart.InvokeCanceled(Transfer(ctx));
    }

    protected override void BindShowGrid()
    {
        pcInputActions.Game.ShowGrid.started += ctx => ShowGrid.InvokeStarted(Transfer(ctx));
        pcInputActions.Game.ShowGrid.performed += ctx => ShowGrid.InvokePerformed(Transfer(ctx));
        pcInputActions.Game.ShowGrid.canceled += ctx => ShowGrid.InvokeCanceled(Transfer(ctx));
    }

    protected override void BindUndo()
    {
        pcInputActions.Game.Undo.started += ctx => Undo.InvokeStarted(Transfer(ctx));
        pcInputActions.Game.Undo.performed += ctx => Undo.InvokePerformed(Transfer(ctx));
        pcInputActions.Game.Undo.canceled += ctx => Undo.InvokeCanceled(Transfer(ctx));
    }
}

