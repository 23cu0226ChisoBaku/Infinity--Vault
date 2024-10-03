using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatCode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        hideFlags = HideFlags.HideInHierarchy;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            var player = FindAnyObjectByType<PlayerController>();

            player.gameObject.transform.position = new Vector3(9.5f,-4f,0f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            var currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
    }
}
