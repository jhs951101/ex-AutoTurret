using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField]
    private GameObject missileObject;

    [SerializeField]
    private Transform tfGunBody;

    [SerializeField]
    private Transform missilePlace;

    [SerializeField]
    private LayerMask enemyLayerMask;

    [SerializeField]
    private float shootingRange;

    [SerializeField]
    private float rotateTimes;

    [SerializeField]
    private float spinAttackSpeed;

    [SerializeField]
    private float fireRate;

    private Transform tfTarget;
    private float currentFireRate;

    private void Start()
    {
        tfTarget = null;
        currentFireRate = fireRate;
        InvokeRepeating("SearchEnemy", 0f, 0.5f);
    }

    private void Update()
    {
        if(tfTarget == null)
        {
            tfGunBody.Rotate(new Vector3(0, rotateTimes * 45, 0) * Time.deltaTime);
        }
        else
        {
            Quaternion t_lookRotation = Quaternion.LookRotation(tfTarget.position);
            Vector3 t_euler = Quaternion.RotateTowards(tfGunBody.rotation, t_lookRotation, spinAttackSpeed * Time.deltaTime).eulerAngles;
            tfGunBody.rotation = Quaternion.Euler(0, t_euler.y, 0);

            Quaternion t_fireRotation = Quaternion.Euler(0, t_lookRotation.eulerAngles.y, 0);
            if(Quaternion.Angle(tfGunBody.rotation, t_fireRotation) < 5f)
            {
                currentFireRate -= Time.deltaTime;
                if (currentFireRate <= 0)
                {
                    currentFireRate = fireRate;

                    if (tfTarget != null)
                    {
                        GameObject missile = Instantiate(missileObject, missilePlace.position, Quaternion.identity);
                        missile.GetComponent<MissileController>().setTargetPosition(tfTarget);
                    }
                }
            }
        }
    }

    private void SearchEnemy()
    {
        Collider[] t_cols = Physics.OverlapSphere(transform.position, shootingRange, enemyLayerMask);
        Transform t_shortestTarget = null;

        if (t_cols.Length > 0)
        {
            float t_shortestDistance = Mathf.Infinity;
            foreach (Collider t_colTarget in t_cols)
            {
                float t_distance = Vector3.SqrMagnitude(transform.position - t_colTarget.transform.position);
                if (t_shortestDistance > t_distance)
                {
                    t_shortestDistance = t_distance;
                    t_shortestTarget = t_colTarget.transform;
                }
            }
        }

        tfTarget = t_shortestTarget;
    }
}
