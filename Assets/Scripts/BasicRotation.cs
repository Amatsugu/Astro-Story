using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRotation : MonoBehaviour
{
    public GameObject planet2Moons;
    public GameObject planet1;
    public GameObject planet2;
    public GameObject planet3;
    public GameObject planet4;
    public GameObject planet5;
    public GameObject planet6;
    public GameObject donut;
    public GameObject crashedShip;
    public GameObject ship;



    void Start()
    {


        planet2Moons.transform.Rotate(0, 0, 0);


    }

    // Update is called once per frame
    void Update()
    {

        planet2Moons.transform.Rotate(.02f, .0055f, 0, Space.Self);
        donut.transform.Rotate(.05f, .0044f, 0, Space.Self);
        crashedShip.transform.Rotate(.05f, .0044f, 0, Space.Self);
        ship.transform.Rotate(.05f, .0044f, 0, Space.Self);
        planet2.transform.Rotate(0, 0.0005f, 0);
        planet3.transform.Rotate(0, 0.0005f, 0);
        planet4.transform.Rotate(0, 0.0005f, 0);
        planet5.transform.Rotate(0, 0.0005f, 0);
        planet6.transform.Rotate(0, 0.0005f, 0);
    }
}
