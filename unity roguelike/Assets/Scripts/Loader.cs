using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject player;
    public GameObject UI;

    void Awake()
    {
        if (GameManager.instance == null)
            Instantiate(gameManager);
        if (UIManager.instance == null)
            Instantiate(UI);
    }
}
