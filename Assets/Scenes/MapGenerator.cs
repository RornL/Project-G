using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [Header("BSP ¼³Á¤")]
    public int mapWidth = 50;
    public int mapHeight = 50;
    public int maxLeafSize = 20;
    public int minLeafSize = 10;

    [Header("·ë ÇÁ¸®ÆÕ (2x1x2)")]
    public GameObject roomPrefab;

    private List<Leaf> leaves = new List<Leaf>();

    void Start()
    {
        GenerateRooms();
    }

    void GenerateRooms()
    {
        Leaf root = new Leaf(0, 0, mapWidth, mapHeight);
        leaves.Add(root);

        bool didSplit = true;
        while (didSplit)
        {
            didSplit = false;
            List<Leaf> newLeaves = new List<Leaf>();

            foreach (Leaf l in leaves)
            {
                if (l.leftChild == null && l.rightChild == null)
                {
                    if (l.width > maxLeafSize || l.height > maxLeafSize || Random.value > 0.5f)
                    {
                        if (l.Split(minLeafSize))
                        {
                            newLeaves.Add(l.leftChild);
                            newLeaves.Add(l.rightChild);
                            didSplit = true;
                        }
                    }
                }
            }
            leaves.AddRange(newLeaves);
        }

        root.CreateRooms();

        foreach (Leaf l in leaves)
        {
            if (l.room != Rect.zero && roomPrefab != null)
            {
                Vector3 position = new Vector3(
                    Mathf.Floor(l.room.x + l.room.width / 2),
                    0,
                    Mathf.Floor(l.room.y + l.room.height / 2)
                );

                position.x = Mathf.Round(position.x / 2f) * 2f;
                position.z = Mathf.Round(position.z / 2f) * 2f;

                GameObject room = Instantiate(roomPrefab, position, Quaternion.identity);
                room.name = $"Room_{position}";
            }
        }
    }

    class Leaf
    {
        public int x, y, width, height;
        public Leaf leftChild;
        public Leaf rightChild;
        public Rect room = Rect.zero;

        public Leaf(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public bool Split(int minLeafSize)
        {
            if (leftChild != null || rightChild != null) return false;

            bool splitH = Random.value > 0.5f;
            if (width / height >= 1.25f) splitH = false;
            else if (height / width >= 1.25f) splitH = true;

            if (splitH)
            {
                if (height < minLeafSize * 2) return false;
                int split = Random.Range(minLeafSize, height - minLeafSize);
                leftChild = new Leaf(x, y, width, split);
                rightChild = new Leaf(x, y + split, width, height - split);
            }
            else
            {
                if (width < minLeafSize * 2) return false;
                int split = Random.Range(minLeafSize, width - minLeafSize);
                leftChild = new Leaf(x, y, split, height);
                rightChild = new Leaf(x + split, y, width - split, height);
            }
            return true;
        }

        public void CreateRooms()
        {
            if (leftChild != null || rightChild != null)
            {
                leftChild?.CreateRooms();
                rightChild?.CreateRooms();
            }
            else
            {
                int roomWidth = Random.Range(4, width - 1);
                int roomHeight = Random.Range(4, height - 1);
                int roomX = x + Random.Range(0, width - roomWidth);
                int roomY = y + Random.Range(0, height - roomHeight);
                room = new Rect(roomX, roomY, roomWidth, roomHeight);
            }
        }
    }
}
