using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAMove : MonoBehaviour
{
    public List<Vector2> followingPath;
    public int indexPath;

    private Rigidbody2D rb;

    void Start()
    {
        Invoke("FollowIA",5);
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        transform.up = rb.velocity;
    }

    public void FollowPath()
    {

        if (indexPath >= followingPath.Count-1) //don't touch before change
        {
            rb.velocity= Vector2.zero;
            FollowIA();
            return;
        }

        rb.velocity = followingPath[indexPath] - (Vector2)transform.position;
        rb.velocity = rb.velocity.normalized * GameManager.Instance.speedIA;

        if (Vector2.Distance(transform.position, followingPath[indexPath]) < 0.5f)
        {
            indexPath++;
        }
    }

    private void FixedUpdate()
    {
        if (followingPath!=null)
        {
            FollowPath();
        }      
    }

    void FollowIA()
    {
        followingPath =
            GameManager.Instance.Dijkstra.BFS(GameManager.Instance.BspScript.targetList[Random.Range(0, GameManager.Instance.BspScript.targetList.Count)].transform.position,
                transform.position);
        indexPath = 0;
        FollowPath();
    }
}
