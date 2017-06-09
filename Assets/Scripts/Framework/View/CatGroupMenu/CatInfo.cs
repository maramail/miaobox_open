using UnityEngine;
using System;
using System.Collections;

public class CatInfo : MonoBehaviour {
    public int Id { get; set; }
    public int CatTypeid { get; set; }
    public int GroupId { get; set; }
    public int Keep { get; set; }
    public DateTime AcqDate { get; set; }
    public int Grow { get; set; }

    public int Evo { set; get; }
    public string Name { get; set; }
    public int Level { set; get; }
    public int Iq { set; get; }
    public int Atk { set; get; }
    public int React { set; get; }
    public int Skill { set; get; }
    public string About { get; set; }
}
