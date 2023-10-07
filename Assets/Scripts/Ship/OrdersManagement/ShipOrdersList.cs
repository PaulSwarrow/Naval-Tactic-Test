using System;
using System.Collections.Generic;
using Ship.AI;
using UnityEngine;

namespace Ship.OrdersManagement
{
    public class ShipOrdersList
    {
        private List<IShipOrder> _activeOrders = new ();
        

        public void AddRange(params IShipOrder[] orders)
        {
            foreach (var order in orders)
            {
                AddOrder(order);
            }
        }
        
        public void AddOrder(IShipOrder order)
        {
            //TODO diff handlers for dif categories?
            if (order.Category != ShipOrderCategory.Sails)
            {
                ForeachOrder(a =>
                {
                    if ((a.Category & order.Category) != ShipOrderCategory.None)
                    {
                        RemoveOrder(order); //TODO mention cause?
                    }
                });
            }

            Debug.Log($"Add order: {order}");
            _activeOrders.Add(order);

        }

        public void RemoveOrder(IShipOrder order)
        {
            _activeOrders.Remove(order);
        }

        public void ForeachOrder(Action<IShipOrder> handler)
        {
            for (int i = _activeOrders.Count - 1; i >= 0; i--)
            {
                handler(_activeOrders[i]);
            }
        }
    }
}