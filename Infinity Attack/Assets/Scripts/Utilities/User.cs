using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    public string _id;
    public string name;
    public int gold;
    public int gem;
    public User user;

    private static User instance;
    public static User Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new User();
            }
            return instance;
        }
    }
}
