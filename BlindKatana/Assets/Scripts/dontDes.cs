using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MoreMountains.CorgiEngine;
using UnityEngine.UI;
public class dontDes : MonoBehaviour
{
    public bool player1Win;
    public bool player2Win;

    int[] levelList= {1,2,3};
    int nowlevel = 1;

    public static dontDes instance;

    public GameObject win;
    // Start is called before the first frame update
    void Awake()
    {
        randomLevel();
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
       
      
    }

    void Start()
    {




    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
            randomLevel();
            nowlevel = 1;
        }

    }
    //随机顺序
    void randomLevel()
    {
        for (int i = 0; i < levelList.Length; i++)
        {
            int tmp = levelList[i];
            int r = Random.Range(i, levelList.Length);
            levelList[i] = levelList[r];
            levelList[r] = tmp;
        }

    }

    //切换到下一关
    public void NextLevel()
    {
        SceneManager.LoadScene(levelList[nowlevel-1]);
    }

    public void Player1Score()
    {
        if(!player1Win)
        {
            player1Win = true;
            nowlevel += 1;
        }
        else
        {
            //MultiplayerLevelManager.Instance.CheckEnd("Player1");
            SceneManager.LoadScene(4);
            Text winText = Instantiate(win, transform).GetComponentInChildren<Text>();
            winText.text = "Player1 Win !";
        }
    }

    public void Player2Score()
    {
        if (!player2Win)
        {
            player2Win = true;
            nowlevel += 1;
        }
        else
        {
            //MultiplayerLevelManager.Instance.CheckEnd("Player2");
            SceneManager.LoadScene(4);
            Text winText = Instantiate(win, transform).GetComponentInChildren<Text>();
            winText.text = "Player2 Win !";
        }
    }

}
