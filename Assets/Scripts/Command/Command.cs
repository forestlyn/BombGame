using System.Collections.Generic;
using UnityEngine;


namespace MyInputSystem
{
    public abstract class Command
    {
        protected int objectId = -1;
        public int ObjectId => objectId;
        public abstract void Execute();
        public abstract void Undo();

        protected Command last;
        protected List<Command> next;
        public Command Last { get => last; }
        public List<Command> Next
        {
            set
            {
                next = value;
            }
            get
            {
                if (next == null)
                    next = new List<Command>();
                return next;
            }
        }

        public Command(MapObject mapObject)
        {
            if (mapObject != null)
                objectId = mapObject.objectId;
            else
            {
                //Debug.LogError("isnull");
            }
        }
    }
}