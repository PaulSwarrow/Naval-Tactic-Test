using UnityEngine;

namespace Ship.View
{
    [ExecuteInEditMode]
    public class GafSailView : MonoBehaviour
    {
        [Range(-1,1)]
        public float InputWind;

        private void Update()
        {
            transform.localScale = new Vector3(InputWind, 1, 1);
        }
    }
}