using DefaultNamespace.Data;
using UnityEngine;

namespace Ship.View
{
    [ExecuteInEditMode]
    public class YardView : MonoBehaviour
    {
        [SerializeField] private ValueRange _range;

        [Range(-1, 1)] public float Angle;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            transform.localRotation = Quaternion.Euler(0, Mathf.Lerp(_range.Min, _range.Max, 0.5f + Angle / 2), 0);
        }
    }
}