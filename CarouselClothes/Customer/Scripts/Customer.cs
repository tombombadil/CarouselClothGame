using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ruckcat;

public class Customer : HyperSceneObj
{
        string _id, _type, _color1, _color2, _color3;

        public string Id { get => _id; set => _id = value; }
        public string Type { get => _type; set => _type = value; }

}
