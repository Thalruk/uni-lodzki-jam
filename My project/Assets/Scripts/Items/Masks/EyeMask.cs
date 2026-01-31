using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EyeMask : ItemBaseClass
{
    List<GameObject> objectsForTracking = new List<GameObject>();
    Transform target;
    [SerializeField] Transform eye;
    [SerializeField] float offset;
    private void Start()
    {
        isPassive = true;
        isHasDurationTime = true;
        useDuration = 6f;
        cooldown = useDuration + cooldown;
        objectsForTracking = GameObject.FindGameObjectsWithTag("Track").ToList();
        eye = GetComponentInChildren<Transform>();
    }
    protected override void Use()
    {
        FindClosestTrackableObject();
    }
    void FindClosestTrackableObject()
    {
        float tempDistance = Mathf.Infinity;
        float currentDistance = 0;
        for (int i = 0; i < objectsForTracking.Count; i++)
        {
            currentDistance = Vector2.Distance(objectsForTracking[i].transform.position, gameObject.transform.position);
            if (tempDistance > currentDistance)
            {
                tempDistance = currentDistance;
                target = objectsForTracking[i].transform;
            }
        }
        print(target.name);
    }

    private void Update()
    {
        if (target)
        {
            Vector3 dir = (target.position - transform.position).normalized;
            eye.transform.position = transform.position + dir * offset;
        }
    }
}
