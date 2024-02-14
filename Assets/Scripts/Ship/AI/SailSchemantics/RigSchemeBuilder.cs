using System;
using System.Collections.Generic;
using System.Diagnostics;
using Ship.Data;
using Ship.Interfaces;
using Debug = UnityEngine.Debug;

namespace Ship.AI.SailSchemantics
{
    public class RigSchemeBuilder
    {
        public void BuildScheme(IShipSetup shipSetup)
        {
            var sails = shipSetup.GetAllSails();
            var initialRigState = new ShipRigState();
            var allConfigurations = new List<ShipRigState>();


            // Start the stopwatch
            Stopwatch stopwatch = Stopwatch.StartNew();
            BuildSchemeRecursive(shipSetup, sails, 0, initialRigState, allConfigurations);

            stopwatch.Stop();
            Debug.Log($"Scheme built! duration {stopwatch.ElapsedMilliseconds}, variants: {allConfigurations.Count}");
        }

        private void BuildSchemeRecursive(IShipSetup shipSetup, SailType[] sails, int currentIndex,
            ShipRigState currentRigState, List<ShipRigState> allConfigurations)
        {
            if (currentIndex >= sails.Length)
            {
                allConfigurations.Add(currentRigState);
                return;
            }

            SailType currentSailType = sails[currentIndex];
            GetSailVariants(shipSetup, currentSailType, sailState =>
            {
                ShipRigState nextRigState = currentRigState;
                nextRigState[currentSailType] = sailState;
                BuildSchemeRecursive(shipSetup, sails, currentIndex + 1, nextRigState, allConfigurations);
            });
        }

        private void GetSailVariants(IShipSetup shipSetup, SailType type, Action<ShipSailState> handler)
        {
            foreach (var angle in shipSetup.SailAnglesAvailable(type))
            {
                foreach (var setup in shipSetup.SailSetupsAvailable(type))
                {
                    handler.Invoke(new ShipSailState()
                    {
                        Angle = angle,
                        Setup = setup
                    });
                }
            }
        }
    }
}