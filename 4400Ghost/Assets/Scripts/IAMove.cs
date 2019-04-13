using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAMove : MonoBehaviour
{
    public List<Vector2> followingPath;
    public int indexPath;

    private Rigidbody2D rg;

    void Start()
    {
        Invoke("FollowIA",5);
        rg = GetComponent<Rigidbody2D>();
    }

    public void FollowPath()
    {

        if (indexPath >= followingPath.Count-2) //don't touch before change
        {
            rg.velocity= Vector2.zero;
            FollowIA();
            return;
        }

        rg.velocity = followingPath[indexPath] - (Vector2)transform.position;
        rg.velocity = rg.velocity.normalized * 2f;

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
            GameManager.Instance.Dijkstra.BFS(new Vector2(Random.Range(0, 50), Random.Range(0, 50)),
                transform.position);
        indexPath = 0;
        FollowPath();
    }
}
