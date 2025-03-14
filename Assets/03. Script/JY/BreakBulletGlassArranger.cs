using UnityEngine;

public class BreakBulletGlassArranger : MonoBehaviour
{
    public GameObject breakBulletGlassPrefab;  // BreakBulletGlass ������Ʈ ������
    public float breakBulletGlassWidth = 1f;   // BreakBulletGlass�� x ũ�� (������)
    public float breakBulletGlassHeight = 1f;  // BreakBulletGlass�� y ũ�� (������)
    public GameObject container;

    void Start()
    {
        ArrangeBreakBulletGlassObjects();
    }

    void ArrangeBreakBulletGlassObjects()
    {
        // �� ������Ʈ�� ũ�� ���ϱ�
        Vector3 parentSize = transform.localScale;
        Vector3 breakSize = new Vector3(parentSize.x / breakBulletGlassWidth, parentSize.y / breakBulletGlassHeight, parentSize.z);

        // �� ������Ʈ�� ũ�⿡ ���� �� �������� �󸶳� ��ġ���� ���
       // int countX = Mathf.FloorToInt(parentSize.x / breakBulletGlassWidth);  // x������ ��ġ�� �� �ִ� ��
        int countY = Mathf.FloorToInt(parentSize.y / breakBulletGlassHeight); // y������ ��ġ�� �� �ִ� ��
        int countZ = Mathf.FloorToInt(parentSize.z / breakBulletGlassWidth);  // z������ ��ġ�� �� �ִ� ��
     
        // Glass ������Ʈ�� ��ġ
        for (int y = 0; y < breakBulletGlassHeight; y++)  // y�� �������� �ݺ�
        {
            for (int x = 0; x <breakBulletGlassWidth; x++)  // x�� �������� �ݺ�
            {
               
                    Vector3 position =transform.position + new Vector3(
                        (x - (breakBulletGlassWidth - 1)/2f ) * breakSize.x,
                        (y - (breakBulletGlassHeight - 1)/2f ) * breakSize.y,
                       //  (x * breakBulletGlassWidth) - parentSize.x / 2 + breakBulletGlassWidth / 2,
                        // (y * breakBulletGlassHeight) - parentSize.y / 2 + breakBulletGlassHeight / 2,
                      0  //transform.position.z + (k * breakBulletGlassDepth) - parentSize.z / 2 + breakBulletGlassDepth / 2
                    );

                    // BreakBulletGlass ������Ʈ �ν��Ͻ� ����
                   GameObject obj = Instantiate(breakBulletGlassPrefab, position, Quaternion.identity,container.transform);
                obj.transform.localScale = breakSize;
              //  obj. transform.localPosition = position;
               
            }
        }
        container.transform.rotation = transform.rotation;
    }
}


