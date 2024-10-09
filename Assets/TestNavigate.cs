using System.Collections;
using System.Collections.Generic;
using IV.Enemy;
using UnityEngine;

public class TestNavigate : MonoBehaviour
{
    [SerializeField]
    private EnemyController enemy;
    // Start is called before the first frame update
    void Start()
    {
        enemy.SetNavigator(new Navigator());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
