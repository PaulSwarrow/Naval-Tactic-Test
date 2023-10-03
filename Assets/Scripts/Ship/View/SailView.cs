using UnityEngine;

namespace Ship.View
{
    [ExecuteInEditMode]
    public class SailView : MonoBehaviour
    {
        [Range(0.1f,1)]
        public float InputWind;

        [Range(0,2)]
        public int Setup;
        
        [SerializeField] private Transform[] _sails;

        private void Update()
        {
            for (int i = 0; i < _sails.Length; i++)
            {
                _sails[i].gameObject.SetActive(Setup > i);
            }
            transform.localScale = new Vector3(1, 1, InputWind);
        }
    }
}