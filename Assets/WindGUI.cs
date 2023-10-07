using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.GameSystems;
using UnityEngine;
using Zenject;

public class WindGUI : MonoBehaviour
{
    [Inject] private WindSystem _windSystem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var angle = _windSystem.Angle;
        var t = (RectTransform) transform;
        t.rotation = Quaternion.Euler(0, 0, -angle);

    }
}
