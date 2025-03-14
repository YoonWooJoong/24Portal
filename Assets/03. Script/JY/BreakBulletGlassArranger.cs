using UnityEngine;

public class BreakBulletGlassArranger : MonoBehaviour
{
    public GameObject breakBulletGlassPrefab;  // BreakBulletGlass 오브젝트 프리팹
    public float breakBulletGlassWidth = 1f;   // BreakBulletGlass의 x 크기 (고정값)
    public float breakBulletGlassHeight = 1f;  // BreakBulletGlass의 y 크기 (고정값)
    public GameObject container;

    void Start()
    {
        ArrangeBreakBulletGlassObjects();
    }

    void ArrangeBreakBulletGlassObjects()
    {
        // 빈 오브젝트의 크기 구하기
        Vector3 parentSize = transform.localScale;
        Vector3 breakSize = new Vector3(parentSize.x / breakBulletGlassWidth, parentSize.y / breakBulletGlassHeight, parentSize.z);

        // 빈 오브젝트의 크기에 맞춰 각 방향으로 얼마나 배치할지 계산
       // int countX = Mathf.FloorToInt(parentSize.x / breakBulletGlassWidth);  // x축으로 배치할 수 있는 수
        int countY = Mathf.FloorToInt(parentSize.y / breakBulletGlassHeight); // y축으로 배치할 수 있는 수
        int countZ = Mathf.FloorToInt(parentSize.z / breakBulletGlassWidth);  // z축으로 배치할 수 있는 수
     
        // Glass 오브젝트를 배치
        for (int y = 0; y < breakBulletGlassHeight; y++)  // y축 방향으로 반복
        {
            for (int x = 0; x <breakBulletGlassWidth; x++)  // x축 방향으로 반복
            {
               
                    Vector3 position =transform.position + new Vector3(
                        (x - (breakBulletGlassWidth - 1)/2f ) * breakSize.x,
                        (y - (breakBulletGlassHeight - 1)/2f ) * breakSize.y,
                       //  (x * breakBulletGlassWidth) - parentSize.x / 2 + breakBulletGlassWidth / 2,
                        // (y * breakBulletGlassHeight) - parentSize.y / 2 + breakBulletGlassHeight / 2,
                      0  //transform.position.z + (k * breakBulletGlassDepth) - parentSize.z / 2 + breakBulletGlassDepth / 2
                    );

                    // BreakBulletGlass 오브젝트 인스턴스 생성
                   GameObject obj = Instantiate(breakBulletGlassPrefab, position, Quaternion.identity,container.transform);
                obj.transform.localScale = breakSize;
              //  obj. transform.localPosition = position;
               
            }
        }
        container.transform.rotation = transform.rotation;
    }
}


