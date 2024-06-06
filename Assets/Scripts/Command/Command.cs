using System.Collections.Generic;
using UnityEditor;

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
    }
}