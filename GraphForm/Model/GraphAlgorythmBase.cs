using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace GraphForm.Model
{
    using GraphType = List<Node>;
    [Serializable]
    public delegate void TableEventHandler(object source, TableEventArgs e);
    [Serializable]
    public delegate void NodeStateChangeEventHandler(object source, NodeStateChangeEventArgs e);
    [Serializable]
    public delegate void EdgeStateChangeEventHandler(object source, EdgeStateChangeEventArgs e);
    [Serializable]
    public delegate void MessageEventHandler(object source, MessageEventArgs e);
    [Serializable]
    public class TableEventArgs : EventArgs
    {
        public string Id { get; private set; }
        public string Caption { get; set; }
        public DataTable Table { get; private set; }
        public TableEventArgs(string id, DataTable table)
        {
            Id = id;
            Table = table;
        }
    }

    [Serializable]
    public class NodeStateChangeEventArgs : EventArgs
    {
        public NodeStateChangeEventArgs(int index, NodeState state)
        {
            Index = index;
            State = state;
        }

        public int Index { get; private set; }
        public NodeState State { get; private set; }
    }

    [Serializable]
    public class EdgeStateChangeEventArgs : EventArgs
    {
        public int Id { get; private set; }
        public EdgeState State { get; private set; }
        public EdgeStateChangeEventArgs(int id, EdgeState state)
        {
            Id = id;
            State = state;
        }
    }

    [Serializable]
    public class MessageEventArgs : EventArgs
    {
        public MessageEventArgs(string message, string caption)
        {
            Message = message;
            Caption = caption;
        }

        public string Message { get; private set; }
        public string Caption { get; private set; }
    }

    [Serializable]
    public abstract class GraphAlgorythmBase
    {
        protected GraphType _graph;
        public event TableEventHandler TableEvent;
        public event NodeStateChangeEventHandler NodeStateChangeEvent;
        public event EdgeStateChangeEventHandler EdgeStateChangeEvent;
        public event MessageEventHandler NotifyEvent;
        protected Dictionary<string, DataTable> tables;

        public bool IsInitalised { get; protected set; }
        public bool IsRunning { get; private set; }

        protected GraphAlgorythmBase(ref GraphType graph)
        {
            _graph = graph;
            tables = new Dictionary<string, DataTable>();
            IsInitalised = false;
            IsRunning = false;
        }

        public abstract void Init();

        public abstract void Next();
        public abstract bool End();

        public void Finish(int timeOut)
        {
            if (IsRunning) return;
            if (!IsInitalised)
            {
                RaiseNotifyEvent("Inicializálatlan algoritmus");
                return;
            }
            IsRunning = true;
            while (!End())
            {
                Next();
                //TODO : softcode the period
                Thread.Sleep(timeOut);
            }
            IsRunning = false;
        }

        protected void RaiseTableEvent(string tableName, string caption = null)
        {
            //keyNotFoundException if table doesn't exist
            var table = tables[tableName];
            var eventArg = new TableEventArgs(tableName, table);
            if (caption != null) eventArg.Caption = caption;
            if (TableEvent != null) TableEvent(this, eventArg);
        }

        protected void RaiseNodeStateChangeEvent(int index, NodeState state)
        {
            if (index >= _graph.Count || index < 0)
            {
                throw new ConstraintException();
            }
            if (NodeStateChangeEvent != null) NodeStateChangeEvent(this, new NodeStateChangeEventArgs(index, state));
        }

        protected void RaiseEdgeStateChangeEvent(int id, EdgeState state)
        {
            if (EdgeStateChangeEvent != null) EdgeStateChangeEvent(this, new EdgeStateChangeEventArgs(id, state));
        }

        protected void RaiseNotifyEvent(String message, String caption = "Hiba")
        {
            if (NotifyEvent != null) NotifyEvent(this, new MessageEventArgs(message, caption));
        }
    }
}