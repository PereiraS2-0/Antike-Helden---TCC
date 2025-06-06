using UnityEngine;

public class XPOrb : MonoBehaviour
{
    public int xpValue = 1;
    public float delayBeforeMove = 2f;
    public float moveSpeed = 5f;
    public float collectionRadius = 0.5f;

    private Transform towerTarget;
    private bool movingToTower = false;

    void Start()
    {
        towerTarget = GameObject.FindGameObjectWithTag("Tower").transform;
        Invoke("StartMovingToTower", delayBeforeMove);
    }

    void StartMovingToTower()
    {
        movingToTower = true;
        // Adiciona um efeito visual opcional
        GetComponent<SpriteRenderer>().color = Color.green;
    }

    void Update()
    {
        if (movingToTower && towerTarget != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, towerTarget.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, towerTarget.position) < collectionRadius)
            {
                CollectXP();
            }
        }
    }

    void CollectXP()
    {
        if (XPManager.Instance != null)
        {
            XPManager.Instance.AddXP(xpValue);
        }
        Destroy(gameObject);
    }
}