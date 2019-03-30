using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Leaf
{
    

    private const  int MIN_LEAF_SIZE = 20;
 
    public int y, x, width, height; // the position and size of this Leaf
 
    public Leaf leftChild; // the Leaf's left child Leaf
    public Leaf rightChild; // the Leaf's right child Leaf
    public Rect room; // the room that is inside this Leaf
    //public Vector.halls; // hallways to connect this Leaf to other Leafs
 
    public Leaf(int X, int Y, int Width, int Height)
    {
        // initialize our leaf
        x = X;
        y = Y;
        width = Width;
        height = Height;
    }

    public bool split()
    {
        // begin splitting the leaf into two children
        if (leftChild != null || rightChild != null)
            return false; // we're already split! Abort!

        // determine direction of split
        // if the width is >25% larger than height, we split vertically
        // if the height is >25% larger than the width, we split horizontally
        // otherwise we split randomly
        bool splitH = Random.value > 0.5f;
        if (width > height && width / height >= 1.25f)
            splitH = false;
        else if (height > width && height / width >= 1.25f)
            splitH = true;

        int max = (splitH ? height : width) - MIN_LEAF_SIZE; // determine the maximum height or width
        if (max <= MIN_LEAF_SIZE)
            return false; // the area is too small to split any more...

        int split = Random.Range(MIN_LEAF_SIZE, max); // determine where we're going to split

        // create our left and right children based on the direction of the split
        if (splitH)
        {
            leftChild = new Leaf(x, y, width, split);
            rightChild = new Leaf(x, y + split, width, height - split);
        }
        else
        {
            leftChild = new Leaf(x, y, split, height);
            rightChild = new Leaf(x + split, y, width - split, height);
        }
        return true; // split successful!
    }
}

public class BSPTest : MonoBehaviour
{
    const int MAX_LEAF_SIZE = 50;

    private List<Leaf> leafs;
    private Leaf root;
    void Start()
    {
        // first, create a Leaf to be the 'root' of all Leafs.
        leafs = new List<Leaf>();
        root = new Leaf(0, 0, 100, 100);
        leafs.Add(root);


        bool did_split = true;
// we loop through every Leaf in our Vector over and over again, until no more Leafs can be split.
        while (did_split)
        {
            did_split = false;

            for (int i = 0; i < leafs.Count; i++)
            {
                  if (leafs[i].leftChild == null && leafs[i].rightChild == null) // if this Leaf is not already split...
                  {
                    // if this Leaf is too big, or 75% chance...
                    if (leafs[i].width > MAX_LEAF_SIZE || leafs[i].height > MAX_LEAF_SIZE || Random.value > 0.25f)
                    {
                        if (leafs[i].split()) // split the Leaf!
                        {
                            // if we did split, push the child leafs to the Vector so we can loop into them next
                            leafs.Add(leafs[i].leftChild);
                            leafs.Add(leafs[i].rightChild);
                            did_split = true;
                        }
                    }
                  }
            }
        }
    }

   // bool nik;

    private void OnDrawGizmos()
    {
        //if (nik) return;
        foreach (Leaf l in leafs)
        {
            if (l.leftChild==null || l.rightChild==null)
            {
                Gizmos.color=new Color(Random.value,Random.value,Random.value);
                Gizmos.DrawCube(new Vector2(l.x + l.width/2, l.y + l.height / 2),new Vector2(l.width,l.height));
                Debug.Log(new Bounds((new Vector3(l.x + l.width / 2, l.y + l.height / 2, 0)), new Vector3(l.width, l.height, 0)));
            }
        }

        //nik = true;
    }
}


