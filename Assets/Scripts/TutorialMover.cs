using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialMover : MonoBehaviour
{

    [SerializeField]
    private float minTime;

    private bool canTransition;

    [SerializeField]
    private int nextSceneIndex;

    // Start is called before the first frame update
    void Start()
    {
        canTransition = false;
        StartCoroutine(CanTransition());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) && canTransition)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    public IEnumerator CanTransition()
    {
        yield return new WaitForSeconds(minTime);
        Debug.Log("Can transition");
        canTransition = true;
    }
}
