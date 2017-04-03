using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LogoManager : MonoBehaviour
{
    public GameObject text;

    public float speed = 360;
    bool colB = false;

    public GameObject loading;
    private AsyncOperation load;
    bool ininvoke = false;

    void Awake()
    {
        Invoke("yell", 0.25f);
    }

    void Update()
    {
        transform.Rotate(0, 0, speed * Time.deltaTime);
        transform.Translate(0, -300 * Time.deltaTime, 0,Space.World);

        if(colB)
        {
            if(text.active)
            {
                if(text.transform.localScale.x < 1.1425f)
                    text.transform.localScale *= 1.02f;

                if (!ininvoke)
                {
                    ininvoke = true;
                    Invoke("change", 2.0f);
                }
            }
        }
    }

    void yell()
    {
        GetComponent<AudioSource>().Play();
    }

    void change()
    {
        StartCoroutine(ChangeScene("Home"));
    }

    IEnumerator ChangeScene(string str)
    {
        loading.SetActive(true);
        load = SceneManager.LoadSceneAsync(str, LoadSceneMode.Single);
        load.allowSceneActivation = true;

        yield return load;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!colB)
        {
            if (col.CompareTag("Ant"))
            {
                colB = true;
                col.GetComponent<Animator>().Play("antBomb");
                text.SetActive(true);
                transform.GetChild(0).GetComponent<AudioSource>().Play();
            }
        }
    }
}
