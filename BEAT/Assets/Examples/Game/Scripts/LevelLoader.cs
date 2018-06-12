using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {
    public string Level;
    public float Delay = 1f;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void LoadLevel()
    {
        StartCoroutine(WaitAndLoad(Delay));
    }

    private IEnumerator WaitAndLoad(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(Level);
    }
}
