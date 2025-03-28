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
        float speed = 2f; // 초당 상승 속도

        Vector3 startPos = door.transform.position;

        while (door.transform.position.y < targetY)
        {
            Vector3 pos = door.transform.position;
            pos.y += speed * Time.deltaTime;
            door.transform.position = pos;

            yield return null; // 다음 프레임까지 대기
        }

        // 정확한 위치 보정
        Vector3 finalPos = door.transform.position;
        finalPos.y = targetY;
        door.transform.position = finalPos;

        isOpening = false;
    }
}
