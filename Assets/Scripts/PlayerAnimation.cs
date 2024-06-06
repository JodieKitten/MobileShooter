using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    private Animator _anim;
    private Player _player;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_player.playerOne == true)
        {
        if(Input.GetKeyDown(KeyCode.A))
        {
            _anim.SetBool("Turn_Left", true);
            _anim.SetBool("Turn_Right", false);
        }
        else if(Input.GetKeyUp(KeyCode.A))
        {
            _anim.SetBool("Turn_Left", false);
            _anim.SetBool("Turn_Right", false);
        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            _anim.SetBool("Turn_Right", true);
            _anim.SetBool("Turn_Left", false);
        }
        else if(Input.GetKeyUp(KeyCode.D))
        {
            _anim.SetBool("Turn_Right", false);
            _anim.SetBool("Turn_Left", false);
        }

        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                _anim.SetBool("Turn_Left", true);
                _anim.SetBool("Turn_Right", false);
            }
            else if (Input.GetKeyUp(KeyCode.Keypad4))
            {
                _anim.SetBool("Turn_Left", false);
                _anim.SetBool("Turn_Right", false);
            }

            if (Input.GetKeyDown(KeyCode.Keypad6))
            {
                _anim.SetBool("Turn_Right", true);
                _anim.SetBool("Turn_Left", false);
            }
            else if (Input.GetKeyUp(KeyCode.Keypad6))
            {
                _anim.SetBool("Turn_Right", false);
                _anim.SetBool("Turn_Left", false);
            }
        }
    }
}
