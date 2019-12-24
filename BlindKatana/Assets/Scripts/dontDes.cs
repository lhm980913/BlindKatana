using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MoreMountains.CorgiEngine;
using UnityEngine.UI;
public class dontDes : MonoBehaviour
{
    public bool player1Win =false;
    public bool player2Win =false;

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
            player1Win = false;
            player2Win = false;
        }
        if (Input.GetKeyDown(KeyCode.Home)&& SceneManager.GetActiveScene().buildIndex==0)
        {
            NextLevel();
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
            GameObject aaa = Instantiate(win, transform);
            Text winText = aaa.GetComponentInChildren<Text>();
            winText.text = "Player1 Win !";
            Destroy(aaa, 2);
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
            GameObject aaa = Instantiate(win, transform);
            Text winText = aaa.GetComponentInChildren<Text>();
            winText.text = "Player2 Win !";
            Destroy(aaa, 2);
        }
    }

}
