using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoDTree : ICollection<Vector2>
{
    private class BinaryNode
    {
        public Vector2 point;
        public BinaryNode leftChild, rightChild;

        public BinaryNode(Vector2 point)
        {
            this.point = point;
            leftChild = null;
            rightChild = null;
        }
    }

    private BinaryNode root;

    public int Count { get; private set; }

    public bool IsReadOnly => false;

    public TwoDTree()
    {
        root = null;
        Count = 0;
    }

    public void Add(Vector2 item)
    {
        Insert(item);
    }

    public void Clear()
    {
        root = null;
        Count = 0;
    }

    public bool Contains(Vector2 item)
    {
        return Search(item);
    }

public void CopyTo(Vector2[] array, int arrayIndex)
{
    if (array == null)
        throw new ArgumentNullException(nameof(array));

    if (arrayIndex < 0 || arrayIndex >= array.Length)
        throw new ArgumentOutOfRangeException(nameof(arrayIndex), "Array index is out of range.");

    if (Count > array.Length - arrayIndex)
        throw new ArgumentException("The number of elements in the collection is greater than the available space from the array index to the end of the array.");

    CopyToRec(root, array, ref arrayIndex);
}

private void CopyToRec(BinaryNode node, Vector2[] array, ref int arrayIndex)
{
    if (node == null)
        return;

    CopyToRec(node.leftChild, array, ref arrayIndex);
    array[arrayIndex++] = node.point;
    CopyToRec(node.rightChild, array, ref arrayIndex);
}

public bool Remove(Vector2 item)
{
    throw new NotSupportedException("Remove operation is not supported in the KDTree.");
}

public IEnumerator<Vector2> GetEnumerator()
{
    return InOrderTraversal(root).GetEnumerator();
}

private IEnumerable<Vector2> InOrderTraversal(BinaryNode node)
{
    if (node != null)
    {
        foreach (Vector2 point in InOrderTraversal(node.leftChild))
            yield return point;

        yield return node.point;

        foreach (Vector2 point in InOrderTraversal(node.rightChild))
            yield return point;
    }
}

IEnumerator IEnumerable.GetEnumerator()
{
    return GetEnumerator();
}
    private BinaryNode InsertRec(BinaryNode node, Vector2 point, int depth)
    {
        if (node == null)
        {
            Count++;
            return new BinaryNode(point);
        }

        int currentDimension = depth % 2; // 2 dimensions in Vector2 (x, y)

        if (currentDimension == 0) // Sort by x-coordinate
        {
            if (point.x < node.point.x)
                node.leftChild = InsertRec(node.leftChild, point, depth + 1);
            else
                node.rightChild = InsertRec(node.rightChild, point, depth + 1);
        }
        else // Sort by y-coordinate
        {
            if (point.y < node.point.y)
                node.leftChild = InsertRec(node.leftChild, point, depth + 1);
            else
                node.rightChild = InsertRec(node.rightChild, point, depth + 1);
        }

        return node;
    }

    public bool Search(Vector2 target)
    {
        return SearchRec(root, target, 0);
    }

    private bool SearchRec(BinaryNode node, Vector2 target, int depth)
    {
        if (node == null)
            return false;

        if (node.point == target)
            return true;

        int currentDimension = depth % 2; // 2 dimensions in Vector2 (x, y)

        if (currentDimension == 0) // Sort by x-coordinate
        {
            if (target.x < node.point.x)
                return SearchRec(node.leftChild, target, depth + 1);
            else
                return SearchRec(node.rightChild, target, depth + 1);
        }
        else // Sort by y-coordinate
        {
            if (target.y < node.point.y)
                return SearchRec(node.leftChild, target, depth + 1);
            else
                return SearchRec(node.rightChild, target, depth + 1);
        }
    }
    public Vector2 FindClosestNode(Vector2 target)
{
    if (root == null)
    {
        throw new InvalidOperationException("KDTree is empty");
    }

    return FindClosestNodeRec(root, target, root.point, float.PositiveInfinity, 0);
}

private Vector2 FindClosestNodeRec(BinaryNode node, Vector2 target, Vector2 bestPoint, float bestDistance, int depth)
{
    if (node == null)
        return bestPoint;

    float currentDistance = Vector2.Distance(node.point, target);

    if (currentDistance < bestDistance)
    {
        bestDistance = currentDistance;
        bestPoint = node.point;
    }

    int currentDimension = depth % 2; // 2 dimensions in Vector2 (x, y)

    BinaryNode firstChild, secondChild;

    if (currentDimension == 0) // Sort by x-coordinate
    {
        if (target.x < node.point.x)
        {
            firstChild = node.leftChild;
            secondChild = node.rightChild;
        }
        else
        {
            firstChild = node.rightChild;
            secondChild = node.leftChild;
        }
    }
    else // Sort by y-coordinate
    {
        if (target.y < node.point.y)
        {
            firstChild = node.leftChild;
            secondChild = node.rightChild;
        }
        else
        {
            firstChild = node.rightChild;
            secondChild = node.leftChild;
        }
    }

    Vector2 closestInFirst = FindClosestNodeRec(firstChild, target, bestPoint, bestDistance, depth + 1);

    float closestDistance = Vector2.Distance(closestInFirst, target);

    if (closestDistance < bestDistance)
    {
        bestPoint = closestInFirst;
        bestDistance = closestDistance;
    }

    // Check if we need to search the second child
    if (Mathf.Abs(node.point[currentDimension] - target[currentDimension]) < bestDistance)
    {
        Vector2 closestInSecond = FindClosestNodeRec(secondChild, target, bestPoint, bestDistance, depth + 1);
        float closestDistanceSecond = Vector2.Distance(closestInSecond, target);

        if (closestDistanceSecond < bestDistance)
        {
            bestPoint = closestInSecond;
        }
    }

    return bestPoint;
}


}