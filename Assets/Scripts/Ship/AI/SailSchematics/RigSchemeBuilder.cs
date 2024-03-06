using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Ship;
using Ship.Data;
using Ship.Interfaces;
using Ship.Utils;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.Ship.AI.SailSchematics
{
    public class RigSchemeBuilder
    {
        public SailRiggingTemplate BuildScheme(IShipSetup shipSetup)
        {
            var result = new SailRiggingTemplate();
            var sails = shipSetup.GetSails();
            var maneuveringSails = sails.Where(s => s.UsedForTurns);
            var pushSails = sails.Where(s => !s.UsedForTurns);
            result.TurnLeft = Turn(maneuveringSails, -1);
            result.TurnRight = Turn(maneuveringSails, 1);

            /*
            var sails = shipSetup.GetAllSails();

            var initialRigState = new ShipRigState();
            var allConfigurations = new List<ShipRigState>();


            // Start the stopwatch
            Stopwatch stopwatch = Stopwatch.StartNew();
            BuildSchemeRecursive(shipSetup, sails, 0, initialRigState, allConfigurations);

            stopwatch.Stop();
            Debug.Log($"Scheme built! duration {stopwatch.ElapsedMilliseconds}, variants: {allConfigurations.Count}");*/
        }

        
        private SailRiggingTemplate.SchemeVariants Turn(IEnumerable<ISailSetup> sails, int sign)
        {
            var result = new SailRiggingTemplate.SchemeVariants();
            foreach (var direction in EnumTools.ToArray<WorldDirection>())
            {
                //TODO better solution!
                var wind = Quaternion.Euler(0, (int)direction, 0) * Vector3.forward;

                var scheme = new SailRigScheme();
                foreach (var sail in sails)
                {
                    var resultState = new ShipSailState();
                    double maxTorque = 0; //assumption that there is always zero torque configuration (setup = 0)
                    foreach (var configuration in sail.GetAllConfigurations())
                    {
                        var forces = ShipPhysics.GetForces(configuration, sail, wind);
                        if (forces.torque * sign > maxTorque * sign)
                        {
                            resultState = configuration;
                            maxTorque = forces.torque;
                        }
                        
                    }

                    scheme.Configuration.Add(sail.SailType, resultState);

                }

                result[direction] = scheme;

            }

            return result;
        }

        public SailRiggingTemplate.SchemeVariants TurnOnPlace(IEnumerable<ISailSetup> sails, int sign)
        {
            var sailsArray = sails.ToArray();
            var result = new SailRiggingTemplate.SchemeVariants();
            foreach (var direction in EnumTools.ToArray<WorldDirection>())
            {
                BuildSchemeRecursive(sailsArray, 0, new ShipRigState(), configuration =>
                {
                    ShipPhysics.GetForces(sailsArray, )
                    
                });

            }


            return result;

        }
        private void BuildSchemeRecursive(ISailSetup[] sails, int currentIndex,
            ShipRigState currentRigState, Action<ShipRigState> handler)
        {
            if (currentIndex >= sails.Length)
            {
                handler.Invoke(currentRigState);
                return;
            }

            var  sail = sails[currentIndex];
            foreach (var configuration in sail.GetAllConfigurations())
            {
                ShipRigState nextRigState = currentRigState;
                nextRigState[sail.SailType] = configuration;
                BuildSchemeRecursive(sails, currentIndex + 1, nextRigState, handler);
                
            }
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