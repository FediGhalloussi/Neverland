using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fee : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float minDistance;
    [SerializeField] private AnimationCurve speedCurve;
    [SerializeField] private FeeIA IAScript;
    float firstAnimDuration;
    float initialDist;
    float currentDist;
    bool startedSwap = false;
    private float defaultHeight;

    private bool isMoving = true;

    private float DistanceManagement()
    {
        currentDist = Vector3.Distance(transform.position, cameraTransform.position)-minDistance;
        return currentDist/initialDist;
    }

    private void swapScript()
    {
        IAScript.started = true;
        Destroy(this);
    }

    void Start()
    {
        defaultHeight = transform.position.y;
        firstAnimDuration = Vector3.Distance(transform.position, cameraTransform.position)*1.1f; // todo: faire un algo de calcul d'intégral d'animationcurve pour avoir une valeur exacte
       
        initialDist=Vector3.Distance(transform.position, cameraTransform.position)-minDistance;
    }
    void Update()
    {
        if (isMoving)
        {
            // Calculer la distance entre l'objet et la caméra
            float distance = Vector3.Distance(transform.position, cameraTransform.position);

            // Si la distance est supérieure à la distance minimale, faites avancer l'objet vers la caméra
            if (distance > minDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(cameraTransform.position.x, defaultHeight, cameraTransform.position.z), moveSpeed * speedCurve.Evaluate(DistanceManagement())*Time.deltaTime);
            }
            else
            {
                // Une fois que l'objet est suffisamment proche, arrêtez de le déplacer et commencez la rotation
                isMoving = false;
            }
        }
        else
        {
            // Faites tourner l'objet autour de la caméra
            transform.RotateAround(cameraTransform.position, Vector3.up, rotationSpeed * Time.deltaTime);
            if (!startedSwap)
            {
                startedSwap=true;
                Invoke("swapScript",4f);
            }
        }
    }
}