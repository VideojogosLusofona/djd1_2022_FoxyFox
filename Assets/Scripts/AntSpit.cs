using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntSpit : AntPatrol
{
    [SerializeField] private float      spitFreezeTime = 4.0f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform  shootPoint;


    private float shotTime = 0.0f;

    override protected void Start()
    {
        base.Start();

        var detectorRedirect = GetComponentInChildren<DetectorRedirect>();
        if (detectorRedirect)
        {
            detectorRedirect.onDetected += DetectedSomething;
            detectorRedirect.onLeave += SomethingLeft;
        }
    }

    private void OnDestroy()
    {
        var detectorRedirect = GetComponentInChildren<DetectorRedirect>();
        if (detectorRedirect)
        {
            detectorRedirect.onDetected -= DetectedSomething;
            detectorRedirect.onLeave -= SomethingLeft;
        }
    }

    override protected void Update()
    {
        base.Update();

        if (shotTime > 0)
        {
            shotTime -= Time.deltaTime;
            if (shotTime <= 0.0f)
            {
                if (projectilePrefab)
                {
                    Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
                }
                shotTime = 2.0f;
            }
        }
    }

    void DetectedSomething(Collider2D collider, DetectorRedirect detector)
    {
        if (detector.name.StartsWith("Player"))
        {
            var player = collider.GetComponent<Player>();
            if (player)
            {
                thawTimer = spitFreezeTime;
                shotTime = 2.0f;
            }
        }
    }

    void SomethingLeft(Collider2D collider, DetectorRedirect detector)
    {
        if (detector.name.StartsWith("Player"))
        {
            var player = collider.GetComponent<Player>();
            if (player)
            {
                thawTimer = 0;
                shotTime = 0;
            }
        }
    }
}
