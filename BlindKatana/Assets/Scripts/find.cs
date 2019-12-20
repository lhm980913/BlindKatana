using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class find : MonoBehaviour
{
    Button thisbutton;


    // Start is called before the first frame update
    private void Awake()
    {
        thisbutton = GetComponent<Button>();
       
        thisbutton.onClick.AddListener(OnClick);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnClick()
    {
        dontDes.instance.NextLevel();
    }
}
