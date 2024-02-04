namespace DefaultNamespace.Utils.MissingNetClasses
{
    using System;
    using System.Collections.Generic;

    public class SimplePriorityQueue<T>
    {
        private List<(T Item, int Priority)> elements = new List<(T, int)>();

        public int Count => elements.Count;

        public void Enqueue(T item, int priority)
        {
            var element = (item, priority);
            bool added = false;
            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].Priority > priority)
                {
                    elements.Insert(i, element);
                    added = true;
                    break;
                }
            }
            if (!added)
            {
                elements.Add(element);
            }
        }

        public T Dequeue()
        {
            if (elements.Count == 0)
            {
                throw new InvalidOperationException("The queue is empty");
            }
            var item = elements[0].Item;
            elements.RemoveAt(0);
            return item;
        }
    }
}