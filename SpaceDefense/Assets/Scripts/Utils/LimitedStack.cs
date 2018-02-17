//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="LimitedStack.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Defines a stack with a limited size that when reached, will discard oldest items
    /// </summary>
    public class LimitedStack<T>
    {
        /// <summary>
        /// Gets the size of the stack
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// Gets the total number of items in this stack
        /// </summary>
        public int Count
        {
            get
            {
                return this._list.Count;
            }
        }

        /// <summary>
        /// The collection of items
        /// </summary>
        private List<T> _list;

        /// <summary>
        /// Index  of the last valid item
        /// </summary>
        private int _index;

        /// <summary>
        /// Creates a new instance of the <see cref="LimitedStack"/> class
        /// </summary>
        /// <param name="size">Limited size of the stack</param>
        public LimitedStack(int size)
        {
            if (size < 1)
            {
                throw new ArgumentOutOfRangeException("Size must be at least 1");
            }

            this.Size = size;
            this._list = new List<T>();
            this._index = -1;
        }

        /// <summary>
        /// Peeks at whats on top
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            return this._index >= 0 ? this._list[this._index] : default(T);
        }

        /// <summary>
        /// Pushes a new item onto the stack
        /// </summary>
        /// <param name="newItem"></param>
        public void Push(T newItem)
        {
            this._index++;
            this._list.Add(newItem);

            while (this._index >= this.Size)
            {
                this._index--;
                this._list.RemoveAt(0);
            }
        }

        /// <summary>
        /// Pops out the last inserted item
        /// </summary>
        /// <returns>The last inserted item</returns>
        public T Pop()
        {
            if (this._index < 0)
            {
                return default(T);
            }

            var result = this._list[this._index];
            this._list.RemoveAt(this._index);
            this._index--;
            return result;
        }
    }
}
