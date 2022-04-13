using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public enum capas
    {
        suelo = 3,
        personaje = 6,
        caracola = 7,
        tomarCaracola = 8
    }
    void Start()
    {
        Physics2D.IgnoreLayerCollision((int)capas.personaje, (int)capas.caracola, true);
        Physics2D.IgnoreLayerCollision((int)capas.tomarCaracola, (int)capas.suelo, true);
        Physics2D.IgnoreLayerCollision((int)capas.tomarCaracola, (int)capas.caracola, true);
    }

}
