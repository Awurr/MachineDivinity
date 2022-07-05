using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class DeathAnimation : MonoBehaviour
{
    bool Dying;
    public Animator Animation;
    public Image Death3;
    public Text Text1, Text2;
    public Image Underlay;
    public string[] FinalWords;

    public void StartAnimation()
    {
        if (Dying)
            return;

        Underlay.DOColor(new Color(0.1f, 0.1f, 0.1f, 0.8f), 4.5f);
        Text1.text = FinalWords[Random.Range(0, FinalWords.Length - 1)];
        Text2.text = FinalWords[Random.Range(0, FinalWords.Length - 1)];
        Dying = true;
        Animation.Play("Dying");
        StartCoroutine(DeathAnim());
    }

    void Update()
    {
        if (Dying)
        {
            Death3.gameObject.SetActive(!Death3.gameObject.activeSelf);
        }
    }

    IEnumerator DeathAnim()
    {
        yield return new WaitForSeconds(1.75f);
        Text2.text = FinalWords[Random.Range(0, FinalWords.Length - 1)];

        yield return new WaitForSeconds(0.25f);
        StartCoroutine(SideOff(transform.Find("LeftSide")));

        yield return new WaitForSeconds(1);
        StartCoroutine(SideOff(transform.Find("RightSide")));

        yield return new WaitForSeconds(0.5f);
        Text1.text = FinalWords[Random.Range(0, FinalWords.Length - 1)];
        yield return new WaitForSeconds(0.5f);

        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync("DeathScreen");
        SceneManager.UnloadSceneAsync("Game");
    }

    IEnumerator SideOff(Transform Side)
    {
        foreach (Transform Child in Side)
        {
            if (Child.childCount > 0)
            {
                StartCoroutine(SideOff(Child));
            }

            yield return new WaitForSeconds(Random.Range(0.3f, 0.7f));

            if (Child.GetComponent<Image>())
            {
                Child.GetComponent<Image>().DOColor(Color.black, 0.2f);
            }
            else if (Child.GetComponent<Text>())
            {
                Child.GetComponent<Text>().DOColor(Color.black, 0.2f);
            }
        }
    }
}
