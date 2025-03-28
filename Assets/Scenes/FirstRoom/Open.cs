using UnityEngine;

public class Open : MonoBehaviour
{
    public GameObject door;
    private bool isOpening = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && !isOpening)
        {
            StartCoroutine(OpenEvent());
        }
    }

    private System.Collections.IEnumerator OpenEvent()
    {
        isOpening = true;

        float targetY = 4f;
        float speed = 2f; // �ʴ� ��� �ӵ�

        Vector3 startPos = door.transform.position;

        while (door.transform.position.y < targetY)
        {
            Vector3 pos = door.transform.position;
            pos.y += speed * Time.deltaTime;
            door.transform.position = pos;

            yield return null; // ���� �����ӱ��� ���
        }

        // ��Ȯ�� ��ġ ����
        Vector3 finalPos = door.transform.position;
        finalPos.y = targetY;
        door.transform.position = finalPos;

        isOpening = false;
    }
}
