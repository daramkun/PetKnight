using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScene : MonoBehaviour
{
    [SerializeField]
    private StageMarquee[] _stages;

    void Start()
    {
        var random = Random.Range(0, _stages.Length);
        for (var i = 0; i < _stages.Length; ++i)
            _stages[i].gameObject.SetActive(i == random);
    }

    public void OnClickStart()
    {
        SceneManager.LoadScene("GameScene");
    }
}
