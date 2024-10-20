using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSwitchScene : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Prototype");
        }
    }
}
