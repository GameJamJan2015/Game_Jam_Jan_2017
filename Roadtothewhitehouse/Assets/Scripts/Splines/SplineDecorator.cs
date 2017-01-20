using System.Collections.Generic;
using UnityEngine;

public class SplineDecorator : MonoBehaviour {

	public BezierSpline spline;

	public Transform splineObject;

    public float distance = 0.4f;

    private float currentStep;

    private List<Transform> objectList = new List<Transform>();


    public void RemoveAll()
    {
        int amount = transform.childCount;
        for (int i = 0; i < amount; i++)
        {
            if(transform.GetChild(0) != null)
                DestroyImmediate(transform.GetChild(0).gameObject);
        }
        objectList.Clear();
    }

	public void GenerateCurve () {
		if (splineObject == null) {
			return;
		}

        float stepSize = 0.0005f;
        currentStep = 0;
        while (currentStep < 1f)
        {
            currentStep += stepSize;
            if (objectList.Count > 0)
            {
                if (Vector3.Distance(objectList[objectList.Count - 1].position, spline.GetPoint(currentStep)) < distance)
                    continue;
            }

            Transform item = Instantiate(splineObject) as Transform;
            Vector3 position = spline.GetPoint(currentStep);
            item.transform.localPosition = position;
            item.transform.LookAt(position + spline.GetDirection(currentStep));
            item.transform.parent = transform;
            objectList.Add(item);
        }
    }
}