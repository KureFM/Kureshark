using System;
using System.Collections.Generic;

namespace Kureshark.Model
{
    /// <summary>
    /// 带有入队出队触发事件的队列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObservableQueue<T> : Queue<T>
    {
        #region 事件及其处理程序

        public event EventHandler HasEnqueue;

        public event EventHandler HasDequeue;

        private void OnHasEnqueue()
        {
            if (HasEnqueue != null)
            {
                HasEnqueue(this, new EventArgs());
            }
        }

        private void OnHasDequeue()
        {
            if (HasDequeue != null)
            {
                HasDequeue(this, new EventArgs());
            }
        }

        #endregion 事件及其处理程序

        #region 构造函数

        public ObservableQueue()
            : base()
        {
        }

        public ObservableQueue(int capacity)
            : base(capacity)
        {
        }

        public ObservableQueue(IEnumerable<T> collection)
            : base(collection)
        {
        }

        #endregion 构造函数

        #region 重写函数以触发事件

        public new void Enqueue(T t)
        {
            base.Enqueue(t);
            OnHasEnqueue();
        }

        public new T Dequeue()
        {
            OnHasDequeue();
            return base.Dequeue();
        }

        #endregion 重写函数以触发事件
    }
}