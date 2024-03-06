using System.Collections.Generic;
using Ship.Data;

namespace Assets.Scripts.Ship.AI.SailSchematics
{
    public class SailRiggingTemplate
    {
        public class SchemeVariants : Dictionary<WorldDirection, SailRigScheme>
        {
            
        }

        public SchemeVariants TurnLeft;
        public SchemeVariants TurnRight;
        
        public SchemeVariants TurnLeftInPlace;
        public SchemeVariants TurnRightInPlace;

        public SchemeVariants HalfSpeed;
        public SchemeVariants FullForward;
        
        public SailRigScheme FullStop;
        
        

    }
}