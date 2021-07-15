using UnityEngine;

public class MissileController : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private GameObject target = null;
    private Transform targetTransform = null;
    private float stoppingDistance = 0;

    private void Update()
    {
        if (targetTransform != null)
        {
            if (Vector3.Distance(transform.position, targetTransform.position) > stoppingDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, speed * Time.deltaTime);
            }
        }
    }

    public void setTargetPosition(Transform targetPosition)
    {
        targetTransform = targetPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        }

        Destroy(this.gameObject);
    }
}
