using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ClearLeashPath : MonoBehaviour
{
    [SerializeField]
    private LayerMask enemy;

    public bool doClearPath = false;

    public List<Transform> enemiesList;
    public List<Vector3> dirList;

    Transform cam;

    private void Start()
    {
        cam = Camera.main.transform;
    }

    private void Update()
    {
        if (!doClearPath) return;

        Collider[] colliders = Physics.OverlapSphere(transform.GetChild(1).position, 2f, enemy);

        foreach (var collider in colliders)
        {
            enemiesList.Add(collider.transform.root);
            enemiesList = enemiesList.Distinct().ToList();
            enemiesList.Remove(transform);

        }

        foreach (Transform enemy in enemiesList)
        {
            enemy.GetComponent<Animator>().Play("Shove Reaction");

            var enemyScreenPos = Camera.main.WorldToScreenPoint(enemy.position);


            if (enemyScreenPos.x > Screen.width / 2)
            {
                enemy.Translate(FPSController.playerTransform.right * 0.1f, Space.World);
            }
            else if (enemyScreenPos.x < Screen.width / 2)
            {
                enemy.Translate(-FPSController.playerTransform.right * 0.1f, Space.World);
            }
        }
    }

}
